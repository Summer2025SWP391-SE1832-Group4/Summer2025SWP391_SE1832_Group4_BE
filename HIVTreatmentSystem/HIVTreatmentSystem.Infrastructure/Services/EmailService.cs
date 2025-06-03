using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIVTreatmentSystem.Infrastructure.Services
{
    /// <summary>
    /// Triển khai gửi email cho hệ thống HIV Treatment System.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Gửi email đơn giản.
        /// </summary>
        /// <param name="to">Địa chỉ nhận.</param>
        /// <param name="subject">Tiêu đề.</param>
        /// <param name="body">Nội dung.</param>
        /// <param name="isHtml">Có phải HTML không.</param>
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = CreateEmailMessage(new[] { to }, subject, body, isHtml);
                await SendEmailAsync(message);
                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}. Subject: {subject}");
                throw;
            }
        }

        /// <summary>
        /// Gửi email cho nhiều người nhận.
        /// </summary>
        /// <param name="to">Danh sách địa chỉ nhận.</param>
        /// <param name="subject">Tiêu đề.</param>
        /// <param name="body">Nội dung.</param>
        /// <param name="isHtml">Có phải HTML không.</param>
        public async Task SendEmailAsync(string[] to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = CreateEmailMessage(to, subject, body, isHtml);
                await SendEmailAsync(message);
                _logger.LogInformation($"Email sent successfully to {string.Join(", ", to)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {string.Join(", ", to)}. Subject: {subject}");
                throw;
            }
        }

        /// <summary>
        /// Gửi nhắc nhở lịch hẹn.
        /// </summary>
        public async Task SendAppointmentReminderAsync(string to, string patientName, string doctorName, DateTime appointmentTime, string location)
        {
            string subject = $"Appointment Reminder - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Appointment Reminder</h2>
                    </div>
                    <p>Dear <strong>{patientName}</strong>,</p>
                    <p>This is a reminder for your upcoming appointment at HIV Treatment System:</p>
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;'>
                        <p><strong>Doctor:</strong> {doctorName}</p>
                        <p><strong>Date & Time:</strong> {appointmentTime:dd/MM/yyyy HH:mm}</p>
                        <p><strong>Location:</strong> {location}</p>
                    </div>
                    <p>Please arrive on time. If you need to reschedule, kindly contact us at least 24 hours in advance.</p>
                    <p style='margin-top: 30px;'>Best regards,<br>The HIV Treatment System Team</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi nhắc nhở uống thuốc.
        /// </summary>
        public async Task SendMedicationReminderAsync(string to, string patientName, string medicationName, string dosage, string instructions)
        {
            string subject = $"Medication Reminder - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Medication Reminder</h2>
                    </div>
                    <p>Dear <strong>{patientName}</strong>,</p>
                    <p>This is a reminder to take your medication as prescribed:</p>
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;'>
                        <p><strong>Medication:</strong> {medicationName}</p>
                        <p><strong>Dosage:</strong> {dosage}</p>
                        <p><strong>Instructions:</strong> {instructions}</p>
                    </div>
                    <p>Adhering to your treatment plan is crucial for your health and well-being.</p>
                    <p style='margin-top: 30px;'>Best regards,<br>The HIV Treatment System Team</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi email đặt lại mật khẩu.
        /// </summary>
        public async Task SendPasswordResetAsync(string to, string resetLink, string userName)
        {
            string subject = $"Password Reset Request - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Password Reset Request</h2>
                    </div>
                    <p>Dear <strong>{userName}</strong>,</p>
                    <p>We have received a request to reset your account password. Please click the button below to reset your password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' style='background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Reset Password</a>
                    </div>
                    <p>This link will expire in 24 hours. If you did not request a password reset, please ignore this email or contact us if you have any concerns.</p>
                    <p style='margin-top: 30px;'>Best regards,<br>The HIV Treatment System Team</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi email xác minh tài khoản.
        /// </summary>
        public async Task SendAccountVerificationAsync(string to, string verificationLink, string userName)
        {
            string subject = $"Account Verification - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Account Verification</h2>
                    </div>
                    <p>Dear <strong>{userName}</strong>,</p>
                    <p>Thank you for registering with the HIV Treatment System. To complete your registration, please verify your email address by clicking the button below:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{verificationLink}' style='background-color: #27ae60; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Verify Account</a>
                    </div>
                    <p>If you did not create this account, please ignore this email.</p>
                    <p style='margin-top: 30px;'>Best regards,<br>The HIV Treatment System Team</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi thông báo kết quả xét nghiệm.
        /// </summary>
        public async Task SendTestResultsNotificationAsync(string to, string patientName, string testType, DateTime testDate)
        {
            string subject = $"Test Results Notification - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Test Results Notification</h2>
                    </div>
                    <p>Dear <strong>{patientName}</strong>,</p>
                    <p>Your <strong>{testType}</strong> test results dated <strong>{testDate:dd/MM/yyyy}</strong> are now available.</p>
                    <p>Please log in to the system or contact your doctor to view the details.</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='https://hivtreatmentsystem.com/login' style='background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Log In</a>
                    </div>
                    <p style='margin-top: 30px;'>Best regards,<br>The HIV Treatment System Team</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        private MailMessage CreateEmailMessage(string[] to, string subject, string body, bool isHtml)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            foreach (var toAddress in to)
            {
                message.To.Add(toAddress);
            }

            return message;
        }

        private async Task SendEmailAsync(MailMessage message)
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass)
            };

            await client.SendMailAsync(message);
        }
    }
}
