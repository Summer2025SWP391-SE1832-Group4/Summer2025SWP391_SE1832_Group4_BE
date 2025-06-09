using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.DoctorSchedule;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Service implementation for managing doctor schedules.
    /// </summary>
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _repo;
        private readonly IDoctorRepository _doctorRepo;

        public DoctorScheduleService(IDoctorScheduleRepository repo, IDoctorRepository doctorRepo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
        }

        public async Task<IEnumerable<DoctorScheduleDto>> GetByDoctorIdAsync(int doctorId)
        {
            var list = await _repo.GetByDoctorIdAsync(doctorId);
            return list.Select(x => ToDto(x));
        }

        public async Task<IEnumerable<DoctorScheduleDto>> GetByDoctorNameAsync(string doctorName)
        {
            var list = await _repo.GetByDoctorNameAsync(doctorName);
            return list.Select(x => ToDto(x));
        }

        public async Task<DoctorScheduleDto> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : ToDto(x);
        }

        public async Task<DoctorScheduleDto> CreateAsync(DoctorScheduleDto dto)
        {
            var entity = ToEntity(dto);
            await _repo.AddAsync(entity);
            return ToDto(entity);
        }

        public async Task<DoctorScheduleDto> UpdateAsync(int id, DoctorScheduleDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            
            entity.DayOfWeek = dto.DayOfWeek;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;
            entity.AvailabilityStatus = (ScheduleAvailability)dto.AvailabilityStatus;
            entity.EffectiveFrom = dto.EffectiveFrom;
            entity.EffectiveTo = dto.EffectiveTo;
            entity.SlotDurationMinutes = dto.SlotDurationMinutes;
            entity.Notes = dto.Notes;
            
            _repo.Update(entity);
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Remove(entity);
            return true;
        }

        public async Task<List<DoctorScheduleDto>> CreateWeeklyScheduleAsync(CreateWeeklyScheduleDto dto)
        {
            // Calculate next Monday and Sunday
            var today = DateTime.Today;
            var daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            var nextMonday = today.AddDays(daysUntilMonday);
            var nextSunday = nextMonday.AddDays(6);

            var schedules = new List<DoctorSchedule>();
            var doctors = await _doctorRepo.GetAllAsync();
            var slotDuration = dto.SlotDurationMinutes ?? 30;

            foreach (var doctor in doctors)
            {
                // Check if doctor already has schedule in this time range
                var existingSchedules = await _repo.GetByDoctorIdAsync(doctor.DoctorId);
                if (existingSchedules.Any(s => 
                    s.EffectiveFrom <= nextSunday && 
                    (s.EffectiveTo == null || s.EffectiveTo >= nextMonday)))
                {
                    continue; // Skip if schedule exists
                }

                // Create schedule for 5 days from Monday to Friday
                for (int day = 1; day <= 5; day++)
                {
                    var currentTime = dto.StartTime;
                    while (currentTime < dto.EndTime)
                    {
                        var remainingMinutes = (dto.EndTime - currentTime).TotalMinutes;
                        var currentSlotDuration = Math.Min(slotDuration, remainingMinutes);

                        // Only create slot if remaining time is at least 15 minutes
                        if (currentSlotDuration >= 15)
                        {
                            var schedule = new DoctorSchedule
                            {
                                DoctorId = doctor.DoctorId,
                                DayOfWeek = day,
                                StartTime = currentTime,
                                EndTime = currentTime.Add(TimeSpan.FromMinutes(currentSlotDuration)),
                                AvailabilityStatus = ScheduleAvailability.Available,
                                EffectiveFrom = nextMonday,
                                EffectiveTo = nextSunday,
                                SlotDurationMinutes = (int)currentSlotDuration,
                                Notes = dto.Notes
                            };

                            schedules.Add(schedule);
                            currentTime = currentTime.Add(TimeSpan.FromMinutes(currentSlotDuration));
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            // Save all slots to database
            if (schedules.Any())
            {
                await _repo.AddRangeAsync(schedules);
            }

            return schedules.Select(ToDto).ToList();
        }

        private DoctorScheduleDto ToDto(DoctorSchedule x) => new DoctorScheduleDto
        {
            DoctorId = x.DoctorId,
            DayOfWeek = x.DayOfWeek,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            AvailabilityStatus = (int)x.AvailabilityStatus,
            EffectiveFrom = x.EffectiveFrom,
            EffectiveTo = x.EffectiveTo,
            SlotDurationMinutes = x.SlotDurationMinutes,
            Notes = x.Notes
        };

        private DoctorSchedule ToEntity(DoctorScheduleDto dto) => new DoctorSchedule
        {
            DoctorId = dto.DoctorId,
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            AvailabilityStatus = (ScheduleAvailability)dto.AvailabilityStatus,
            EffectiveFrom = dto.EffectiveFrom,
            EffectiveTo = dto.EffectiveTo,
            SlotDurationMinutes = dto.SlotDurationMinutes,
            Notes = dto.Notes
        };
    }
} 