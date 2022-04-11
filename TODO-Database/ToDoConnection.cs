using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<List<ToDoModel>> GetToDoList()
        {
            List<ToDoModel> list = new List<ToDoModel>();

            string readDataQuery = $"select *" +
                $" from ToDo" +
                $" order by listNum;";
            DataSet dataSet = await _dbConnection.ReadData(readDataQuery);
            DataTable data = dataSet.Tables[0];

            foreach (DataRow row in data.Rows)
            { 
                list.Add(ConvertDataToModel(row));
            }
            return list;
        }
        public async Task<List<ToDoModel>> GetToDoListByPriority(Priority priority) 
        {
            List<ToDoModel> list = new List<ToDoModel>();

            string readDataQuery = $"select *" +
                $" from ToDo" +
                $" where \"priority\" = {(int)priority}" +
                $" order by listNum;";
            DataSet dataSet = await _dbConnection.ReadData(readDataQuery);
            DataTable data = dataSet.Tables[0];

            foreach (DataRow row in data.Rows)
            {
                list.Add(ConvertDataToModel(row));
            }
            return list;
        }
        public async Task InsertToDoItem(string message, int priority)
        {
            string sqlQuery = $"insert into ToDo(\"message\", \"priority\")" +
                $" values('{message}', {priority});";

            await _dbConnection.ModifyData(sqlQuery);
        }
        public async Task DeleteToDoItem(int listNum)
        {
            string sqlQuery = $"delete from ToDo" +
                $" where listNum={listNum};";
            await _dbConnection.ModifyData(sqlQuery);
        }
        public async Task<int> GetMostRecentListNum()
        {
            string sqlQuery = $"select top 1 listNum from ToDo" +
                $" order by listNum Desc";
            DataSet set = await _dbConnection.ReadData(sqlQuery);
            DataTable toDoTable = set.Tables[0];
            return (int)toDoTable.Rows[0]["listNum"];
        }
        public async Task<List<ToDoModel>> GetPriorityToDoList(int priority)
        {
            List<ToDoModel> list = new List<ToDoModel>();

            string readDataQuery = $"select *" +
                $" from ToDo" +
                $" where \"priority\" = {priority}" +
                $" order by listNum";
            DataSet dataSet = await _dbConnection.ReadData(readDataQuery);
            DataTable data = dataSet.Tables[0];

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
