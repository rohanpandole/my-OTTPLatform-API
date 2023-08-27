using System.Data;

namespace OTTMyPlatform.Repository.Interface.Context
{
    public interface IDBContext
    {
        IDbConnection DbConnection();
    }
}
