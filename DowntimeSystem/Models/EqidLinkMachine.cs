using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class EqidLinkMachine
    {
        public string Eqid { get; set; }
        public string Machine { get; set; }
        public string Department { get; set; }
        public string Project { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public double? Index { get; set; }
    }
}
