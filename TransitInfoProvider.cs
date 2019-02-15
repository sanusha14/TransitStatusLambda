using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace TransitLambdaV1
{
    internal class TransitInfoProvider
    {
        private const string OneBusAwayAPIKey = "f2bda33e-702d-4354-ac0b-c792070b976c";
        private static readonly HttpClient _client = new HttpClient();
        private static readonly JsonSerializer _serializer = new JsonSerializer();

        private const string OBAStopsIdsUri = "http://api.pugetsound.onebusaway.org/api/where/stops-for-location.json?key={0}&lat={1}&lon={2}&radius=1000";
        private const string OBAArrivalsUri = "http://api.pugetsound.onebusaway.org/api/where/arrivals-and-departures-for-stop/{0}.json?key={1}";

        public async Task<IEnumerable<Response.ArrivalResponse>> GetArrivalInfoForRoute(string routeId, Response.Geocode geoCode, ILambdaContext context)
        {
            var stops = await RetrieveStopInfo(routeId, geoCode, context);
            context.Logger.Log($"Received '{stops.Count()}' responses from 'Stops' API.");

            var result = new List<Response.ArrivalResponse>();

            foreach (var stop in stops)
            {
                var arrivalInfo = await RetrieveArrivalInfoForStop(stop.id, routeId, context);
                result.Add(new Response.ArrivalResponse
                {
                    TripHeadSign = arrivalInfo.Item1,
                    Arrival = arrivalInfo.Item2,
                    StopName = stop.name
                });
            }

            return result;
        }

        private async Task<IEnumerable<Response.OBA.Stops.Stop>> RetrieveStopInfo(string routeId, Response.Geocode geocode, ILambdaContext context)
        {
            string uri = string.Format(OBAStopsIdsUri, OneBusAwayAPIKey, geocode.Latitude, geocode.Longitude);
            context.Logger.Log($"Generated OBA URI is {uri}");

            using (var responseStream = await _client.GetStreamAsync(uri))
            {
                var response = _serializer.Deserialize<Response.OBA.Stops.Response>(responseStream);
                return response.data.list.Where(stop => stop.routeIds.Any(route => routeId.Contains($"_{routeId}")));
            }
        }

        private async Task<Tuple<string, DateTime>> RetrieveArrivalInfoForStop(string stopId, string routeId, ILambdaContext context)
        {
            string uri = string.Format(OBAArrivalsUri, OneBusAwayAPIKey, stopId);
            context.Logger.Log($"Generated OBA URI is {uri}");

            using (var responseStream = await _client.GetStreamAsync(uri))
            {
                var response = _serializer.Deserialize<Response.OBA.Arrivals.Response>(responseStream);
                var arrivalInfoForRoute = response.data.entry.arrivalsAndDepartures.SingleOrDefault(info => info.routeId.Contains($"_{routeId}"));
                if (arrivalInfoForRoute == null)
                    return default(Tuple<string, DateTime>);

                return new Tuple<string, DateTime>(arrivalInfoForRoute.tripHeadsign, FromEpoch(arrivalInfoForRoute.scheduledArrivalTime));
            }
        }

        private static DateTime FromEpoch(long offset)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(offset);
        }
    }
}
