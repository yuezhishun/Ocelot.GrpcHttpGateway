using Grpc.Core;

namespace Ocelot.GrpcHttpGateway
{
    public interface IGrpcChannelFactory
    {
        Channel GetGrpcChannel(string address, int port);
    }
}