using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Flights.Application.Errors
{
    public class Delete : BaseModel
    {
        #region Data

        public string DeleteCount { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> PostAsync()
        {
            if (!ModelState.IsValid) return View(this);

            try
            {
                if (DeleteCount == "All")
                {
                    await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Error]");
                    Success = "All error records deleted";
                }
                else
                {
                    int totalCount = await _db.Error.CountAsync();
                    int deleteCount = int.Parse(DeleteCount);

                    await _db.Database.ExecuteSqlInterpolatedAsync(
                        $"DELETE FROM [Error] WHERE Id IN (SELECT TOP {deleteCount} Id FROM [Error] ORDER BY Id);");

                    Success = Math.Min(totalCount, deleteCount) + " error records deleted";
                }
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<ErrorsController>();
                logger.LogError(0, ex, "Error deleting error records");

                Failure = "Unable to delete error records";
            }

            return LocalRedirect("/admin/errors");
        }

        #endregion
    }
}