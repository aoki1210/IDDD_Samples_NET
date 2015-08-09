using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.AgilePM.Domain.Model.Discussions;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテムディスカッション : ValueObject
    {
        public static バックログアイテムディスカッション FromAvailability(ディスカッションアベイラビリティ availability)
        {
            if (availability == ディスカッションアベイラビリティ.Ready)
                throw new ArgumentException("Cannot be created ready.");

            return new バックログアイテムディスカッション(
                new ディスカッション記述子(ディスカッション記述子.UNDEFINED_ID),
                availability);
        }

        public バックログアイテムディスカッション(ディスカッション記述子 descriptor, ディスカッションアベイラビリティ availability)
        {
            this.Descriptor = descriptor;
            this.Availability = availability;
        }        

        public ディスカッション記述子 Descriptor { get; private set; }

        public ディスカッションアベイラビリティ Availability { get; private set; }

        public バックログアイテムディスカッション NowReady(ディスカッション記述子 descriptor)
        {
            if (descriptor == null || descriptor.IsUndefined)
                throw new InvalidOperationException("The discussion descriptor must be defined.");

            if (this.Availability != ディスカッションアベイラビリティ.Requested)
                throw new InvalidOperationException("The discussion must be requested first.");

            return new バックログアイテムディスカッション(descriptor, ディスカッションアベイラビリティ.Ready);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Availability;
            yield return this.Descriptor;
        }
    }
}
