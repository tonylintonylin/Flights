// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;

// namespace Flights.Application.Issues
// {
//     public class Export : List
//     {
//         #region Data

//         public byte[] Bytes { get; private set; }
//         public string FileName { get; private set; }

//         #endregion

//         #region Handlers

//         public override async Task<IActionResult> GetAsync()
//         {
//             var query = BuildQuery();

//             Bytes = _excel.ExportIssues(await query.ToListAsync());
//             FileName = "ExcelExportedIssues-" + System.DateTime.Now.ToFileStampDateTime() + ".xlsx";

//             return File(Bytes, "application/vnd.ms-excel", FileName);
//         }

//         #endregion
//     }
// }
