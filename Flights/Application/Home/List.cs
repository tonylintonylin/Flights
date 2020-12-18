using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Home
{
    public class List : PagedModel
    {
        #region Data
        public int CompletedIssues { get; set; }
        public int NewIssues { get; set; }
        public int OpenProjects { get; set; }
        public int IssueCount { get; set; }
        public int UrgentIssues { get; set; }
   
        public string BarChartLabels { get; set; }
        public string BarChartValues { get; set; }

        public string DealsByStage { get; set; }
        public string PieChartValues { get; set; }

        public List<_Project> Projects { get; } = new List<_Project>();
        public List<_Viewed> Vieweds { get; } = new List<_Viewed>();

        #endregion

        #region Handlers

        private Dictionary<string, Dictionary<string, int>> buckets = new Dictionary<string, Dictionary<string, int>>();

        public override async Task<IActionResult> GetAsync()
        {
            var issues = await _db.Issue.ToListAsync();
            var projects = await _db.Project.ToListAsync();
            var users = await _db.User.ToListAsync();

            CompletedIssues = issues.Where(i => i.Status == "Done").Count();
            NewIssues = issues.Where(i => i.Status == "New").Count();
            OpenProjects = projects.Where(p => p.Issue.Where(i => 
                i.Status == "In Progress"
                || i.Status == "In Review"
                || i.Status == "New").Count() > 0).Count();
            IssueCount = issues.Count();
            UrgentIssues = issues.Where(i => i.Priority == "Urgent").Count();

            // Recently viewed
            var viewed = _db.Viewed.Where(v => v.UserId == _currentUser.Id)
                                   .OrderByDescending(v => v.ViewDate)
                                   .Take(9);

            _mapper.Map(viewed, Vieweds);

            // Top Projects by Issue count
            var topProjects = await _db.Project.OrderByDescending(o => o.TotalIssues)
                                            .Take(7).ToListAsync();

            _mapper.Map(topProjects, Projects);

            return View(this);
        }

        #endregion

        #region Mapping

        public class MapperProfile : BaseProfile
        {
            public MapperProfile()
            {
                CreateMap<Viewed, _Viewed>()
                    .Map(dest => dest.Url, opt => opt.MapFrom(src => _cache.MetaTypes[src.WhatType].Url + "/" + src.WhatId))
                    .Map(dest => dest.Icon, opt => opt.MapFrom(src => _cache.MetaTypes[src.WhatType].Icon))
                    .Map(dest => dest.Name, opt => opt.MapFrom(src => src.WhatName));

                CreateMap<Project, _Project>()
                    .Map(dest => dest.TotalIssues, opt => opt.MapFrom(src => src.TotalIssues));
            }
        }

        #endregion
    }
}
