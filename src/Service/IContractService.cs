using Sample.Energy.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Energy.Service
{
    public interface IContractService
    {
        Task<List<ContractMO>> Get();
        Task<List<ContractMO>> GetByClientName(string clientName);
        Task<ContractMO> Get(int code);        
        Task<ContractMO> Save(ContractMO contract);
        Task<bool> Delete(int code);
    }
}
