using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrpcService1.Services
{
    [Controller]
    [Produces("application/json")]
    public class HelloController : Controller
    {
        private readonly ILogger<HelloController> _logger;
        
        /// <summary>
        /// Hello constructor
        /// </summary>
        public HelloController(ILogger<HelloController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// A simple method to say hello
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /hello/{name}
        ///
        /// </remarks>
        /// <param name="name"></param>
        [HttpGet("{name}", Name = "SayHello")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HelloReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SayHello(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();
            
            return Ok();
        }
    }
}
