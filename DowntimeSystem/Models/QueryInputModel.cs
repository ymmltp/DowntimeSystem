using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DowntimeSystem.Models
{
    public class DowntimeQueryInput
    {
        public int Id { get; set; }
        public string[] Comefrom { get; set; }
        public string[] Project { get; set; }
        public string[] Line { get; set; }
        public string[] Station { get; set; }
        public short? Incidentstatus { get; set; }
        public short? Actionstatus { get; set; }
        public string[] Department { get; set; }
        public string Respperson { get; set; }

         public DateTime starttime { get; set; }
         public DateTime endtime { get; set; }

        // private DateTime _starttime;

        // public DateTime starttime
        // {
        //     get => _starttime;
        //     set => _starttime = value.Kind == DateTimeKind.Unspecified
        //         ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
        //         : value.ToUniversalTime();
        // }
        
        //  private DateTime _endtime;

        // public DateTime endtime
        // {
        //     get => _endtime;
        //     set => _endtime = value.Kind == DateTimeKind.Unspecified
        //         ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
        //         : value.ToUniversalTime();
        // }

    }
}
