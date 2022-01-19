using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace cm.Infrastructure.Dapper
{
    public interface IDapper
    {
        DbConnection GetDbconnection();

        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);

        IEnumerable<T> GetAllItem<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);

        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        T InsertUpdate<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        DataTable SelectRows(string queryString);
    }
}