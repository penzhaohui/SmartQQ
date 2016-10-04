using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// http://www.cnblogs.com/akwwl/p/3232679.html
// http://www.360doc.com/content/12/1130/22/1519571_251281101.shtml - C#中Func的用法和Lambda表达式
namespace SmartTask
{
    /// <summary>
    ///   时间委托
    /// </summary>
    /// <description class = "CS.TaskScheduling.SystemTime">
    ///   此处为何要用委托时间? 为了特殊需要。如调试，为了将系统的时间重置为某一个特定的时间，总不能一直调你的Windows时间吧？!
    /// </description>
    public static class SystemTime
    {
        /// <summary>
        /// Return current UTC time via <see cref="Func&lt;T&gt;" />. Allows easier unit testing.
        /// </summary>
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;

        /// <summary>
        /// Return current time in current time zone via <see cref="Func&lt;T&gt;" />. Allows easier unit testing.
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}
