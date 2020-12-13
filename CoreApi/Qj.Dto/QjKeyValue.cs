using System;
using System.Collections.Generic;
using System.Text;

namespace Qj.Dto
{
    public class QjKeyValue<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
    }
}