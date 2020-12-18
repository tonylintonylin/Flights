using AutoMapper;
using Flights.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Flights
{
    public abstract class BaseProfile : Profile
    {
        // Base class to all Profiles
        #region Dependency Injection

        private static HttpContext HttpContext => ServiceLocator.Resolve<IHttpContextAccessor>().HttpContext;

        private ICurrentUser currentUser;
        protected ICurrentUser _currentUser => currentUser ??= HttpContext.RequestServices.GetService<ICurrentUser>();

        protected ICache _cache => HttpContext.RequestServices.GetService<ICache>();
        protected FlightsContext _db => HttpContext.RequestServices.GetService<FlightsContext>();

        #endregion
    }
}
