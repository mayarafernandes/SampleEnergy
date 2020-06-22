using Sample.Energy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Sample.Energy.Repository
{
    public class ContractSQLite : IContractRepository
    {
        private const string SQL_SELECT_CONTRACT = "SELECT codContract, clientName, typeContract, quantity, valueContract, startDate, endDate FROM REP_CONTRACT @WHERE";
        private const string SQL_INSERT_CONTRACT = "INSERT INTO REP_CONTRACT (clientName, typeContract, quantity, valueContract, startDate, endDate) VALUES (@clientName, @typeContract, @quantity, @valueContract, @startDate, @endDate)";
        private const string SQL_UPDATE_CONTRACT = "UPDATE REP_CONTRACT SET clientName = @clientName, typeContract = @typeContract, quantity = @quantity, valueContract = @valueContract, startDate = @startDate, endDate = @endDate WHERE (codContract = @codContract)";
        private const string SQL_DELETE_CONTRACT = "DELETE FROM REP_CONTRACT WHERE (codContract = @codContract)";

        private const string PARAMETER_codContract = "@codContract";
        private const string PARAMETER_clientName = "@clientName";
        private const string PARAMETER_type = "@typeContract";
        private const string PARAMETER_quantity = "@quantity";
        private const string PARAMETER_value = "@valueContract";
        private const string PARAMETER_startDate = "@startDate";
        private const string PARAMETER_endDate = "@endDate";

        private readonly IRepository _repository;

        public ContractSQLite(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContractMO>> Get()
        {
            try
            {
                List<ContractMO> contracts = new List<ContractMO>();
                string sql = SQL_SELECT_CONTRACT.Replace(" @WHERE", string.Empty);
                using (IDataReader reader = await _repository.GetReader(sql))
                {
                    while (reader.Read())
                        contracts.Add(CreateContract(reader));
                }
                return contracts;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<ContractMO>> GetByClientName(string clientName)
        {
            try
            {
                List<ContractMO> contracts = new List<ContractMO>();
                List<IDbDataParameter> parameters = new List<IDbDataParameter>();
                parameters.Add(_repository.CreateParameter(PARAMETER_clientName, DbType.String, clientName));
                string sql = SQL_SELECT_CONTRACT.Replace(" @WHERE", $" WHERE clientName LIKE '%{clientName}%'");
                using (IDataReader reader = await _repository.GetReader(sql, parameters))
                {
                    while (reader.Read())
                        contracts.Add(CreateContract(reader));
                }
                return contracts;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ContractMO> Get(int code)
        {
            try
            {
                List<IDbDataParameter> parameters = new List<IDbDataParameter>();
                parameters.Add(_repository.CreateParameter(PARAMETER_codContract, DbType.Int32, code));
                string sql = SQL_SELECT_CONTRACT.Replace(" @WHERE", $" WHERE codContract = {PARAMETER_codContract}");
                using (IDataReader reader = await _repository.GetReader(sql, parameters))
                {
                    if (reader.Read())
                        return CreateContract(reader);
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }        

        public async Task<ContractMO> Save(ContractMO contract)
        {
            try
            {
                List<IDbDataParameter> parameters = CreateParameters(contract);                
                if (contract.Code > 0)
                {
                    await _repository.ExecuteNonQuery(SQL_UPDATE_CONTRACT, parameters);
                }
                else
                {
                    int lastIdentity = await _repository.ExecuteNonQueryIdentity(SQL_INSERT_CONTRACT, parameters);
                    contract.Code = lastIdentity;
                }
                return contract;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> Delete(int code)
        {
            try
            {
                List<IDbDataParameter> parameters = new List<IDbDataParameter>();
                parameters.Add(_repository.CreateParameter(PARAMETER_codContract, DbType.Int32, code));
                await _repository.ExecuteNonQuery(SQL_DELETE_CONTRACT, parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<IDbDataParameter> CreateParameters(ContractMO contract)
        {
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();
            parameters.Add(_repository.CreateParameter(PARAMETER_codContract, DbType.Int32, contract.Code));
            parameters.Add(_repository.CreateParameter(PARAMETER_clientName, DbType.String, contract.ClientName));
            parameters.Add(_repository.CreateParameter(PARAMETER_type, DbType.Int32, (int)contract.Type));
            parameters.Add(_repository.CreateParameter(PARAMETER_quantity, DbType.Int32, contract.Quantity));
            parameters.Add(_repository.CreateParameter(PARAMETER_value, DbType.Decimal, contract.Value));
            parameters.Add(_repository.CreateParameter(PARAMETER_startDate, DbType.DateTime, contract.StartDate));
            parameters.Add(_repository.CreateParameter(PARAMETER_endDate, DbType.DateTime, contract.EndDate));
            return parameters;
        }

        private ContractMO CreateContract(IDataReader reader)
        {
            ContractMO contract = new ContractMO();
            contract.Code = reader.GetInt32(0);
            contract.ClientName = reader.GetString(1);
            contract.Type = (ContractType)reader.GetInt32(2);
            contract.Quantity = reader.GetInt32(3);
            contract.Value = reader.GetDecimal(4);
            contract.StartDate = Convert.ToDateTime(reader.GetString(5));
            if (!reader.IsDBNull(6))
                contract.EndDate = Convert.ToDateTime(reader.GetString(6));
            return contract;
        }
    }
}
