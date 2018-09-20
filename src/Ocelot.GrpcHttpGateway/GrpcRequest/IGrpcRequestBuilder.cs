using Ocelot.Middleware;
using Ocelot.Responses;

namespace Ocelot.GrpcHttpGateway
{
    public interface IGrpcRequestBuilder
    {
        Response<GrpcRequest> BuildRequest(DownstreamContext context);
    }
}