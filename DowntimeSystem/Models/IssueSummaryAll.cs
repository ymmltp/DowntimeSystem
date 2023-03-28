using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class IssueSummaryAll
    {
        public string Department { get; set; }
        public string Project { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string Issue { get; set; }
        public string Rootcause { get; set; }
        public long? Qty { get; set; }
        public string Action { get; set; }
        public string Editor { get; set; }
        public DateTime? Lastupdatedate { get; set; }
        public string Correctiveaction { get; set; }
        public string Preventiveaction { get; set; }
    }
}
