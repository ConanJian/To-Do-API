using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DatabaseConnection
{
    //Class connects to Database and transforms the data to objects
    public class ToDoConnection
    {
        private readonly DbConnection _dbConnection;
        public ToDoConnection(string connectionString)
        { 
            _dbConnection = new DbConnection(connectionString);
        }
        public List<ToDoModel> GetToDoList()
        {
            List<ToDoModel> list = new List<ToDoModel>();

            string readDataQuery = $"select 'message', 'priority'" +
                $" from ToDo" +
                $" order by listNum";
            DataTable data = _dbConnection.ReadData(readDataQuery).Tables[0];

            foreach (DataRow row in data.Rows)
            { 
                list.Add(ConvertDataToModel(row));
            }
            return list;
        }
        private ToDoModel ConvertDataToModel(DataRow row)
        {
            int listNum = (int)row["listNum"];
            string message = row["message"].ToString();
            int priority = (int)row["priority"];
            ToDoModel listItem = new ToDoModel(listNum, message, priority);
            return listItem;
        }
    }
}
