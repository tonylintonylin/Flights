using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Issues
{
    public class Detail : BaseModel
    {
        #region Data

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public int? AssigneeId { get; set; }
        public string AssigneeName { get; set; }

        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }

        public int? OwnerId { get; set; }
        public string OwnerAlias { get; set; }
        public string OwnerName { get; set; }
        
        public DateTime CreatedDate { get; set; }

        // Related Lists (none)
        public string Tab { get; set; } = "details";

        // History
        public List<_IssueHistory> IssueHistory { get; set; } = new List<_IssueHistory>();

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            var issue = await _db.Issue.SingleOrDefaultAsync(c => c.Id == Id);
            _mapper.Map(issue, this);
            
            var history = await _db.IssueHistory.Where(o => o.IssueId == issue.Id).ToListAsync();
            _mapper.Map(history, IssueHistory);

            await _viewedService.Log(Id, "Issue", issue.Title);

            return View(this);
        }

        #endregion

        #region Mapping

        public class MapperProfile : BaseProfile
        {
            public MapperProfile()
            {
                CreateMap<Issue, Detail>()
                  .Map(dest => dest.OwnerName, opt => opt.MapFrom(src => _cache.Users[src.OwnerId].Name));

                CreateMap<IssueHistory, _IssueHistory>()
                    .Map(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .Map(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .Map(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                    .Map(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToDate()))
                    .Map(dest => dest.OwnerName, opt => opt.MapFrom(src => _cache.Users[src.OwnerId].Name));
            }
        }

        #endregion
    }
}
