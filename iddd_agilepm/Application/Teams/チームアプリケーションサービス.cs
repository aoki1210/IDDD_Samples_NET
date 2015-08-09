using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.AgilePM.Domain.Model.Teams;
using SaaSOvation.AgilePM.Domain.Model.Tenants;

namespace SaaSOvation.AgilePM.Application.Teams
{
    public class チームアプリケーションサービス
    {
        public チームアプリケーションサービス(IチームメンバRepository teamMemberRepository, IプロダクトオーナRepository productOwnerRepository)
        {
            this.teamMemberRepository = teamMemberRepository;
            this.productOwnerRepository = productOwnerRepository;
        }

        readonly IチームメンバRepository teamMemberRepository;
        readonly IプロダクトオーナRepository productOwnerRepository;

        public void EnableProductOwner(プロダクトオーナー有効化コマンド command)
        {
            var tenantId = new テナントId(command.TenantId);
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var productOwner = this.productOwnerRepository.Get(tenantId, command.Username);
                if (productOwner != null)
                {
                    productOwner.Enable(command.OccurredOn);
                }
                else
                {
                    productOwner = new プロダクトオーナ(tenantId, command.Username, command.FirstName, command.LastName, command.EmailAddress, command.OccurredOn);
                    this.productOwnerRepository.Save(productOwner);
                }
                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        public void EnableTeamMember(チームメンバー有効化コマンド command)
        {
            var tenantId = new テナントId(command.TenantId);
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var teamMember = this.teamMemberRepository.Get(tenantId, command.Username);
                if (teamMember != null)
                {
                    teamMember.Enable(command.OccurredOn);
                }
                else
                {
                    teamMember = new チームメンバ(tenantId, command.Username, command.FirstName, command.LastName, command.EmailAddress, command.OccurredOn);
                    this.teamMemberRepository.Save(teamMember);
                }
                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        public void ChangeTeamMemberEmailAddress(チームメンバーメールアドレス変更コマンド command)
        {
            var tenantId = new テナントId(command.TenantId);
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var productOwner = this.productOwnerRepository.Get(tenantId, command.Username);
                if (productOwner != null)
                {
                    productOwner.ChangeEmailAddress(command.EmailAddress, command.OccurredOn);
                    this.productOwnerRepository.Save(productOwner);
                }

                var teamMember = this.teamMemberRepository.Get(tenantId, command.Username);
                if (teamMember != null)
                {
                    teamMember.ChangeEmailAddress(command.EmailAddress, command.OccurredOn);
                    this.teamMemberRepository.Save(teamMember);
                }

                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        public void ChangeTeamMemberName(チームメンバー名変更コマンド command)
        {
            var tenantId = new テナントId(command.TenantId);
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var productOwner = this.productOwnerRepository.Get(tenantId, command.Username);
                if (productOwner != null)
                {
                    productOwner.ChangeName(command.FirstName, command.LastName, command.OccurredOn);
                    this.productOwnerRepository.Save(productOwner);
                }

                var teamMember = this.teamMemberRepository.Get(tenantId, command.Username);
                if (teamMember != null)
                {
                    teamMember.ChangeName(command.FirstName, command.LastName, command.OccurredOn);
                    this.teamMemberRepository.Save(teamMember);
                }

                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        public void DisableProductOwner(プロダクトオーナーコマンド無効化 command)
        {
            var tenantId = new テナントId(command.TenantId);
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var productOwner = this.productOwnerRepository.Get(tenantId, command.Username);
                if (productOwner != null)
                {
                    productOwner.Disable(command.OccurredOn);
                    this.productOwnerRepository.Save(productOwner);
                }
                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        public void DisableTeamMember(チームメンバー無効化コマンド command)
        {
            var tenantId = new テナントId(command.TenantId);
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var teamMember = this.teamMemberRepository.Get(tenantId, command.Username);
                if (teamMember != null)
                {
                    teamMember.Disable(command.OccurredOn);
                    this.teamMemberRepository.Save(teamMember);
                }
                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }
    }
}
