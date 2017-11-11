using System;
using System.Runtime.Serialization;

namespace UtilSharpDX.Exceptions
{
    [Serializable]
    internal class NotCreatedException : Exception
    {
        public NotCreatedException()
        {
        }

        public NotCreatedException(string message) : base(message)
        {
        }

        public NotCreatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotCreatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}