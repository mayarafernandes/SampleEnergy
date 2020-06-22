using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sample.Energy.Model;
using Sample.Energy.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Energy.WebEnergy.Pages.Contract
{
    public class ListModel : PageModel
    {
        private readonly IContractService _contractService;

        [BindProperty]
        public IEnumerable<ContractMO> Contracts { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchClientName { get; set; }

        public ListModel(IContractService contractService)
        {
            _contractService = contractService;
        }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(SearchClientName))
                Contracts = await _contractService.Get();
            else
                Contracts = await _contractService.GetByClientName(SearchClientName);
        }

        public async Task OnPostDeleteAsync(int code)
        {
            bool success = await _contractService.Delete(code);
            if (success)
                Contracts = await _contractService.Get();
        }
    }
}