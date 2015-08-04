﻿// Copyright 2012,2013 Vaughn Vernon
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
    using SaaSOvation.AgilePM.Domain.Model.Products.Releases;
    using SaaSOvation.AgilePM.Domain.Model.Tenants;
    using SaaSOvation.Common.Domain.Model;
    using SaaSOvation.AgilePM.Domain.Model.Products.Sprints;

    public class プロダクトスプリントスケジュール時 : IDomainEvent
    {
        public プロダクトスプリントスケジュール時(
            テナントId tenantId,
            プロダクトId productId,
            スプリントId sprintId,
            string name,
            string goals,
            DateTime starts,
            DateTime ends)
        {
            this.Ends = ends;
            this.EventVersion = 1;
            this.Goals = goals;
            this.Name = name;
            this.OccurredOn = DateTime.Now;
            this.ProductId = productId;
            this.SprintId = sprintId;
            this.Starts = starts;
            this.TenantId = tenantId;
        }

        public DateTime Ends { get; private set; }

        public int EventVersion { get; set; }

        public string Goals { get; private set; }

        public string Name { get; private set; }

        public DateTime OccurredOn { get; set; }

        public プロダクトId ProductId { get; private set; }

        public スプリントId SprintId { get; private set; }

        public DateTime Starts { get; private set; }

        public テナントId TenantId { get; private set; }
    }
}
