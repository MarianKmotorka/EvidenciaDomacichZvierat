using System.Threading.Tasks;
using EvidenciaDomacichZvierat.Features.Majitel;
using Microsoft.AspNetCore.Mvc;

namespace EvidenciaDomacichZvierat.Controllers
{
    [Route("api/majitelia")]
    public class MajitelController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await Mediator.Send(new GetMajitelList.Query());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await Mediator.Send(new GetMajitelDetail.Query { Id = id });
            return Ok(response);
        }

        [HttpPost("statistics")]
        public async Task<ActionResult> GetStatistics(GetMajitelStatistics.Query query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
    }
}
