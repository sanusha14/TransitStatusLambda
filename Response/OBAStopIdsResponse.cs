using System;
using System.Collections.Generic;
using System.Text;

namespace TransitLambdaV1.Response.OBA.Stops
{
    public sealed class Response
    {
        public string code { get; set; }
        public Data data { get; set; }
    }

    public sealed class Data
    {
        public IList<Stop> list { get; set; }
    }

    public sealed class Stop
    {
        public string id { get; set; }
        public string name { get; set; }
        public IList<string> routeIds { get; set; }
    }
}
