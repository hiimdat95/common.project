using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enum;

namespace Infrastructure.Dapper
{
    public interface IDapperProvider<T> where T : class
    {
        Task<IEnumerable<T>> ExecuteQueryAsync(string query, CommandQueryType commandQueryType, object[] parameter = null);

        Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string query, Dictionary<string, object> parameters = null);

        Task<IEnumerable<T>> ExecuteQueryCommandAsync<T>(string query, Dictionary<string, object> parameters = null);

        Task<object> ExecuteScalarAsync(string query, Dictionary<string, object> parameters = null, CommandType commandType = CommandType.Text);

        Task<List<int>> ExcuteProcedureOutputAsync(string query, CommandType commandQueryType, object[] parameter = null);

        Task<List<Guid>> ExcuteProcedureOutputGuidAsync(string query, CommandType commandQueryType, object[] parameter = null);

        Task ExcuteNonQueryAsync(string query, CommandQueryType commandType, object[] parameter = null);

        Task<(List<int> Outputs, IEnumerable<T> Result)> QueryProcedureOutputAsync(string query, CommandType commandType, object[] parameter = null);
    }

    public class DapperProvider<T> : IDapperProvider<T> where T : class
    {
        private readonly IConfiguration _configuration;

        public DapperProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private string GetConnectionString()
        {
            string connectionString = _configuration.GetConnectionString(SystemConstants.MainConnectionString);
            return connectionString;
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync(string query, CommandQueryType commandQueryType, object[] parameter = null)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                if (parameter != null)
                {
                    string[] lstPara = query.Split(' ');
                    query = lstPara[0];
                    int index = 0;
                    foreach (string item in lstPara)
                    {
                        if (item.Contains('@'))
                        {
                            if (item.EndsWith(","))
                            {
                                string para = item.Remove(item.Length - 1);
                                dynamicParameters.Add(para, parameter[index]);
                            }
                            else
                            {
                                dynamicParameters.Add(item, parameter[index]);
                            }
                            index++;
                        }
                    }
                }

                if (commandQueryType == CommandQueryType.CommandType_StoredProcedure)
                    return await sqlConnection.QueryAsync<T>(query, dynamicParameters, commandType: CommandType.StoredProcedure);
                else
                    return await sqlConnection.QueryAsync<T>(query, dynamicParameters, commandType: CommandType.Text);
            }
        }

        public async Task<List<int>> ExcuteProcedureOutputAsync(string query, CommandType commandQueryType, object[] parameter = null)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    await sqlConnection.OpenAsync();
                }
                DynamicParameters dynamicParameters = new DynamicParameters();
                List<string> _listParamsOutput = new List<string>();

                if (parameter != null)
                {
                    string[] _listParams = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (!string.IsNullOrEmpty(_listParams[0]))
                        query = _listParams[0];
                    int index = 0;
                    for (int i = 0; i < _listParams.Count() - 1; i++)
                    {
                        if (_listParams[i].Contains("@"))
                        {
                            if (_listParams[i + 1].ToLower() == "output")
                            {
                                dynamicParameters.Add(_listParams[i].Replace(",", ""), direction: ParameterDirection.Output, value: parameter[index], dbType: DbType.Int32);
                                _listParamsOutput.Add(_listParams[i].Replace(",", ""));
                            }
                            else
                            {
                                dynamicParameters.Add(_listParams[i].Replace(",", ""), direction: ParameterDirection.Input, value: parameter[index]);
                            }
                            index++;
                        }
                    }
                    if (_listParams.Last().ToLower() != "output")
                    {
                        dynamicParameters.Add(_listParams.Last(), direction: ParameterDirection.Input, value: parameter[parameter.Length]);
                    }
                }
                await sqlConnection.ExecuteAsync(query, dynamicParameters, null, null, commandQueryType);
                List<int> result = new List<int>();
                _listParamsOutput.ForEach(c =>
                {
                    result.Add(dynamicParameters.Get<int>(c));
                });
                return result;
            }
        }

        public async Task<List<Guid>> ExcuteProcedureOutputGuidAsync(string query, CommandType commandQueryType, object[] parameter = null)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    await sqlConnection.OpenAsync();
                }
                DynamicParameters dynamicParameters = new DynamicParameters();
                List<string> _listParamsOutput = new List<string>();

                if (parameter != null)
                {
                    string[] _listParams = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (!string.IsNullOrEmpty(_listParams[0]))
                        query = _listParams[0];
                    int index = 0;
                    for (int i = 0; i < _listParams.Count() - 1; i++)
                    {
                        if (_listParams[i].Contains("@"))
                        {
                            if (_listParams[i + 1].ToLower() == "output")
                            {
                                dynamicParameters.Add(_listParams[i].Replace(",", ""), direction: ParameterDirection.Output, value: parameter[index], dbType: DbType.Int32);
                                _listParamsOutput.Add(_listParams[i].Replace(",", ""));
                            }
                            else
                            {
                                dynamicParameters.Add(_listParams[i].Replace(",", ""), direction: ParameterDirection.Input, value: parameter[index]);
                            }
                            index++;
                        }
                    }
                    if (_listParams.Last().ToLower() != "output")
                    {
                        dynamicParameters.Add(_listParams.Last(), direction: ParameterDirection.Input, value: parameter[parameter.Length]);
                    }
                }
                await sqlConnection.ExecuteAsync(query, dynamicParameters, null, null, commandQueryType);
                List<Guid> result = new List<Guid>();
                _listParamsOutput.ForEach(c =>
                {
                    result.Add(dynamicParameters.Get<Guid>(c));
                });
                return result;
            }
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string query, Dictionary<string, object> parameters = null)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                if (parameters?.Count > 0)
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add(parameter.Key, parameter.Value);
                    }

                return await sqlConnection.QueryAsync<T>(query, dynamicParameters, commandType: CommandType.StoredProcedure);
            };
        }

        public async Task<IEnumerable<T>> ExecuteQueryCommandAsync<T>(string query, Dictionary<string, object> parameters = null)
        {
            using var sqlConnection = new SqlConnection(GetConnectionString());
            if (sqlConnection.State == ConnectionState.Closed)
                await sqlConnection.OpenAsync();

            var dynamicParameters = new DynamicParameters();
            if (parameters?.Count > 0)
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

            return await sqlConnection.QueryAsync<T>(query, dynamicParameters, commandType: CommandType.Text);
        }

        public async Task<object> ExecuteScalarAsync(string query, Dictionary<string, object> parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                if (parameters?.Count > 0)
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add(parameter.Key, parameter.Value);
                    }
                return await sqlConnection.ExecuteScalarAsync(query, dynamicParameters, commandType: commandType);
            }
        }

        public async Task ExcuteNonQueryAsync(string query, CommandQueryType commandType, object[] parameter = null)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    await sqlConnection.OpenAsync();

                var dynamicParameters = new DynamicParameters();
                if (parameter != null)
                {
                    string[] lstPara = query.Split(' ');
                    query = lstPara[0];
                    int index = 0;
                    foreach (string item in lstPara)
                    {
                        if (item.Contains('@'))
                        {
                            if (item.EndsWith(","))
                            {
                                string para = item.Remove(item.Length - 1);
                                dynamicParameters.Add(para, parameter[index]);
                            }
                            else
                            {
                                dynamicParameters.Add(item, parameter[index]);
                            }
                            index++;
                        }
                    }
                }

                if (commandType == CommandQueryType.CommandType_StoredProcedure)
                    await sqlConnection.ExecuteAsync(query, dynamicParameters, commandType: CommandType.StoredProcedure);
                else
                    await sqlConnection.ExecuteAsync(query, dynamicParameters, commandType: CommandType.Text);
            }
        }

        public async Task<(List<int> Outputs, IEnumerable<T> Result)> QueryProcedureOutputAsync(string query, CommandType commandType, object[] parameter = null)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    await sqlConnection.OpenAsync();
                }
                DynamicParameters dynamicParameters = new DynamicParameters();
                List<string> _listParamsOutput = new List<string>();

                if (parameter != null)
                {
                    string[] _listParams = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (!string.IsNullOrEmpty(_listParams[0]))
                        query = _listParams[0];
                    int index = 0;
                    for (int i = 0; i < _listParams.Count() - 1; i++)
                    {
                        if (_listParams[i].Contains("@"))
                        {
                            if (_listParams[i + 1].ToLower() == "output")
                            {
                                dynamicParameters.Add(_listParams[i].Replace(",", ""), direction: ParameterDirection.Output, value: parameter[index], dbType: DbType.Int32);
                                _listParamsOutput.Add(_listParams[i].Replace(",", ""));
                            }
                            else
                            {
                                dynamicParameters.Add(_listParams[i].Replace(",", ""), direction: ParameterDirection.Input, value: parameter[index]);
                            }
                            index++;
                        }
                    }
                    if (_listParams.Last().ToLower() != "output")
                    {
                        dynamicParameters.Add(_listParams.Last(), direction: ParameterDirection.Input, value: parameter[parameter.Length]);
                    }
                }
                var data = await sqlConnection.QueryAsync<T>(query, dynamicParameters, null, null, CommandType.StoredProcedure);
                List<int> result = new List<int>();
                _listParamsOutput.ForEach(c =>
                {
                    result.Add(dynamicParameters.Get<int>(c));
                });
                return (result, data);
            }
        }
    }
}