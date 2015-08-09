using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public class ディスカッション : EventSourcedRootEntity
    {
        public ディスカッション(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }
    
        public ディスカッション(テナント tenantId, フォーラムId forumId, ディスカッションId discussionId, 著者 author, string subject, string exclusiveOwner = null)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.AssertArgumentNotNull(forumId, "The forum id must be provided.");
            AssertionConcern.AssertArgumentNotNull(discussionId, "The discussion id must be provided.");
            AssertionConcern.AssertArgumentNotNull(author, "The author must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(subject, "The subject must be provided.");

            Apply(new ディスカッション開始時(tenantId, forumId, discussionId, author, subject, exclusiveOwner));
        }        

        void When(ディスカッション開始時 e)
        {
            this.tenantId = e.TenantId;
            this.forumId = e.ForumId;
            this.discussionId = e.DiscussionId;
            this.author = e.Author;
            this.subject = e.Subject;
            this.exclusiveOwner = e.ExclusiveOwner;
        }

        テナント tenantId;
        フォーラムId forumId;
        ディスカッションId discussionId;
        著者 author;
        string subject;
        string exclusiveOwner;    
        bool closed;

        public ディスカッションId DiscussionId
        {
            get { return this.discussionId; }
        }

        void AssertClosed()
        {
            if (!this.closed)
                throw new InvalidOperationException("This discussion is already open.");
        }

        public void Close()
        {
            if (this.closed)
                throw new InvalidOperationException("This discussion is already closed.");

            Apply(new ディスカッション終了時(this.tenantId, this.forumId, this.discussionId, this.exclusiveOwner));
        }

        void When(ディスカッション終了時 e)
        {
            this.closed = true;
        }


        public ポスト Post(フォーラム識別サービス forumIdService, 著者 author, string subject, string bodyText, ポストId replyToPostId = null)
        {
            return new ポスト(
                this.tenantId,
                this.forumId,
                this.discussionId,
                forumIdService.GetNexPostId(),
                author,
                subject,
                bodyText,
                replyToPostId);
        }


        public void ReOpen()
        {
            AssertClosed();
            Apply(new ディスカッション再開時(this.tenantId, this.forumId, this.discussionId, this.exclusiveOwner));
        }

        void When(ディスカッション再開時 e)
        {
            this.closed = false;
        }




        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.tenantId;
            yield return this.forumId;
            yield return this.discussionId;
        }
    }
}
