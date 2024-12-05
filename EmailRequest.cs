using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Verify_Duplicate_Submissions_For_Core_API
{
    public class EmailRequest
    {
        [FromForm]
        [DefaultValue("Fly@github.com")]
        public string To { get; set; } = string.Empty;
        [FromForm]
        [DefaultValue("Test")]
        public string Subject { get; set; } = string.Empty;
        [FromForm]
        [DefaultValue("Dear Fly")]
        public string Body { get; set; } = string.Empty;
    }
}
