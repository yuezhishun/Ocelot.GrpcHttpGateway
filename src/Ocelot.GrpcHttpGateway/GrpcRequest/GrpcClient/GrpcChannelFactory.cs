using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.GrpcHttpGateway
{
    public class GrpcChannelFactory : IGrpcChannelFactory
    {
        private readonly object _syncObj = new object();
        private ConcurrentDictionary<string, Channel> grpcServices;

        public GrpcChannelFactory()
        {
            grpcServices = new ConcurrentDictionary<string, Channel>();
        }
        public Channel GetGrpcChannel(string address, int port)
        {
            string key = $"{address}:{port}";
            if (grpcServices.TryGetValue(key, out Channel channel))
                return channel;
            channel = new Channel(key, ChannelCredentials.Insecure);
            return grpcServices.GetOrAdd(key, channel);
        }
    }
}
