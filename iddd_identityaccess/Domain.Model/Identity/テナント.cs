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
	using System.Linq;

	using SaaSOvation.Common.Domain.Model;
	using SaaSOvation.IdentityAccess.Domain.Model.Access;

	/// <summary>
	/// An entity representing a tenant in a multi-tenant
	/// "Identity and Access" Bounded Context.
	/// </summary>
	/// <remarks>
	/// <see cref="人"/>, <see cref="ユーザー"/>, <see cref="グループ"/>,
	/// and <see cref="ロール"/> entities are each bound to a single tenant.
	/// </remarks>
	[CLSCompliant(true)]
	public class テナント : EntityWithCompositeId
	{
		#region [ Fields and Constructor Overloads ]

		private readonly ISet<レジストレーション招待> registrationInvitations;

		/// <summary>
		/// Initializes a new instance of the <see cref="テナント"/> class.
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
		/// <param name="active">
		/// Initial value of the <see cref="Active"/> property.
		/// </param>
		public テナント(テナントId tenantId, string name, string description, bool active)
		{
			AssertionConcern.AssertArgumentNotNull(tenantId, "TenentId is required.");
			AssertionConcern.AssertArgumentNotEmpty(name, "The tenant name is required.");
			AssertionConcern.AssertArgumentLength(name, 1, 100, "The name must be 100 characters or less.");
			AssertionConcern.AssertArgumentNotEmpty(description, "The tenant description is required.");
			AssertionConcern.AssertArgumentLength(description, 1, 100, "The name description be 100 characters or less.");

			this.TenantId = tenantId;
			this.Name = name;
			this.Description = description;
			this.Active = active;

			this.registrationInvitations = new HashSet<レジストレーション招待>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="テナント"/> class for a derived type,
		/// and otherwise blocks new instances from being created with an empty constructor.
		/// </summary>
		protected テナント()
		{
		}

		#endregion

		#region [ Public Properties ]

		public テナントId TenantId { get; private set; }

		public string Name { get; private set; }

		public bool Active { get; private set; }

		public string Description { get; private set; }

		#endregion

		#region [ Command Methods which Publish Domain Events ]

		public void Activate()
		{
			if (!this.Active)
			{
				this.Active = true;
				DomainEventPublisher.Instance.Publish(new テナントアクティベート時(this.TenantId));
			}
		}

		public void Deactivate()
		{
			if (this.Active)
			{
				this.Active = false;

				DomainEventPublisher.Instance.Publish(new テナントディアクティベート時(this.TenantId));
			}
		}

		public bool IsRegistrationAvailableThrough(string invitationIdentifier)
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");

			レジストレーション招待 invitation = this.GetInvitation(invitationIdentifier);
	
			return ((invitation != null) && invitation.IsAvailable());
		}

		public レジストレーション招待 OfferRegistrationInvitation(string description)
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");
			AssertionConcern.AssertArgumentTrue(this.IsRegistrationAvailableThrough(description), "Invitation already exists.");

			レジストレーション招待 invitation = new レジストレーション招待(this.TenantId, new Guid().ToString(), description);

			AssertionConcern.AssertStateTrue(this.registrationInvitations.Add(invitation), "The invitation should have been added.");

			return invitation;
		}

		public グループ ProvisionGroup(string name, string description)
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");

			グループ group = new グループ(this.TenantId, name, description);

			DomainEventPublisher.Instance.Publish(new グループプロビジョン時(this.TenantId, name));

			return group;
		}

		public ロール ProvisionRole(string name, string description, bool supportsNesting = false)
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");

			ロール role = new ロール(this.TenantId, name, description, supportsNesting);

			DomainEventPublisher.Instance.Publish(new ロールプロビジョン時(this.TenantId, name));

			return role;
		}

		public レジストレーション招待 RedefineRegistrationInvitationAs(string invitationIdentifier)
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");

			レジストレーション招待 invitation = this.GetInvitation(invitationIdentifier);
			if (invitation != null)
			{
				invitation.RedefineAs().OpenEnded();
			}

			return invitation;
		}

		public ユーザー RegisterUser(string invitationIdentifier, string username, string password, 有効化 enablement, 人 person)
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");

			ユーザー user = null;
			if (this.IsRegistrationAvailableThrough(invitationIdentifier))
			{
				// ensure same tenant
				person.TenantId = this.TenantId;
				user = new ユーザー(this.TenantId, username, password, enablement, person);
			}

			return user;
		}

		public void WithdrawInvitation(string invitationIdentifier)
		{
			レジストレーション招待 invitation = this.GetInvitation(invitationIdentifier);
			if (invitation != null)
			{
				this.registrationInvitations.Remove(invitation);
			}
		}

		#endregion

		#region [ Additional Methods ]

		public ICollection<招待記述子> AllAvailableRegistrationInvitations()
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");

			return this.AllRegistrationInvitationsFor(true);
		}

		public ICollection<招待記述子> AllUnavailableRegistrationInvitations()
		{
			AssertionConcern.AssertStateTrue(this.Active, "Tenant is not active.");

			return this.AllRegistrationInvitationsFor(false);
		}

		/// <summary>
		/// Returns a string that represents the current entity.
		/// </summary>
		/// <returns>
		/// A unique string representation of an instance of this entity.
		/// </returns>
		public override string ToString()
		{
			const string Format = "Tenant [tenantId={0}, name={1}, description={2}, active={3}]";
			return string.Format(Format, this.TenantId, this.Name, this.Description, this.Active);
		}

		/// <summary>
		/// Gets the values which identify a <see cref="テナント"/> entity,
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

		private List<招待記述子> AllRegistrationInvitationsFor(bool isAvailable)
		{
			return this.registrationInvitations
				.Where(x => (x.IsAvailable() == isAvailable))
				.Select(x => x.ToDescriptor())
				.ToList();
		}

		private レジストレーション招待 GetInvitation(string invitationIdentifier)
		{
			return this.registrationInvitations.FirstOrDefault(x => x.IsIdentifiedBy(invitationIdentifier));
		}

		#endregion
	}
}