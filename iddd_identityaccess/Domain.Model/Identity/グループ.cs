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
	using System.Collections.Generic;

	using SaaSOvation.Common.Domain.Model;

	/// <summary>
	/// An entity representing a group of users
	/// and possibly of other groups for a
	/// particular <see cref="テナント"/>.
	/// </summary>
	[CLSCompliant(true)]
	public class グループ : EntityWithCompositeId
	{
		#region [ Fields and Constructor Overloads ]

		/// <summary>
		/// String constant with a prefix used by the private
		/// <see cref="IsInternalGroup"/> property to determine
		/// whether a <see cref="グループ"/> is used only as an
		/// internal member of a <see cref="Domain.Model.Access.ロール"/>.
		/// </summary>
		internal const string RoleGroupPrefix = "ROLE-INTERNAL-GROUP: ";

		/// <summary>
		/// Initializes a new instance of the <see cref="グループ"/> class.
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
		public グループ(テナントId tenantId, string name, string description)
			: this()
		{
			// Defer validation to the property setters.
			this.Description = description;
			this.Name = name;
			this.TenantId = tenantId;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="グループ"/> class for a derived type,
		/// and otherwise blocks new instances from being created with an empty constructor.
		/// </summary>
		protected グループ()
		{
			this.GroupMembers = new HashSet<グループメンバー>();
		}

		#endregion

		#region [ Public Properties and Private IsInternalGroup Property ]

		public テナントId TenantId { get; private set; }

		public string Name { get; private set; }

		public string Description { get; private set; }

		public ISet<グループメンバー> GroupMembers { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this group exists only
		/// to manage the membership in an authentication
		/// <see cref="Domain.Model.Access.ロール"/>.
		/// If <c>true</c>, this group should not be
		/// shown on any lists of groups.
		/// </summary>
		private bool IsInternalGroup
		{
			get { return this.Name.StartsWith(RoleGroupPrefix, StringComparison.Ordinal); }
		}

		#endregion

		#region [ Command Methods which Publish Domain Events ]

		public void AddGroup(グループ group, グループメンバーサービス groupMemberService)
		{
			AssertionConcern.AssertArgumentNotNull(group, "Group must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, group.TenantId, "Wrong tenant for this group.");
			AssertionConcern.AssertArgumentFalse(groupMemberService.IsMemberGroup(group, this.ToGroupMember()), "Group recurrsion.");

			if (this.GroupMembers.Add(group.ToGroupMember()) && (!this.IsInternalGroup))
			{
				DomainEventPublisher
					.Instance
					.Publish(new グループグループ追加時(
							this.TenantId,
							this.Name,
							group.Name));
			}
		}

		public void AddUser(ユーザー user)
		{
			AssertionConcern.AssertArgumentNotNull(user, "User must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, user.TenantId, "Wrong tenant for this group.");
			AssertionConcern.AssertArgumentTrue(user.IsEnabled, "User is not enabled.");

			if (this.GroupMembers.Add(user.ToGroupMember()) && (!this.IsInternalGroup))
			{
				DomainEventPublisher
					.Instance
					.Publish(new グループユーザー追加時(
							this.TenantId,
							this.Name,
							user.Username));
			}
		}

		public void RemoveGroup(グループ group)
		{
			AssertionConcern.AssertArgumentNotNull(group, "Group must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, group.TenantId, "Wrong tenant for this group.");

			// not a nested remove, only direct member
			if (this.GroupMembers.Remove(group.ToGroupMember()) && (!this.IsInternalGroup))
			{
				DomainEventPublisher
					.Instance
					.Publish(new グループグループ削除時(
							this.TenantId,
							this.Name,
							group.Name));
			}
		}

		public void RemoveUser(ユーザー user)
		{
			AssertionConcern.AssertArgumentNotNull(user, "User must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, user.TenantId, "Wrong tenant for this group.");

			// not a nested remove, only direct member
			if (this.GroupMembers.Remove(user.ToGroupMember()) && (!this.IsInternalGroup))
			{
				DomainEventPublisher
					.Instance
					.Publish(new グループユーザー削除時(
							this.TenantId,
							this.Name,
							user.Username));
			}
		}

		#endregion

		#region [ Additional Methods ]

		/// <summary>
		/// Uses a <see cref="グループメンバーサービス"/> to determine
		/// whether a given <see cref="ユーザー"/> is a member of this
		/// or of a nested <see cref="グループ"/>.
		/// </summary>
		/// <param name="user">
		/// A <see cref="ユーザー"/> entity to check.
		/// </param>
		/// <param name="groupMemberService">
		/// The instance of <see cref="グループメンバーサービス"/>
		/// to use for checking nested group membership.
		/// </param>
		/// <returns>
		/// <c>true</c> if the given <paramref name="user"/>
		/// is a member of this group or of a nested group;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool IsMember(ユーザー user, グループメンバーサービス groupMemberService)
		{
			AssertionConcern.AssertArgumentNotNull(user, "User must not be null.");
			AssertionConcern.AssertArgumentEquals(this.TenantId, user.TenantId, "Wrong tenant for this group.");
			AssertionConcern.AssertArgumentTrue(user.IsEnabled, "User is not enabled.");

			bool isMember = this.GroupMembers.Contains(user.ToGroupMember());
			if (isMember)
			{
				isMember = groupMemberService.ConfirmUser(this, user);
			}
			else
			{
				isMember = groupMemberService.IsUserInNestedGroup(this, user);
			}

			return isMember;
		}

		/// <summary>
		/// Returns a string that represents the current entity.
		/// </summary>
		/// <returns>
		/// A unique string representation of an instance of this entity.
		/// </returns>
		public override string ToString()
		{
			const string Format = "Group [tenantId={0}, name={1}, description={2}]";
			return string.Format(Format, this.TenantId, this.Name, this.Description);
		}

		/// <summary>
		/// Creates a <see cref="グループメンバー"/> value of
		/// type <see cref="グループメンバータイプ.Group"/>
		/// based on this <see cref="グループ"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="グループメンバー"/> value of type
		/// <see cref="グループメンバータイプ.Group"/>
		/// based on this <see cref="グループ"/>.
		/// </returns>
		internal グループメンバー ToGroupMember()
		{
			return new グループメンバー(this.TenantId, this.Name, グループメンバータイプ.Group);
		}

		/// <summary>
		/// Gets the values which identify a <see cref="グループ"/> entity,
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

		#endregion
	}
}