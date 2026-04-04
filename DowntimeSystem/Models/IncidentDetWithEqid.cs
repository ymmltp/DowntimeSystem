using System;
using System.Collections.Generic;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class IncidentDetWithEqid
    {
        public int? Id { get; set; }
        public string Comefrom { get; set; }
        public string Alarmtype { get; set; }
        public string Project { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public short? Urgentlevel { get; set; }
        public bool? Calcdowntime { get; set; }
        public int? Downtime { get; set; }
        public double? Labor { get; set; }
        public short? Incidentstatus { get; set; }
        public string Department { get; set; }
        public string Respperson { get; set; }
        public short? Actionstatus { get; set; }
        public string Issue { get; set; }
        public string Issueremark { get; set; }
        public string Rootcause { get; set; }
        public string Rootcauseremark { get; set; }
        public string Action { get; set; }
        public string Actionremark { get; set; }
        public string Creator { get; set; }
        public double? Pieces { get; set; }
        public double? Frequency { get; set; }
        public string Machine { get; set; }

        public DateTime Ctime { get; set; }
        public DateTime Occurtime { get; set; }
        public DateTime? Finishtime { get; set; }
        public DateTime? Repairtime { get; set; }

        // private DateTime? _Ctime;

        // public DateTime? Ctime
        // {
        //     get => _Ctime;
        //     set => _Ctime = value.HasValue
        //         ? (value.Value.Kind == DateTimeKind.Unspecified
        //             ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
        //             : value.Value.ToUniversalTime())
        //         : null;
        // }


        // private DateTime? _Occurtime;

        // public DateTime? Occurtime
        // {
        //     get => _Occurtime;
        //     set => _Occurtime = value.HasValue
        //         ? (value.Value.Kind == DateTimeKind.Unspecified
        //             ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
        //             : value.Value.ToUniversalTime())
        //         : null;
        // }
        // private DateTime? _Finishtime;

        // public DateTime? Finishtime
        // {
        //     get => _Finishtime;
        //     set => _Finishtime = value.HasValue
        //         ? (value.Value.Kind == DateTimeKind.Unspecified
        //             ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
        //             : value.Value.ToUniversalTime())
        //         : null;
        // }
        
        // private DateTime? _Repairtime;
 
        // public DateTime? Repairtime
        // {
        //     get => _Repairtime;
        //     set => _Repairtime = value.HasValue
        //         ? (value.Value.Kind == DateTimeKind.Unspecified
        //             ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
        //             : value.Value.ToUniversalTime())
        //         : null;
        // }
    }
}
