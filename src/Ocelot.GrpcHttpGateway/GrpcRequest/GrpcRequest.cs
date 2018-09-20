using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.GrpcHttpGateway
{
    public class GrpcRequest
    {
        public MethodDescriptor GrpcMethod { get; set; }

        public string RequestMessage { get; set; }
    }
}
