using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Features.Majitel;
using Microsoft.AspNetCore.Mvc;

namespace EvidenciaDomacichZvierat.Controllers
{
    [Route("api/majitel")]
    public class MajitelController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await Mediator.Send(new GetMajitelList.Query());
            return Ok(response);
        }
    }
}
