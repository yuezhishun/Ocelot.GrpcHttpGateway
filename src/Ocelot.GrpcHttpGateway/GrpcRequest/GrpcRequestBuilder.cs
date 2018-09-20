using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Request.Mapper;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocelot.GrpcHttpGateway
{
    public class GrpcRequestBuilder : IGrpcRequestBuilder
    {
        private readonly IOcelotLogger logger;
        private IGrpcServiceDescriptor grpcServiceDescriptor;

        public GrpcRequestBuilder(IOcelotLoggerFactory factory, IGrpcServiceDescriptor grpcServiceDescriptor)
        {
            this.logger = factory.CreateLogger<GrpcRequestBuilder>();
            this.grpcServiceDescriptor = grpcServiceDescriptor;
        }

        public Response<GrpcRequest> BuildRequest(DownstreamContext context)
        {
            var route = context.DownstreamRequest.AbsolutePath.Trim('/').Split('/');
            if (route.Length !=2)
            {
                return SetError($"error request:{route},must do like this:http://domain:port/grpc/ServiceName/MethordName/");
            }
            string svcName = route[0].ToUpper();
            string methodName = route[1].ToUpper();
            //判断是否存在对应grpc服务、方法
            var grpcDescript = grpcServiceDescriptor.GetGrpcDescript();
            if (!grpcDescript.ContainsKey(svcName))
            {
                return SetError($"service name is not defined.{svcName}");
            }
            if (!grpcDescript[svcName].ContainsKey(methodName))
            {
                return SetError($"method name is not defined.{methodName}");
            }
            GrpcRequest grpcRequest = new GrpcRequest
            {
                GrpcMethod = grpcDescript[svcName][methodName]
            };
            try
            {
                //需要替换Scheme
                context.DownstreamRequest.Scheme = "http";
                var requestMessage = context.DownstreamRequest.ToHttpRequestMessage();
                var stream = requestMessage.Content.ReadAsStreamAsync().Result;
                var encoding = context.HttpContext.Request.GetTypedHeaders().ContentType?.Encoding ?? Encoding.UTF8;
                using (var reader = new StreamReader(stream,encoding))
                {
                    grpcRequest.RequestMessage = reader.ReadToEnd();
                }
                //兼容请求参数为空
                if (string.IsNullOrEmpty(grpcRequest.RequestMessage))
                {
                    grpcRequest.RequestMessage = "{}";
                }

            }
            catch(Exception)
            {
                return SetError("request parameter error");
            }
            context.DownstreamRequest.Scheme = "grpc";
            return new OkResponse<GrpcRequest>(grpcRequest);
        }

        ErrorResponse<GrpcRequest> SetError(Exception exception)
        {
            return new ErrorResponse<GrpcRequest>(new UnmappableRequestError(exception));
        }
        ErrorResponse<GrpcRequest> SetError(string message)
        {
            var exception = new Exception(message);
            return SetError(exception);
        }

    }
}
