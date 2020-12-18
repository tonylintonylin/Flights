using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Travelers
{
    public class List : PagedModel<Detail>
    {
        #region Data

        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int TotalBookings { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            var query = BuildQuery();

            TotalRows = await query.CountAsync();
            var items = await query.Skip(Skip).Take(Take).ToListAsync();

            _mapper.Map(items, Items);

            return View(this);
        }

        #endregion

        #region Helpers

        protected IQueryable<Traveler> BuildQuery()
        {
            var query = _db.Traveler.AsQueryable();

            // Filters

            if (AdvancedFilter)
            {
                if (Name != null)
                {
                    query = query.Where(c => c.Name.Contains(Name));
                }

                if (Email != null)
                {
                    query = query.Where(c => c.Email == Email);
                }

                if (City != null)
                {
                    query = query.Where(c => c.City == City);
                }
            }
            else
            {
                switch (Filter)
                {
                    case 1: query = query.Where(c => _viewedService.GetIds("Traveler").Contains(c.Id)); break;
                }
            }

            // Sorting

            query = Sort switch
            {
                "Id" => query.OrderBy(c => c.Id),
                "-Id" => query.OrderByDescending(c => c.Id),
                "FirstName" => query.OrderBy(c => c.FirstName),
                "-FirstName" => query.OrderByDescending(c => c.FirstName),
                "LastName" => query.OrderBy(c => c.LastName),
                "-LastName" => query.OrderByDescending(c => c.LastName),
                "Email" => query.OrderBy(c => c.Email),
                "-Email" => query.OrderByDescending(c => c.Email),
                "City" => query.OrderBy(c => c.City),
                "-City" => query.OrderByDescending(c => c.City),
                "Country" => query.OrderBy(c => c.Country),
                "-Country" => query.OrderByDescending(c => c.Country),
                _ => query.OrderByDescending(c => c.Id),
            };

            return query;
        }

        #endregion

        #region Mapping

        // Mappings are already defined in the associated Detail.cs page

        #endregion
    }
}
