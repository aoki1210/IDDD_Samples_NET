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
    using SaaSOvation.AgilePM.Domain.Model.Tenants;
    using SaaSOvation.Common.Domain.Model;
    using System;
    
    public class プロダクトディスカッション初期時 : IDomainEvent
    {
        public プロダクトディスカッション初期時(TenantId tenantId, プロダクトId productId, プロダクトディスカッション productDiscussion)
        {
            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
            this.ProductDiscussion = productDiscussion;
            this.ProductId = productId;
            this.TenantId = tenantId;
        }

        public int EventVersion { get; set; }

        public DateTime OccurredOn { get; set; }

        public プロダクトディスカッション ProductDiscussion { get; private set; }

        public プロダクトId ProductId { get; private set; }

        public TenantId TenantId { get; private set; }
    }
}
