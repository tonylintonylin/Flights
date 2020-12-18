using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Auth
{
    [Menu("")]
    public class AuthController : Controller
    {
        #region Dependency Injection For Demo Purposes Only

        private readonly FlightsContext _db;

        public AuthController(FlightsContext db)
        {
            _db = db;
        }

        #endregion
        
        #region Login

        [HttpGet("")]
        public ViewResult Login(string returnUrl)
        {
            var model = new Login { ReturnUrl = returnUrl };

            // For demo purposes
            GetUsers(model);
            return View(model);
        }

        // For demo purposes
        private void GetUsers(Login model)
        {
            var list = new List<_User>();

            var users = _db.User.Where(u => new int?[] { 1, 2, 3, 4, 5 }.Contains(u.Id));

            foreach (var user in users.OrderBy(u => u.Id == 1 ? 10 : u.Id)) 
            {
                var _user = new _User
                {
                    UserId = user.Id,
                    User = user.Name,
                    Role = user.Role,
                    Email = user.Email,
                    Password = "Secret123!"
                };

                list.Add(_user);
            }

            model.Users = list;
        }

        [HttpPost("")]
        public async Task<IActionResult> Login([FromForm]Login model) => await model.PostAsync();

        #endregion

        #region Logout

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(Logout model) => await model.GetAsync();

        #endregion

        #region Activate

        [HttpGet("activate/{cd}")]
        public async Task<IActionResult> Activate(Activate model, string cd) => await model.GetAsync();

        [HttpPost("activate/{cd}")]
        public async Task<IActionResult> Activate([FromForm]Activate model) => await model.PostAsync();

        [HttpGet("activateconfirm")]
        public IActionResult ActivateConfirm() => View();

        [HttpGet("activatefailed")]
        public IActionResult ActivateFailed() => View();

        #endregion

        #region Forgot

        [HttpGet("forgot")]
        public IActionResult Forgot() => new Forgot().Get();

        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot(Forgot model) => await model.PostAsync();

        [HttpGet("forgotconfirm")]
        public IActionResult ForgotConfirm() => View();

        #endregion

        #region Reset

        [HttpGet("reset/{cd}")]
        public async Task<IActionResult> Reset(Reset model, string cd) => await model.GetAsync();

        [HttpPost("reset/{cd}")]
        public async Task<IActionResult> Reset(Reset model) => await model.PostAsync();

        [HttpGet("resetconfirm")]
        public IActionResult ResetConfirm() => View();

        [HttpGet("resetfailed")]
        public IActionResult ResetFailed() => View();

        #endregion

        [HttpGet("error")]
        public IActionResult Error(Error model) => model.Get();

        // Catches all requests for which there is no route handler
        [Route("{*url}", Order = 9999)]
        public IActionResult CatchAll(CatchAll model) => model.GetOrPost();
    }
}
