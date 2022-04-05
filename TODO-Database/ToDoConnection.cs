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

            string readDataQuery = $"select *" +
                $" from ToDo" +
                $" order by listNum;";
            DataTable data = _dbConnection.ReadData(readDataQuery).Tables[0];

            foreach (DataRow row in data.Rows)
            { 
                list.Add(ConvertDataToModel(row));
            }
            return list;
        }
        public List<ToDoModel> GetToDoListByPriority(Priority priority) 
        {
            List<ToDoModel> list = new List<ToDoModel>();

            string readDataQuery = $"select *" +
                $" from ToDo" +
                $" where \"priority\" = {(int)priority}" +
                $" order by listNum;";
            DataTable data = _dbConnection.ReadData(readDataQuery).Tables[0];

            foreach (DataRow row in data.Rows)
            {
                list.Add(ConvertDataToModel(row));
            }
            return list;
        }
        public void InsertToDoItem(string message, int priority)
        {
            string sqlQuery = $"insert into ToDo(\"message\", \"priority\")" +
                $" values('{message}', {priority});";

            _dbConnection.ModifyData(sqlQuery);
        }
        public void DeleteToDoItem(int listNum)
        {
            string sqlQuery = $"delete from ToDo" +
                $" where listNum={listNum};";
            _dbConnection.ModifyData(sqlQuery);
        }
        public List<ToDoModel> GetPriorityToDoList(int priority)
        {
            List<ToDoModel> list = new List<ToDoModel>();

            string readDataQuery = $"select *" +
                $" from ToDo" +
                $" where \"priority\" = {priority}" +
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
