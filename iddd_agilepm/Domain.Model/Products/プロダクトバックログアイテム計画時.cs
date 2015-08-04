// Copyright 2012,2013 Vaughn Vernon
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SaaSOvation.AgilePM.Domain.Model.Products
{
    using System;
    using SaaSOvation.Common.Domain.Model;
    using SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems;
    using SaaSOvation.AgilePM.Domain.Model.Tenants;

    public class プロダクトバックログアイテム計画時 : IDomainEvent
    {
        public プロダクトバックログアイテム計画時(
            テナントId tenantId,
            プロダクトId productId,
            バックログアイテムId backlogItemId,
            string summary,
            string category,
            BacklogItems.バックログアイテムタイプ backlogItemType,
            BacklogItems.ストーリポイント storyPoints)
        {
            this.BacklogItemId = backlogItemId;
            this.Category = category;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
            this.ProductId = productId;
            this.StoryPoints = storyPoints;
            this.Summary = summary;
            this.TenantId = tenantId;
            this.Type = backlogItemType;
        }

        public バックログアイテムId BacklogItemId { get; private set; }

        public string Category { get; private set; }

        public int EventVersion { get; set; }

        public DateTime OccurredOn { get; set; }

        public プロダクトId ProductId { get; private set; }

        public ストーリポイント StoryPoints { get; private set; }

        public string Summary { get; private set; }

        public テナントId TenantId { get; private set; }

        public バックログアイテムタイプ Type { get; private set; }
    }
}
