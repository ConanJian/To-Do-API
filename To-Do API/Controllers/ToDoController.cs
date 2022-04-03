using DatabaseConnection;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ToDoController> _logger;
        private readonly IConfiguration _config;

        //How does dependency injector know which IConfiguration to Inject?
        //Isn't the default IConfiguration still a thing? Doesn't seem like it or maybe it got removed?
        public ToDoController(ILogger<ToDoController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet("GetWeather", Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("GetEntireToDoList")]
        public string GetEntireToDoList()
        {
            ToDoConnection connection = new ToDoConnection(_config.GetRequiredSection("ConnectionStrings").GetValue<String>("DefaultConnection"));
            List<ToDoModel> items = connection.GetToDoList();
            
            ToDoModel item = items[0];
            return $"ListNum: {item.ListNum}\n" +
                $"Message: {item.Message}\n" +
                $"Priority: {item.Priority}";
        }
        [HttpGet("GetPriorityList")]
        public string GetPriorityList(int priority)
        {
            ToDoConnection connection = new ToDoConnection(_config.GetRequiredSection("ConnectionStrings").GetValue<String>("DefaultConnection"));
            List<ToDoModel> items = connection.GetPriorityToDoList(priority);

            ToDoModel item = items[0];
            return $"ListNum: {item.ListNum}\n" +
                $"Message: {item.Message}\n" +
                $"Priority: {item.Priority}";
        }
    }
}