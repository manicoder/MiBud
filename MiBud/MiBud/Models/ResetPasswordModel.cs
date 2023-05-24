using System.Net;

namespace MiBud.Models
{
    public class ResetPasswordModel
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class VerifyOtpRequestModel
    {
        public string email { get; set; }
        public string otp { get; set; }
    }
    public class VerifyOtpResponseModel
    {
        public string message { get; set; }
        public string reset_url { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }
}
