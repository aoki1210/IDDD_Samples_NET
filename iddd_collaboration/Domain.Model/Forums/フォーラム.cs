using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public class フォーラム : EventSourcedRootEntity
    {
        public フォーラム(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        public フォーラム(テナント tenantId, フォーラムId forumId, クリエイタ creator, モデレータ moderator, string subject, string description, string exclusiveOwner)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.AssertArgumentNotNull(forumId, "The forum id must be provided.");
            AssertionConcern.AssertArgumentNotNull(creator, "The creator must be provided.");
            AssertionConcern.AssertArgumentNotNull(moderator, "The moderator must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(subject, "The subject must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(description, "The description must be provided.");

            Apply(new フォーラム開始時(tenantId, forumId, creator, moderator, subject, description, exclusiveOwner));
        }

        void When(フォーラム開始時 e)
        {
            this.tenantId = e.TenantId;
            this.forumId = e.ForumId;
            this.creator = e.Creator;
            this.moderator = e.Moderator;
            this.subject = e.Subject;
            this.description = e.Description;
            this.exclusiveOwner = e.ExclusiveOwner;
        }

        テナント tenantId;
        フォーラムId forumId;
        クリエイタ creator;
        モデレータ moderator;
        string subject;
        string description;
        string exclusiveOwner;
        bool closed;

        public フォーラムId ForumId
        {
            get { return this.forumId; }
        }

        void AssertOpen()
        {
            if (this.closed)
                throw new InvalidOperationException("Forum is closed.");
        }

        void AssertClosed()
        {
            if (!this.closed)
                throw new InvalidOperationException("Forum is open.");
        }

        public void AssignModerator(モデレータ moderator)
        {
            AssertOpen();
            AssertionConcern.AssertArgumentNotNull(moderator, "The moderator must be provided.");
            Apply(new フォーラムモデレータ変更時(this.tenantId, this.forumId, moderator, this.exclusiveOwner));
        }

        void When(フォーラムモデレータ変更時 e)
        {
            this.moderator = e.Moderator;
        }


        public void ChangeDescription(string description)
        {
            AssertOpen();
            AssertionConcern.AssertArgumentNotEmpty(description, "The description must be provided.");
            Apply(new フォーラム詳細変更時(this.tenantId, this.forumId, description, this.exclusiveOwner));
        }

        void When(フォーラム詳細変更時 e)
        {
            this.description = e.Description;
        }


        public void ChangeSubject(string subject)
        {
            AssertOpen();
            AssertionConcern.AssertArgumentNotEmpty(subject, "The subject must be provided.");
            Apply(new フォーラムタイトル変更時(this.tenantId, this.forumId, subject, this.exclusiveOwner));
        }

        void When(フォーラムタイトル変更時 e)
        {
            this.subject = e.Subject;
        }


        public void Close()
        {
            AssertOpen();
            Apply(new フォーラム終了時(this.tenantId, this.forumId, this.exclusiveOwner));
        }

        void When(フォーラム終了時 e)
        {
            this.closed = true;
        }


        public void ModeratePost(ポスト post, モデレータ moderator, string subject, string bodyText)
        {
            AssertOpen();
            AssertionConcern.AssertArgumentNotNull(post, "Post may not be null.");
            AssertionConcern.AssertArgumentEquals(this.forumId, post.ForumId, "Not a post of this forum.");
            AssertionConcern.AssertArgumentTrue(IsModeratedBy(moderator), "Not the moderator of this forum.");
            post.AlterPostContent(subject, bodyText);
        }


        public void ReOpen()
        {
            AssertClosed();
            Apply(new フォーラム再開時(this.tenantId, this.forumId, this.exclusiveOwner));
        }

        void When(フォーラム再開時 e)
        {
            this.closed = false;
        }

        public ディスカッション StartDiscussionFor(フォーラム識別サービス forumIdService, 著者 author, string subject, string exclusiveOwner = null)
        {
            AssertOpen();
            return new ディスカッション(
                this.tenantId,
                this.forumId,
                forumIdService.GetNextDiscussionId(),
                author,
                subject,
                exclusiveOwner);
        }


        public bool IsModeratedBy(モデレータ moderator)
        {
            return this.moderator.Equals(moderator);
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.tenantId;
            yield return this.forumId;
        }
    }
}
