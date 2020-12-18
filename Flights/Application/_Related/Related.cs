using Flights.Domain;
using System.Collections.Generic;

namespace Flights.Application._Related
{
    // Prepares related entities for different detail pages

    public class Related : IRelated
    {
        public void Prepare(List<Issue> issues, _Issues model, int totalIssues, int parentId, string parentName)
        {
            model.TotalIssues = totalIssues;
            model.ParentId = parentId;
            model.ParentName = parentName;

            foreach (var issue in issues)
            {
                var _issue = new _Issue { Id = issue.Id, Title = issue.Title, Type = issue.Type, Status = issue.Status , Priority = issue.Priority  };
                model.Issues.Add(_issue);
            }
        }

        public void Prepare(List<Project> projects, _Projects model, int totalProjects, int parentId, string parentName)
        {
            model.TotalProjects = totalProjects;
            model.ParentId = parentId;
            model.ParentName = parentName;

            foreach (var project in projects)
            {
                var _project = new _Project { Id = project.Id, Title = project.Title };
                model.Projects.Add(_project);
            }
        }
    }
}
