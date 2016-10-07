using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyQQ
{
    /// <summary>
    /// 单词枚举器
    /// </summary>
    public class WordStream : IEnumerable
    {
        private char[] buffer;
        public WordStream(char[] buffer)
        {
            this.buffer = buffer;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public WordStreamEnum GetEnumerator()
        {
            return new WordStreamEnum(this.buffer);
        }
    }
}