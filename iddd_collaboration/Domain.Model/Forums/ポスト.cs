using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public class ポスト : EventSourcedRootEntity
    {
        public ポスト(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        テナント tenantId; 
        フォーラムId forumId; 
        ディスカッションId discussionId; 
        ポストId postId; 
        著者 author; 
        string subject; 
        string bodyText; 
        ポストId replyToPostId;

        public フォーラムId ForumId
        {
            get { return this.forumId; }
        }

        public ポストId PostId
        {
            get { return this.postId; }
        }

        public ポスト(テナント tenantId, フォーラムId forumId, ディスカッションId discussionId, ポストId postId, 著者 author, string subject, string bodyText, ポストId replyToPostId = null)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.AssertArgumentNotNull(forumId, "The forum id must be provided.");
            AssertionConcern.AssertArgumentNotNull(discussionId, "The discussion id must be provided.");
            AssertionConcern.AssertArgumentNotNull(postId, "The post id must be provided.");
            AssertionConcern.AssertArgumentNotNull(author, "The author must be provided.");
            AssertPostContent(subject, bodyText);

            Apply(new ディスカッションにポスト時(tenantId, forumId, discussionId, postId, author, subject, bodyText, replyToPostId));
        }

        void When(ディスカッションにポスト時 e)
        {
            this.tenantId = e.TenantId;
            this.forumId = e.ForumId;
            this.discussionId = e.DiscussionId;
            this.postId = e.PostId;
            this.author = e.Author;
            this.subject = e.Subject;
            this.bodyText = e.BodyText;
            this.replyToPostId = e.ReplyToPostId;
        }

        void AssertPostContent(string subject, string bodyText)
        {
            AssertionConcern.AssertArgumentNotEmpty(subject, "The subject must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(bodyText, "The body text must be provided.");
        }


        internal void AlterPostContent(string subject, string bodyText)
        {
            AssertPostContent(subject, bodyText);
            Apply(new PostedContentAltered(this.tenantId, this.forumId, this.discussionId, this.postId, subject, bodyText));
        }

        void When(PostedContentAltered e)
        {
            this.subject = e.Subject;
            this.bodyText = e.BodyText;
        }
        

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.tenantId;
            yield return this.forumId;
            yield return this.discussionId;
            yield return this.postId;
        }
    }
}
