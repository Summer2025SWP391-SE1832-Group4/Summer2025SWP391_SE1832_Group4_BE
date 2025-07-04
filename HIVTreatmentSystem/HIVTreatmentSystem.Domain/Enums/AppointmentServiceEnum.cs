

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
        PostTestCounseling,

        // Nếu chọn điều trị
        FirstTreatmentVisit,
        FollowUpTreatment

    }
}
