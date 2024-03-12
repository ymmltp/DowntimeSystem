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

    }
}
