using DatabaseConnection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        //Figure out how to return the correct type
        //Probably has to do with Headers and HttpResponse
        //Also has to do with IActionResult
        private readonly ILogger<ToDoController> _logger;
        private readonly ToDoSqlServerConnection _connection;

        public ToDoController(ILogger<ToDoController> logger, ToDoSqlServerConnection todoConnection)
        {
            _logger = logger;
            _connection = todoConnection;
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
                JObject jObject = new JObject();
                jObject.Add(new JProperty("ErrorMessage", e.Message));
                jArray.Add(jObject);
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
                JObject jObject = new JObject();
                jObject.Add(new JProperty("ErrorMessage", e.Message));
                jArray.Add(jObject);
                return jArray.ToString();
            }
        }
        
        [HttpPost("CreateToDoItem")]
        public async Task<string> CreateToDoItem(string message, int priority)
        {
            bool isSuccessful = false;
            if (priority <= 3 && priority >= 0)
            {
                try
                {
                    isSuccessful = await _connection.InsertToDoItem(message, priority);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                if (isSuccessful)
                    return "Success";
                else
                    return "Insert Failed";
            }
            else 
            {
                return "Invalid priorityNum. Valid Numbers are 1, 2, 3";
            }
        }
        
        [HttpPost("DeleteToDoItem")]
        public async Task<string> DeleteToDoItem(int listNum)
        {
            bool isSuccessful = false;
            try
            {
                isSuccessful = await _connection.DeleteToDoItem(listNum);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (isSuccessful)
                return "Success";
            else
                return "Delete Failed";

        }

        [HttpGet("GetLatestListNum")]
        public async Task<string> LatestListNum()
        {
            try
            {
                int listNum = await _connection.GetMostRecentListNum();
                return "" + listNum;
            }
            catch (Exception e)
            {
                return "ErrorMessage: "+e.Message;
            }
        }
    }
}