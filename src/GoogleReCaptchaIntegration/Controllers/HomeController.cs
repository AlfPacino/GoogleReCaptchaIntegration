using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace GoogleReCaptchaIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index() => View();

        private const string GoogleApiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
        private bool Validate(string token, string secretKey)
        {
            var req = WebRequest.Create(string.Format(GoogleApiUrl, secretKey, token)) as HttpWebRequest;
            using (WebResponse response = req.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                JObject jObjectResponse = JObject.Parse(stream.ReadToEnd());
                var dto = jObjectResponse.ToObject<GoogleResponseDTO>();
                return dto.IsSuccess;
            }
        }

        public class GoogleResponseDTO
        {
            [JsonProperty("success")]
            public bool IsSuccess { get; set; }
            [JsonProperty("score")]
            public double? Score { get; set; }
        }

        [HttpPost]
        public IActionResult ValidateV2(string token)
        {
            var res = Validate(token, _config.GetValue<string>("CAPTCHA_SECRET_KEY"));
            return Json(new { Success = res });
        }

        [HttpPost]
        public IActionResult ValidateV3(string token)
        {
            var res = Validate(token, _config.GetValue<string>("CAPTCHA_V3_SECRET_KEY"));
            return Json(new { Success = res });
        }

        public IActionResult Submit()
        {
            return Json(new { Success = true });
        }
    }
}
