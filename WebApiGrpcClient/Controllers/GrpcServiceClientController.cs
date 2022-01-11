using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;

namespace WebApiGrpcClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrpcServiceClientController : ControllerBase
    {

        private readonly ILogger<GrpcServiceClientController> _logger;

        public GrpcServiceClientController(ILogger<GrpcServiceClientController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "CallGrpcService")]
        public async Task<IActionResult> Get()
        {
            var myproxygrpcaddress = "http://localhost:5000";
            using var channel = GrpcChannel.ForAddress(myproxygrpcaddress);
            var client = new Greeter.GreeterClient(channel);
            var helloRequest = new HelloRequest
            {
                Name = "GreeterClient1"
            };
            var reply = await client.SayHelloAsync(helloRequest);
            return Ok(reply.Message);
        }
    }
}