using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model.LongRunningProcess;

using SaaSOvation.AgilePM.Domain.Model.Products;
using SaaSOvation.AgilePM.Domain.Model.Teams;
using SaaSOvation.AgilePM.Domain.Model.Tenants;
using SaaSOvation.AgilePM.Domain.Model.Discussions;

namespace SaaSOvation.AgilePM.Application.Products
{
    public class プロダクトアプリケーションサービス
    {
        public プロダクトアプリケーションサービス(IプロダクトRepository productRepository, IプロダクトオーナRepository productOwnerRepository, I時間制約プロセストラッカーRepository processTrackerRepository)
        {
            this.productRepository = productRepository;
            this.productOwnerRepository = productOwnerRepository;
            this.processTrackerRepository = processTrackerRepository;
        }

        readonly IプロダクトRepository productRepository;
        readonly IプロダクトオーナRepository productOwnerRepository;
        readonly I時間制約プロセストラッカーRepository processTrackerRepository;

        public void InitiateDiscussion(ディスカッション加入コマンド command)
        {
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var product = this.productRepository.Get(new テナントId(command.TenantId), new プロダクトId(command.ProductId));
                if (product == null)
                    throw new InvalidOperationException(
                        string.Format("Unknown product of tenant id: {0} and product id: {1}.", command.TenantId, command.ProductId));

                product.InitiateDiscussion(new ディスカッション記述子(command.DiscussionId));

                this.productRepository.Save(product);

                var processId = プロセスId.ExistingProcessId(product.DiscussionInitiationId);

                var tracker = this.processTrackerRepository.Get(command.TenantId, processId);

                tracker.MarkProcessCompleted();

                this.processTrackerRepository.Save(tracker);

                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        public string NewProduct(新プロダクトコマンド command)
        {
            return NewProductWith(command.TenantId, command.ProductOwnerId, command.Name, command.Description, ディスカッションアベイラビリティ.NotRequested);
        }

        public string NewProductWithDiscussion(新プロダクトコマンド command)
        {
            return NewProductWith(command.TenantId, command.ProductOwnerId, command.Name, command.Description, RequestDiscussionIfAvailable());
        }

        public void RequestProductDiscussion(リクエストプロダクトディスカッションコマンド command)
        {
            var product = this.productRepository.Get(new テナントId(command.TenantId), new プロダクトId(command.ProductId));
            if (product == null)
                throw new InvalidOperationException(
                    string.Format("Unknown product of tenant id: {0} and product id: {1}.", command.TenantId, command.ProductId));

            RequestProductDiscussionFor(product);
        }

        public void RetryProductDiscussionRequest(プロダクトディスカッションリクエストリトライコマンド command)
        {
            var processId = プロセスId.ExistingProcessId(command.ProcessId);
            var tenantId = new テナントId(command.TenantId);
            var product = this.productRepository.GetByDiscussionInitiationId(tenantId, processId.Id);
            if (product == null)
                throw new InvalidOperationException(
                    string.Format("Unknown product of tenant id: {0} and discussion initiation id: {1}.", command.TenantId, command.ProcessId));

            RequestProductDiscussionFor(product);
        }

        public void StartDiscussionInitiation(ディスカッション加入開始コマンド command)
        {
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var product = this.productRepository.Get(new テナントId(command.TenantId), new プロダクトId(command.ProductId));
                if (product == null)
                    throw new InvalidOperationException(
                        string.Format("Unknown product of tenant id: {0} and product id: {1}.", command.TenantId, command.ProductId));

                var timedOutEventName = typeof(プロダクトディスカッションリクエストタイムアウト).Name;

                var tracker = new 時間制約プロセストラッカー(
                    tenantId: command.TenantId,
                    processId: プロセスId.NewProcessId(),
                    description: "Create discussion for product: " + product.Name,
                    originalStartTime: DateTime.UtcNow,
                    allowableDuration: 5L * 60L * 1000L,
                    totalRetriesPermitted: 3,
                    processTimedOutEventType: timedOutEventName);

                this.processTrackerRepository.Save(tracker);

                product.StartDiscussionInitiation(tracker.ProcessId.Id);

                this.productRepository.Save(product);

                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        public void TimeOutProductDiscussionRequest(タイムアウトプロダクトディスカッションリクエストコマンド command)
        {
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var processId = プロセスId.ExistingProcessId(command.ProcessId);

                var tenantId = new テナントId(command.TenantId);

                var product = this.productRepository.GetByDiscussionInitiationId(tenantId, processId.Id);

                SendEmailForTimedOutProcess(product);

                product.FailDiscussionInitiation();

                this.productRepository.Save(product);

                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        void SendEmailForTimedOutProcess(プロダクト product)
        {
            // TODO: implement
        }

        void RequestProductDiscussionFor(プロダクト product)
        {
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                product.RequestDiscussion(RequestDiscussionIfAvailable());

                this.productRepository.Save(product);

                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }

        ディスカッションアベイラビリティ RequestDiscussionIfAvailable()
        {
            var availability = ディスカッションアベイラビリティ.AddOnNotEnabled;
            var enabled = true; // TODO: determine add-on enabled
            if (enabled)
            {
                availability = ディスカッションアベイラビリティ.Requested;
            }
            return availability;
        }

        string NewProductWith(string tenantId, string productOwnerId, string name, string description, ディスカッションアベイラビリティ discussionAvailability)
        {
            var tid = new テナントId(tenantId);
            var productId = default(プロダクトId);
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                productId = this.productRepository.GetNextIdentity();

                var productOwner = this.productOwnerRepository.Get(tid, productOwnerId);

                var product = new プロダクト(tid, productId, productOwner.ProductOwnerId, name, description, discussionAvailability);

                this.productRepository.Save(product);

                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
            // TODO: handle null properly
            return productId.Id;
        }
    }
}
