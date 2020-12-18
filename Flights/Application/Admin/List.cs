using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Flights.Application.Admin
{
    public class List : PagedModel<Detail>
    {
        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {    
            Items.Add(new Detail { Icon = _cache.ProjectIcon, Name = "Projects", Url = "/projects", Count = await _db.Project.CountAsync() });
            Items.Add(new Detail { Icon = _cache.IssueIcon, Name = "Issues", Url = "/issues", Count = await _db.Issue.CountAsync() });

            Items.Add(new Detail { Icon = _cache.PeopleIcon, Name = "People", Url = "/people", Count = await _db.User.CountAsync(u => u.IsDeleted == false) });
            
            Items.Add(new Detail { Icon = _cache.UserIcon, Name = "Users", Url = "/admin/users", Count = await _db.User.CountAsync(u => u.IsDeleted == false) });
            Items.Add(new Detail { Icon = _cache.AdminIcon, Name = "Errors", Url = "/admin/errors", Count = await _db.Error.CountAsync() });

            return View(this);
        }

        #endregion
    }
}
