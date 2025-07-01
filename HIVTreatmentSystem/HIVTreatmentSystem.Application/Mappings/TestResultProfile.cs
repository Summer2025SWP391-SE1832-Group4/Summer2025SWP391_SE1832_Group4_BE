using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for TestResultService entity
    /// </summary>
    public class TestResultProfile : Profile
    {
        /// <summary>
        /// Constructor for TestResultProfile
        /// </summary>
        public TestResultProfile()
        {
            CreateMap<TestResult, TestResultResponse>()
                // Doctor information from Appointment.Doctor
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Appointment != null ? src.Appointment.DoctorId : (int?)null))
                .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null && src.Appointment.Doctor.Account != null ? src.Appointment.Doctor.Account.FullName : null))
                .ForMember(dest => dest.DoctorEmail, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null && src.Appointment.Doctor.Account != null ? src.Appointment.Doctor.Account.Email : null))
                .ForMember(dest => dest.DoctorPhoneNumber, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null && src.Appointment.Doctor.Account != null ? src.Appointment.Doctor.Account.PhoneNumber : null))
                .ForMember(dest => dest.DoctorUsername, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null && src.Appointment.Doctor.Account != null ? src.Appointment.Doctor.Account.Username : null))
                .ForMember(dest => dest.DoctorSpecialty, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null ? src.Appointment.Doctor.Specialty : null))
                .ForMember(dest => dest.DoctorQualifications, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null ? src.Appointment.Doctor.Qualifications : null))
                .ForMember(dest => dest.DoctorYearsOfExperience, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null ? src.Appointment.Doctor.YearsOfExperience : null))
                .ForMember(dest => dest.DoctorShortDescription, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null ? src.Appointment.Doctor.ShortDescription : null))
                .ForMember(dest => dest.DoctorProfileImageUrl, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null && src.Appointment.Doctor.Account != null ? src.Appointment.Doctor.Account.ProfileImageUrl : null))
                .ForMember(dest => dest.DoctorAccountStatus, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null && src.Appointment.Doctor.Account != null ? src.Appointment.Doctor.Account.AccountStatus.ToString() : null))
                .ForMember(dest => dest.DoctorRoleId, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Doctor != null && src.Appointment.Doctor.Account != null ? src.Appointment.Doctor.Account.RoleId : (int?)null))
                
                // Patient information
                .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient != null && src.Patient.Account != null ? src.Patient.Account.FullName : null))
                .ForMember(dest => dest.PatientEmail, opt => opt.MapFrom(src => src.Patient != null && src.Patient.Account != null ? src.Patient.Account.Email : null))
                .ForMember(dest => dest.PatientPhoneNumber, opt => opt.MapFrom(src => src.Patient != null && src.Patient.Account != null ? src.Patient.Account.PhoneNumber : null))
                .ForMember(dest => dest.PatientUsername, opt => opt.MapFrom(src => src.Patient != null && src.Patient.Account != null ? src.Patient.Account.Username : null))
                .ForMember(dest => dest.PatientCodeAtFacility, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientCodeAtFacility : null))
                .ForMember(dest => dest.PatientDateOfBirth, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.DateOfBirth : (DateTime?)null))
                .ForMember(dest => dest.PatientGender, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Gender.ToString() : null))
                .ForMember(dest => dest.PatientAddress, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Address : null))
                .ForMember(dest => dest.PatientHivDiagnosisDate, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.HivDiagnosisDate : (DateTime?)null))
                .ForMember(dest => dest.PatientConsentInformation, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.ConsentInformation : null))
                .ForMember(dest => dest.PatientAnonymousIdentifier, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.AnonymousIdentifier : null))
                .ForMember(dest => dest.PatientAdditionalNotes, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.AdditionalNotes : null))
                .ForMember(dest => dest.PatientProfileImageUrl, opt => opt.MapFrom(src => src.Patient != null && src.Patient.Account != null ? src.Patient.Account.ProfileImageUrl : null))
                
                // Medical record information
                .ForMember(dest => dest.MedicalRecordConsultationDate, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.ConsultationDate : (DateTime?)null))
                .ForMember(dest => dest.MedicalRecordSymptoms, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.Symptoms : null))
                .ForMember(dest => dest.MedicalRecordDiagnosis, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.Diagnosis : null))
                .ForMember(dest => dest.MedicalRecordPregnancyStatus, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.PregnancyStatus.ToString() : null))
                .ForMember(dest => dest.MedicalRecordPregnancyWeek, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.PregnancyWeek : (int?)null))
                .ForMember(dest => dest.MedicalRecordDoctorNotes, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.DoctorNotes : null))
                .ForMember(dest => dest.MedicalRecordNextSteps, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.NextSteps : null))
                .ForMember(dest => dest.MedicalRecordUnderlyingDisease, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.UnderlyingDisease : null))
                .ForMember(dest => dest.MedicalRecordDrugAllergyHistory, opt => opt.MapFrom(src => src.MedicalRecord != null ? src.MedicalRecord.DrugAllergyHistory : null));
            
            CreateMap<TestResultRequest, TestResult>();
        }
    }
} 