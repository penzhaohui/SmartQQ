using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Web;

namespace MyQQ
{
    public class WordCount
    {
        public void Demo()
        {
            DateTime dt = DateTime.Now;

            var buffer = new BufferBlock<WordStream>();

            //创建工作BufferBlock
            WordProcessBufferBlock wb = new WordProcessBufferBlock(8, buffer);
            wb.StartWord();

            //创建读取文件,发送的BufferBlock
            FileBufferBlock fb = new FileBufferBlock(buffer, @"D:\Server Log.txt");
            fb.ReadFile();

            Dictionary<string, int> dic = new Dictionary<string, int>();

            //等待工作完成汇总结果
            wb.WaitAll(p =>
            {
                foreach (var row in p)
                {
                    if (!dic.ContainsKey(row.Key))
                        dic.Add(row.Key, row.Value);
                    else
                    {
                        dic[row.Key] += row.Value;
                    }
                }
            }
                );

            var myList = dic.ToList();
            myList.Sort((p, v) => v.Value.CompareTo(p.Value));
            foreach (var row in myList.Take(10))
            {
                Console.WriteLine(row);
                System.Diagnostics.Debug.WriteLine(row);
            }


            Console.WriteLine(DateTime.Now - dt);
            System.Diagnostics.Debug.WriteLine(DateTime.Now - dt);
        }
    }
}