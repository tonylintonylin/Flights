using System;
using System.Collections.Generic;

namespace Flights.Domain
{
    public partial class Login
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public DateTime LoginDate { get; set; }
        public string Result { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public int? ChangedBy { get; set; }

        public virtual User User { get; set; }
    }
}
