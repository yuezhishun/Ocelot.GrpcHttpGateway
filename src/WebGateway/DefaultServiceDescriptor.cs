using Google.Protobuf.Reflection;
using Ocelot.GrpcHttpGateway;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebGateway
{
    public class DefaultServiceDescriptor : GrpcServiceDescriptor, IGrpcServiceDescriptor
    {

        public override ConcurrentDictionary<string, ConcurrentDictionary<string, MethodDescriptor>> GetGrpcDescript()
        {
            return CurrentService;
        }
    }
}
