using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Web;

namespace MyQQ
{
    public class WordProcessBufferBlock
    {
        private int _taskCount = 1;
        BufferBlock<WordStream> _buffer = null;
        private List<Task<Dictionary<string, int>>> _list = new List<Task<Dictionary<string, int>>>();

        /// <summary>
        /// 单词处理类
        /// </summary>
        /// <param name="taskCount">工作线程数</param>
        /// <param name="buffer">DataFlow的BufferBlock</param>
        public WordProcessBufferBlock(int taskCount, BufferBlock<WordStream> buffer)
        {
            _taskCount = taskCount;
            this._buffer = buffer;
        }

        public void StartWord()
        {
            for (int i = 0; i < _taskCount; i++)
            {
                _list.Add(Process());
            }
        }
        /// <summary>
        /// 等待所有工作完成
        /// </summary>
        /// <param name="f">完成后的工作函数</param>
        public void WaitAll(Action<Dictionary<string, int>> f)
        {
            Task.WaitAll(_list.ToArray());
            foreach (var row in _list)
            {
                f(row.Result);
            }
        }

        /// <summary>
        /// 使用BufferBlock.TryReceive循环从消息里取从FileBufferBlock发送的buffer
        /// </summary>
        /// <returns>工作结果</returns>
        private async Task<Dictionary<string, int>> Process()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            while (await _buffer.OutputAvailableAsync())
            {
                WordStream ws;
                while (_buffer.TryReceive(out ws))
                {
                    foreach (string value in ws)
                    {
                        if (dic.ContainsKey(value))
                        {
                            dic[value]++;
                        }
                        else
                        {
                            dic.Add(value, 1);
                        }
                    }
                }
            }
            return dic;
        }
    }
}