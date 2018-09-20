#Ocelot.GrpcHttpGateway
Ocelot.GrpcHttpGateway
#route
    {
      "DownstreamPathTemplate": "/{url}",
      "DownstreamScheme": "grpc",
      "DownstreamHostAndPorts": [
        {
          "Host": "127.0.0.1",
          "Port": 50001
        },
        {
          "Host": "127.0.0.1",
          "Port": 50002
        }

      ],
      "UpstreamPathTemplate": "/grpc/{url}",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      },
      "UpstreamHttpMethod": [ "Get", "Post" ]
    }
#Url
http://domain:port/grpc/ServiceName/MethodName
example ：http://localhost:5000/grpc/HelloGrpc/SayHello/
#proto：

syntax = "proto3";

option csharp_namespace = "GrpcServerImpl";
package GrpcServer;


service HelloGrpc {

  rpc SayHello (HelloRequest) returns (HelloReply) {}
}


message HelloRequest {
  string name = 1;
}


message HelloReply {
  string message = 1;
}




syntax = "proto3";

option csharp_namespace = "GrpcServerImpl";
package GrpcServer;

service HelloNullGrpc {

  rpc SayHello (NullRequest) returns (NullReply) {}

}

message NullRequest {
  
}

message NullReply {

}
