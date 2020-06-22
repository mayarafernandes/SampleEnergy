using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Sample.Energy.Repository
{
    public class RepositorySQLite : IRepository
    {
        private string DATABASE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\SampleEnergy.sqlite3");
        
        public IDbConnection CreateConnection()
        {
            return new SqliteConnection();
        }       

        public async Task ExecuteNonQuery(string sql, List<IDbDataParameter> parameters = null)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source = {DATABASE_PATH}"))
            {
                try
                {
                    connection.Open();
                    using (SqliteTransaction transaction = connection.BeginTransaction())
                    {
                        SqliteCommand command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = sql;
                        InsertParameters(command, parameters);
                        await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                    }
                    connection.Close();
                }
                catch
                {
                    throw;
                }                
            }
        }

        public async Task<int> ExecuteNonQueryIdentity(string sql, List<IDbDataParameter> parameters = null)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source = {DATABASE_PATH}"))
            {
                try
                {
                    int identity = 0;
                    connection.Open();
                    using (SqliteTransaction transaction = connection.BeginTransaction())
                    {
                        SqliteCommand command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = sql;
                        InsertParameters(command, parameters);
                        await command.ExecuteNonQueryAsync();
                        identity = await GetIdentity(transaction);
                        transaction.Commit();
                    }
                    connection.Close();
                    return identity;
                }
                catch
                {
                    throw;
                }
            }
        }

        private async Task<int> GetIdentity(SqliteTransaction transaction)
        {
            using (SqliteCommand command = transaction.Connection.CreateCommand())
            {
                command.CommandText = "SELECT last_insert_rowid()";
                return int.Parse((await command.ExecuteScalarAsync()).ToString());
            }
        }

        public async Task<IDataReader> GetReader(string sql, List<IDbDataParameter> parameters = null)
        {
            SqliteConnection connection = new SqliteConnection($"Data Source = {DATABASE_PATH}");
            try
            {
                await connection.OpenAsync();
                SqliteCommand command = connection.CreateCommand();                
                command.CommandText = sql;
                InsertParameters(command, parameters);
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch
            {                
                connection.Close();
                throw;
            }
        }

        public IDbDataParameter CreateParameter(string name, DbType type, object value = null)
        {
            SqliteParameter parameter = new SqliteParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = (value is null) ? DBNull.Value : value;
            parameter.IsNullable = (value is null);
            return parameter;
        }

        private void InsertParameters(SqliteCommand command, List<IDbDataParameter> parameters)
        {
            if ((parameters is null) || (parameters.Count == 0))
                return;
            foreach (IDbDataParameter parameter in parameters)
            {
                SqliteParameter sqliteParameter = new SqliteParameter();
                sqliteParameter.ParameterName = parameter.ParameterName;
                if (parameter.DbType == DbType.DateTime)
                {
                    sqliteParameter.DbType = DbType.String;
                    if (!string.IsNullOrEmpty(parameter.Value.ToString()))
                        sqliteParameter.Value = Convert.ToDateTime(parameter.Value).ToString("yyyy-MM-dd HH:MM:ss.sss");
                    else
                        sqliteParameter.Value = DBNull.Value;
                }
                else
                {
                    sqliteParameter.DbType = parameter.DbType;
                    sqliteParameter.Value = parameter.Value;
                }
                command.Parameters.Add(sqliteParameter);
            }
        }
    }
}
