using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

public interface ICertificateRepository
{
    public Task<IEnumerable<Certificate>> GetAllCertificatesAsync(
    string? title,
    string? issuedBy,
    string? doctorName,
    DateTime? startDate,
    DateTime? endDate,
    bool isDescending,
    string sortBy
);
    Task<Certificate?> GetByIdAsync(int id);
    Task<Certificate> CreateAsync(Certificate certificate);

    Task<bool> UpdateAsync(Certificate certificate);

    Task<bool> DeleteAsync(Certificate certificate);

    // Thêm phương thức này
    Task<List<Certificate>> GetAllAsync();
}
