using Flights.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Flights
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
    {
        #region Dependency Injection

        private readonly ICache _cache;
        private readonly FlightsContext _db;
        private readonly ILoggerFactory _loggerFactory;

        public ClaimsPrincipalFactory(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            FlightsContext db,
            ICache cache,
            ILoggerFactory loggerFactory) : base(userManager, roleManager, optionsAccessor)
        {
            _db = db;
            _cache = cache;
            _loggerFactory = loggerFactory;
        }

        #endregion

        public async override Task<ClaimsPrincipal> CreateAsync(IdentityUser appUser)
        {
            try
            {
                var principal = await base.CreateAsync(appUser);
                var identity = ((ClaimsIdentity)principal.Identity);

                var user = _db.User.AsNoTracking().Single(u => u.IdentityId == appUser.Id);

                // Add User claims
                identity.AddClaim(new Claim(ClaimTypes.UserId, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.FirstName, user.FirstName));
                identity.AddClaim(new Claim(ClaimTypes.LastName, user.LastName));
                identity.AddClaim(new Claim(ClaimTypes.Alias, user.Alias));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
             
                return principal;

            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<ClaimsPrincipalFactory>();
                logger.LogError(0, ex, "Error in CreateAsync in ClaimPrincipalFactory.");

                throw new Exception("In CreateAsync. A claim value is possibly null, this is not allowed.", ex);
            }
        }
    }
}
