using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyQQ.Entity
{
    public class ResponseWrapper<T>
    {
        public int ReturnCode { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public T Result { get; set; }
    }
}
