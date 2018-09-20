using Google.Protobuf;
using Google.Protobuf.Reflection;
using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ocelot.GrpcHttpGateway
{
    /// <summary>
    /// TODO：用SingletonDictionary进行改造
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="KResult"></typeparam>
    public class GrpcMethod<TRequest, KResult> where TRequest : class, IMessage<TRequest> where KResult : class, IMessage<KResult>
    {
        private static ConcurrentDictionary<MethodDescriptor, Method<TRequest, KResult>> methods
            = new ConcurrentDictionary<MethodDescriptor, Method<TRequest, KResult>>();

        public static Method<TRequest, KResult> GetMethod(MethodDescriptor methodDescriptor)
        {
            Method<TRequest, KResult> method;
            if (methods.TryGetValue(methodDescriptor, out method))
                return method;

            int mtype = 0;
            if (methodDescriptor.IsClientStreaming)
                mtype = 1;
            if (methodDescriptor.IsServerStreaming)
                mtype += 2;
            var methodType = (MethodType)Enum.ToObject(typeof(MethodType), mtype);

            var _method = new Method<TRequest, KResult>(methodType, methodDescriptor.Service.FullName
                , methodDescriptor.Name, ArgsParser<TRequest>.Marshaller, ArgsParser<KResult>.Marshaller);

            methods.TryAdd(methodDescriptor, _method);

            return _method;
        }
    }
}
