using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Web;

namespace MyQQ
{
    public class FileBufferBlock
    {

        private string _fileName;
        BufferBlock<WordStream> _buffer = null;
        public FileBufferBlock(BufferBlock<WordStream> buffer, string fileName)
        {
            this._fileName = fileName;
            this._buffer = buffer;
        }

        /// <summary>
        /// 按32M读取文件，循环发送给WordBufferBlock
        /// </summary>
        public void ReadFile()
        {
            using (FileStream fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {

                        char[] charBuffer = new char[32 * 1024 * 1024];
                        sr.ReadBlock(charBuffer, 0, charBuffer.Length);
                        _buffer.Post(new WordStream(charBuffer));
                    }
                }
            }
            _buffer.Complete();
        }
    }
}