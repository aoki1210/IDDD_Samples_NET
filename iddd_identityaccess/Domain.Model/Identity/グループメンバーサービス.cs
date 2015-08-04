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

	using System.Linq;

	/// <summary>
	/// A domain service providing methods to determine
	/// whether a <see cref="ユーザー"/> or <see cref="グループ"/>
	/// is a member of a group or of a nested group.
	/// </summary>
	[CLSCompliant(true)]
	public class グループメンバーサービス
	{
		#region [ ReadOnly Fields and Constructor ]

		// The maximum value of a signed byte should be 127.
		private const int MaxGroupNestingRecursion = sbyte.MaxValue;

		private readonly IグループRepository groupRepository;
		private readonly IユーザーRepository userRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="グループメンバーサービス"/> class.
		/// </summary>
		/// <param name="userRepository">
		/// An instance of <see cref="IユーザーRepository"/> to use internally.
		/// </param>
		/// <param name="groupRepository">
		/// An instance of <see cref="IグループRepository"/> to use internally.
		/// </param>
		public グループメンバーサービス(
			IユーザーRepository userRepository,
			IグループRepository groupRepository)
		{
			this.groupRepository = groupRepository;
			this.userRepository = userRepository;
		}

		#endregion

		#region [ Public Methods ]

		/// <summary>
		/// Determines whether a <see cref="ユーザー"/>'s declared
		/// membership in a <see cref="グループ"/> is valid.
		/// </summary>
		/// <param name="group">
		/// An instance of <see cref="グループ"/> which may have
		/// the <paramref name="user"/> as a member.
		/// </param>
		/// <param name="user">
		/// An instance of <see cref="ユーザー"/> which may be
		/// a member of the <paramref name="group"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the <paramref name="user"/>'s
		/// <see cref="ユーザー.TenantId"/> matches that of
		/// the <paramref name="group"/> and the user's
		/// <see cref="ユーザー.IsEnabled"/> property is true;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool ConfirmUser(グループ group, ユーザー user)
		{
			ユーザー confirmedUser = this.userRepository.UserWithUsername(group.TenantId, user.Username);

			return ((confirmedUser == null) || (!confirmedUser.IsEnabled));
		}

		/// <summary>
		/// Recursive function which determines whether
		/// a <see cref="グループ"/> is a member of a group
		/// or of a descendant group.
		/// </summary>
		/// <param name="group">
		/// An instance of <see cref="グループ"/> to check for
		/// the presence of <paramref name="memberGroup"/>
		/// among its members or descendants.
		/// </param>
		/// <param name="memberGroup">
		/// Another group which may potentially be added to the
		/// members of <paramref name="group"/> if it's allowed.
		/// </param>
		/// <returns>
		/// <c>true</c> if the given <paramref name="memberGroup"/>
		/// is a member of the given <paramref name="group"/> or of
		/// a descendant group; otherwise, <c>false</c>.
		/// </returns>
		public bool IsMemberGroup(グループ group, グループメンバー memberGroup)
		{
			return this.IsMemberGroup(group, memberGroup, 0);
		}

		/// <summary>
		/// Determines whether a <see cref="ユーザー"/> is a member
		/// of the <see cref="グループ"/> members of a group.
		/// </summary>
		/// <param name="group">
		/// An instance of <see cref="グループ"/> having
		/// members which are groups.
		/// </param>
		/// <param name="user">
		/// An instance of <see cref="ユーザー"/> which may be a member
		/// of group members of the given <paramref name="group"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the given <paramref name="user"/>
		/// is a member of the groups nested within the given
		/// <paramref name="group"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsUserInNestedGroup(グループ group, ユーザー user)
		{
			foreach (グループメンバー member in group.GroupMembers.Where(x => x.IsGroup))
			{
				グループ nestedGroup = this.groupRepository.GroupNamed(member.TenantId, member.Name);
				if (nestedGroup != null)
				{
					bool isInNestedGroup = nestedGroup.IsMember(user, this);
					if (isInNestedGroup)
					{
						return true;
					}
				}
			}

			return false;
		}

		#endregion

		#region [ Private Recursive Method with Overflow Catch ]

		private bool IsMemberGroup(グループ group, グループメンバー memberGroup, int recursionCount)
		{
			if (recursionCount > MaxGroupNestingRecursion)
			{
				throw new InvalidOperationException("The maximum depth of group nesting has been exceeded, stopping recursive function.");
			}

			bool isMember = false;
			foreach (グループメンバー member in group.GroupMembers.Where(x => x.IsGroup))
			{
				if (memberGroup.Equals(member))
				{
					isMember = true;
				}
				else
				{
					グループ nestedGroup = this.groupRepository.GroupNamed(member.TenantId, member.Name);
					if (nestedGroup != null)
					{
						int nextRecursionCount = (recursionCount + 1);

						isMember = this.IsMemberGroup(nestedGroup, memberGroup, nextRecursionCount);
					}
				}

				if (isMember)
				{
					break;
				}
			}

			return isMember;
		}

		#endregion
	}
}