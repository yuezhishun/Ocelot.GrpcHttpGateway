using Google.Protobuf;
using Grpc.Core;
using System;

namespace Ocelot.GrpcHttpGateway
{
    public static class ArgsParser<T> where T : class, IMessage<T>
    {
        public static MessageParser<T> Parser = new MessageParser<T>(() => Activator.CreateInstance<T>());
        public static Marshaller<T> Marshaller = Marshallers.Create((arg) => MessageExtensions.ToByteArray(arg), Parser.ParseFrom);
    }
}
