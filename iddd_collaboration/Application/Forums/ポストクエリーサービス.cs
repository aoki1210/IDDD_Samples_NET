using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Port.Adapters.Persistence;
using SaaSOvation.Collaboration.Application.Forums.Data;

namespace SaaSOvation.Collaboration.Application.Forums
{
    public class ポストクエリーサービス : AbstractQueryService
    {
        public ポストクエリーサービス(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public IList<ポストデータ> GetAllPostsDataByDiscussion(string tenantId, string discussionId)
        {
            return QueryObjects<ポストデータ>(
                    "select * from tbl_vw_post where tenant_id = ? and discussion_id = ?",
                    new JoinOn(),
                    tenantId,
                    discussionId);
        }

        public ポストデータ GetPostDataById(string tenantId, string postId)
        {
            return QueryObject<ポストデータ>(
                    "select * from tbl_vw_post where tenant_id = ? and post_id = ?",
                    new JoinOn(),
                    tenantId,
                    postId);
        }
    }
}
