using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flights
{
    public class AdminMenuAttribute : ActionFilterAttribute
    {
        private string _adminMenu { get; set; }

        public AdminMenuAttribute(string adminMenu)
        {
            _adminMenu = adminMenu;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            (filterContext.Controller as Controller).ViewBag.AdminMenu = _adminMenu;

            base.OnActionExecuting(filterContext);
        }
    }
}
