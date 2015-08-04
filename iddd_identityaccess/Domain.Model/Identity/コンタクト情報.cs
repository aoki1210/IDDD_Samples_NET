// Copyright 2012,2013 Vaughn Vernon
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    using System;
    using SaaSOvation.Common.Domain.Model;

    public class コンタクト情報 : ValueObject
    {
        public コンタクト情報(
                Emailアドレス emailAddress,
                郵便住所 postalAddress,
                電話 primaryTelephone,
                電話 secondaryTelephone)
        {
            this.EmailAddress = emailAddress;
            this.PostalAddress = postalAddress;
            this.PrimaryTelephone = primaryTelephone;
            this.SecondaryTelephone = secondaryTelephone;
        }

        public コンタクト情報(コンタクト情報 contactInformation)
            : this(contactInformation.EmailAddress,
                   contactInformation.PostalAddress,
                   contactInformation.PrimaryTelephone,
                   contactInformation.SecondaryTelephone)
        {
        }

        internal コンタクト情報()
        {
        }

        public Emailアドレス EmailAddress { get; private set; }

        public 郵便住所 PostalAddress { get; private set; }

        public 電話 PrimaryTelephone { get; private set; }

        public 電話 SecondaryTelephone { get; private set; }

        public コンタクト情報 ChangeEmailAddress(Emailアドレス emailAddress)
        {
            return new コンタクト情報(
                    emailAddress,
                    this.PostalAddress,
                    this.PrimaryTelephone,
                    this.SecondaryTelephone);
        }

        public コンタクト情報 ChangePostalAddress(郵便住所 postalAddress)
        {
            return new コンタクト情報(
                   this.EmailAddress,
                   postalAddress,
                   this.PrimaryTelephone,
                   this.SecondaryTelephone);
        }

        public コンタクト情報 ChangePrimaryTelephone(電話 telephone)
        {
            return new コンタクト情報(
                   this.EmailAddress,
                   this.PostalAddress,
                   telephone,
                   this.SecondaryTelephone);
        }

        public コンタクト情報 ChangeSecondaryTelephone(電話 telephone)
        {
            return new コンタクト情報(
                   this.EmailAddress,
                   this.PostalAddress,
                   this.PrimaryTelephone,
                   telephone);
        }

        public override string ToString()
        {
            return "ContactInformation [emailAddress=" + EmailAddress
                    + ", postalAddress=" + PostalAddress
                    + ", primaryTelephone=" + PrimaryTelephone
                    + ", secondaryTelephone=" + SecondaryTelephone + "]";
        }

        protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
        {
            yield return this.EmailAddress;
            yield return this.PostalAddress;
            yield return this.PrimaryTelephone;
            yield return this.SecondaryTelephone;
        }
    }
}
