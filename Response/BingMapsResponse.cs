using System;
using System.Collections.Generic;
using System.Text;

namespace TransitLambdaV1.Response
{
    public sealed class LocationResponse
    {
        public IList<ResourceSet> resourceSets { get; set; }
    }

    public sealed class ResourceSet
    {
        public IList<Resource> resources { get; set; }
    }

    public sealed class Resource
    {
        public Point point { get; set; }
    }

    public sealed class Point
    {
        public IList<double> coordinates { get; set; }
    }
}
