using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTechLab.DTOs
{
    public class Filter
    {
        public DateTime? FilterDateStart { get; set; }
        public DateTime? FilterDateEnd { get; set; }
        public uint MinAge { get; set; }
        public uint MaxAge { get; set; }
    }
}
