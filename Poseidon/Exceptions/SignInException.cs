using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Exceptions
{
    public class SignInException : Exception
    {
        public SignInException()
            : base() { }

        public SignInException(string message)
            : base(message) { }

        public SignInException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
