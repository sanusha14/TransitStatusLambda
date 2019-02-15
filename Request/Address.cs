using System;
using System.Collections.Generic;
using System.Text;

namespace TransitLambdaV1.Request
{
    public sealed class Address
    {
        public string StateOrRegion { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string DistrictOrCounty { get; set; }

        public string ToQuery()
        {
            var result = new StringBuilder(AddressLine1);

            if (!string.IsNullOrEmpty(AddressLine2))
                result.Append($",{AddressLine2}");
            if (!string.IsNullOrEmpty(AddressLine3))
                result.Append($",{AddressLine3}");

            result.Append($",{City}");
            result.Append($",{StateOrRegion}");
            result.Append($",{CountryCode}");
            result.Append($",{PostalCode}");

            return result.ToString();
        }
    }
}
