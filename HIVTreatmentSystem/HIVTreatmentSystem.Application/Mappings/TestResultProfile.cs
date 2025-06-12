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
            CreateMap<TestResult, TestResultResponse>();
            CreateMap<TestResultRequest, TestResult>();
        }
    }
} 