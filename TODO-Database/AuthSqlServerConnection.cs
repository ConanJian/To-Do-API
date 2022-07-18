using DatabaseConnection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseConnection
{
    public class AuthSqlServerConnection
    {
        private readonly DbConnection _dbConnection;
        public AuthSqlServerConnection(string connectionString)
        {
            _dbConnection = new DbConnection(connectionString);
        }
        public void CreateUser(string userName, string password)
        { 
            
        }
    }
}
