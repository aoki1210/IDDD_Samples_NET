using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model.LongRunningProcess;

namespace SaaSOvation.AgilePM.Application.Processes
{
    public class アプリケーションプロセスサービス
    {
        public アプリケーションプロセスサービス(I時間制約プロセストラッカーRepository processTrackerRepository)
        {
            this.processTrackerRepository = processTrackerRepository;
        }

        readonly I時間制約プロセストラッカーRepository processTrackerRepository;

        public void CheckForTimedOutProccesses()
        {
            アプリケーションサービスライフサイクル.Begin();
            try
            {
                var trackers = this.processTrackerRepository.GetAllTimedOut();

                foreach (var tracker in trackers)
                {
                    tracker.InformProcessTimedOut();
                    this.processTrackerRepository.Save(tracker);
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
