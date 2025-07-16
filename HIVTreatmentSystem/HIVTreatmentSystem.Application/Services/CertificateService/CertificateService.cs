using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;


namespace HIVTreatmentSystem.Application.Services.CertificateService
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        public CertificateService(ICertificateRepository certificateRepository, IMapper mapper, IDoctorRepository doctorRepository)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
            _doctorRepository = doctorRepository;
        }

        public async Task<PageResult<CertificateResponse>> GetAllCertificatesAsync(
            string? title,
            string? issuedBy,
            string? doctorName,
            DateTime? startDate,
            DateTime? endDate,
            bool isDescending,
            string sortBy,
            int pageIndex,
            int pageSize)
        {
            var certificates = await _certificateRepository.GetAllCertificatesAsync(
        title, issuedBy, doctorName, startDate, endDate, isDescending, sortBy);


            var pagedCertificates = certificates
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var dtoList = _mapper.Map<List<CertificateResponse>>(pagedCertificates);


            return new PageResult<CertificateResponse>(dtoList, pageSize, pageIndex, certificates.Count());

        }

        public async Task<CertificateResponse?> GetCertificateByIdAsync(int id)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);

            if (certificate == null)
                return null;

            return _mapper.Map<CertificateResponse>(certificate);
        }
        public async Task<bool> CreateCertificateAsync(CertificateRequest request)
        {
            
                var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
                if (doctor == null)
                {
                    throw new ArgumentException("Invalid Doctor ID.");
                }
                if (request.IssuedDate == default)
                {
                    throw new ArgumentException("Invalid IssuedDate.");
                }
                var certificate = _mapper.Map<Certificate>(request);

                certificate = await _certificateRepository.CreateAsync(certificate);

                return true;
        }

        public async Task<bool> UpdateCertificateAsync(int id, CertificateRequest request)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);
            if (certificate == null)
                throw new ArgumentException("Certificate not found.");

            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
                throw new ArgumentException("Invalid Doctor ID.");

            try
            {
                _mapper.Map(request, certificate);
                await _certificateRepository.UpdateAsync(certificate);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update certificate.", ex);
            }
        }

        public async Task<bool> DeleteCertificateAsync(int id)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);
            if (certificate == null)
                throw new ArgumentException("Certificate not found.");

            await _certificateRepository.DeleteAsync(certificate);
            return true;
        }

        public async Task<List<CertificateResponse>> GetCertificatesByDoctorIdAsync(int doctorId)
        {
            // First check if doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new ArgumentException($"Doctor with ID {doctorId} not found.");
            }

            var certificates = await _certificateRepository.GetAllAsync();
            var doctorCertificates = certificates
                .Where(c => c.DoctorId == doctorId)
                .ToList();

            return _mapper.Map<List<CertificateResponse>>(doctorCertificates);
        }
    }
}

