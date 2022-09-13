using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoNicotin_Business.Queries;

namespace NoNicotineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatternsExampleController : Controller
    {
        public PatternsExampleController(IMediator mediator) => _mediator = mediator;
        private readonly IMediator _mediator;

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var entities = await _mediator.Send(new GetExampleEntitiesQuery());

            return Ok(entities);
        }

    }
}
