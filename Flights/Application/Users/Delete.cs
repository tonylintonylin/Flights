using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Flights.Application.Users
{
    public class Delete : BaseModel
    {
        #region Data 

        public int Id { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> PostAsync()
        {
            var user = await _db.User.SingleAsync(u => u.Id == Id);

            user.IsDeleted = true;
            user.DeletedBy = _currentUser.Id;
            user.DeletedOn = DateTime.Now;

            _db.User.Update(user);
            await _db.SaveChangesAsync();

            SettleDelete(user);

            return Json(true);
        }

        #endregion

        #region Helpers

        private void SettleDelete(User user)
        {
            // Softdelete
            _cache.MergeUser(user);
        }

        #endregion
    }
}
