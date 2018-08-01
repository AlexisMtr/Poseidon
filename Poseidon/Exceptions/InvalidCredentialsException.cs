using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Poseidon.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base() { }

        public InvalidCredentialsException(string message)
            : base(message) { }

        public InvalidCredentialsException(string message, Exception innerException)
            : base(message, innerException) { }

    }
}
