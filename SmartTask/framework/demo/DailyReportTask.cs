using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.demo
{
    /// <summary>
    /// 发送Daily Report
    /// </summary>
    public class DailyReportTask : AbstractTask
    {
        private static readonly Random rand = new Random();
        protected static int RandSeconds()
        {
            return rand.Next(2000, 6000);
        }

        protected override TaskResult Work()
        {
            string message = "";
            var stamp = TaskResultType.Succeed;
            var rs = RandSeconds();
            if (rs > 4500)
            {
                stamp = TaskResultType.Failed;
                message = "Time out";
            }

            Thread.Sleep(rs);

            Logger.Info("我执行了发送Daily Report的任务,工作了{0}秒, 结果是{1}", rs / 1000, stamp);

            return new TaskResult() { Result = stamp, Message = message };
        }
    }
}
