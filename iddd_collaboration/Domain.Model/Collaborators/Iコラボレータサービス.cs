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

namespace SaaSOvation.Collaboration.Domain.Model.Collaborators
{
    using SaaSOvation.Collaboration.Domain.Model.Tenants;

    public interface Iコラボレータサービス
    {
        著者 GetAuthorFrom(テナント tenant, string identity);

        クリエイタ GetCreatorFrom(テナント tenant, string identity);

        モデレータ GetModeratorFrom(テナント tenant, string identity);

        オーナ GetOwnerFrom(テナント enant, string identity);

        参加者 GetParticipantFrom(テナント tenant, string identity);
    }
}
