using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights
{
    // Intermediary between entities and excel rows

    public class DataGrid
    {
        public List<string> Headers { get; } = new List<string>();
        public Dictionary<string, string> Types { get; } = new Dictionary<string, string>();
        public List<Dictionary<string, string>> Rows { get; } = new List<Dictionary<string, string>>();
    }
}
