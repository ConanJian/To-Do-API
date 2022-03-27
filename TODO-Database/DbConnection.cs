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
            _connection.Open();
        }
        public DataSet ReadData(string sqlQuery)
        {
            DataSet result = new DataSet();
            SqlDataAdapter adpater = new SqlDataAdapter();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection);
            adpater.SelectCommand = sqlCommand;
            //adpater.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adpater.Fill(result);
            return result;
        }
        public void ModifyData(string sqlQuery)
        {
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
