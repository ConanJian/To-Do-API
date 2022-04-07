using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseConnection
{
    internal class DbConnection
    {
        private readonly SqlConnection _connection;
        public DbConnection(string connectionString)
        { 
            _connection = new SqlConnection(connectionString);
        }
        public async Task<DataSet> ReadData(string sqlQuery)
        {
            _connection.Open();
            DataSet result = new DataSet();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection);

            //Create Adapater to convert results of sql into DataSet
            SqlDataAdapter adapater = new SqlDataAdapter();
            adapater.SelectCommand = sqlCommand;
            adapater.Fill(result);

            await sqlCommand.DisposeAsync();
            await _connection.CloseAsync();
            return result;
        }
        public async Task ModifyData(string sqlQuery)
        {
            _connection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection);
            
            SqlTransaction transaction = _connection.BeginTransaction();
            sqlCommand.Transaction = transaction;
            sqlCommand.CommandType = CommandType.Text;

            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                transaction.Rollback();
            }

            await sqlCommand.DisposeAsync();
            await _connection.CloseAsync();
        }
    }
}
