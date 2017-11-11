using System;
using System.Runtime.Serialization;

namespace UtilSharpDX.Exceptions
{
    [Serializable]
    internal class NotExistenceException : Exception
    {
        public NotExistenceException()
        {
        }

        public NotExistenceException(string message) : base(message)
        {
        }

        public NotExistenceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotExistenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}