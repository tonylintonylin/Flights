using Flights.Application._Related;
using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Travelers
{
    public class Detail : BaseModel
    {
        #region Data

        public int? Id { get; set; }
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
            var traveler = await _db.Traveler.SingleOrDefaultAsync(c => c.Id == Id);
            _mapper.Map(traveler, this);

            return View(this);
        }

        #endregion
    }
}
