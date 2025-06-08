using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly HIVDbContext _context;

        public CertificateRepository(HIVDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Certificate>> GetAllCertificatesAsync(
    string? title,
    string? issuedBy,
    string? doctorName,
    DateTime? startDate,
    DateTime? endDate,
    bool isDescending,
    string sortBy)
        {
            var query = _context.Certificates
                .Include(c => c.Doctor)
                .ThenInclude(d => d.Account)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(c => c.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(issuedBy))
                query = query.Where(c => c.IssuedBy.Contains(issuedBy));

            if (!string.IsNullOrWhiteSpace(doctorName))
                query = query.Where(c => c.Doctor.Account.FullName.Contains(doctorName));

            if (startDate.HasValue)
                query = query.Where(c => c.IssuedDate >= startDate.Value);

            if (endDate.HasValue)
            {
                var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(c => c.IssuedDate <= endOfDay);
            }


            query = sortBy?.ToLower() switch
            {
                "title" => isDescending ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
                "issuedby" => isDescending ? query.OrderByDescending(c => c.IssuedBy) : query.OrderBy(c => c.IssuedBy),
                "doctorname" => isDescending ? query.OrderByDescending(c => c.Doctor.Account.FullName) : query.OrderBy(c => c.Doctor.Account.FullName),
                _ => isDescending ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
            };

            return await query.ToListAsync();
        }
        public async Task<Certificate?> GetByIdAsync(int id)
        {
            return await _context.Certificates
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(c => c.CertificateId == id);
        }
        public async Task<Certificate> CreateAsync(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();


            return certificate;
        }

        public async Task<bool> UpdateAsync(Certificate certificate)
        {
            _context.Certificates.Update(certificate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Certificate certificate)
        {
            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
