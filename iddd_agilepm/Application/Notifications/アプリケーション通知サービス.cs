using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Notifications;

namespace SaaSOvation.AgilePM.Application.Notifications
{
    public class アプリケーション通知サービス
    {
        public アプリケーション通知サービス(INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;
        }

        readonly INotificationPublisher notificationPublisher;

        public void PublishNotifications()
        {
            アプリケーションサービスライフサイクル.Begin(false);
            try
            {
                this.notificationPublisher.PublishNotifications();
                アプリケーションサービスライフサイクル.Success();
            }
            catch (Exception ex)
            {
                アプリケーションサービスライフサイクル.Fail(ex);
            }
        }
    }
}
