using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SmartTask
{
    /// <summary>
    /// 任务运行状态配置，可读可写[序列化与反序列化时使用]
    /// </summary>
    [Serializable,
    XmlRoot(ElementName = "Tasks")]
    public class ExecutionStatus : ConfigBase
    {
        private const string FILE_NAME = "Tasks.Execution.xml";

        /// <summary>
        /// 文件名
        /// </summary>
        protected override string FileName
        {
            get
            {
                return FILE_NAME;
            }
        }

        /// <summary>
        /// 禁止直接实例化
        /// </summary>
        public ExecutionStatus()
        {
            Tasks = new TaskExecutionCollection();
        }

        /// <summary>
        /// Note:序列化，从配置中获取所有数据
        /// </summary>
        /// <returns></returns>
        public static ExecutionStatus Instance()
        {
            ExecutionStatus executionStatus;
            try
            {
                var fullFileName = GetFullPath(FILE_NAME);
                var exists = File.Exists(fullFileName);
                if (!exists)
                {
                    executionStatus = new ExecutionStatus();
                }
                else
                {
                    using (var reader = XmlReader.Create(fullFileName))
                    {
                        var slz = new XmlSerializer(typeof(ExecutionStatus));
                        var rst = slz.Deserialize(reader) as ExecutionStatus;
                        reader.Close();
                        executionStatus = rst;
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = new ExecutionStatus();   //文件未找到的异常，自已创建一个了
                Logger.Fatal("实例化任务状态时异常", ex);
            }
            return executionStatus;
        }

        /// <summary>
        /// 任务名称及执行状态
        /// </summary>
        [XmlElement("Task")]
        public TaskExecutionCollection Tasks { get; set; }
    }
}
