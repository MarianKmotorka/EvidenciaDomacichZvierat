using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Features.Zviera;
using Microsoft.AspNetCore.Mvc;

namespace EvidenciaDomacichZvierat.Controllers
{
    [Route("api/zvierata")]
    public class ZvieraController : BaseController
    {
        [HttpPost("{id}/nakrmit")]
        public async Task<ActionResult> Nakrmit(int id)
        {
            await Mediator.Send(new NakrmitZviera.Command { Id = id });
            return NoContent();
        }
    }
}
