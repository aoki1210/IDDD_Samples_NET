namespace SaaSOvation.IdentityAccess.Application
{
	using System;

	using SaaSOvation.IdentityAccess.Application.Commands;
	using SaaSOvation.IdentityAccess.Domain.Model.Access;
	using SaaSOvation.IdentityAccess.Domain.Model.Identity;

	[CLSCompliant(true)]
	public sealed class アクセスアプリケーションサービス
	{
		private readonly IグループRepository groupRepository;
		private readonly IロールRepository roleRepository;
		private readonly IテナントRepository tenantRepository;
		private readonly IユーザーRepository userRepository;

		public アクセスアプリケーションサービス(
			IグループRepository groupRepository,
			IロールRepository roleRepository,
			IテナントRepository tenantRepository,
			IユーザーRepository userRepository)
		{
			this.groupRepository = groupRepository;
			this.roleRepository = roleRepository;
			this.tenantRepository = tenantRepository;
			this.userRepository = userRepository;
		}

		public void AssignUserToRole(ユーザーをロールにアサインコマンド command)
		{
			var tenantId = new テナントId(command.TenantId);
			var user = this.userRepository.UserWithUsername(tenantId, command.Username);
			if (user != null)
			{
				var role = this.roleRepository.RoleNamed(tenantId, command.RoleName);
				if (role != null)
				{
					role.AssignUser(user);
				}
			}
		}

		public bool IsUserInRole(string tenantId, string userName, string roleName)
		{
			return UserInRole(tenantId, userName, roleName) != null;
		}

		public ユーザー UserInRole(string tenantId, string userName, string roleName)
		{
			var id = new テナントId(tenantId);
			var user = this.userRepository.UserWithUsername(id, userName);
			if (user != null)
			{
				var role = this.roleRepository.RoleNamed(id, roleName);
				if (role != null)
				{
					if (role.IsInRole(user, new グループメンバーサービス(this.userRepository, this.groupRepository)))
					{
						return user;
					}
				}
			}

			return null;
		}

		public void ProvisionRole(プロビジョンロールコマンド command)
		{
			var tenantId = new テナントId(command.TenantId);
			var tenant = this.tenantRepository.Get(tenantId);
			var role = tenant.ProvisionRole(command.RoleName, command.Description, command.SupportsNesting);
			this.roleRepository.Add(role);
		}
	}
}