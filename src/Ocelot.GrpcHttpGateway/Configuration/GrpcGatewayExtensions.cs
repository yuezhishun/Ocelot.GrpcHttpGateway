using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.DependencyInjection;

namespace Ocelot.GrpcHttpGateway
{
    public static class GrpcGatewayExtensions
    {
        public static IOcelotBuilder AddGrpcHttpGateway(this IOcelotBuilder builder)
        {
            builder.Services.AddGrpcHttpGateway();
            return builder;
        }

        private static IServiceCollection AddGrpcHttpGateway(this IServiceCollection services)
        {
            services.TryAddSingleton<IGrpcServiceDescriptor,GrpcServiceDescriptor>();
            services.TryAddSingleton<IGrpcChannelFactory, GrpcChannelFactory>();
            services.TryAddTransient<IGrpcRequestBuilder, GrpcRequestBuilder>();
            return services;
        }

    }
}
