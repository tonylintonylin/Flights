using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Flights.Application.Auth
{
    public class Reset : BaseModel
    {
        #region Data

        // reset code
        public string Code { get; set; } 

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,35}$", ErrorMessage = "Password must be at least 8 characters long and contain 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and Confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            var user = await _db.User.SingleAsync(u => u.ResetCode == Code && u.ResetDate == null);
            if (user != null) return View(this);

            return RedirectToAction("ResetFailed");
        }

        public override async Task<IActionResult> PostAsync()
        {
            if (!ModelState.IsValid) return View(this);

            var user = await _db.User.SingleAsync(u => u.ResetCode == Code && u.ResetDate == null);
            if (user != null)
            {
                await _identityService.ResetPasswordAsync(user, NewPassword);

                user.ResetDate = DateTime.Now;
                _db.User.Update(user);
                await _db.SaveChangesAsync();

                return RedirectToAction("ResetConfirm");
            }

            Failure = "Password reset was unsuccessful.";

            return View(this);
        }

        #endregion
    }
}