using Amazon.Lambda.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TransitLambdaV1
{
    public sealed class Function
    {
        private readonly GeocodeProvider _geocodeProvider = new GeocodeProvider();
        private readonly TransitInfoProvider _transitInfoProvider = new TransitInfoProvider();

        /// <summary>
        /// Logic to retrieve arrival info for a given address and route.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Response.ArrivalResponse>> FunctionHandler(Request.Request input, ILambdaContext context)
        {
            var geocode = await _geocodeProvider.ConvertAddressToGeocode(input.Address, context);
            return await _transitInfoProvider.GetArrivalInfoForRoute(input.RouteId, geocode, context);
        }
    }
}
