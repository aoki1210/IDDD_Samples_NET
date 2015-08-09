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
    public class ディスカッションアプリケーションサービス
    {
        public ディスカッションアプリケーションサービス(
            IディスカッションRepository discussionRepository,
            フォーラム識別サービス forumIdentityService,
            IポストRepository postRepository,
            Iコラボレータサービス collaboratorService)
        {
            this.discussionRepository = discussionRepository;
            this.forumIdentityService = forumIdentityService;
            this.postRepository = postRepository;
            this.collaboratorService = collaboratorService;
        }

        readonly Iコラボレータサービス collaboratorService;
        readonly IディスカッションRepository discussionRepository;
        readonly フォーラム識別サービス forumIdentityService;
        readonly IポストRepository postRepository;

        public void CloseDiscussion(string tenantId, string discussionId)
        {
            var discussion = this.discussionRepository.Get(new テナント(tenantId), new ディスカッションId(discussionId));

            discussion.Close();

            this.discussionRepository.Save(discussion);
        }

        public void PostToDiscussion(string tenantId, string discussionId, string authorId, string subject, string bodyText, Iディスカッションコマンド結果 discussionCommandResult)
        {
            var discussion = this.discussionRepository.Get(new テナント(tenantId), new ディスカッションId(discussionId));

            var author = this.collaboratorService.GetAuthorFrom(new テナント(tenantId), authorId);

            var post = discussion.Post(this.forumIdentityService, author, subject, bodyText);

            this.postRepository.Save(post);

            discussionCommandResult.SetResultingDiscussionId(discussionId);
            discussionCommandResult.SetResultingPostId(post.PostId.Id);
        }

        public void PostToDiscussionInReplyTo(string tenantId, string discussionId, string replyToPostId, string authorId,
            string subject, string bodyText, Iディスカッションコマンド結果 discussionCommandResult)
        {
            var discussion = this.discussionRepository.Get(new テナント(tenantId), new ディスカッションId(discussionId));

            var author = this.collaboratorService.GetAuthorFrom(new テナント(tenantId), authorId);

            var post = discussion.Post(this.forumIdentityService, author, subject, bodyText, new ポストId(replyToPostId));

            this.postRepository.Save(post);

            discussionCommandResult.SetResultingDiscussionId(discussionId);
            discussionCommandResult.SetResultingPostId(post.PostId.Id);
            discussionCommandResult.SetRresultingInReplyToPostId(replyToPostId);
        }

        public void ReOpenDiscussion(string tenantId, string discussionId)
        {
            var discussion = this.discussionRepository.Get(new テナント(tenantId), new ディスカッションId(discussionId));

            discussion.ReOpen();

            this.discussionRepository.Save(discussion);
        }
    }
}
