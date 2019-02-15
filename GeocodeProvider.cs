using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TransitLambdaV1
{
    internal sealed class GeocodeProvider
    {
        private const string BingMapsKey = "AkRzrpwitV6GgonMYSUUBZnPdklYtGKMXDVH14eLSIT4xMqB-g9UGAyoydaamTdc";
        private const string _bingMapsUri = "http://dev.virtualearth.net/REST/v1/Locations?q={0}&key={1}";

        private static readonly HttpClient _client = new HttpClient();
        private static readonly JsonSerializer _serializer = new JsonSerializer();

        public async Task<Response.Geocode> ConvertAddressToGeocode(Request.Address userAddress, ILambdaContext context)
        {
            string uri = string.Format(_bingMapsUri, userAddress.ToQuery(), BingMapsKey);
            context.Logger.Log($"Generated Bing Maps URI is {uri}");

            using (var responseStream = await _client.GetStreamAsync(uri))
            {
                var response = _serializer.Deserialize<Response.LocationResponse>(responseStream);

                var coordinates = response.resourceSets[0].resources[0].point.coordinates;
                return new Response.Geocode
                {
                    Latitude = coordinates[0],
                    Longitude = coordinates[1],
                };
            }
        }
    }
}
