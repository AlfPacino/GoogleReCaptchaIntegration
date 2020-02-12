using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        private bool Validate(string token, string secretKey)
        {
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, token);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    var score = jResponse.Value<double>("score");
                    if (isSuccess && jResponse.ContainsKey("score"))
                    {
                        return jResponse.Value<double>("score") > 0.5;
                    }
                    else
                    {
                        return isSuccess;
                    }
                }
            }
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
