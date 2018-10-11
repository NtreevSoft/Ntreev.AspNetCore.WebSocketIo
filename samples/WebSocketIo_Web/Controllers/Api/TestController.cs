using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo;
using WebSocketIo_Web.Models;

namespace WebSocketIo_Web.Controllers.Api
{
    [Route("/api/test")]
    public class TestController : WebSocketIoController
    {
        private readonly IWebSocketIo _webSocketIo;

        public TestController(IWebSocketIo webSocketIo) : base(webSocketIo)
        {
            _webSocketIo = webSocketIo;
        }

        [Route("ping")]
        public IActionResult GetPing()
        {
            return Ok(new
            {
                Status = "ok"
            });
        }

        [Route("websocket")]
        public async Task<IActionResult> GetWebSocketIo()
        {
            await _webSocketIo.SendDataAsync("/* This is only websocket response. */", false);
            
            return Ok(new
            {
                WebSocketData = "ok"
            });
        }

        [Route("user")]
        public IActionResult GetUser(UserModel user)
        {
            return Ok(user);
        }

        [Route("user/{userEmail}")]
        public IActionResult GetUserFromRoute(string userEmail, string q, UserModel user)
        {
            return Ok(new
            {
                Email = userEmail,
                Query = q,
                Id = user?.Id,
                Name = user?.Name
            });
        }
    }
}