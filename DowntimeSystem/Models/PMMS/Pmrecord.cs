using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models.PMMS
{
    public partial class Pmrecord
    {
        public int Prid { get; set; }
        public int? Pmid { get; set; }
        public DateTime? Pmdate { get; set; }
        public string Owner { get; set; }
        public string Operator { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public string DocLocation { get; set; }
        public string LabCode { get; set; }

        public virtual Pmitem Pm { get; set; }
    }
}
