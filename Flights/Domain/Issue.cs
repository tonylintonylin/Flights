using System;
using System.Collections.Generic;

namespace Flights.Domain
{
    public partial class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public int? AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public int OwnerId { get; set; }
        public string OwnerAlias { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public int? ChangedBy { get; set; }

        public virtual User Owner { get; set; }
        public virtual Project Project { get; set; }
    }
}
