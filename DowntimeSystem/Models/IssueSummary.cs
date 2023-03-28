using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class IssueSummary
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public string Project { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string Issue { get; set; }
        public string Rootcause { get; set; }
        public int? Qty { get; set; }
        public int? Totaldowntime { get; set; }
        public string Action { get; set; }
        public string Editor { get; set; }
        public DateTime Lastupdatedate { get; set; }
        public string Correctiveaction { get; set; }
        public string Preventiveaction { get; set; }
        public string Week { get; set; }
    }
}
