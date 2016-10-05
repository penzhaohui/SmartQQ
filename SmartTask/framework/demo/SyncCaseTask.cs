using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.demo
{
    /// <summary>
    /// 同步Case到Jira
    /// </summary>
    public class SyncCaseTask : AbstractTask
    {
        private static readonly Random rand = new Random();
        protected static int RandSeconds()
        {
            return rand.Next(2000, 6000);
        }

        protected override TaskResult Work()
        {
            var stamp = TaskResultType.Succeed;
            var rs = RandSeconds();
            if (rs > 4500)
                stamp = TaskResultType.Failed;

            Thread.Sleep(rs);

            Logger.Info("执行同步Case到Jira的任务,工作了{0}秒", rs / 1000);
            
            return new TaskResult() { Result = stamp };
        }
    }
}
