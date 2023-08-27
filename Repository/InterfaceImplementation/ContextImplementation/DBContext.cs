using Microsoft.Data.SqlClient;
using OTTMyPlatform.Repository.Interface.Context;
using System.Data;

namespace OTTMyPlatform.Repository.InterfaceImplementation.ContextImplementation
{
    public class DBContext:IDBContext
    {
        private readonly IConfiguration _configuration;
        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection DbConnection()
        {
            var mySqlConnection = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString());
            return mySqlConnection;
        }
    }
}
