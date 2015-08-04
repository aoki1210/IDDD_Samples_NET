using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    public class Emailアドレス : ValueObject
    {
        public Emailアドレス(string address)
        {
            this.Address = address;
        }

        public Emailアドレス(Emailアドレス emailAddress)
            : this(emailAddress.Address)
        {
        }

        protected Emailアドレス() { }

        string address;

        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                AssertionConcern.AssertArgumentNotEmpty(value, "The email address is required.");
                AssertionConcern.AssertArgumentLength(value, 1, 100, "Email address must be 100 characters or less.");
                AssertionConcern.AssertArgumentMatches(
                        "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*",
                        value,
                        "Email address format is invalid.");

                this.address = value;
            }
        }

        public override string ToString()
        {
            return "EmailAddress [address=" + Address + "]";
        }

        protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
        {
            yield return address.ToUpper();
        }
    }
}
