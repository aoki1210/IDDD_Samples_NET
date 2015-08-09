using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public class PostedContentAltered : IDomainEvent
    {
        public PostedContentAltered(テナント tenantId, フォーラムId forumId, ディスカッションId discussionId, ポストId postId, string subject, string bodyText)
        {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.DiscussionId = discussionId;
            this.PostId = postId;
            this.Subject = subject;
            this.BodyText = bodyText;
        }

        public テナント TenantId { get; private set; }
        public フォーラムId ForumId { get; private set; }
        public ディスカッションId DiscussionId { get; private set; }
        public ポストId PostId { get; private set; }
        public string Subject { get; private set; }
        public string BodyText { get; private set; }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
