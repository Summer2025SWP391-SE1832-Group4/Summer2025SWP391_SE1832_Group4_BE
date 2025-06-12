using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    /// <summary>
    /// AutoMapper profile for Standard ARV Regimen mappings
    /// </summary>
    public class StandardARVRegimenMapper : Profile
    {
        public StandardARVRegimenMapper()
        {
            // Map from Entity to Response
            CreateMap<StandardARVRegimen, StandardARVRegimenResponse>();

            // Map from Request to Entity
            CreateMap<StandardARVRegimenRequest, StandardARVRegimen>();
        }
    }
} 