using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Users
{
    public class List : PagedModel<Detail>
    {
        #region Data

        public List() { Sort = "Id"; }

        // Search
        public string q { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            var query = BuildQuery();

            TotalRows = query.Count();
            var items = await query.Skip(Skip).Take(Take).ToListAsync();

            _mapper.Map(items, Items);

            return View(this);
        }

        #endregion

        #region Helpers

        private IQueryable<User> BuildQuery()
        {
            var query = _db.User.AsQueryable().Where(a => a.IsDeleted == false);

            // Search
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(u => u.FirstName.Contains(q) ||
                                         u.LastName.Contains(q) ||
                                         u.Email.Contains(q));
            }

            // Sorting

            query = Sort switch
            {
                "LastName" => query.OrderBy(u => u.LastName),
                "-LastName" => query.OrderByDescending(u => u.LastName),
                "Email" => query.OrderBy(u => u.Email),
                "-Email" => query.OrderByDescending(u => u.Email),
                "Alias" => query.OrderBy(u => u.Alias),
                "-Alias" => query.OrderByDescending(u => u.Alias),
                "EmployeeNumber" => query.OrderBy(u => u.EmployeeNumber),
                "-EmployeeNumber" => query.OrderByDescending(u => u.EmployeeNumber),
                "Department" => query.OrderBy(u => u.Department),
                "-Department" => query.OrderByDescending(u => u.Department),
                "Role" => query.OrderBy(u => u.Role),
                "-Role" => query.OrderByDescending(u => u.Role),
                "LastLoginDate" => query.OrderBy(u => u.LastLoginDate),
                "-LastLoginDate" => query.OrderByDescending(u => u.LastLoginDate),
                "Id" => query.OrderBy(u => u.Id),
                "-Id" => query.OrderByDescending(u => u.Id),
                _ => query.OrderByDescending(u => u.Id),
            };

            return query;
        }

        #endregion

        #region Mapping

        // Mapping done in Detail.cs

        #endregion
    }
}
