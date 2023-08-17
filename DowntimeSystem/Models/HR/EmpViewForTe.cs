using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models.HR
{
    public partial class EmpViewForTe
    {
        public string CardId { get; set; }
        public string Empid { get; set; }
        public string Dept { get; set; }
        public string ChineseName { get; set; }
        public string Workcell { get; set; }
    }
}
