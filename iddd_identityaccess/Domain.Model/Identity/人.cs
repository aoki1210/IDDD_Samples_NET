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
	/// An entity representing a person associated with
	/// a <see cref="Domain.Model.Identity.ユーザー"/>, and
	/// identified by that <see cref="User"/>'s
	/// <see cref="Domain.Model.Identity.ユーザー.Username"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This entity is not an aggregate, but is used to compose
	/// the <see cref="Domain.Model.Identity.ユーザー"/> aggregate.
	/// Is it assumed that all users are persons,
	/// and that there are not other kinds of users?
	/// </para>
	/// <para>
	/// Because a <see cref="人"/> instance is identified
	/// by the <see cref="Domain.Model.Identity.ユーザー.Username"/>
	/// of the associated <see cref="User"/>, no more than
	/// one person can be associated with a single user.
	/// </para>
	/// </remarks>
	[CLSCompliant(true)]
	public class 人 : EntityWithCompositeId
	{
		#region [ Fields and Constructor Overloads ]

		private テナントId tenantId;
		private フルネーム name;
		private コンタクト情報 contactInformation;

		/// <summary>
		/// Initializes a new instance of the <see cref="人"/> class.
		/// </summary>
		/// <param name="tenantId">
		/// Initial value of the <see cref="TenantId"/> property.
		/// </param>
		/// <param name="name">
		/// Initial value of the <see cref="Name"/> property.
		/// </param>
		/// <param name="contactInformation">
		/// Initial value of the <see cref="ContactInformation"/> property.
		/// </param>
		public 人(テナントId tenantId, フルネーム name, コンタクト情報 contactInformation)
		{
			// Defer validation to the property setters.
			this.ContactInformation = contactInformation;
			this.Name = name;
			this.TenantId = tenantId;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="人"/> class for a derived type,
		/// and otherwise blocks new instances from being created with an empty constructor.
		/// </summary>
		protected 人()
		{
		}

		#endregion

		#region [ Public Properties ]

		public テナントId TenantId
		{
			get
			{
				return this.tenantId;
			}

			internal set
			{
				AssertionConcern.AssertArgumentNotNull(value, "The tenantId is required.");
				this.tenantId = value;
			}
		}

		public フルネーム Name
		{
			get
			{
				return this.name;
			}

			private set
			{
				AssertionConcern.AssertArgumentNotNull(value, "The person name is required.");
				this.name = value;
			}
		}

		public ユーザー User { get; internal set; }

		public コンタクト情報 ContactInformation
		{
			get
			{
				return this.contactInformation;
			}

			private set
			{
				AssertionConcern.AssertArgumentNotNull(value, "The person contact information is required.");
				this.contactInformation = value;
			}
		}

		public Emailアドレス EmailAddress
		{
			get { return this.ContactInformation.EmailAddress; }
		}

		#endregion

		#region [ Command Methods which Publish Domain Events ]

		public void ChangeContactInformation(コンタクト情報 newContactInformation)
		{
			// Defer validation to the property setter.
			this.ContactInformation = newContactInformation;

			DomainEventPublisher
				.Instance
				.Publish(new 人コンタクト情報変更時(
						this.TenantId,
						this.User.Username,
						this.ContactInformation));
		}

		public void ChangeName(フルネーム newName)
		{
			// Defer validation to the property setter.
			this.Name = newName;

			DomainEventPublisher
				.Instance
				.Publish(new 人名変更時(
						this.TenantId,
						this.User.Username,
						this.Name));
		}

		#endregion

		#region [ Additional Methods ]

		/// <summary>
		/// Returns a string that represents the current entity.
		/// </summary>
		/// <returns>
		/// A unique string representation of an instance of this entity.
		/// </returns>
		public override string ToString()
		{
			const string Format = "Person [tenantId={0}, name={1}, contactInformation={2}]";
			return string.Format(Format, this.TenantId, this.Name, this.ContactInformation);
		}

		/// <summary>
		/// Gets the values which identify a <see cref="人"/> entity,
		/// which are the <see cref="TenantId"/> and the unique
		/// <see cref="Domain.Model.Identity.ユーザー.Username"/>
		/// of the associated <see cref="User"/>.
		/// </summary>
		/// <returns>
		/// A sequence of values which uniquely identifies an instance of this entity.
		/// </returns>
		protected override IEnumerable<object> GetIdentityComponents()
		{
			yield return this.TenantId;
			yield return this.User.Username;
		}

		#endregion
	}
}