using Ocelot.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.GrpcHttpGateway
{
    public class UnknownError : Error
    {
        public UnknownError(string message) : base(message, OcelotErrorCode.UnknownError)
        {
        }

        public UnknownError(string message,Exception exception) : base(message, OcelotErrorCode.UnknownError)
        {
        }

    }
}
