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
        public static バックログアイテムディスカッション FromAvailability(DiscussionAvailability availability)
        {
            if (availability == DiscussionAvailability.Ready)
                throw new ArgumentException("Cannot be created ready.");

            return new バックログアイテムディスカッション(
                new DiscussionDescriptor(DiscussionDescriptor.UNDEFINED_ID),
                availability);
        }

        public バックログアイテムディスカッション(DiscussionDescriptor descriptor, DiscussionAvailability availability)
        {
            this.Descriptor = descriptor;
            this.Availability = availability;
        }        

        public DiscussionDescriptor Descriptor { get; private set; }

        public DiscussionAvailability Availability { get; private set; }

        public バックログアイテムディスカッション NowReady(DiscussionDescriptor descriptor)
        {
            if (descriptor == null || descriptor.IsUndefined)
                throw new InvalidOperationException("The discussion descriptor must be defined.");

            if (this.Availability != DiscussionAvailability.Requested)
                throw new InvalidOperationException("The discussion must be requested first.");

            return new バックログアイテムディスカッション(descriptor, DiscussionAvailability.Ready);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Availability;
            yield return this.Descriptor;
        }
    }
}
