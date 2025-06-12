using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    /// <summary>
    /// AutoMapper profile for Medical Record mappings
    /// </summary>
    public class MedicalRecordMapper : Profile
    {
        public MedicalRecordMapper()
        {
            // Map from Entity to Response
            CreateMap<MedicalRecord, MedicalRecordResponse>();

            // Map from Request to Entity
            CreateMap<MedicalRecordRequest, MedicalRecord>();
        }
    }
} 