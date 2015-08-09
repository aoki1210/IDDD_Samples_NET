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

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
	using System;

	/// <summary>
	/// Contract for a collection-oriented repository of <see cref="テナント"/> entities.
	/// </summary>
	/// <remarks>
	/// Because this is a collection-oriented repository, the <see cref="Add"/>
	/// method needs to be called no more than once per stored entity.
	/// Subsequent changes to any stored <see cref="テナント"/> are implicitly
	/// persisted, and adding the same entity a second time will have no effect.
	/// </remarks>
	[CLSCompliant(true)]
	public interface IテナントRepository
	{
		/// <summary>
		/// Creates an identifier to use as the value of the
		/// <see cref="テナント.TenantId"/> property for
		/// a new instance of <see cref="テナント"/>
		/// before the entity is stored in the repository.
		/// </summary>
		/// <returns>
		/// A <see cref="テナントId"/> value to use to identify
		/// a new instance of <see cref="テナント"/>.
		/// </returns>
		テナントId GetNextIdentity();

		/// <summary>
		/// Removes a given <see cref="テナント"/> from the repository.
		/// </summary>
		/// <param name="tenant">
		/// The instance of <see cref="テナント"/> to remove.
		/// </param>
		void Remove(テナント tenant);

		/// <summary>
		/// Stores a given <see cref="テナント"/> in the repository.
		/// </summary>
		/// <param name="tenant">
		/// The instance of <see cref="テナント"/> to store.
		/// </param>
		/// <remarks>
		/// Because this is a collection-oriented repository, the <see cref="Add"/>
		/// method needs to be called no more than once per stored entity.
		/// Subsequent changes to any stored <see cref="テナント"/> are implicitly
		/// persisted, and adding the same entity a second time will have no effect.
		/// </remarks>
		void Add(テナント tenant);

		/// <summary>
		/// Retrieves a <see cref="テナント"/> from the repository,
		/// and implicitly persists any changes to the retrieved entity.
		/// </summary>
		/// <param name="tenantId">
		/// The identifier of a <see cref="テナント"/>.
		/// </param>
		/// <returns>
		/// The instance of <see cref="テナント"/> retrieved,
		/// or a null reference if no matching entity exists
		/// in the repository.
		/// </returns>
		テナント Get(テナントId tenantId);

		/// <summary>
		/// Retrieves a <see cref="テナント"/> from the repository
		/// based on its name, and implicitly persists any changes
		/// to the retrieved entity.
		/// </summary>
		/// <param name="name">
		/// The unique name of a <see cref="テナント"/>.
		/// </param>
		/// <returns>
		/// The instance of <see cref="テナント"/> retrieved,
		/// or a null reference if no matching entity exists
		/// in the repository.
		/// </returns>
		テナント GetByName(string name);
	}
}
