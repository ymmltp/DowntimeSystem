using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models.PMMS
{
    public partial class Pmitem
    {
        public Pmitem()
        {
            Pmrecords = new HashSet<Pmrecord>();
        }

        public int Pmid { get; set; }
        public string Eqid { get; set; }
        public string Pmtype { get; set; }
        public int? Cycle { get; set; }
        public string PmitemStatus { get; set; }
        public DateTime? LastConfirmDate { get; set; }
        public string VerifyResult { get; set; }
        public DateTime? VerifyTime { get; set; }
        public string LockAt { get; set; }

        public virtual ICollection<Pmrecord> Pmrecords { get; set; }
    }
}
