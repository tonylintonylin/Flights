using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Flights.Application.Issues
{
    public class Edit : BaseModel
    {
        #region Service Locator

        private IIssueService issueService;
        private IIssueService _issueService => issueService ??= ServiceLocator.Resolve<IIssueService>();

        #endregion

        #region Data

        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Project is required")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Assignee is required")]
        public int AssigneeId { get; set; }

        public int? OwnerId { get; set; }
        public string OwnerAlias { get; set; }
        public string OwnerName { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            if (Id == 0)
            {
                OwnerId = _currentUser.Id;
                OwnerName = _currentUser.Name;
                AssigneeId = HttpContext.Request.Query["AssigneeId"].GetId();
                ProjectId = HttpContext.Request.Query["ProjectId"].GetId();
            }
            else
            {
                _mapper.Map(await _db.Issue.SingleOrDefaultAsync(c => c.Id == Id), this);
            }

            return View(this);
        }

        public override async Task<IActionResult> PostAsync()
        {
            if (!ModelState.IsValid) return View(this);

            if (Id == 0) // new item
            {
                var issue = _mapper.Map<Issue>(this);
                issue.CreatedDate = DateTime.Now;

                _db.Issue.Add(issue);
                await _db.SaveChangesAsync();

                await SettleInsertAsync(issue);
            }
            else
            {
                var issue = await _db.Issue.SingleOrDefaultAsync(c => c.Id == Id);
                var originalIssue = new OriginalIssue(issue);

                _mapper.Map(this, issue);

                _db.Issue.Update(issue);
                await _db.SaveChangesAsync();

                await SettleUpdateAsync(originalIssue, issue);
            }

            return LocalRedirect(Referer ?? "/issues");
        }

        #endregion

        #region Helpers

        private async Task SettleInsertAsync(Issue issue)
        {
            await _issueService.LogChangeHistoryAsync(issue);

            _cache.MergeIssue(issue);

            await _rollup.RollupProjectAsync(issue.ProjectId);
            await _rollup.RollupUserAsync(issue.OwnerId);
            await _rollup.RollupUserAsync(issue.AssigneeId);
        }

        private async Task SettleUpdateAsync(OriginalIssue original, Issue issue)
        {
            _cache.MergeIssue(issue);

            if (original.Type != issue.Type || original.Status != issue.Status || original.Priority != issue.Priority)
            {
                await _issueService.LogChangeHistoryAsync(issue);
            }

            if (original.AssigneeId != issue.AssigneeId)
            {
                await _rollup.RollupUserAsync(original.AssigneeId);
                await _rollup.RollupUserAsync(issue.AssigneeId);
            }
            if (original.ProjectId != issue.ProjectId)
            {
                await _rollup.RollupProjectAsync(original.ProjectId);
                await _rollup.RollupProjectAsync(issue.ProjectId);
            }
            if (original.OwnerId != issue.OwnerId)
            {
                await _rollup.RollupUserAsync(original.OwnerId);
                await _rollup.RollupUserAsync(issue.OwnerId);
            }
        }

        private class OriginalIssue
        {
            // Used to temporarily hold a copy of the relevant fields
            public int ProjectId { get; }
            public int? AssigneeId { get; }
            public int OwnerId { get; }

            public string Type { get; }
            public string Status { get; }
            public string Priority { get; }
            
            public OriginalIssue(Issue issue)
            {
                AssigneeId = issue.AssigneeId;
                ProjectId = issue.ProjectId;
                OwnerId = issue.OwnerId;
                Type = issue.Type;
                Status = issue.Status;
                Priority = issue.Priority;
            }
        }

        #endregion

        #region Mapping

        public class MapperProfile : BaseProfile
        {
            public MapperProfile()
            {
                CreateMap<Issue, Edit>()
                    .Map(dest => dest.OwnerName, opt => opt.MapFrom(src => src.OwnerId == 0 ? "" : _cache.Users[src.OwnerId].Name));

                CreateMap<Edit, Issue>()
                    .Map(dest => dest.AssigneeName, opt => opt.MapFrom(src => _cache.Users[src.AssigneeId].Alias))
                    .Map(dest => dest.ProjectTitle, opt => opt.MapFrom(src => _cache.Projects[src.ProjectId].Title))
                    .Map(dest => dest.OwnerAlias, opt => opt.MapFrom(src => _cache.Users[src.OwnerId.Value].Alias));
            }
        }

        #endregion
    }
}
