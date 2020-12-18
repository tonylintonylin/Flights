using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Flights.Application.Admin
{
    [Authorize(Roles ="Admin")]
    [Menu("Admin")]
    [Route("admin")]
    [AdminMenu("Overview")]
    public class AdminController : Controller
    {
        #region Pages

        [HttpGet]
        public async Task<IActionResult> List(List model) => await model.GetAsync();

        [HttpGet("rollup")]
        public async Task<IActionResult> Rollup(Rollup model) => await model.GetAsync();

        [HttpGet("clearcache")]
        public IActionResult ClearCache(ClearCache model) => model.Get();

        #endregion
    }
}
