using AutoMapper;
using Flights.Application._Related;
using Flights.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Flights
{
    // Base class to all Models
    public class BaseModel
    {
        #region Dependency Injection

        protected static HttpContext HttpContext => ServiceLocator.Resolve<IHttpContextAccessor>().HttpContext;

        private ICache cache;
        private IExcel excel;
        private IEmail email;
        private IMapper mapper;
        private IRollup rollup;
        private FlightsContext db;
        private IRelated related;
        private IWebHostEnvironment env;
        private ICurrentUser currentUser;
        private IViewedService viewedService;
        private ILoggerFactory loggerFactory;
        private IIdentityService identityService;

        protected ICache _cache => cache ??= HttpContext.RequestServices.GetService<ICache>();
        protected IEmail _email => email ??= HttpContext.RequestServices.GetService<IEmail>();
        protected IExcel _excel => excel ??= HttpContext.RequestServices.GetService<IExcel>();
        protected IMapper _mapper => mapper ??= HttpContext.RequestServices.GetService<IMapper>();
        protected IRollup _rollup => rollup ??= HttpContext.RequestServices.GetService<IRollup>();
        protected FlightsContext _db => db ??= HttpContext.RequestServices.GetService<FlightsContext>();
        protected IRelated _related => related ??= HttpContext.RequestServices.GetService<IRelated>();
        protected IWebHostEnvironment _env => env ??= HttpContext.RequestServices.GetService<IWebHostEnvironment>();
        protected ICurrentUser _currentUser => currentUser ??= HttpContext.RequestServices.GetService<ICurrentUser>();
        protected IViewedService _viewedService => viewedService ??= HttpContext.RequestServices.GetService<IViewedService>();
        protected ILoggerFactory _loggerFactory => loggerFactory ??= HttpContext.RequestServices.GetService<ILoggerFactory>();
        protected IIdentityService _identityService => identityService ??= HttpContext.RequestServices.GetService<IIdentityService>();

        #endregion

        #region Controller

        private static Controller Controller { get => HttpContext.Features.Get<ControllerAccessor>()?.Controller; }
      
        // Add/Delete controller methods and properties when neccessary.

        protected static RouteData RouteData { get => Controller.RouteData; }
        protected static HttpRequest Request { get => Controller.Request; }
        protected static HttpResponse Response { get => Controller.Response; }
        protected static ModelStateDictionary ModelState { get => Controller.ModelState; }

        protected virtual IActionResult View() => Controller.View();
        protected virtual IActionResult View(object model) => Controller.View(model);
        protected virtual IActionResult View(string viewName) => Controller.View(viewName);
        protected virtual IActionResult View(string viewName, object model) => Controller.View(viewName, model);

        protected virtual RedirectResult Redirect(string url) => Controller.Redirect(url);
        protected virtual RedirectResult RedirectPermanent(string url) => Controller.RedirectPermanent(url);
        protected virtual LocalRedirectResult LocalRedirect(string localUrl) => Controller.LocalRedirect(localUrl);
        protected virtual LocalRedirectResult LocalRedirectPermanent(string localUrl) => Controller.LocalRedirectPermanent(localUrl);

        protected virtual RedirectToActionResult RedirectToAction(string actionName) => Controller.RedirectToAction(actionName);
        protected virtual RedirectToActionResult RedirectToAction(string actionName, object routeValues) => Controller.RedirectToAction(actionName, routeValues);
        protected virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName) => Controller.RedirectToAction(actionName, controllerName);
        protected virtual RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues) => Controller.RedirectToAction(actionName, controllerName, routeValues);

        protected virtual JsonResult Json(object data) => Controller.Json(data);
        protected virtual JsonResult Json(object data, object serializerSetting) => Controller.Json(data, serializerSetting);

        protected virtual FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName) => Controller.File(fileContents, contentType, fileDownloadName);

        #endregion

        #region Handlers

        public virtual IActionResult Get() => View(this);
        public virtual IActionResult Post() => LocalRedirect(Referer);

        public virtual async Task<IActionResult> GetAsync() { await Task.Yield(); return View(this); }
        public virtual async Task<IActionResult> PostAsync() { await Task.Yield(); return LocalRedirect(Referer); }

        #endregion

        #region Referer

        public string Referer
        {
            get => HttpContext.PostedReferer() ??
                   HttpContext.Referer();
        }

        #endregion

        #region Alert

        public static string Success { set => Controller.TempData["Success"] = value; get => Controller?.TempData["Success"]?.ToString(); }
        public static string Failure { set => Controller.TempData["Failure"] = value; get => Controller?.TempData["Failure"]?.ToString(); }

        #endregion

        #region Meta

        public string MetaTitle { set => Controller.ViewBag.Title = value; get => Controller?.ViewBag.Title; }
        public string MetaKeywords { set => Controller.ViewBag.Keywords = value; get => Controller?.ViewBag.Keywords; }
        public string MetaDescription { set => Controller.ViewBag.Description = value; get => Controller?.ViewBag.Description; }

        #endregion
    }
}
