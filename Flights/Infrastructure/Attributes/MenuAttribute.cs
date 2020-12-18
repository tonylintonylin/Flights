using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flights
{
    // Confirms the currently active menu
    public class MenuAttribute : ActionFilterAttribute
    {
        private string _menu { get; set; }

        public MenuAttribute(string menu)
        {
            _menu = menu;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            (filterContext.Controller as Controller).ViewBag.Menu = _menu;

            base.OnActionExecuting(filterContext);
        }
    }
}