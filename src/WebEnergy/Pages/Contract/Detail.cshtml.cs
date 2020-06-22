using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sample.Energy.Model;
using Sample.Energy.Service;

namespace Sample.Energy.WebEnergy.Pages.Contract
{
    public class DetailModel : PageModel
    {
        private readonly IContractService _contractService;

        [TempData]
        public string Message { get; set; }
        public ContractMO Contract { get; set; }

        public DetailModel(IContractService contractService)
        {
            _contractService = contractService;
        }
        
        public async Task<IActionResult> OnGetAsync(int code)
        {
            Contract = await _contractService.Get(code);
            if (Contract is null)
                return RedirectToPage("./NotFound");
            return Page();
        }
    }
}