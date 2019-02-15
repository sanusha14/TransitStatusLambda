using System;
using System.Collections.Generic;
using System.Text;

namespace TransitLambdaV1.Response.OBA.Arrivals
{
    public sealed class Response
    {
        public string code { get; set; }
        public Data data { get; set; }
    }

    public sealed class Data
    {
        public Entry entry { get; set; }
    }

    public sealed class Entry
    {
        public IList<ArrivalAndDeparture> arrivalsAndDepartures { get; set; }
    }

    public sealed class ArrivalAndDeparture
    {
        public string tripHeadsign { get; set; }
        public long scheduledArrivalTime { get; set; }
        public string routeId { get; set; }
        public string routeShortName { get; set; }
    }
}
