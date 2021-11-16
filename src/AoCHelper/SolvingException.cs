using System.Runtime.Serialization;

namespace AoCHelper
{
    [Serializable]
    public class SolvingException : Exception
    {
        public SolvingException()
        {
        }

        public SolvingException(string message) : base(message)
        {
        }

        public SolvingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SolvingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
