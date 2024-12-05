using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;

namespace Verify_Duplicate_Submissions_For_Core_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKeyPrefix = "EmailRequest_";

        public EmailController(ILogger<EmailController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromForm] EmailRequest emailRequest)
        {
            try
            {
                // 生成缓存键, 用于存储请求, 防止重复提交
                string cacheKey = $"{CacheKeyPrefix}{emailRequest.To}_{emailRequest.Subject}_{emailRequest.Body.GetHashCode()}";
                // 如果缓存中存在相同的请求, 则返回错误
                if (_memoryCache.TryGetValue(cacheKey, out _))
                {
                    return BadRequest("Duplicate email request detected.");
                }

                #region 此处不实现发送邮件
                //var message = new MimeMessage();
                //message.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
                //message.To.Add(new MailboxAddress("", emailRequest.To));
                //message.Subject = emailRequest.Subject;

                //message.Body = new TextPart("plain")
                //{
                //    Text = emailRequest.Body
                //};

                //using (var client = new SmtpClient())
                //{
                //    client.Connect("smtp.example.com", 587, false);
                //    client.Authenticate("your-email@example.com", "your-email-password");

                //    client.Send(message);
                //    client.Disconnect(true);
                //}
                #endregion


                // 将请求存储到缓存中，有效期设置为5秒钟
                _memoryCache.Set(cacheKey, emailRequest, TimeSpan.FromSeconds(5));

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
