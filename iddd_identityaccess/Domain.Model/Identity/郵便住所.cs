﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    public class 郵便住所 : ValueObject
    {
        public 郵便住所(
                String streetAddress,
                String city,
                String stateProvince,
                String postalCode,
                String countryCode)
        {
            this.City = city;
            this.CountryCode = countryCode;
            this.PostalCode = postalCode;
            this.StateProvince = stateProvince;
            this.StreetAddress = streetAddress;
        }

        public 郵便住所(郵便住所 postalAddress)
            : this(postalAddress.StreetAddress,
                   postalAddress.City,
                   postalAddress.StateProvince,
                   postalAddress.PostalCode,
                   postalAddress.CountryCode)
        {
        }

        protected 郵便住所() { }

        public string City { get; private set; }

        public string CountryCode { get; private set; }

        public string PostalCode { get; private set; }

        public string StateProvince { get; private set; }

        public string StreetAddress { get; private set; }

        public override string ToString()
        {
            return "PostalAddress [streetAddress=" + StreetAddress
                    + ", city=" + City + ", stateProvince=" + StateProvince
                    + ", postalCode=" + PostalCode
                    + ", countryCode=" + CountryCode + "]";
        }

        protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StreetAddress;
            yield return this.City;
            yield return this.StateProvince;
            yield return this.PostalCode;
            yield return this.CountryCode;
        }
    }
}
