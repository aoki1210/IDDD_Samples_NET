namespace SaaSOvation.IdentityAccess.Application
{
	using System;

	using SaaSOvation.IdentityAccess.Application.Commands;
	using SaaSOvation.IdentityAccess.Domain.Model.Identity;

	/// <summary>
	/// Coordinates interactions among entities in the "Domain.Model.Identity" namespace.
	/// </summary>
	[CLSCompliant(true)]
    public class アイデンティティアプリケーションサービス
    {
        readonly 認証サービス authenticationService;
        readonly グループメンバーサービス groupMemberService;
        readonly IグループRepository groupRepository;
        readonly テナントプロビジョニングサービス tenantProvisioningService;
        readonly IテナントRepository tenantRepository;
        readonly IユーザーRepository userRepository;

        public void ActivateTenant(テナント有効化コマンド command)
        {
            var tenant = GetExistingTenant(command.TenantId);
            tenant.Activate();
        }

        public void AddGroupToGroup(グループをグループに追加コマンド command)
        {
            var parentGroup = GetExistingGroup(command.TenantId, command.ParentGroupName);
            var childGroup = GetExistingGroup(command.TenantId, command.ChildGroupName);
            parentGroup.AddGroup(childGroup, this.groupMemberService);
        }

        public void AddUserToGroup(ユーザーをグループに追加コマンド command)
        {
            var group = GetExistingGroup(command.TenantId, command.GroupName);
            var user = GetExistingUser(command.TenantId, command.Username);
            group.AddUser(user);
        }

        public ユーザー記述子 AuthenticateUser(ユーザー認証コマンド command)
        {
            return this.authenticationService.Authenticate(new テナントId(command.TenantId), command.Username, command.Password);
        }

        public void DeactivateTenant(テナントを非アクティブ化コマンド command)
        {
            var tenant = GetExistingTenant(command.TenantId);
            tenant.Deactivate();
        }

        public void ChangeUserContactInformation(コンタクト情報を変更コマンド command)
        {
            var user = GetExistingUser(command.TenantId, command.Username);
            user.Person.ChangeContactInformation(
                new コンタクト情報(
                    new Emailアドレス(command.EmailAddress),
                    new 郵便住所(
                        command.AddressStreetAddress,
                        command.AddressCity,
                        command.AddressStateProvince,
                        command.AddressPostalCode,
                        command.AddressCountryCode),
                    new 電話(command.PrimaryTelephone),
                    new 電話(command.SecondaryTelephone)));
        }

        public void ChangeUserEmailAddress(Emailアドレスを変更コマンド command)
        {
            var user = GetExistingUser(command.TenantId, command.Username);
            user.Person.ContactInformation.ChangeEmailAddress(new Emailアドレス(command.EmailAddress));
        }

        public void ChangeUserPostalAddress(郵便住所を変更コマンド command)
        {
            var user = GetExistingUser(command.TenantId, command.Username);
            user.Person.ContactInformation.ChangePostalAddress(
                new 郵便住所(
                    command.AddressStreetAddress,
                    command.AddressCity,
                    command.AddressStateProvince,
                    command.AddressPostalCode,
                    command.AddressCountryCode));
        }

        public void ChangeUserPrimaryTelephone(一次電話番号を変更コマンド command)
        {
            var user = GetExistingUser(command.Telephone, command.Username);
            user.Person.ContactInformation.ChangePrimaryTelephone(
                new 電話(command.Telephone)); 
        }

        public void ChangeUserSecondaryTelephone(二次電話番号を変更コマンド command)
        {
            var user = GetExistingUser(command.Telephone, command.Username);
            user.Person.ContactInformation.ChangeSecondaryTelephone(
                new 電話(command.Telephone));
        }

        public void ChangeUserPassword(ユーザーパスワード変更コマンド command)
        {
            var user = GetExistingUser(command.TenantId, command.Username);
            user.ChangePassword(command.CurrentPassword, command.ChangedPassword);
        }

        public void ChangeUserPersonalName(ユーザー個人名変更コマンド command)
        {
            var user = GetExistingUser(command.TenantId, command.Username);
            user.ChangePersonalName(new フルネーム(command.FirstName, command.LastName));
        }

        public void DefineUserEnablement(ユーザー有効化定義コマンド command)
        {
            var user = GetExistingUser(command.TenantId, command.Username);
            user.DefineEnablement(new 有効化(command.Enabled, command.StartDate, command.EndDate));
        }

        public グループ GetGroup(string tenantId, string groupName)
        {
            return this.groupRepository.GroupNamed(new テナントId(tenantId), groupName);
        }

        public bool IsGroupMember(string tenantId, string groupName, string userName)
        {
            var group = GetExistingGroup(tenantId, groupName);
            var user = GetExistingUser(tenantId, userName);
            return group.IsMember(user, this.groupMemberService);
        }

        public グループ ProvisionGroup(グループをプロビジョニングするコマンド command)
        {
            var tenant = GetExistingTenant(command.TenantId);
            var group = tenant.ProvisionGroup(command.GroupName, command.Description);
            this.groupRepository.Add(group);
            return group;
        }

        public テナント ProvisionTenant(プロビジョンテナントコマンド command)
        {
            return this.tenantProvisioningService.ProvisionTenant(
                command.TenantName,
                command.TenantDescription,
                new フルネーム(command.AdministorFirstName, command.AdministorLastName),
                new Emailアドレス(command.EmailAddress),
                new 郵便住所(
                    command.AddressStreetAddress,
                    command.AddressCity,
                    command.AddressStateProvince,
                    command.AddressPostalCode,
                    command.AddressCountryCode),
                new 電話(command.PrimaryTelephone),
                new 電話(command.SecondaryTelephone));
        }

        public ユーザー RegisterUser(ユーザー登録コマンド command)
        {
            var tenant = GetExistingTenant(command.TenantId);
            var user = tenant.RegisterUser(
                command.InvitationIdentifier,
                command.Username,
                command.Password,
                new 有効化(command.Enabled, command.StartDate, command.EndDate),
                new 人(
                    new テナントId(command.TenantId),
                    new フルネーム(command.FirstName, command.LastName),
                    new コンタクト情報(
                        new Emailアドレス(command.EmailAddress),
                        new 郵便住所(
                            command.AddressStreetAddress,
                            command.AddressCity,
                            command.AddressStateProvince,
                            command.AddressPostalCode,
                            command.AddressCountryCode),
                        new 電話(command.PrimaryTelephone),
                        new 電話(command.SecondaryTelephone))));

            if (user == null)
                throw new InvalidOperationException("User not registered.");

            this.userRepository.Add(user);

            return user;
        }

        public void RemoveGroupFromGroup(グループからグループフォーラムを削除コマンド command)
        {
            var parentGroup = GetExistingGroup(command.TenantId, command.ParentGroupName);
            var childGroup = GetExistingGroup(command.TenantId, command.ChildGroupName);
            parentGroup.RemoveGroup(childGroup);
        }

        public void RemoveUserFromGroup(グループからユーザーを削除コマンド command)
        {
            var group = GetExistingGroup(command.TenantId, command.GroupName);
            var user = GetExistingUser(command.TenantId, command.Username);
            group.RemoveUser(user);
        }

        public ユーザー GetUser(string tenantId, string userName)
        {
            return this.userRepository.UserWithUsername(new テナントId(tenantId), userName);
        }

        ユーザー GetExistingUser(string tenantId, string userName)
        {
            var user = GetUser(tenantId, userName);
            if (user == null)
                throw new ArgumentException(
                    string.Format("User does not exist for {0} and {1}.", tenantId, userName));
            return user;
        }

        グループ GetExistingGroup(string tenantId, string groupName)
        {
            var group = GetGroup(tenantId, groupName);
            if (group == null)
                throw new ArgumentException(
                    string.Format("Group does not exist for {0} and {1}.", tenantId, groupName));
            return group;
        }

        public テナント GetTenant(string tenantId)
        {
            return this.tenantRepository.Get(new テナントId(tenantId));
        }

        テナント GetExistingTenant(string tenantId)
        {
            var tenant = GetTenant(tenantId);
            if (tenant == null)
                throw new ArgumentException(
                    string.Format("Tenant does not exist for: {0}", tenantId));
            return tenant;
        }

        public ユーザー記述子 GetUserDescriptor(string tenantId, string userName)
        {
            var user = GetUser(tenantId, userName);
            if (user != null)
            {
                return user.UserDescriptor;
            }
            else
            {
                return null;
            }
        }
    }
}
