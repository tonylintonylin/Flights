using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Issues
{
    public class List : PagedModel<Detail>
    {
        #region Data

        public string Title { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public int? ProjectId { get; set; }
        public int? AssigneeId { get; set; }
        public int? OwnerId { get; set; }

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

        protected IQueryable<Issue> BuildQuery()
        {
            var query = _db.Issue.AsQueryable();

            // Filters

            if (AdvancedFilter)
            {
                if (Title != null)
                {
                    query = query.Where(c => c.Title.Contains(Title));
                }

                if (ProjectId != null)
                {
                    query = query.Where(c => c.ProjectId == ProjectId);
                }

                if (AssigneeId != null)
                {
                    query = query.Where(c => c.AssigneeId == AssigneeId.Value);
                }

                if (OwnerId != null)
                {
                    query = query.Where(c => c.OwnerId == OwnerId.Value);
                }
            }
            else
            {
                switch (Filter)
                {
                    case 1: query = query.Where(c => _viewedService.GetIds("Issue").Contains(c.Id)); break;
                    case 2: query = query.Where(c => c.OwnerId == _currentUser.Id); break;
                }
            }

            // Sorting

            query = Sort switch
            {
                "Id" => query.OrderBy(c => c.Id),
                "-Id" => query.OrderByDescending(c => c.Id),
                "ProjectTitle" => query.OrderBy(c => c.ProjectTitle),
                "-ProjectTitle" => query.OrderByDescending(c => c.ProjectTitle),
                "Title" => query.OrderBy(c => c.Title),
                "-Title" => query.OrderByDescending(c => c.Title),
                "Type" => query.OrderBy(c => c.Type),
                "-Type" => query.OrderByDescending(c => c.Type),
                "Status" => query.OrderBy(c => c.Status),
                "-Status" => query.OrderByDescending(c => c.Status),
                "Priority" => query.OrderBy(c => c.Priority),
                "-Priority" => query.OrderByDescending(c => c.Priority),
                "OwnerAlias" => query.OrderBy(c => c.OwnerAlias),
                "-OwnerAlias" => query.OrderByDescending(c => c.OwnerAlias),
                "AssigneeName" => query.OrderBy(c => c.AssigneeName),
                "-AssigneeName" => query.OrderByDescending(c => c.AssigneeName),
                "CreatedDate" => query.OrderBy(c => c.CreatedDate),
                "-CreatedDate" => query.OrderByDescending(c => c.CreatedDate),
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
