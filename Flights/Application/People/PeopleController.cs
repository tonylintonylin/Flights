using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Flights.Application.People
{
    [Authorize]
    [Menu("People")]
    [Route("people")]
    public class PeopleController : Controller
    {
        #region Pages

        [HttpGet]
        public async Task<IActionResult> List(List model) => await model.GetAsync();

        [HttpGet("{id}", Order = 10)]
        public async Task<IActionResult> Detail(Detail model) => await model.GetAsync();

        // Ancillary actions

        [HttpGet("export")]
        public async Task<IActionResult> Export(Export model) => await model.GetAsync();

        #endregion
    }
}

