using System;
using System.Collections.Generic;
using System.Text;

namespace TransitLambdaV1.Response
{
    public sealed class ArrivalResponse
    {
        public string StopName { get; set; }
        public DateTime Arrival { get; set; }
        public string TripHeadSign { get; set; }
    }
}
