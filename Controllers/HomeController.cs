using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WeatherApp.Models;


namespace WeatherApp.Controllers;

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

    public IActionResult WeatherAPI(WeatherLocation model) 
    {
        if (model.Location == null)
        {
            return RedirectToAction("Index");
        }
        else
        {
            var client = new HttpClient();

            var url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{model.Location}?key=NCSKQBZJMX57EGEPAMDJ4AXLD";

            var response = client.GetStringAsync(url).Result;
            var formattedResponse = JObject.Parse(response);

            var currentConditions = formattedResponse["currentConditions"];
            var time = currentConditions?["datetime"];
            var temperature = currentConditions?["temp"];
            var feelsLike = currentConditions?["feelslike"];
            var uvIndex = currentConditions?["uvindex"];
            var coverage = currentConditions?["conditions"];


            ViewBag.CurrentTime = time;
            ViewBag.Temperature = temperature;
            ViewBag.FeelsLike = feelsLike;
            ViewBag.UvIndex = uvIndex;
            ViewBag.Coverage = coverage;

            return View("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
