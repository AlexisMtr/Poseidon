using System;

namespace Poseidon.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
        
        public NotFoundException(Type objectType)
            : this($"{objectType} not found") { }
    }
}
