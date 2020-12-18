using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Home
{
    public class Recents : PagedModel<_Viewed>
    {
        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            var viewed = _db.Viewed.AsQueryable();
            viewed = viewed.Where(v => v.UserId == _currentUser.Id)
                           .OrderByDescending(v => v.ViewDate);

            TotalRows = viewed.Count();
            var items = await viewed.Skip(Skip).Take(Take).ToListAsync();

            _mapper.Map(items, Items);

            return View(this);
        }

        #endregion

        // Mapping done in List.cs.
    }
}
