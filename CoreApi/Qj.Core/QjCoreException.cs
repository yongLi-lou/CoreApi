using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Core
{
    public class QjCoreException : Exception
    {
        public QjCoreException()
        {
        }

        public QjCoreException(string message) : base(message)
        {
        }

        public QjCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QjCoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}