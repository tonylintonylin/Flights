using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Flights.Application.Projects
{
    public class Delete : BaseModel
    {
        #region Data

        public int Id { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> PostAsync()
        {
            var project = await _db.Project.SingleOrDefaultAsync(c => c.Id == Id);
            _db.Project.Remove(project);
            await _db.SaveChangesAsync();

            await SettleDeleteAsync(project);

            return Json(true);
        }

        #endregion

        #region Helpers

        private async Task SettleDeleteAsync(Project project)
        {
            _cache.DeleteProject(project);
            await _db.Database.ExecuteSqlInterpolatedAsync(
                     $"DELETE FROM Viewed WHERE WhatId = {project.Id} AND WhatType = 'Project';");

            await _rollup.RollupUserAsync(project.OwnerId);
        }

        #endregion
    }
}
