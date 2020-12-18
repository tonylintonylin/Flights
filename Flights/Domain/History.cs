using System;
using System.Collections.Generic;

namespace Flights.Domain
{
    public partial class History
    {
        public int Id { get; set; }
        public string Transaction { get; set; }
        public DateTime HistoryDate { get; set; }
        public string Action { get; set; }
        public string Table { get; set; }
        public int TableId { get; set; }
        public int Version { get; set; }
        public string Content { get; set; }
    }
}
