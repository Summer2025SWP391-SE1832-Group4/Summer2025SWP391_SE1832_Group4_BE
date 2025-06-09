using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.DTOs;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using HIVTreatmentSystem.Infrastructure.Data;

namespace HIVTreatmentSystem.Infrastructure.Services
{
    public class DoctorScheduleService : IMonthlyScheduleService
    {
        private readonly HIVDbContext _context;

        public DoctorScheduleService(HIVDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorSchedule>> CreateMonthlyScheduleAsync(CreateMonthlyScheduleDTO dto)
        {
            var schedules = new List<DoctorSchedule>();
            
            // Calculate next week's Monday
            var today = DateTime.Today;
            var daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            var nextMonday = today.AddDays(daysUntilMonday);
            var nextSunday = nextMonday.AddDays(6);

            // Get all doctors
            var doctors = await _context.Doctors
                .Include(d => d.Account)
                .ToListAsync();

            foreach (var doctor in doctors)
            {
                // Check if doctor already has schedules in this period
                var existingSchedules = await _context.DoctorSchedules
                    .Where(s => s.DoctorId == doctor.DoctorId &&
                               s.EffectiveFrom <= nextSunday &&
                               (s.EffectiveTo == null || s.EffectiveTo >= nextMonday))
                    .ToListAsync();

                if (existingSchedules.Any())
                {
                    // Skip this doctor if they already have schedules in this period
                    continue;
                }

                // Create schedule for Monday to Friday (5 days)
                for (int day = 1; day <= 5; day++)
                {
                    var schedule = new DoctorSchedule
                    {
                        DoctorId = doctor.DoctorId,
                        DayOfWeek = day, // 1=Monday, 2=Tuesday, ..., 5=Friday
                        StartTime = dto.StartTime,
                        EndTime = dto.EndTime,
                        AvailabilityStatus = ScheduleAvailability.Available,
                        EffectiveFrom = nextMonday,
                        EffectiveTo = nextSunday,
                        SlotDurationMinutes = dto.SlotDurationMinutes ?? 30, // Default 30 minutes if not specified
                        Notes = dto.Notes
                    };

                    schedules.Add(schedule);
                }
            }

            if (schedules.Any())
            {
                await _context.DoctorSchedules.AddRangeAsync(schedules);
                await _context.SaveChangesAsync();
            }

            return schedules;
        }
    }
} 