using System;
using System.Collections.Generic;

namespace Flights.Domain
{
    public partial class IssueHistory
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public string IssueTitle { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OwnerId { get; set; }
        public string OwnerAlias { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public int? ChangedBy { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
