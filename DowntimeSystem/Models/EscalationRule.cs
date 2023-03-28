using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class EscalationRule
    {
        public string Department { get; set; }
        public string Project { get; set; }
        public int Level { get; set; }
        public int Timespan { get; set; }
    }
}
