using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Flights
{
    // Gets controller reference to actionmodel
    public class ControllerAccessorFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            context.HttpContext.Features.Set<ControllerAccessor>(new ControllerAccessor(controller));

            await next();
        }
    }

    public class ControllerAccessor
    {
        public Controller Controller { get; private set; }

        public ControllerAccessor(Controller controller)
        {
            Controller = controller;
        }
    }
}