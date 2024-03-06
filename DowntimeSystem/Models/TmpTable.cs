using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class TmpTable
    {
        public double Downtimeid { get; set; }
        public double Status { get; set; }
        public string Rootcause { get; set; }
        public string Correctaction { get; set; }
    }
}
