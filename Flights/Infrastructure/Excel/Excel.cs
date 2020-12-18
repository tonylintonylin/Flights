using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using System.IO;
using Flights.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Flights
{
    #region Interface

    public interface IExcel
    {
        byte[] ExportProjects(List<Project> projects);
        byte[] ExportIssues(List<Issue> issues);

        byte[] ExportPeople(List<User> users);

        Task<(int, int)> ImportProjectsAsync(string fileName);
        Task<(int, int)> ImportIssuesAsync(string fileName);

        Task<(string fileName, DataGrid grid)> SaveAsync(IFormFile file);
    }
    #endregion

    public class Excel : IExcel
    {
        #region Dependency Injection

        private readonly IWebHostEnvironment _env;
        private readonly ICurrentUser _currentUser;
        private readonly FlightsContext _db;
        private readonly ICache _cache;
        private readonly ILoggerFactory _loggerFactory;

        private string Qualify(string fileName) => Path.Combine(_env.ContentRootPath, @"Data\Imports", fileName);

        public Excel(IWebHostEnvironment env, ICurrentUser currentUser, FlightsContext db,
                    ICache cache, ILoggerFactory loggerFactory)
        {
            _env = env;
            _currentUser = currentUser;
            _db = db;
            _cache = cache;
            _loggerFactory = loggerFactory;
        }

        #endregion

        #region Export

        public byte[] ExportProjects(List<Project> projects)
        {
            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                var row = new Row();

                row.Append(
                    CreateCell("Id", CellValues.String),
                    CreateCell("Title", CellValues.String),
                    CreateCell("Description", CellValues.String),
                    CreateCell("Type", CellValues.String),
                    CreateCell("CreatedDate", CellValues.String),
                    CreateCell("OwnerId", CellValues.String),
                    CreateCell("OwnerAlias", CellValues.String),
                    CreateCell("TotalIssues", CellValues.String));

                sheetData.AppendChild(row);

                foreach (var project in projects)
                {
                    row = new Row();

                    row.Append(
                        CreateCell(project.Id.CleanNumber(), CellValues.Number),
                        CreateCell(project.Title, CellValues.String),
                        CreateCell(project.Description, CellValues.String),
                        CreateCell(project.Type, CellValues.String),
                        CreateCell(project.CreatedDate.ToDate(), CellValues.String),
                        CreateCell(project.OwnerId.CleanNumber(), CellValues.String),
                        CreateCell(project.OwnerAlias, CellValues.String),
                        CreateCell(project.TotalIssues.CleanNumber(), CellValues.Number));

                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
                document.Close();
            }

            stream.Position = 0;
            return stream.ToArray();
        }

        public byte[] ExportIssues(List<Issue> issues)
        {
            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                var row = new Row();

                row.Append(
                    CreateCell("Id", CellValues.String),
                    CreateCell("Title", CellValues.String),
                    CreateCell("Description", CellValues.String),
                    CreateCell("Type", CellValues.String),
                    CreateCell("Status", CellValues.String),
                    CreateCell("Priority", CellValues.String),
                    CreateCell("OwnerAlias", CellValues.String),
                    CreateCell("AssigneeName", CellValues.String),
                    CreateCell("CreatedDate", CellValues.String),
                    CreateCell("OwnerId", CellValues.String),
                    CreateCell("OwnerAlias", CellValues.String));

                sheetData.AppendChild(row);

                foreach (var issue in issues)
                {
                    row = new Row();

                    row.Append(
                        CreateCell(issue.Id.CleanNumber(), CellValues.Number),
                        CreateCell(issue.Title, CellValues.String),
                        CreateCell(issue.Description, CellValues.String),
                        CreateCell(issue.Type, CellValues.String),
                        CreateCell(issue.Status, CellValues.String),
                        CreateCell(issue.Priority, CellValues.String),
                        CreateCell(issue.OwnerAlias, CellValues.String),
                        CreateCell(issue.AssigneeName, CellValues.String),
                        CreateCell(issue.CreatedDate.ToDate(), CellValues.String),
                        CreateCell(issue.OwnerId.CleanNumber(), CellValues.String),
                        CreateCell(issue.OwnerAlias, CellValues.String));

                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
                document.Close();
            }

            stream.Position = 0;
            return stream.ToArray();
        }

        public byte[] ExportPeople(List<User> users)
        {
            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "People" };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                var row = new Row();

                row.Append(
                    CreateCell("Id", CellValues.String),
                    CreateCell("FirstName", CellValues.String),
                    CreateCell("LastName", CellValues.String),
                    CreateCell("Email", CellValues.String),
                    CreateCell("Alias", CellValues.String),
                    CreateCell("City", CellValues.String),
                    CreateCell("Country", CellValues.String),
                    CreateCell("EmployeeNumber", CellValues.String),
                    CreateCell("Department", CellValues.String),
                    CreateCell("LastLoginDate", CellValues.String),
                    CreateCell("TotalProjects", CellValues.String),
                    CreateCell("TotalIssues", CellValues.String),
                    CreateCell("Role", CellValues.String));

                sheetData.AppendChild(row);

                foreach (var user in users)
                {
                    row = new Row();

                    row.Append(
                        CreateCell(user.Id.CleanNumber(), CellValues.Number),
                        CreateCell(user.FirstName, CellValues.String),
                        CreateCell(user.LastName, CellValues.String),
                        CreateCell(user.Email, CellValues.String),
                        CreateCell(user.Alias, CellValues.String),
                        CreateCell(user.City, CellValues.String),
                        CreateCell(user.Country, CellValues.String),
                        CreateCell(user.EmployeeNumber.CleanNumber(), CellValues.Number),
                        CreateCell(user.Department, CellValues.String),
                        CreateCell(user.LastLoginDate.ToDateTime(), CellValues.String),
                        CreateCell(user.TotalProjects.CleanNumber(), CellValues.Number),
                        CreateCell(user.TotalIssues.CleanNumber(), CellValues.Number),
                        CreateCell(user.Role, CellValues.String));

                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
                document.Close();
            }

            stream.Position = 0;
            return stream.ToArray();
        }

        #endregion

        #region Import

        public async Task<(string fileName, DataGrid grid)> SaveAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (extension.IndexOf("xls") == -1) throw new Exception("Invalid file type. Please use a valid Excel file.");

            try
            {
                var fileName = "Excel-" + Crypto.RandomString(10) + extension;
                var filePath = Qualify(fileName);

                // Save file to temporary Imports folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var grid = ReadToGrid(fileName);

                return (fileName, grid);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<Excel>();
                logger.LogError(0, ex, "Error in Excel.SaveForPreviewAsync.");

                throw new ImportException("Unable to read file. Please use a valid Excel file.", ex);
            }
        }

        public async Task<(int, int)> ImportProjectsAsync(string fileName)
        {
            int count = 0;
            var grid = ReadToGrid(fileName);
            foreach (var row in grid.Rows)
            {
                try
                {
                    var project = new Project { CreatedOn = DateTime.Now, ChangedOn = DateTime.Now, CreatedDate = DateTime.Now, CreatedBy = _currentUser.Id, OwnerId = _currentUser.Id, OwnerAlias = _cache.Users[_currentUser.Id].Alias };
                    project.Title = row["Title"];
                    if (row.ContainsKey("Description") && row["Description"] != null) project.Description = row["Description"];
                    if (row.ContainsKey("Type") && row["Type"] != null) project.Type = row["Type"];

                    _db.Project.Add(project);
                    await _db.SaveChangesAsync();

                    count++;
                }
                catch
                {
                    // No logging here. Just one import row less (for now)
                }
            }

            // temp file can be deleted

            var filePath = Qualify(fileName);
            File.Delete(filePath);

            if (count == 0) throw new ImportException("No records were loaded. Please try again", null);

            return (count, grid.Rows.Count);
        }

        public async Task<(int, int)> ImportIssuesAsync(string fileName)
        {
            int count = 0;
            var grid = ReadToGrid(fileName);
            foreach (var row in grid.Rows)
            {
                try
                {
                    var issue = new Issue { CreatedOn = DateTime.Now, ChangedOn = DateTime.Now, CreatedDate = DateTime.Now, CreatedBy = _currentUser.Id, OwnerId = _currentUser.Id, OwnerAlias = _cache.Users[_currentUser.Id].Alias };
                    issue.Title = row["Title"];
                    if (row.ContainsKey("Description") && row["Description"] != null) issue.Description = row["Description"];
                    if (row.ContainsKey("Type") && row["Type"] != null) issue.Type = row["Type"];
                    if (row.ContainsKey("Status") && row["Status"] != null) issue.Status = row["Status"];
                    if (row.ContainsKey("Priority") && row["Priority"] != null) issue.Priority = row["Priority"];
                    // if (row.ContainsKey("ProjectId") && row["ProjectId"] != null) issue.ProjectId = int.Parse(row["ProjectId"].ToString());
                    if (row.ContainsKey("ProjectTitle") && row["ProjectTitle"] != null) issue.ProjectTitle = row["ProjectTitle"];
                    // if (row.ContainsKey("AssigneeId") && row["AssigneeId"] != null) issue.AssigneeId = int.Parse(row["AssigneeId"].ToString());
                    if (row.ContainsKey("AssigneeName") && row["AssigneeName"] != null) issue.AssigneeName = row["AssigneeName"];

                    _db.Issue.Add(issue);
                    await _db.SaveChangesAsync();

                    count++;
                }
                catch
                {
                    // No logging here. Just one import row less (for now)
                }
            }

            // temp file can be deleted

            var filePath = Qualify(fileName);
            File.Delete(filePath);

            if (count == 0) throw new ImportException("No records were loaded. Please try again", null);

            return (count, grid.Rows.Count);
        }

        #endregion

        #region Helpers

        private DataGrid ReadToGrid(string fileName)
        {
            var grid = new DataGrid();

            var filePath = Qualify(fileName);

            var document = SpreadsheetDocument.Open(filePath, false);
            var sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;
            var value = string.Empty;

            bool isheader = true;
            foreach (var worksheetPart in document.WorkbookPart.WorksheetParts)
            {
                foreach (var sheetData in worksheetPart.Worksheet.Elements<SheetData>())
                {
                    if (sheetData.HasChildren)
                    {
                        foreach (var row in sheetData.Elements<Row>())
                        {
                            int columnIndex = 0;
                            var dictionary = new Dictionary<string, string>();

                            foreach (var cell in row.Elements<Cell>())
                            {
                                value = cell.InnerText;

                                if (value != null && cell.DataType != null && cell.DataType == CellValues.SharedString)
                                    value = sharedStringTable.ElementAt(int.Parse(value)).InnerText;

                                if (isheader)
                                {
                                    grid.Headers.Add(value);
                                }
                                else
                                {
                                    var header = grid.Headers[columnIndex];
                                    dictionary.Add(header, value);
                                }

                                columnIndex++;
                            }
        
                            if (!isheader)
                                grid.Rows.Add(dictionary);

                            isheader = false;
                        }
                    }
                }
            }
            document.Close();
            return grid;
        }

        private static Cell CreateCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        #endregion
    }
}
