using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class TreatmentStatusCountResponse
    {
        public int InTreatment { get; set; }
        public int Completed { get; set; }
        public int Discontinued { get; set; }
        public int SwitchedRegimen { get; set; }
    }
}
