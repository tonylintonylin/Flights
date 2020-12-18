using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Flights.Application.Users
{
    public class Detail : BaseModel
    {
        #region Data

        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Alias { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string EmployeeNumber { get; set; }
        public string Department { get; set; }
        public string LastLoginDate { get; set; }
        
        public int TotalProjects { get; set; }
        public int TotalIssues { get; set; }

        public string ActivationCode { get; set; }
        public string ActivationDate { get; set; }
        public string Role { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            var user = await _db.User.SingleAsync(u => u.Id == Id);
            _mapper.Map(user, this);

            return View(this);
        }

        #endregion

        #region Mapping

        public class MapperProfile : BaseProfile
        {
            public MapperProfile()
            {
                CreateMap<User, Detail>()
                   .Map(dest => dest.LastLoginDate, opt => opt.MapFrom(src => src.LastLoginDate.ToDateTime()));
            }
        }

        #endregion
    }
}
