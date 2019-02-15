using System;
using System.Collections.Generic;
using System.Text;

namespace TransitLambdaV1.Request
{
    public sealed class Request
    {
        public string RouteId { get; set; }
        public Address Address { get; set; }
    }
}
