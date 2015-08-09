using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems;

namespace SaaSOvation.AgilePM.Application.Products.BacklogItems
{
    public class バックログアイテムアプリケーションサービス
    {
        public バックログアイテムアプリケーションサービス(IバックログアイテムRepository backlogItemRepository)
        {
            this.backlogItemRepository = backlogItemRepository;
        }

        readonly IバックログアイテムRepository backlogItemRepository;

        // TODO: APIs for student assignment
    }
}
