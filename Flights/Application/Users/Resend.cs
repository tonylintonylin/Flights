using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Flights.Application.Users
{
    public class Resend : BaseModel
    {
        #region Data

        public int Id { get; set; }

        #endregion

        #region Handler

        public override async Task<IActionResult> PostAsync()
        {
            var user = await _db.User.SingleAsync(u => u.Id == Id);

            user.ActivationCode = Crypto.RandomString(7);
            user.ActivationDate = null;

            _db.User.Update(user);

            await _db.SaveChangesAsync();

            _email.SendActivationMessage(user);

            Success = "Activation code has been sent";

            return LocalRedirect(Referer ?? "/admin/users");
        }

        #endregion
    }
}
