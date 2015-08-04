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

namespace SaaSOvation.AgilePM.Domain.Model.Products.Releases
{
    using System;
    using SaaSOvation.AgilePM.Domain.Model.Tenants;
    using SaaSOvation.Common.Domain.Model;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class リリース : Entity
    {
        public リリース(
            テナントId tenantId,
            プロダクトId productId,
            リリースId releaseId,
            string name,
            string description,
            DateTime begins,
            DateTime ends)
        {
            if (ends.Ticks < begins.Ticks)
                throw new InvalidOperationException("Release must not end before it begins.");
            
            this.Begins = begins;
            this.Description = description;
            this.Ends = ends;
            this.Name = name;
            this.ProductId = productId;
            this.ReleaseId = releaseId;
            this.TenantId = tenantId;
            this.backlogItems = new HashSet<計画済みバックログアイテム>();
        }

        public プロダクトId ProductId { get; private set; }

        public リリースId ReleaseId { get; private set; }

        public テナントId TenantId { get; private set; }


        public DateTime Begins { get; private set; }

        public string Description { get; private set; }

        public DateTime Ends { get; private set; }

        public string Name { get; private set; }        

        readonly ISet<計画済みバックログアイテム> backlogItems;

        public ICollection<計画済みバックログアイテム> AllScheduledBacklogItems()
        {
            return new ReadOnlyCollection<計画済みバックログアイテム>(new List<計画済みバックログアイテム>(this.backlogItems));
        }
    }
}
