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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.AgilePM.Domain.Model.Tenants;

namespace SaaSOvation.AgilePM.Domain.Model.Products.Sprints
{
    public interface IスプリントRepository
    {
        スプリント Get(テナントId tenantId, スプリントId sprintId);

        ICollection<スプリント> GetAllProductSprints(テナントId tenantId, プロダクトId productId);

        スプリントId GetNextIdentity();

        void Remove(スプリント sprint);

        void RemoveAll(IEnumerable<スプリント> sprints);

        void Save(スプリント sprint);

        void SaveAll(IEnumerable<スプリント> sprints);
    }
}
