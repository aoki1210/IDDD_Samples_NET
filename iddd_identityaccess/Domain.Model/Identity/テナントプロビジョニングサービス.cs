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

	using SaaSOvation.Common.Domain.Model;
	using SaaSOvation.IdentityAccess.Domain.Model.Access;

	/// <summary>
	/// <para>
	/// A domain service encapsulating the process to create
	/// and store a new <see cref="テナント"/> instance.
	/// </para>
	/// <para>
	/// This operation is complex, involving the creation
	/// of a <see cref="ユーザー"/> and <see cref="ロール"/>
	/// for default administration of the new tenant,
	/// and publication of requisite domain events.
	/// </para>
	/// </summary>
	[CLSCompliant(true)]
	public class テナントプロビジョニングサービス
	{
		#region [ Fields and Constructor ]

		private readonly IロールRepository roleRepository;
		private readonly IテナントRepository tenantRepository;
		private readonly IユーザーRepository userRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="テナントプロビジョニングサービス"/> class.
		/// </summary>
		/// <param name="tenantRepository">
		/// An instance of <see cref="IテナントRepository"/> to use internally.
		/// </param>
		/// <param name="userRepository">
		/// An instance of <see cref="IユーザーRepository"/> to use internally.
		/// </param>
		/// <param name="roleRepository">
		/// An instance of <see cref="IロールRepository"/> to use internally.
		/// </param>
		public テナントプロビジョニングサービス(
			IテナントRepository tenantRepository,
			IユーザーRepository userRepository,
			IロールRepository roleRepository)
		{
			this.roleRepository = roleRepository;
			this.tenantRepository = tenantRepository;
			this.userRepository = userRepository;
		}

		#endregion

		#region [ Public Method ProvisionTenant() ]

		/// <summary>
		/// Creates a new <see cref="テナント"/>, stores it in
		/// its <see cref="IテナントRepository"/> instance, and
		/// publishes a <see cref="テナントプロビジョニング時"/> event,
		/// along with requisite domain events for the creation
		/// of a <see cref="ユーザー"/> and <see cref="ロール"/>
		/// for default administration of the new tenant.
		/// Refer to remarks for details.
		/// </summary>
		/// <param name="tenantName">
		/// The <see cref="テナント.Name"/> of the new tenant.
		/// </param>
		/// <param name="tenantDescription">
		/// The <see cref="テナント.Description"/> of the new tenant.
		/// </param>
		/// <param name="administorName">
		/// The <see cref="人.Name"/> of the
		/// default administrator for the new tenant.
		/// </param>
		/// <param name="emailAddress">
		/// The <see cref="人.EmailAddress"/> of the
		/// default administrator for the new tenant.
		/// </param>
		/// <param name="postalAddress">
		/// The <see cref="コンタクト情報.PostalAddress"/>
		/// of the default administrator for the new tenant.
		/// </param>
		/// <param name="primaryTelephone">
		/// The <see cref="コンタクト情報.PrimaryTelephone"/>
		/// of the default administrator for the new tenant.
		/// </param>
		/// <param name="secondaryTelephone">
		/// The <see cref="コンタクト情報.SecondaryTelephone"/>
		/// of the default administrator for the new tenant.
		/// </param>
		/// <returns>
		/// The newly registered <see cref="テナント"/>,
		/// which has already been added to the internal
		/// <see cref="IテナントRepository"/> instance.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The events published, in order, are:
		/// </para>
		/// <list type="bullet">
		/// <item><description><see cref="ユーザー登録時"/></description></item>
		/// <item><description><see cref="ロールプロビジョン時"/></description></item>
		/// <item><description><see cref="UserAssignedToRole"/></description></item>
		/// <item><description><see cref="テナント管理者登録時"/></description></item>
		/// <item><description><see cref="テナントプロビジョニング時"/></description></item>
		/// </list>
		/// </remarks>
		public テナント ProvisionTenant(
			string tenantName,
			string tenantDescription,
			フルネーム administorName,
			Emailアドレス emailAddress,
			郵便住所 postalAddress,
			電話 primaryTelephone,
			電話 secondaryTelephone)
		{
			try
			{
				// must be active to register admin
				テナント tenant = new テナント(this.tenantRepository.GetNextIdentity(), tenantName, tenantDescription, true);

				// Since this is a new entity, add it to
				// the collection-oriented repository.
				// Subsequent changes to the entity
				// are implicitly persisted.
				this.tenantRepository.Add(tenant);

				// Creates user and role entities and stores them
				// in their respective repositories, and publishes
				// domain events UserRegistered, RoleProvisioned,
				// UserAssignedToRole, and TenantAdministratorRegistered.
				this.RegisterAdministratorFor(
					tenant,
					administorName,
					emailAddress,
					postalAddress,
					primaryTelephone,
					secondaryTelephone);

				DomainEventPublisher
					.Instance
					.Publish(new テナントプロビジョニング時(
							tenant.TenantId));

				return tenant;
			}
			catch (Exception e)
			{
				throw new InvalidOperationException(
					string.Concat("Cannot provision tenant because: ", e.Message), e);
			}
		}

		#endregion

		#region [ Private Method used by ProvisionTenant() ]

		private void RegisterAdministratorFor(
			テナント tenant,
			フルネーム administorName,
			Emailアドレス emailAddress,
			郵便住所 postalAddress,
			電話 primaryTelephone,
			電話 secondaryTelephone)
		{
			レジストレーション招待 invitation = tenant.OfferRegistrationInvitation("init").OpenEnded();
			string strongPassword = new パスワードサービス().GenerateStrongPassword();

			// Publishes domain event UserRegistered.
			ユーザー admin = tenant.RegisterUser(
				invitation.InvitationId,
				"admin",
				strongPassword,
				有効化.IndefiniteEnablement(),
				new 人(
					tenant.TenantId,
					administorName,
					new コンタクト情報(
						emailAddress,
						postalAddress,
						primaryTelephone,
						secondaryTelephone)));

			tenant.WithdrawInvitation(invitation.InvitationId);

			// Since this is a new entity, add it to
			// the collection-oriented repository.
			// Subsequent changes to the entity
			// are implicitly persisted.
			this.userRepository.Add(admin);

			// Publishes domain event RoleProvisioned.
			ロール adminRole = tenant.ProvisionRole(
				"Administrator",
				string.Format("Default {0} administrator.", tenant.Name));

			// Publishes domain event UserAssignedToRole,
			// but not GroupUserAdded because the group
			// reference held by the role is an "internal" group.
			adminRole.AssignUser(admin);

			// Since this is a new entity, add it to
			// the collection-oriented repository.
			// Subsequent changes to the entity
			// are implicitly persisted.
			this.roleRepository.Add(adminRole);

			DomainEventPublisher
				.Instance
				.Publish(new テナント管理者登録時(
						tenant.TenantId,
						tenant.Name,
						administorName,
						emailAddress,
						admin.Username,
						strongPassword));
		}

		#endregion
	}
}