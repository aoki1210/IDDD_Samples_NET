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

namespace SaaSOvation.IdentityAccess.Domain.Model.Access
{
	using System;
	using System.Collections.Generic;

	using SaaSOvation.Common.Domain.Model;
	using SaaSOvation.IdentityAccess.Domain.Model.Identity;

	/// <summary>
	/// An entity representing an authentication role for a
	/// particular <see cref="テナント"/>, which determines
	/// the types of access granted or denied to a
	/// <see cref="ユーザー"/> or <see cref="グループ"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class borrows functionality from an internal
	/// instance of <see cref="グループ"/> to be able to
	/// assign users and groups to this role.
	/// </para>
	/// <para>
	/// A role might also be called a "claim" in some
	/// authentication approaches.
	/// </para>
	/// </remarks>
	[CLSCompliant(true)]
	public class ロール : EntityWithCompositeId
	{
		#region [ Fields and Constructor Overloads ]

		/// <summary>
		/// An internal instance of <see cref="グループ"/>
		/// which provides functionality for assigning
		/// users and groups to this role.
		/// </summary>
		private readonly グループ internalGroup;

		/// <summary>
		/// Initializes a new instance of the <see cref="ロール"/> class.
		/// </summary>
		/// <param name="tenantId">
		/// Initial value of the <see cref="TenantId"/> property.
		/// </param>
		/// <param name="name">
		/// Initial value of the <see cref="Name"/> property.
		/// </param>
		/// <param name="description">
		/// Initial value of the <see cref="Description"/> property.
		/// </param>
		/// <param name="supportsNesting">
		/// Initial value of the <see cref="SupportsNesting"/> property.
		/// </param>
		public ロール(テナントId tenantId, string name, string description, bool supportsNesting)
		{
			// Defer validation to the property setters.
			this.Description = description;
			this.Name = name;
			this.SupportsNesting = supportsNesting;
			this.TenantId = tenantId;

			this.internalGroup = this.CreateInternalGroup();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ロール"/> class for a derived type,
		/// and otherwise blocks new instances from being created with an empty constructor.
		/// </summary>
		protected ロール()
		{
		}

		#endregion

		#region [ Public Properties ]

		// The "Id" property is unused.
		/*
			/// <summary>
			/// Gets or sets an integer.
			/// </summary>
			public int Id { get; set; }
		*/

		public string Description { get; private set; }

		public string Name { get; private set; }

		public bool SupportsNesting { get; private set; }

		public テナントId TenantId { get; private set; }

		#endregion

		#region [ Command Methods which Publish Domain Events ]

		public void AssignGroup(グループ group, グループメンバーサービス groupMemberService)
		{
			AssertionConcern.AssertStateTrue(this.SupportsNesting, "This role does not support group nesting.");
			AssertionConcern.AssertArgumentNotNull(group, "Group must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, group.TenantId, "Wrong tenant for this group.");

			this.internalGroup.AddGroup(group, groupMemberService);

			DomainEventPublisher
				.Instance
				.Publish(new グループロールアサイン時(
						this.TenantId,
						this.Name,
						group.Name));
		}

		public void AssignUser(ユーザー user)
		{
			AssertionConcern.AssertArgumentNotNull(user, "User must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, user.TenantId, "Wrong tenant for this user.");

			this.internalGroup.AddUser(user);

			DomainEventPublisher
				.Instance
				.Publish(new UserAssignedToRole(
						this.TenantId,
						this.Name,
						user.Username,
						user.Person.Name.FirstName,
						user.Person.Name.LastName,
						user.Person.EmailAddress.Address));
		}

		public void UnassignGroup(グループ group)
		{
			AssertionConcern.AssertStateTrue(this.SupportsNesting, "This role does not support group nesting.");
			AssertionConcern.AssertArgumentNotNull(group, "Group must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, group.TenantId, "Wrong tenant for this group.");

			this.internalGroup.RemoveGroup(group);

			DomainEventPublisher
				.Instance
				.Publish(new グループロールアサイン解除時(
						this.TenantId,
						this.Name,
						group.Name));
		}

		public void UnassignUser(ユーザー user)
		{
			AssertionConcern.AssertArgumentNotNull(user, "User must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, user.TenantId, "Wrong tenant for this user.");

			this.internalGroup.RemoveUser(user);

			DomainEventPublisher
				.Instance
				.Publish(new ユーザーロールアサイン解除時(
						this.TenantId,
						this.Name,
						user.Username));
		}

		#endregion

		#region [ Additional Methods ]

		/// <summary>
		/// Uses a <see cref="グループメンバーサービス"/> to determine
		/// whether a given <see cref="ユーザー"/> has this role, including
		/// by way of nested <see cref="グループ"/> membership.
		/// </summary>
		/// <param name="user">
		/// A <see cref="ユーザー"/> entity to check.
		/// </param>
		/// <param name="groupMemberService">
		/// The instance of <see cref="グループメンバーサービス"/>
		/// relayed to the <see cref="グループ.IsMember"/> method.
		/// </param>
		/// <returns>
		/// <c>true</c> if the given <paramref name="user"/>
		/// has this role; otherwise, <c>false</c>.
		/// </returns>
		public bool IsInRole(ユーザー user, グループメンバーサービス groupMemberService)
		{
			return this.internalGroup.IsMember(user, groupMemberService);
		}

		/// <summary>
		/// Returns a string that represents the current entity.
		/// </summary>
		/// <returns>
		/// A unique string representation of an instance of this entity.
		/// </returns>
		public override string ToString()
		{
			const string Format = "Role [tenantId={0}, name={1}, description={2}, supportsNesting={3}, group={4}]";
			return string.Format(Format, this.TenantId, this.Name, this.Description, this.SupportsNesting, this.internalGroup);
		}

		/// <summary>
		/// Gets the values which identify a <see cref="ロール"/> entity,
		/// which are the <see cref="TenantId"/> and the <see cref="Name"/>.
		/// </summary>
		/// <returns>
		/// A sequence of values which uniquely identifies an instance of this entity.
		/// </returns>
		protected override IEnumerable<object> GetIdentityComponents()
		{
			yield return this.TenantId;
			yield return this.Name;
		}

		private グループ CreateInternalGroup()
		{
			string groupName = string.Concat(グループ.RoleGroupPrefix, Guid.NewGuid());
			return new グループ(this.TenantId, groupName, string.Concat("Role backing group for: ", this.Name));
		}

		#endregion
	}
}
