using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperIntroHw.Data
{
    public class DogContext
    {
        private readonly string _connectionString;

        public DogContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
    }
}
