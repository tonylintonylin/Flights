using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Issues
{
    public class Delete : BaseModel
    {
        #region Data

        public int Id { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> PostAsync()
        {
            var history = _db.IssueHistory.Where(s => s.IssueId == Id);
            foreach (var row in history)
            {
                _db.IssueHistory.Remove(row);
            }

            await _db.SaveChangesAsync();

            var issue = await _db.Issue.SingleOrDefaultAsync(c => c.Id == Id);

            _db.Issue.Remove(issue);
            await _db.SaveChangesAsync();

            await SettleDeleteAsync(issue);

            return Json(true);
        }

        #endregion

        #region Helpers

        private async Task SettleDeleteAsync(Issue issue)
        {
            _cache.DeleteIssue(issue);
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM Viewed WHERE WhatId = {issue.Id} AND WhatType = 'Issue';");

            await _rollup.RollupProjectAsync(issue.ProjectId);
            await _rollup.RollupUserAsync(issue.OwnerId);
        }

        #endregion
    }
}
