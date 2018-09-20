using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.GrpcHttpGateway
{
    public static class GrpcHttpMiddlewareExtensions
    {
        public static IOcelotPipelineBuilder UseGrpcHttpMiddleware(this IOcelotPipelineBuilder builder)
        {
            return builder.UseMiddleware<GrpcHttpMiddleware>();
        }
    }
}
