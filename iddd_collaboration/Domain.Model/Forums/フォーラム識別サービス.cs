using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public class フォーラム識別サービス
    {
        public フォーラム識別サービス(IディスカッションRepository discussionRepository, IフォーラムRepository forumRepository, IポストRepository postRepository)
        {
            this.discussionRepository = discussionRepository;
            this.forumRepository = forumRepository;
            this.postRepository = postRepository;
        }

        readonly IディスカッションRepository discussionRepository;
        readonly IフォーラムRepository forumRepository;
        readonly IポストRepository postRepository;

        public ディスカッションId GetNextDiscussionId()
        {
            return this.discussionRepository.GetNextIdentity();
        }

        public フォーラムId GetNextForumId()
        {
            return this.forumRepository.GetNextIdentity();
        }

        public ポストId GetNexPostId()
        {
            return this.postRepository.GetNextIdentity();
        }
    }
}
