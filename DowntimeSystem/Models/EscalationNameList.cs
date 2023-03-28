using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class EscalationNameList
    {
        public string Department { get; set; }
        public string Project { get; set; }
        public string Jobtitle { get; set; }
        public int Level { get; set; }
        public string Chinesename { get; set; }
        public string Englishname { get; set; }
        public string Email { get; set; }
        public string Contacttype { get; set; }
    }
}
