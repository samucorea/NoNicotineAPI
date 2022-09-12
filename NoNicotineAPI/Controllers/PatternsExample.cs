using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoNicotin_Business.Queries;

namespace NoNicotineAPI.Controllers
{
    public class PatternsExample : Controller
    {
        public PatternsExample(IMediator mediator) => _mediator = mediator;
        private readonly IMediator _mediator;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var entities = await _mediator.Send(new GetExampleEntitiesQuery());

            return Ok(entities);
        }

    }
}
