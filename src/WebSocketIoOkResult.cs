using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoOkResult : OkResult
    {
        public override void ExecuteResult(ActionContext context)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            return Task.CompletedTask;
        }
    }
}