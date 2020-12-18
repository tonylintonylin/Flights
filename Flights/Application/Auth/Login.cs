using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Auth
{
    public class Login : BaseModel
    {
        #region Data

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public List<_User> Users = new List<_User>();

        #endregion

        #region Handlers

        public override async Task<IActionResult> PostAsync()
        {
            if (!ModelState.IsValid) return View(this);

            var login = new Domain.Login
            {
                Email = Email,
                IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                LoginDate = DateTime.Now,
                CreatedOn = DateTime.Now,
                ChangedOn = DateTime.Now
            };

            var result = await _identityService.PasswordSignInAsync(Email, Password);
            
            if (result.Succeeded)
            {
                var user = _db.User.SingleOrDefault(u => u.Email == Email);
                if (user != null)
                {
                    login.UserId = user.Id;
                    login.FirstName = user.FirstName;
                    login.LastName = user.LastName;

                    login.Result = "Success";
                    _db.Login.Add(login);
                    await _db.SaveChangesAsync();

                    user.LastLoginDate = DateTime.Now;
                    _db.User.Update(user);
                    await _db.SaveChangesAsync();

                    return LocalRedirect(ReturnUrl ?? "/home");
                }
            }

            login.Result = "Failure";
            _db.Login.Add(login);
            await _db.SaveChangesAsync();

            Failure = "Login was unsuccessful";
            return View(this);
        }

        #endregion
    }
}
