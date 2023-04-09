using Microsoft.AspNetCore.Mvc;
using MyApiUsage.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace MyApiUsage.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Visualization()
        {
            HttpClient client = new HttpClient();
            var content = client.GetAsync("https://localhost:5001/YouTubeApi/videosbyrequest?request=yarmak").Result;

            var videoList = JsonConvert.DeserializeObject<List<Video>>(content.Content.ReadAsStringAsync().Result);

            return View(videoList);
        }

        [HttpGet]
        public IActionResult UserData()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AboutUser(string? login)
        {
           
            string userData = string.Empty;
            if (!string.IsNullOrEmpty(login))
            {
                using (StreamReader reader = new StreamReader("D:\\Code\\C#\\ynik\\MyApiUsage\\MyApiUsage\\data.txt"))
                {
                    while (true)
                    {
                        var line = reader.ReadLine();
                        if (line == null)
                            break;
                        if (line.StartsWith(login))
                        {
                            userData = line;
                            break;
                        }
                    }
                }
            }
            if (userData!= string.Empty)
            {
                List<string> res = new List<string>(userData.Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries));

                res.RemoveAt(0);

                return View("AboutUser", res);
            }
            return RedirectToAction("UserData");

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}