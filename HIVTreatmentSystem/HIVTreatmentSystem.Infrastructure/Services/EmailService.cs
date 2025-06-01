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
            string subject = $"Nhắc nhở lịch hẹn - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Nhắc nhở lịch hẹn</h2>
                    </div>
                    <p>Xin chào <strong>{patientName}</strong>,</p>
                    <p>Đây là email nhắc nhở về lịch hẹn sắp tới của bạn tại HIV Treatment System:</p>
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;'>
                        <p><strong>Bác sĩ:</strong> {doctorName}</p>
                        <p><strong>Thời gian:</strong> {appointmentTime:dd/MM/yyyy HH:mm}</p>
                        <p><strong>Địa điểm:</strong> {location}</p>
                    </div>
                    <p>Vui lòng đến đúng giờ. Nếu bạn cần thay đổi lịch hẹn, vui lòng liên hệ với chúng tôi trước ít nhất 24 giờ.</p>
                    <p style='margin-top: 30px;'>Trân trọng,<br>Đội ngũ HIV Treatment System</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi nhắc nhở uống thuốc.
        /// </summary>
        public async Task SendMedicationReminderAsync(string to, string patientName, string medicationName, string dosage, string instructions)
        {
            string subject = $"Nhắc nhở uống thuốc - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Nhắc nhở uống thuốc</h2>
                    </div>
                    <p>Xin chào <strong>{patientName}</strong>,</p>
                    <p>Đây là email nhắc nhở bạn uống thuốc đúng giờ theo chỉ định:</p>
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;'>
                        <p><strong>Thuốc:</strong> {medicationName}</p>
                        <p><strong>Liều lượng:</strong> {dosage}</p>
                        <p><strong>Hướng dẫn:</strong> {instructions}</p>
                    </div>
                    <p>Việc tuân thủ đúng liệu trình điều trị rất quan trọng cho sức khỏe của bạn.</p>
                    <p style='margin-top: 30px;'>Trân trọng,<br>Đội ngũ HIV Treatment System</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi email đặt lại mật khẩu.
        /// </summary>
        public async Task SendPasswordResetAsync(string to, string resetLink, string userName)
        {
            string subject = $"Đặt lại mật khẩu - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Yêu cầu đặt lại mật khẩu</h2>
                    </div>
                    <p>Xin chào <strong>{userName}</strong>,</p>
                    <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. Vui lòng nhấp vào nút bên dưới để đặt lại mật khẩu:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' style='background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Đặt lại mật khẩu</a>
                    </div>
                    <p>Liên kết này sẽ hết hạn sau 24 giờ. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này hoặc liên hệ với chúng tôi nếu bạn có thắc mắc.</p>
                    <p style='margin-top: 30px;'>Trân trọng,<br>Đội ngũ HIV Treatment System</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi email xác minh tài khoản.
        /// </summary>
        public async Task SendAccountVerificationAsync(string to, string verificationLink, string userName)
        {
            string subject = $"Xác minh tài khoản - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Xác minh tài khoản</h2>
                    </div>
                    <p>Xin chào <strong>{userName}</strong>,</p>
                    <p>Cảm ơn bạn đã đăng ký tài khoản tại HIV Treatment System. Để hoàn tất quá trình đăng ký, vui lòng xác minh email của bạn bằng cách nhấp vào nút bên dưới:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{verificationLink}' style='background-color: #27ae60; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Xác minh tài khoản</a>
                    </div>
                    <p>Nếu bạn không tạo tài khoản này, vui lòng bỏ qua email này.</p>
                    <p style='margin-top: 30px;'>Trân trọng,<br>Đội ngũ HIV Treatment System</p>
                </div>
            ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Gửi thông báo kết quả xét nghiệm.
        /// </summary>
        public async Task SendTestResultsNotificationAsync(string to, string patientName, string testType, DateTime testDate)
        {
            string subject = $"Thông báo kết quả xét nghiệm - HIV Treatment System";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #3498db;'>Thông báo kết quả xét nghiệm</h2>
                    </div>
                    <p>Xin chào <strong>{patientName}</strong>,</p>
                    <p>Kết quả xét nghiệm <strong>{testType}</strong> của bạn ngày <strong>{testDate:dd/MM/yyyy}</strong> đã có.</p>
                    <p>Vui lòng đăng nhập vào hệ thống hoặc liên hệ với bác sĩ của bạn để xem chi tiết kết quả.</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='https://hivtreatmentsystem.com/login' style='background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Đăng nhập</a>
                    </div>
                    <p style='margin-top: 30px;'>Trân trọng,<br>Đội ngũ HIV Treatment System</p>
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
