using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Flights.Application.Home
{
    [Authorize]
    [Menu("Home")]
    public class HomeController: Controller
    {
        #region Pages

        [HttpGet("home")]
        public async Task<IActionResult> List(List model) => await model.GetAsync();

        [HttpGet("recents")]
        public async Task<IActionResult> Recents(Recents model) => await model.GetAsync();

        #endregion
    }
}
