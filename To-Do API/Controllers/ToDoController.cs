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
        //Figure out how to return the correct type
        //Probably has to do with Headers and HttpResponse
        //Also has to do with IActionResult
        private readonly ILogger<ToDoController> _logger;
        private readonly IConfiguration _config;
        private readonly ToDoConnection _connection;

        public ToDoController(ILogger<ToDoController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _connection = new ToDoConnection(_config.GetRequiredSection("ConnectionStrings").GetValue<String>("DefaultConnection"));
        }
        [HttpGet("GetEntireToDoList")]
        public async Task<string> GetEntireToDoList()
        {
            try
            {
                List<ToDoModel> items = await _connection.GetToDoList();

                string item = JsonConvert.SerializeObject(items);
                return item;
            }
            catch (Exception e)
            {
                JArray jArray = new JArray();
                jArray.Add(new JObject("ErrorMessage", e.Message));
                return jArray.ToString();
            }
        }
        
        [HttpGet("GetPriorityList")]
        public async Task<string> GetPriorityList(int priority)
        {
            try
            {
                List<ToDoModel> items = await _connection.GetPriorityToDoList(priority);
                string item = JsonConvert.SerializeObject(items);
                return item;
            }
            catch (Exception e)
            {
                JArray jArray = new JArray();
                jArray.Add(new JObject("ErrorMessage", e.Message));
                return jArray.ToString();
            }
        }
        
        [HttpPost("CreateToDoItem")]
        public async Task<string> CreateToDoItem(string message, int priority)
        {
            if (priority <= 3 && priority >= 0)
            {
                try
                {
                    await _connection.InsertToDoItem(message, priority);
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
        
        [HttpPost("DeleteToDoItem")]
        public async Task<string> DeleteToDoItem(int listNum)
        {
            try
            {
                await _connection.DeleteToDoItem(listNum);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";

        }
    }
}