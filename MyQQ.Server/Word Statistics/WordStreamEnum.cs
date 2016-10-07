using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyQQ
{
    public class WordStreamEnum : IEnumerator
    {
        private char[] buffer;
        int pos = 0;
        int endCount = 0;
        int index = -1;

        public WordStreamEnum(char[] buffer)
        {
            this.buffer = buffer;
        }

        public bool MoveNext()
        {
            while (index < buffer.Length - 1)
            {
                index++;
                char buff = buffer[index];
                if ((buff >= 'a' && buff <= 'z') || (buff >= 'A' && buff <= 'Z'))
                {
                    if (endCount == 0)
                    {
                        pos = index;
                        endCount++;
                    }
                    else
                    {
                        endCount++;
                    }
                }
                else
                {
                    if (endCount != 0)
                        return true;
                }
                if (buff == '\0')
                {
                    return false;
                }
            }
            return false;
        }

        public object Current
        {
            get
            {
                int tempInt = endCount;
                endCount = 0;
                return new string(buffer, pos, tempInt);
            }
        }

        public void Reset()
        {
            index = -1;
        }
    }
}