using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing feedback records
    /// </summary>
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly HIVDbContext _context;

        public FeedbackRepository(HIVDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Feedback> Items, int TotalCount)> GetPagedAsync(
            int? patientId,
            int? appointmentId,
            int? rating,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Feedbacks.AsNoTracking();

            if (patientId.HasValue)
                query = query.Where(f => f.PatientId == patientId.Value);
            if (appointmentId.HasValue)
                query = query.Where(f => f.AppointmentId == appointmentId.Value);
            if (rating.HasValue)
                query = query.Where(f => f.Rating == rating.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        /// <inheritdoc />
        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks.AsNoTracking().FirstOrDefaultAsync(f => f.FeedbackId == id);
        }

        /// <inheritdoc />
        public async Task<Feedback> AddAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        /// <inheritdoc />
        public async Task<Feedback> UpdateAsync(Feedback feedback)
        {
            _context.Entry(feedback).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return feedback;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Feedbacks.FindAsync(id);
            if (entity == null)
                return false;
            _context.Feedbacks.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<(double AverageRating, int TotalFeedbacks, int OneStarCount, int TwoStarCount, int ThreeStarCount, int FourStarCount, int FiveStarCount)> GetDoctorRatingStatisticsAsync(int doctorId)
        {
            var feedbacksQuery = _context.Feedbacks
                .Include(f => f.Appointment)
                .Where(f => f.Appointment.DoctorId == doctorId)
                .AsNoTracking();

            var feedbacks = await feedbacksQuery.ToListAsync();

            if (!feedbacks.Any())
            {
                return (0, 0, 0, 0, 0, 0, 0);
            }

            var totalFeedbacks = feedbacks.Count;
            var averageRating = feedbacks.Average(f => f.Rating);
            
            var oneStarCount = feedbacks.Count(f => f.Rating == 1);
            var twoStarCount = feedbacks.Count(f => f.Rating == 2);
            var threeStarCount = feedbacks.Count(f => f.Rating == 3);
            var fourStarCount = feedbacks.Count(f => f.Rating == 4);
            var fiveStarCount = feedbacks.Count(f => f.Rating == 5);

            return (averageRating, totalFeedbacks, oneStarCount, twoStarCount, threeStarCount, fourStarCount, fiveStarCount);
        }
    }
} 