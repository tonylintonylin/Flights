using System;
using System.Collections.Generic;

namespace Flights.Domain
{
    public partial class User
    {
        public User()
        {
            Error = new HashSet<Error>();
            Issue = new HashSet<Issue>();
            Login = new HashSet<Login>();
            Project = new HashSet<Project>();
            Viewed = new HashSet<Viewed>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Alias { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string EmployeeNumber { get; set; }
        public string Department { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int TotalProjects { get; set; }
        public int TotalIssues { get; set; }
        public string Role { get; set; }
        public string IdentityId { get; set; }
        public string IdentityName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ActivationCode { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string ResetCode { get; set; }
        public DateTime? ResetDate { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Error> Error { get; set; }
        public virtual ICollection<Issue> Issue { get; set; }
        public virtual ICollection<Login> Login { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<Viewed> Viewed { get; set; }
    }
}
