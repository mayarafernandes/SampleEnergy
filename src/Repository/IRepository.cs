using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Sample.Energy.Repository
{
    public interface IRepository
    {        
        Task ExecuteNonQuery(string sql, List<IDbDataParameter> parameters = null);
        Task<int> ExecuteNonQueryIdentity(string sql, List<IDbDataParameter> parameters = null);
        Task<IDataReader> GetReader(string sql, List<IDbDataParameter> parameters = null);
        IDbDataParameter CreateParameter(string name, DbType type, object value = null);
    }
}
