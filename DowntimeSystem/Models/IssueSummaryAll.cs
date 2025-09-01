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
        public string Correctiveaction { get; set; }
        public string Preventiveaction { get; set; }

        private DateTime? _Lastupdatedate;

        public DateTime? Lastupdatedate
        {
            get => _Lastupdatedate;
            set => _Lastupdatedate = value.HasValue
                ? (value.Value.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                    : value.Value.ToUniversalTime())
                : null;
        }
    }
}
