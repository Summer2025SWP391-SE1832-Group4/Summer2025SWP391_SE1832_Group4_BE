namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model for test result data
    /// </summary>
    public class TestResultResponse
    {
        /// <summary>
        /// Unique identifier for the test result
        /// </summary>
        public int TestResultId { get; set; }

        /// <summary>
        /// ID of the patient
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// ID of the associated medical record (if any)
        /// </summary>
        public int? MedicalRecordId { get; set; }

        /// <summary>
        /// ID of the associated appointment (if any)
        /// </summary>
        public int? AppointmentId { get; set; }

        /// <summary>
        /// ID of the doctor (from associated appointment)
        /// </summary>
        public int? DoctorId { get; set; }

        /// <summary>
        /// Doctor full name (from associated appointment)
        /// </summary>
        public string? DoctorFullName { get; set; }

        /// <summary>
        /// Doctor email (from associated appointment)
        /// </summary>
        public string? DoctorEmail { get; set; }

        /// <summary>
        /// Doctor phone number (from associated appointment)
        /// </summary>
        public string? DoctorPhoneNumber { get; set; }

        /// <summary>
        /// Doctor username (from associated appointment)
        /// </summary>
        public string? DoctorUsername { get; set; }

        /// <summary>
        /// Doctor specialty (from associated appointment)
        /// </summary>
        public string? DoctorSpecialty { get; set; }

        /// <summary>
        /// Doctor qualifications (from associated appointment)
        /// </summary>
        public string? DoctorQualifications { get; set; }

        /// <summary>
        /// Doctor years of experience (from associated appointment)
        /// </summary>
        public int? DoctorYearsOfExperience { get; set; }

        /// <summary>
        /// Doctor short description (from associated appointment)
        /// </summary>
        public string? DoctorShortDescription { get; set; }

        /// <summary>
        /// Doctor profile image URL (from associated appointment)
        /// </summary>
        public string? DoctorProfileImageUrl { get; set; }

        /// <summary>
        /// Doctor account status (from associated appointment)
        /// </summary>
        public string? DoctorAccountStatus { get; set; }

        /// <summary>
        /// Doctor role ID (from associated appointment)
        /// </summary>
        public int? DoctorRoleId { get; set; }

        /// <summary>
        /// Patient full name
        /// </summary>
        public string? PatientFullName { get; set; }

        /// <summary>
        /// Patient email
        /// </summary>
        public string? PatientEmail { get; set; }

        /// <summary>
        /// Patient phone number
        /// </summary>
        public string? PatientPhoneNumber { get; set; }

        /// <summary>
        /// Patient username
        /// </summary>
        public string? PatientUsername { get; set; }

        /// <summary>
        /// Patient code at facility
        /// </summary>
        public string? PatientCodeAtFacility { get; set; }

        /// <summary>
        /// Patient date of birth
        /// </summary>
        public DateTime? PatientDateOfBirth { get; set; }

        /// <summary>
        /// Patient gender
        /// </summary>
        public string? PatientGender { get; set; }

        /// <summary>
        /// Patient address
        /// </summary>
        public string? PatientAddress { get; set; }

        /// <summary>
        /// Patient HIV diagnosis date
        /// </summary>
        public DateTime? PatientHivDiagnosisDate { get; set; }

        /// <summary>
        /// Patient consent information
        /// </summary>
        public string? PatientConsentInformation { get; set; }

        /// <summary>
        /// Patient anonymous identifier
        /// </summary>
        public string? PatientAnonymousIdentifier { get; set; }

        /// <summary>
        /// Patient additional notes
        /// </summary>
        public string? PatientAdditionalNotes { get; set; }

        /// <summary>
        /// Patient profile image URL
        /// </summary>
        public string? PatientProfileImageUrl { get; set; }

        /// <summary>
        /// Medical record consultation date
        /// </summary>
        public DateTime? MedicalRecordConsultationDate { get; set; }

        /// <summary>
        /// Medical record symptoms
        /// </summary>
        public string? MedicalRecordSymptoms { get; set; }

        /// <summary>
        /// Medical record diagnosis
        /// </summary>
        public string? MedicalRecordDiagnosis { get; set; }

        /// <summary>
        /// Medical record pregnancy status
        /// </summary>
        public string? MedicalRecordPregnancyStatus { get; set; }

        /// <summary>
        /// Medical record pregnancy week
        /// </summary>
        public int? MedicalRecordPregnancyWeek { get; set; }

        /// <summary>
        /// Medical record doctor notes
        /// </summary>
        public string? MedicalRecordDoctorNotes { get; set; }

        /// <summary>
        /// Medical record next steps
        /// </summary>
        public string? MedicalRecordNextSteps { get; set; }

        /// <summary>
        /// Medical record underlying disease
        /// </summary>
        public string? MedicalRecordUnderlyingDisease { get; set; }

        /// <summary>
        /// Medical record drug allergy history
        /// </summary>
        public string? MedicalRecordDrugAllergyHistory { get; set; }

        /// <summary>
        /// Date when the test was performed
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Type of test performed
        /// </summary>
        public string TestType { get; set; } = string.Empty;

        /// <summary>
        /// CD4 count result (if applicable)
        /// </summary>
        public int? CD4Count { get; set; }

        /// <summary>
        /// HIV viral load value (if applicable)
        /// </summary>
        public string? HivViralLoadValue { get; set; }

        /// <summary>
        /// Name of the laboratory where the test was performed
        /// </summary>
        public string? LabName { get; set; }

        /// <summary>
        /// Comments from the doctor about the test results
        /// </summary>
        public string? DoctorComments { get; set; }

        /// <summary>
        /// Test results data
        /// </summary>
        public string? TestResults { get; set; }
    }
} 