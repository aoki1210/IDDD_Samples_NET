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
    public class ポストアプリケーションサービス
    {
        public ポストアプリケーションサービス(IポストRepository postRepository, IフォーラムRepository forumRepository, Iコラボレータサービス collaboratorService)
        {
            this.postRepository = postRepository;
            this.forumRepository = forumRepository;
            this.collaboratorService = collaboratorService;
        }

        readonly IポストRepository postRepository;
        readonly IフォーラムRepository forumRepository;
        readonly Iコラボレータサービス collaboratorService;

        public void ModeratePost(
            string tenantId,
            string forumId,
            string postId,
            string moderatorId,
            string subject,
            string bodyText)
        {
            var tenant = new テナント(tenantId);

            var forum = this.forumRepository.Get(tenant, new フォーラムId(forumId));

            var moderator = this.collaboratorService.GetModeratorFrom(tenant, moderatorId);

            var post = this.postRepository.Get(tenant, new ポストId(postId));

            forum.ModeratePost(post, moderator, subject, bodyText);

            this.postRepository.Save(post);
        }
    }
}
