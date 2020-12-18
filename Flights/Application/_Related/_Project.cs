using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application._Related
{
    public class _Projects 
    {
        public int TotalProjects  { get; set; }
        public List<_Project> Projects  { get; set; } = new List<_Project>();
        public int ParentId { get; set; }
        public string ParentIdName { get; set; }
        public string ParentName { get; set; }
    }

    public class _Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int TotalIssues { get; set; }
    }
}
