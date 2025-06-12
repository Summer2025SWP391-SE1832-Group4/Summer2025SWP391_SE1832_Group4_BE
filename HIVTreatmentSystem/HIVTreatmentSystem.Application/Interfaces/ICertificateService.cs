using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface ICertificateService
    {
        Task<PageResult<CertificateResponse>> GetAllCertificatesAsync(
        string? title,
        string? issuedBy,
        string? doctorName,
        DateTime? startDate,
        DateTime? endDate,
        bool isDescending,
        string sortBy, 
        int pageIndex,
        int pageSize);

        Task<CertificateResponse?> GetCertificateByIdAsync(int id);
        Task<bool> CreateCertificateAsync(CertificateRequest request);

        Task<bool> UpdateCertificateAsync(int id, CertificateRequest request);
        Task<bool> DeleteCertificateAsync(int id);
        Task<List<CertificateResponse>> GetCertificatesByDoctorIdAsync(int doctorId);
    }

}
