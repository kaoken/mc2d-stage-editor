using System;
using System.Runtime.Serialization;

namespace UtilSharpDX.Exceptions
{
    [Serializable]
    internal class CreatedException : Exception
    {
        public CreatedException()
        {
        }

        public CreatedException(string message) : base(message)
        {
        }

        public CreatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CreatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}