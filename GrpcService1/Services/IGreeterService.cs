using Grpc.Core;

namespace GrpcService1.Services
{
    public interface IGreeterService
    {
        Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context);
    }
}