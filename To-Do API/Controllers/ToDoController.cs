using DatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        private readonly ToDoConnection _connection;

        //How does dependency injector know which IConfiguration to Inject?
        //Isn't the default IConfiguration still a thing? Doesn't seem like it or maybe it got removed?
        public ToDoController(ILogger<ToDoController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _connection = new ToDoConnection(_config.GetRequiredSection("ConnectionStrings").GetValue<String>("DefaultConnection"));
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
            List<ToDoModel> items = _connection.GetToDoList();

            string item = JsonConvert.SerializeObject(items);
            return item;
        }
        [HttpGet("GetPriorityList")]
        public string GetPriorityList(int priority)
        {
            List<ToDoModel> items = _connection.GetPriorityToDoList(priority);

            string item = JsonConvert.SerializeObject(items);
            return item;
        }
        [HttpPost("CreateToDoItem")]
        public string CreateToDoItem(string message, int priority)
        {
            if (priority <= 3 && priority >= 0)
            {
                try
                {
                    _connection.InsertToDoItem(message, priority);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return "Success";
            }
            else 
            {
                return "Invalid priorityNum. Valid Numbers are 1, 2, 3";
            }
        }
    }
}