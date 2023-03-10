using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models.PMMS
{
    public partial class Eq
    {
        public int Id { get; set; }
        public string Eqid { get; set; }
        public string Sn { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string Source { get; set; }
        public string Descriptions { get; set; }
        public int? EStatus { get; set; }
        public string Owners { get; set; }
        public string Line { get; set; }
        public string Department { get; set; }
        public string Workcell { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? DeviationDate { get; set; }
    }
}
