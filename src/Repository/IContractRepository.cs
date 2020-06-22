using Sample.Energy.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Energy.Repository
{
    public interface IContractRepository
    {
        Task<List<ContractMO>> Get();
        Task<List<ContractMO>> GetByClientName(string clientName);
        Task<ContractMO> Get(int code);
        Task<ContractMO> Save(ContractMO contract);
        Task<bool> Delete(int code);
    }
}
