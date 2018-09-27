using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoStatusCodeResult : StatusCodeResult
    {
        public WebSocketIoStatusCodeResult() : base(200)
        {
        }

        public override void ExecuteResult(ActionContext context)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
