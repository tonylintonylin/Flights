using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Linq;

namespace Flights
{
    // Properties of currently authenticated user

    #region Interface

    public interface ICurrentUser
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string Name { get; }
        string Alias { get; }
        string Email { get; }
        string FirstLetter { get; }

        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsUser { get; }
    }

    #endregion

    public class CurrentUser : ICurrentUser
    {
        #region Dependency Injection

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Services

        public bool IsAuthenticated { get { return IsAdmin || IsUser; } }
        public bool IsAdmin { get { return _httpContextAccessor.HttpContext.User.IsInRole("Admin"); } }
        public bool IsUser { get { return _httpContextAccessor.HttpContext.User.IsInRole("User"); } }

        public int Id { get { return GetClaim(ClaimTypes.UserId).GetId(); } }
        public string FirstName { get { return GetClaim(ClaimTypes.FirstName); } }
        public string LastName { get { return GetClaim(ClaimTypes.LastName); } }
        public string Name { get { return FirstName + " " + LastName; } }
        public string Alias { get { return GetClaim(ClaimTypes.Alias); } }
        public string Email { get { return GetClaim(ClaimTypes.Email); } }
        public string FirstLetter { get { return FirstName?.Substring(0, 1); } }

        #endregion

        #region Helpers

        private string GetClaim(string name)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == name);
            if (claims != null && claims.Any())
                return claims.First().Value;

            return null;
        }

        #endregion
    }
}