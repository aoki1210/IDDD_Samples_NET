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
	/// A value object, based on either <see cref="ユーザー"/> or <see cref="グループ"/>,
	/// which may be among the <see cref="グループ.GroupMembers"/> of a group.
	/// </summary>
	/// <remarks>
	/// The <see cref="ユーザー"/> and <see cref="グループ"/> entities include factory methods
	/// <see cref="ユーザー.ToGroupMember"/> and <see cref="グループ.ToGroupMember"/>,
	/// respectively, which are used to create instances of this value.
	/// </remarks>
	[CLSCompliant(true)]
	public class グループメンバー : ValueObject
	{
		#region [ Internal Constructor ]

		/// <summary>
		/// Initializes a new instance of the <see cref="グループメンバー"/> class,
		/// restricted to internal access.
		/// </summary>
		/// <param name="tenantId">
		/// Initial value of the <see cref="TenantId"/> property.
		/// </param>
		/// <param name="name">
		/// Initial value of the <see cref="Name"/> property.
		/// </param>
		/// <param name="type">
		/// Initial value of the <see cref="Type"/> property.
		/// </param>
		/// <remarks>
		/// This constructor is invoked by the <see cref="ユーザー.ToGroupMember"/>
		/// or <see cref="グループ.ToGroupMember"/> factory methods of
		/// <see cref="ユーザー"/> or <see cref="グループ"/>, respectively.
		/// </remarks>
		internal グループメンバー(テナントId tenantId, string name, グループメンバータイプ type)
		{
			AssertionConcern.AssertArgumentNotNull(tenantId, "The tenantId must be provided.");
			AssertionConcern.AssertArgumentNotEmpty(name, "Member name is required.");
			AssertionConcern.AssertArgumentLength(name, 1, 100, "Member name must be 100 characters or less.");

			this.Name = name;
			this.TenantId = tenantId;
			this.Type = type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="グループメンバー"/> class for a derived type,
		/// and otherwise blocks new instances from being created with an empty constructor.
		/// </summary>
		protected グループメンバー()
		{
		}

		#endregion

		#region [ Public Properties ]

		public テナントId TenantId { get; private set; }

		public string Name { get; private set; }

		public グループメンバータイプ Type { get; private set; }

		public bool IsGroup
		{
			get { return this.Type == グループメンバータイプ.Group; }
		}

		public bool IsUser
		{
			get { return this.Type == グループメンバータイプ.User; }
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Returns a string that represents the current value object.
		/// </summary>
		/// <returns>
		/// A unique string representation of an instance of this value object.
		/// </returns>
		public override string ToString()
		{
			const string Format = "GroupMember [tenantId={0}, name={1}, type={2:G}]";
			return string.Format(Format, this.TenantId, this.Name, this.Type);
		}

		/// <summary>
		/// Gets the values which define one <see cref="グループメンバー"/> value
		/// as compared to another, which are the <see cref="TenantId"/>,
		/// and <see cref="Name"/>, and <see cref="Type"/>.
		/// </summary>
		/// <returns>
		/// A sequence of values which defines this value object.
		/// </returns>
		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return this.TenantId;
			yield return this.Name;
			yield return this.Type;
		}

		#endregion
	}
}