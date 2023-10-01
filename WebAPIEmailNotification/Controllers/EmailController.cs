using Microsoft.AspNetCore.Mvc;
using WebAPIEmailNotification.Interfaces;
using WebAPIEmailNotification.Models;

namespace WebAPIEmailNotification.Controllers
{
    [Route("api/Email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailNotificationService _emailService;

        public EmailController(IEmailNotificationService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("getEmail")]
        public async Task<IActionResult> getEmail()
        {
            try
            {
                EmailModel emailModel = new EmailModel();
                //await _emailService.SendEmailAsync(emailModel);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email: {ex.Message}");
            }
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel emailModel)
        {
           
            try
            {
                if (emailModel != null && emailModel.CustomerEmail != null)
                {
                    await _emailService.SendEmailAsync(emailModel);
                    return Ok("Email sent successfully.");
                }
                else
                {
                    return StatusCode(500, $"Internal Server Error: Some of the customer details is missing - {emailModel}");
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email: {ex.Message}");
            }
        }
    }
}
