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
            var project = await _db.Project.SingleOrDefaultAsync(c => c.Id == Id);
            _mapper.Map(project, this);
            
            var issues = await _db.Issue.Where(o => o.ProjectId == project.Id).OrderByDescending(o => o.Id).Take(6).ToListAsync();
            _related.Prepare(issues, Issues, project.TotalIssues, project.Id, project.Title);
            
            await _viewedService.Log(Id, "Project", project.Title);

            return View(this);
        }

        #endregion

        #region Mapping

        public class MapperProfile : BaseProfile
        {
            public MapperProfile()
            {
                CreateMap<Project, Detail>()
                  .Map(dest => dest.OwnerName, opt => opt.MapFrom(src => _cache.Users[src.OwnerId].Name));
            }
        }

        #endregion
    }
}
