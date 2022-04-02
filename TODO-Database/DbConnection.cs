using System;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseConnection
{
    internal class DbConnection: IDisposable
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
            SqlDataAdapter adapater = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection);
            adapater.SelectCommand = sqlCommand;
            //adapater.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapater.Fill(result);
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
        }
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}

/*
    add dependency injection for connection
    get connection string from configuration file
    
 */
