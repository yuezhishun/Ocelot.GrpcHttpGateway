using System.Collections.Concurrent;
using System.Collections.Generic;
using Google.Protobuf.Reflection;

namespace Ocelot.GrpcHttpGateway
{
    public interface IGrpcServiceDescriptor
    {
        ConcurrentDictionary<string, ConcurrentDictionary<string, MethodDescriptor>> GetGrpcDescript();
    }
}