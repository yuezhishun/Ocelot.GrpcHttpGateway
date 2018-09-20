using Grpc.Core;
using GrpcServerImpl;
using System;
using System.Threading.Tasks;

namespace GrpcService
{
    class Program
    {
        static void Main(string[] args)
        {

            int port1 = 50001;
            int port2 = 50002;
            var server1 = new Server
            {
                Services = {
                        HelloGrpc.BindService(new HelloImpl()),
                        HelloNullGrpc.BindService(new HelloNullImpl())
                    },
                Ports = { new ServerPort("localhost", port1, ServerCredentials.Insecure) }
            };
            
            server1.Start();
            Console.WriteLine($"server1 listening on port:{port1}");
            var server2 = new Server
            {
                Services = {
                        HelloGrpc.BindService(new HelloImpl()),
                        HelloNullGrpc.BindService(new HelloNullImpl())
                    },
                Ports = { new ServerPort("localhost", port2, ServerCredentials.Insecure) }
            };
            server2.Start();
            Console.WriteLine($"server2 listening on port:{port2}");

            Console.ReadLine();
            server1.ShutdownAsync().Wait();
            server2.ShutdownAsync().Wait();
        }
    }
    class HelloImpl : HelloGrpc.HelloGrpcBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = context.Host });
        }
    }
    class HelloNullImpl : HelloNullGrpc.HelloNullGrpcBase
    {
        // Server side handler of the SayHello RPC
        public override Task<NullReply> SayHello(NullRequest request, ServerCallContext context)
        {
            return Task.FromResult(new NullReply());
        }
    }

}
