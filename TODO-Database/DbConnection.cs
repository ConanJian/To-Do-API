using System;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseConnection
{
    internal class DbConnection
    {
        private readonly SqlConnection _connection;
        public DbConnection(string connectionString)
        { 
            _connection = new SqlConnection(connectionString);
        }
        public DataSet ReadData(string sqlQuery)
        {
            _connection.Open();
            DataSet result = new DataSet();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection);

            //Create Adapater to convert results of sql into DataSet
            SqlDataAdapter adapater = new SqlDataAdapter();
            adapater.SelectCommand = sqlCommand;
            adapater.Fill(result);
            _connection.Close();
            return result;
        }
        public void ModifyData(string sqlQuery)
        {
            _connection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
