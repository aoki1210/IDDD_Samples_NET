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

	using SaaSOvation.Common.Domain.Model;
	using SaaSOvation.IdentityAccess.Domain.Model.Identity;

	/// <summary>
	/// A domain service providing methods to determine
	/// whether a <see cref="ユーザー"/> has a <see cref="ロール"/>.
	/// </summary>
	[CLSCompliant(true)]
	public class 認証サービス
	{
		#region [ ReadOnly Fields and Constructor ]

		private readonly IユーザーRepository userRepository;
		private readonly IグループRepository groupRepository;
		private readonly IロールRepository roleRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="認証サービス"/> class.
		/// </summary>
		/// <param name="userRepository">
		/// An instance of <see cref="IユーザーRepository"/> to use internally.
		/// </param>
		/// <param name="groupRepository">
		/// An instance of <see cref="IグループRepository"/> to use internally.
		/// </param>
		/// <param name="roleRepository">
		/// An instance of <see cref="IロールRepository"/> to use internally.
		/// </param>
		public 認証サービス(
			IユーザーRepository userRepository,
			IグループRepository groupRepository,
			IロールRepository roleRepository)
		{
			this.groupRepository = groupRepository;
			this.roleRepository = roleRepository;
			this.userRepository = userRepository;
		}

		#endregion

		/// <summary>
		/// Determines whether a <see cref="ユーザー"/> has a <see cref="ロール"/>,
		/// given the names of the user and the role.
		/// </summary>
		/// <param name="tenantId">
		/// A <see cref="テナントId"/> identifying a <see cref="テナント"/> with
		/// which a <see cref="ユーザー"/> and <see cref="ロール"/> are associated.
		/// </param>
		/// <param name="username">
		/// The unique username identifying a <see cref="ユーザー"/>.
		/// </param>
		/// <param name="roleName">
		/// The unique name identifying a <see cref="ロール"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the <see cref="ユーザー"/> has the
		/// <see cref="ロール"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsUserInRole(テナントId tenantId, string username, string roleName)
		{
			AssertionConcern.AssertArgumentNotNull(tenantId, "TenantId must not be null.");
			AssertionConcern.AssertArgumentNotEmpty(username, "Username must not be provided.");
			AssertionConcern.AssertArgumentNotEmpty(roleName, "Role name must not be null.");

			ユーザー user = this.userRepository.UserWithUsername(tenantId, username);
			return ((user != null) && this.IsUserInRole(user, roleName));
		}

		/// <summary>
		/// Determines whether a <see cref="ユーザー"/> has a <see cref="ロール"/>,
		/// given the user and the name of the role.
		/// </summary>
		/// <param name="user">
		/// A <see cref="ユーザー"/> instance.
		/// </param>
		/// <param name="roleName">
		/// The unique name identifying a <see cref="ロール"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the <see cref="ユーザー"/> has the
		/// <see cref="ロール"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsUserInRole(ユーザー user, string roleName)
		{
			AssertionConcern.AssertArgumentNotNull(user, "User must not be null.");
			AssertionConcern.AssertArgumentNotEmpty(roleName, "Role name must not be null.");

			bool authorized = false;
			if (user.IsEnabled)
			{
				ロール role = this.roleRepository.RoleNamed(user.TenantId, roleName);
				if (role != null)
				{
					authorized = role.IsInRole(user, new グループメンバーサービス(this.userRepository, this.groupRepository));
				}
			}

			return authorized;
		}
	}
}