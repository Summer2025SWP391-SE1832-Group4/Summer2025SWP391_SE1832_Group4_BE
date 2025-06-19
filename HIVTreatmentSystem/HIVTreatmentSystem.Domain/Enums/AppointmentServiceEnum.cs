using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Enums
{
    public enum AppointmentServiceEnum
    {
        //Nếu chọn xét nghiệm
        RapidTest,
        PCR,
        ELISA,

        //Nếu chọn tư vấn
        PreTestCounseling,
        PostTestCounseling
    }
}
