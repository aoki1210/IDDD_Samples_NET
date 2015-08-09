using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Collaboration.Domain.Model.Forums;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Application.Forums.Data;

namespace SaaSOvation.Collaboration.Application.Forums
{
    public class フォーラムアプリケーションサービス
    {
        public フォーラムアプリケーションサービス(
            フォーラムクエリーサービス forumQueryService,
            IフォーラムRepository forumRepository,
            フォーラム識別サービス forumIdentityService,
            ディスカッションクエリーサービス discussionQueryService,
            IディスカッションRepository discussionRepository,
            Iコラボレータサービス collaboratorService)
        {
            this.forumQueryService = forumQueryService;
            this.forumRepository = forumRepository;
            this.forumIdentityService = forumIdentityService;
            this.discussionQueryService = discussionQueryService;
            this.discussionRepository = discussionRepository;
            this.collaboratorService = collaboratorService;
        }

        readonly フォーラムクエリーサービス forumQueryService;
        readonly IフォーラムRepository forumRepository;
        readonly フォーラム識別サービス forumIdentityService;
        readonly ディスカッションクエリーサービス discussionQueryService;
        readonly IディスカッションRepository discussionRepository;
        readonly Iコラボレータサービス collaboratorService;

        public void AssignModeratorToForum(string tenantId, string forumId, string moderatorId)
        {
            var tenant = new テナント(tenantId);

            var forum = this.forumRepository.Get(tenant, new フォーラムId(forumId));

            var moderator = this.collaboratorService.GetModeratorFrom(tenant, moderatorId);

            forum.AssignModerator(moderator);

            this.forumRepository.Save(forum);
        }

        public void ChangeForumDescription(string tenantId, string forumId, string description)
        {
            var forum = this.forumRepository.Get(new テナント(tenantId), new フォーラムId(forumId));

            forum.ChangeDescription(description);

            this.forumRepository.Save(forum);
        }

        public void ChangeForumSubject(string tenantId, string forumId, string subject)
        {
            var forum = this.forumRepository.Get(new テナント(tenantId), new フォーラムId(forumId));

            forum.ChangeSubject(subject);

            this.forumRepository.Save(forum);
        }

        public void CloseForum(string tenantId, string forumId)
        {
            var forum = this.forumRepository.Get(new テナント(tenantId), new フォーラムId(forumId));

            forum.Close();

            this.forumRepository.Save(forum);
        }

        public void ReOpenForum(string tenantId, string forumId)
        {
            var forum = this.forumRepository.Get(new テナント(tenantId), new フォーラムId(forumId));

            forum.ReOpen();

            this.forumRepository.Save(forum);
        }

        public void StartForum(string tenantId, string creatorId, string moderatorId, string subject, string description, Iフォーラムコマンド結果 result = null)
        {
            var forum = StartNewForum(new テナント(tenantId), creatorId, moderatorId, subject, description, null);

            if (result != null)
            {
                result.SetResultingForumId(forum.ForumId.Id);
            }
        }

        public void StartExclusiveForum(string tenantId, string exclusiveOwner, string creatorId, string moderatorId, string subject, string description, Iフォーラムコマンド結果 result = null)
        {
            var tenant = new テナント(tenantId);

            フォーラム forum = null;

            var forumId = this.forumQueryService.GetForumIdByExclusiveOwner(tenantId, exclusiveOwner);
            if (forumId != null)
            {
                forum = this.forumRepository.Get(tenant, new フォーラムId(forumId));
            }

            if (forum == null)
            {
                forum = StartNewForum(tenant, creatorId, moderatorId, subject, description, exclusiveOwner);
            }

            if (result != null)
            {
                result.SetResultingForumId(forum.ForumId.Id);
            }
        }

        public void StartExclusiveForumWithDiscussion(
            string tenantId,
            string exclusiveOwner,
            string creatorId,
            string moderatorId,
            string authorId,
            string forumSubject,
            string forumDescription,
            string discussionSubject,
            Iフォーラムコマンド結果 result = null)
        {

            var tenant = new テナント(tenantId);

            フォーラム forum = null;

            var forumId = this.forumQueryService.GetForumIdByExclusiveOwner(tenantId, exclusiveOwner);
            if (forumId != null)
            {
                forum = this.forumRepository.Get(tenant, new フォーラムId(forumId));
            }

            if (forum == null)
            {
                forum = StartNewForum(tenant, creatorId, moderatorId, forumSubject, forumDescription, exclusiveOwner);
            }

            ディスカッション discussion = null;

            var discussionId = this.discussionQueryService.GetDiscussionIdByExclusiveOwner(tenantId, exclusiveOwner);
            if (discussionId != null)
            {
                discussion = this.discussionRepository.Get(tenant, new ディスカッションId(discussionId));
            }

            if (discussion == null)
            {
                var author = this.collaboratorService.GetAuthorFrom(tenant, authorId);

                discussion = forum.StartDiscussionFor(this.forumIdentityService, author, discussionSubject, exclusiveOwner);

                this.discussionRepository.Save(discussion);
            }

            if (result != null)
            {
                result.SetResultingForumId(forum.ForumId.Id);
                result.SetResultingDiscussionId(discussion.DiscussionId.Id);
            }
        }

        フォーラム StartNewForum(
            テナント tenant,
            string creatorId,
            string moderatorId,
            string subject,
            string description,
            string exclusiveOwner)
        {
            var creator = this.collaboratorService.GetCreatorFrom(tenant, creatorId);

            var moderator = this.collaboratorService.GetModeratorFrom(tenant, moderatorId);

            var newForum = new フォーラム(
                        tenant,
                        this.forumRepository.GetNextIdentity(),
                        creator,
                        moderator,
                        subject,
                        description,
                        exclusiveOwner);

            this.forumRepository.Save(newForum);

            return newForum;
        }
    }
}
