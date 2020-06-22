using Sample.Energy.Model;
using Sample.Energy.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Energy.Service
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;

        public ContractService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<bool> Delete(int code)
        {
            return (await _contractRepository.Delete(code));
        }

        public async Task<List<ContractMO>> Get()
        {
            return (await _contractRepository.Get());
        }

        public async Task<List<ContractMO>> GetByClientName(string clientName)
        {
            return (await _contractRepository.GetByClientName(clientName));
        }

        public async Task<ContractMO> Get(int code)
        {
            return (await _contractRepository.Get(code));
        }        

        public async Task<ContractMO> Save(ContractMO contract)
        {
            //TODO: Validate if can save
            return (await _contractRepository.Save(contract));
        }
    }
}
