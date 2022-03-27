using System;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseConnection
{
    internal class DbConnection
    {
        private readonly SqlConnection connection;
        public DbConnection(string connectionString)
        { 
            connection = new SqlConnection(connectionString);
        }
        public DataSet ReadData(string sqlQuery)
        { 
            
        }
        public void WriteData(string sqlQuery)
        { 
            
        }
    }
}

/*
    add dependency injection for connection
    get connection string from configuration file
    
 */
