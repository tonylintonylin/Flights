using Flights.Domain;
using System.Collections.Generic;

namespace Flights.Application._Related
{
    public interface IRelated
    {
        void Prepare(List<Issue> issues, _Issues model, int totalIssues, int parentId, string parentName);
        void Prepare(List<Project> projects, _Projects model, int totalProjects, int parentId, string parentName);
    }
}
