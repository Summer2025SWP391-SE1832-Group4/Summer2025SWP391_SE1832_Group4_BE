using AutoMapper;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;


namespace HIVTreatmentSystem.Application.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPatientRepository _patientRepository;
        private readonly IEmailService _emailService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ITestResultRepository _testResultRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPatientRepository patientRepository,
            IEmailService emailService,
            IDoctorRepository doctorRepository,
            ITestResultRepository testResultRepository
        )
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _patientRepository = patientRepository;
            _emailService = emailService;
            _doctorRepository = doctorRepository;
            _testResultRepository = testResultRepository;
        }

        public async Task<PageResult<AppointmentResponse>> GetAllAppointmentsAsync(
            string? doctorName,
            string? patientName,
            AppointmentTypeEnum? appointmentType,
            AppointmentStatus? status,
            AppointmentServiceEnum? appointmentService,
            DateOnly? startDate,
            DateOnly? endDate,
            int? accountId,
            bool isDescending,
            string? sortBy,
            int pageIndex,
            int pageSize
        )
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAsync(
                doctorName,
                patientName,
                appointmentType,
                status,
                appointmentService,
                startDate,
                endDate,
                accountId,
                isDescending,
                sortBy
            );

            var paged = appointments.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var dtoList = _mapper.Map<List<AppointmentResponse>>(paged);

            return new PageResult<AppointmentResponse>(
                dtoList,
                pageSize,
                pageIndex,
                appointments.Count()
            );
        }

        public async Task<AppointmentResponse?> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(id);

            if (appointment == null)
                return null;

            return _mapper.Map<AppointmentResponse>(appointment);
        }

        public async Task<ApiResponse> CreateAppointmentAsync(AppointmentRequest request)
        {
            try
            {
                var dayOfWeek = request.AppointmentDate.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    return new ApiResponse("Error: Can't create appointment on Sunday.");
                }
                var vnTz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var vnNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTz);

                var today = DateOnly.FromDateTime(vnNow);

                //var minDate = today.AddDays(2);
                //if (request.AppointmentDate < minDate)
                //{
                //    return new ApiResponse(
                //        $"Error: Please book at least 2 days in advance (first available day: {minDate:yyyy-MM-dd}).");
                //}

                var time = request.AppointmentTime;

                bool isMorning = time >= new TimeOnly(8, 0) && time <= new TimeOnly(11, 30);
                bool isAfternoon = time >= new TimeOnly(13, 0) && time <= new TimeOnly(16, 30);

                if (!isMorning && !isAfternoon)
                {
                    return new ApiResponse(
                        "Error: Please create in range 8:00 - 11:30 & 13:00 - 16:30"
                    );
                }

                if (time.Minute != 0 && time.Minute != 30)
                {
                    return new ApiResponse("Error: Please choose 8:00, 8:30, 9:00...");
                }

                var existingAppointments =
                    await _appointmentRepository.GetAppointmentsByDoctorAsync(
                        request.DoctorId,
                        request.AppointmentDate
                    );

                if (existingAppointments.Any(a => a.AppointmentTime == request.AppointmentTime))
                {
                    return new ApiResponse("Error: The doctor is already scheduled at this time..");
                }

                var accountIdStr = _httpContextAccessor
                    .HttpContext?.User?.FindFirst(
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                    )
                    ?.Value;

                if (!int.TryParse(accountIdStr, out var accountId))
                {
                    return new ApiResponse("Error: Invalid AccountId from token.");
                }

                var patient = await _patientRepository.GetByAccountIdAsync(accountId);
                if (patient == null)
                {
                    return new ApiResponse("Error: No patient found for this account.");
                }
                
                if (request.AppointmentType == AppointmentTypeEnum.Therapy)
                {
                    var testResults = await _testResultRepository.GetByPatientIdAsync(patient.PatientId);
                    if (testResults == null || !testResults.Any())
                    {
                        return new ApiResponse(
                            "Error: A test result is required before you can book a therapy appointment.");
                    }
                }

                var appointment = _mapper.Map<Appointment>(request);
                appointment.CreatedByUserId = accountId;
                appointment.PatientId = patient.PatientId;
                await _appointmentRepository.CreateAsync(appointment);

                var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
                var email = patient.Account.Email;
                var dateMail = appointment.AppointmentDate.ToString("dddd, dd MMMM yyyy");
                var timeMail = appointment.AppointmentTime.ToString(@"HH\:mm");
                await _emailService.SendEmailAsync(
                    email,"Your Appointment have been scheduled",
                $@"<html>
  <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
      <h2 style='color: #2e6c80; border-bottom: 1px solid #e0e0e0; padding-bottom: 10px;'>Appointment Confirmation</h2>
      <p style='font-size: 16px;'>Dear <strong>{patient.Account.FullName}</strong>,</p>

      <p style='font-size: 16px;'>
        Your appointment with <strong>Dr. {doctor.Account.FullName}</strong> has been 
        <span style='color: green; font-weight: bold;'>successfully scheduled</span>.
      </p>

      <table style='margin-top: 20px; font-size: 16px;'>
        <tr>
          <td style='padding: 6px 0;'><strong>Date:</strong></td>
          <td style='padding: 6px 0;'>{dateMail}</td>
        </tr>
        <tr>
          <td style='padding: 6px 0;'><strong>Time:</strong></td>
          <td style='padding: 6px 0;'>{timeMail}</td>
        </tr>
      </table>

      <p style='margin-top: 20px; font-size: 16px;'>Please arrive at least <strong>10 minutes early</strong>. If you have any questions, feel free to contact us.</p>

      <p style='margin-top: 40px; font-size: 16px;'>Thank you,<br/><strong>HIV Treatment Center</strong></p>
    </div>
  </body>
</html>"

                        );
                return new ApiResponse("Appointment created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(
                    "Error: Failed to create appointment: " + ex.InnerException.Message
                );
            }
        }

        public async Task<ApiResponse> UpdateAppointmentAsync(
            int id,
            AppointmentUpdateRequest request
        )
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(id);
                if (appointment == null)
                    return new ApiResponse("Error: Appointment not found");

                var appointmentDate = request.AppointmentDate ?? appointment.AppointmentDate;
                var appointmentTime = request.AppointmentTime ?? appointment.AppointmentTime;

                if (appointmentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return new ApiResponse("Error: Can't create appointment on Sunday.");
                }

                bool isMorning =
                    appointmentTime >= new TimeOnly(8, 0)
                    && appointmentTime <= new TimeOnly(11, 30);
                bool isAfternoon =
                    appointmentTime >= new TimeOnly(13, 0)
                    && appointmentTime <= new TimeOnly(16, 30);

                if (!isMorning && !isAfternoon)
                {
                    return new ApiResponse(
                        "Error: Please create in range 8:00 - 11:30 & 13:00 - 16:30"
                    );
                }

                if (appointmentTime.Minute != 0 && appointmentTime.Minute != 30)
                {
                    return new ApiResponse("Error: Please choose 8:00, 8:30, 9:00...");
                }

                var existingAppointments =
                    await _appointmentRepository.GetAppointmentsByDoctorAsync(
                        appointment.DoctorId,
                        appointmentDate
                    );

                if (
                    existingAppointments.Any(a =>
                        a.AppointmentId != id && a.AppointmentTime == appointmentTime
                    )
                )
                {
                    return new ApiResponse("Error: The doctor is already scheduled at this time.");
                }

                if (request.AppointmentDate.HasValue)
                    appointment.AppointmentDate = request.AppointmentDate.Value;

                if (request.AppointmentTime.HasValue)
                    appointment.AppointmentTime = request.AppointmentTime.Value;

                if (request.AppointmentType.HasValue)
                    appointment.AppointmentType = request.AppointmentType.Value;

                if (request.Status.HasValue)
                    appointment.Status = request.Status.Value;

                if (request.AppointmentService.HasValue)
                    appointment.AppointmentService = request.AppointmentService.Value;

                if (!string.IsNullOrWhiteSpace(request.AppointmentNotes))
                    appointment.AppointmentNotes = request.AppointmentNotes;

                await _appointmentRepository.UpdateAsync(appointment);
                return new ApiResponse("Appointment updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(
                    "Error: Failed to update appointment: " + ex.InnerException.Message
                );
            }
        }

        public async Task<ApiResponse> DeleteAppointmentAsync(int appointmentId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(
                    appointmentId
                );
                if (appointment == null)
                    return new ApiResponse("Error: Appointment not found");
                await _appointmentRepository.DeleteAsync(appointment);
                return new ApiResponse("Appointment deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(
                    "Error: Failed to update appointment: " + ex.InnerException.Message
                );
            }
        }

        public async Task<List<AppointmentResponse>> GetAppointmentsByTokenAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var accountIdClaim = _httpContextAccessor
                .HttpContext?.User?.FindFirst(
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                )
                ?.Value;

            if (accountIdClaim == null || !int.TryParse(accountIdClaim, out var accountId))
            {
                throw new UnauthorizedAccessException("AccountId not found or invalid in token.");
            }

            var appointments = await _appointmentRepository.GetAppointmentsByAccountIdAsync(
                accountId
            );
            return _mapper.Map<List<AppointmentResponse>>(appointments);
        }

        public async Task<List<AppointmentResponse>> GetTodaysAppointmentsAsync(string? phoneNumber)
        {
            var today = DateOnly.FromDateTime(
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time")
            );

            var appointments = await _appointmentRepository.GetAppointmentsByDateAsync(
                today,
                phoneNumber
            );
            return _mapper.Map<List<AppointmentResponse>>(appointments);
        }

        public async Task<ApiResponse> SetStatusCheckedInAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(appointmentId);
            if (appointment == null)
            {
                return new ApiResponse("Error: Appointment not found");
            }

            if (appointment.Status == AppointmentStatus.CheckedIn)
            {
                return new ApiResponse("Appointment is already checked in");
            }


            //var start = appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime);
            //var vnTz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            //var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTz);
            //if (Math.Abs((now - start).TotalMinutes) > 5)
            //{
            //    return new ApiResponse(
            //        $"Error: Check-in allowed only from 5 minutes before to 5 minutes after "
            //      + $"the scheduled start ({start:yyyy-MM-dd HH:mm}).");
            //}


            appointment.Status = AppointmentStatus.CheckedIn;
            await _appointmentRepository.UpdateAsync(appointment);
            return new ApiResponse("Checked in successfully", true);
        }

        public async Task<ApiResponse> SetStatusCompletedAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(appointmentId);
            if (appointment == null)
            {
                return new ApiResponse("Error: Appointment not found");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                return new ApiResponse("Appointment is already marked as completed");
            }

            appointment.Status = AppointmentStatus.Completed;
            await _appointmentRepository.UpdateAsync(appointment);

            return new ApiResponse("Appointment marked as completed", true);
        }

        public async Task<ApiResponse> CreateAppointmentForDoctorAsync(AppointmentByDoctorRequest request)
        {
            try
            {
                var dayOfWeek = request.AppointmentDate.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    return new ApiResponse("Error: Can't create appointment on Sunday.");
                }

                var appointmentTime = request.AppointmentTime;

                bool isMorning = appointmentTime >= new TimeOnly(8, 0) && appointmentTime <= new TimeOnly(11, 30);
                bool isAfternoon = appointmentTime >= new TimeOnly(13, 0) && appointmentTime <= new TimeOnly(16, 30);

                if (!isMorning && !isAfternoon)
                {
                    return new ApiResponse("Error: Please create in range 8:00 - 11:30 & 13:00 - 16:30");
                }

                if (appointmentTime.Minute != 0 && appointmentTime.Minute != 30)
                {
                    return new ApiResponse("Error: Please choose 8:00, 8:30, 9:00...");
                }

                //var vnTz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                //var vnNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTz);

                //var today = DateOnly.FromDateTime(vnNow);

                //var minDate = today.AddDays(2);
                //if (request.AppointmentDate < minDate)
                //{
                //    return new ApiResponse(
                //        $"Error: Please book at least 2 days in advance (first available day: {minDate:yyyy-MM-dd}).");
                //}

                var existingAppointments = await _appointmentRepository.GetAppointmentsByDoctorAsync(
                    request.DoctorId, request.AppointmentDate);

                if (existingAppointments.Any(a => a.AppointmentTime == request.AppointmentTime))
                {
                    return new ApiResponse("Error: The doctor is already scheduled at this time.");
                }
                var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);

                var appointment = _mapper.Map<Appointment>(request);
                appointment.CreatedByUserId = doctor.AccountId;

                await _appointmentRepository.CreateAsync(appointment);
                var patient = await _patientRepository.GetByIdAsync(request.PatientId);

                var email = patient.Account.Email;
                var date = appointment.AppointmentDate.ToString("dddd, dd MMMM yyyy");
                var time = appointment.AppointmentTime.ToString(@"HH\:mm");
                await _emailService.SendEmailAsync(
                email,
                "Your Appointment have been scheduled",
                $@"<html>
  <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
      <h2 style='color: #2e6c80; border-bottom: 1px solid #e0e0e0; padding-bottom: 10px;'>Appointment Confirmation</h2>
      <p style='font-size: 16px;'>Dear <strong>{patient.Account.FullName}</strong>,</p>

      <p style='font-size: 16px;'>
        Your appointment with <strong>Dr. {doctor.Account.FullName}</strong> has been 
        <span style='color: green; font-weight: bold;'>successfully scheduled</span>.
      </p>

      <table style='margin-top: 20px; font-size: 16px;'>
        <tr>
          <td style='padding: 6px 0;'><strong>Date:</strong></td>
          <td style='padding: 6px 0;'>{date}</td>
        </tr>
        <tr>
          <td style='padding: 6px 0;'><strong>Time:</strong></td>
          <td style='padding: 6px 0;'>{time}</td>
        </tr>
      </table>

      <p style='margin-top: 20px; font-size: 16px;'>Please arrive at least <strong>10 minutes early</strong>. If you have any questions, feel free to contact us.</p>

      <p style='margin-top: 40px; font-size: 16px;'>Thank you,<br/><strong>HIV Treatment Center</strong></p>
    </div>
  </body>
</html>"

            );
                return new ApiResponse("Appointment created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse("Error: Failed to create appointment: " + ex.InnerException.Message);
            }
        }

    }
}
