using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TemplateApi.Common
{
    public class QjApiException : Exception
    {
        public QjApiException()
        {
        }

        public QjApiException(string message) : base(message)
        {
        }

        public QjApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QjApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}