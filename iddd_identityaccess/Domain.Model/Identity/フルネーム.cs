﻿// Copyright 2012,2013 Vaughn Vernon
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

    public class フルネーム
    {
        public フルネーム(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public フルネーム(フルネーム fullName)
            : this(fullName.FirstName, fullName.LastName)
        {
        }

        protected フルネーム() { }


        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public String asFormattedName()
        {
            return this.FirstName + " " + this.LastName;
        }

        public フルネーム WithChangedFirstName(string firstName)
        {
            return new フルネーム(firstName, this.LastName);
        }

        public フルネーム WithChangedLastName(string lastName)
        {
            return new フルネーム(this.FirstName, lastName);
        }

        public override bool Equals(object anotherObject)
        {
            bool equalObjects = false;

            if (anotherObject != null && this.GetType() == anotherObject.GetType()) {
                フルネーム typedObject = (フルネーム) anotherObject;
                equalObjects =
                    this.FirstName.Equals(typedObject.FirstName) &&
                    this.LastName.Equals(typedObject.LastName);
            }

            return equalObjects;
        }

        public override int GetHashCode()
        {
            int hashCodeValue =
                + (59151 * 191)
                + this.FirstName.GetHashCode()
                + this.LastName.GetHashCode();

            return hashCodeValue;
        }

        public override string ToString()
        {
            return "FullName [firstName=" + FirstName + ", lastName=" + LastName + "]";
        }
    }
}
