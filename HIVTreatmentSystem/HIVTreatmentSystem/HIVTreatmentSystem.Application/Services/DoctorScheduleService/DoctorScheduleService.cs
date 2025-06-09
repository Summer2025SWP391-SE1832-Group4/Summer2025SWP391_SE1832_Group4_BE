// [DOCTOR SCHEDULE API] - Service implementation for managing doctor schedules
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.DoctorSchedule;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Services.DoctorScheduleService
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        // [DOCTOR SCHEDULE API] - Dependencies
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemAuditLogService _auditService;
        private readonly IMonthlyScheduleService _monthlyScheduleService;
        private const int MIN_SLOT_DURATION = 15; // Minimum slot duration in minutes

        // [DOCTOR SCHEDULE API] - Constructor with all required services
        public DoctorScheduleService(
            IUnitOfWork unitOfWork,
            ISystemAuditLogService auditService,
            IMonthlyScheduleService monthlyScheduleService)
        {
            _unitOfWork = unitOfWork;
            _auditService = auditService;
            _monthlyScheduleService = monthlyScheduleService;
        }

        // [DOCTOR SCHEDULE API] - Get schedules by doctor ID
        public async Task<IEnumerable<DoctorScheduleDto>> GetByDoctorIdAsync(int doctorId)
        {
            var schedules = await _unitOfWork.DoctorScheduleRepository
                .GetAll()
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();

            return schedules.Select(ToDto);
        }

        // [DOCTOR SCHEDULE API] - Get schedules by doctor name
        public async Task<IEnumerable<DoctorScheduleDto>> GetByDoctorNameAsync(string doctorName)
        {
            var schedules = await _unitOfWork.DoctorScheduleRepository
                .GetAll()
                .Where(s => s.Doctor.FullName.Contains(doctorName))
                .ToListAsync();

            return schedules.Select(ToDto);
        }

        // [DOCTOR SCHEDULE API] - Get schedule by ID
        public async Task<DoctorScheduleDto> GetByIdAsync(int id)
        {
            var schedule = await _unitOfWork.DoctorScheduleRepository.GetByIdAsync(id);
            return schedule != null ? ToDto(schedule) : null;
        }

        // [DOCTOR SCHEDULE API] - Create new schedule
        public async Task<DoctorScheduleDto> CreateAsync(DoctorScheduleDto dto)
        {
            var schedule = ToEntity(dto);
            await _unitOfWork.DoctorScheduleRepository.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();
            return ToDto(schedule);
        }

        // [DOCTOR SCHEDULE API] - Update existing schedule
        public async Task<DoctorScheduleDto> UpdateAsync(int id, DoctorScheduleDto dto)
        {
            var schedule = await _unitOfWork.DoctorScheduleRepository.GetByIdAsync(id);
            if (schedule == null) return null;

            UpdateEntity(schedule, dto);
            await _unitOfWork.SaveChangesAsync();
            return ToDto(schedule);
        }

        // [DOCTOR SCHEDULE API] - Delete schedule
        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await _unitOfWork.DoctorScheduleRepository.GetByIdAsync(id);
            if (schedule == null) return false;

            _unitOfWork.DoctorScheduleRepository.Delete(schedule);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // [DOCTOR SCHEDULE API] - Create weekly schedules
        public async Task<List<DoctorScheduleDto>> CreateWeeklyScheduleAsync(CreateWeeklyScheduleDto dto)
        {
            try
            {
                // Validate input
                if (dto.StartTime >= dto.EndTime)
                {
                    throw new ArgumentException("Start time must be before end time");
                }

                var slotDuration = dto.SlotDurationMinutes ?? 30;
                if (slotDuration < MIN_SLOT_DURATION)
                {
                    throw new ArgumentException($"Slot duration must be at least {MIN_SLOT_DURATION} minutes");
                }

                var createdSchedules = new List<DoctorScheduleDto>();

                // Get all active doctors
                var doctors = await _unitOfWork.DoctorRepository
                    .GetAll()
                    .Where(d => d.Status == true)
                    .ToListAsync();

                if (!doctors.Any())
                {
                    throw new InvalidOperationException("No active doctors found");
                }

                // Calculate next Monday and Sunday
                var nextMonday = DateTime.Today.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek + 7) % 7);
                var nextSunday = nextMonday.AddDays(6);

                foreach (var doctor in doctors)
                {
                    try
                    {
                        // Check if schedule already exists for this week
                        var existingSchedule = await _unitOfWork.DoctorScheduleRepository
                            .GetAll()
                            .AnyAsync(s => s.DoctorId == doctor.Id && 
                                         s.StartTime.Date >= nextMonday && 
                                         s.EndTime.Date <= nextSunday);

                        if (existingSchedule)
                        {
                            await _auditService.LogAsync(
                                $"Skipped creating schedule for doctor {doctor.Id} - Schedule already exists for week {nextMonday:yyyy-MM-dd} to {nextSunday:yyyy-MM-dd}",
                                "DoctorSchedule",
                                "Create");
                            continue;
                        }

                        // Create time slots for each weekday
                        for (int day = 0; day < 7; day++)
                        {
                            var currentDate = nextMonday.AddDays(day);
                            var startTime = currentDate.Add(dto.StartTime.TimeOfDay);
                            var endTime = currentDate.Add(dto.EndTime.TimeOfDay);

                            // Create slots for the day
                            var currentSlotStart = startTime;
                            while (currentSlotStart.AddMinutes(slotDuration) <= endTime)
                            {
                                var schedule = new DoctorSchedule
                                {
                                    DoctorId = doctor.Id,
                                    StartTime = currentSlotStart,
                                    EndTime = currentSlotStart.AddMinutes(slotDuration),
                                    Status = true
                                };

                                await _unitOfWork.DoctorScheduleRepository.AddAsync(schedule);
                                createdSchedules.Add(ToDto(schedule));
                                currentSlotStart = currentSlotStart.AddMinutes(slotDuration);
                            }
                        }

                        await _auditService.LogAsync(
                            $"Created weekly schedule for doctor {doctor.Id} from {nextMonday:yyyy-MM-dd} to {nextSunday:yyyy-MM-dd}",
                            "DoctorSchedule",
                            "Create");
                    }
                    catch (Exception ex)
                    {
                        await _auditService.LogAsync(
                            $"Error creating schedule for doctor {doctor.Id}: {ex.Message}",
                            "DoctorSchedule",
                            "Error");
                        throw;
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                return createdSchedules;
            }
            catch (Exception ex)
            {
                await _auditService.LogAsync(
                    $"Error in CreateWeeklyScheduleAsync: {ex.Message}",
                    "DoctorSchedule",
                    "Error");
                throw;
            }
        }

        // [DOCTOR SCHEDULE API] - Helper methods for DTO conversion
        private DoctorScheduleDto ToDto(DoctorSchedule entity)
        {
            return new DoctorScheduleDto
            {
                Id = entity.Id,
                DoctorId = entity.DoctorId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Status = entity.Status
            };
        }

        private DoctorSchedule ToEntity(DoctorScheduleDto dto)
        {
            return new DoctorSchedule
            {
                Id = dto.Id,
                DoctorId = dto.DoctorId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status
            };
        }

        private void UpdateEntity(DoctorSchedule entity, DoctorScheduleDto dto)
        {
            entity.DoctorId = dto.DoctorId;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;
            entity.Status = dto.Status;
        }
    }
} 