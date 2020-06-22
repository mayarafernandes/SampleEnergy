using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sample.Energy.Model;
using Sample.Energy.Service;

namespace Sample.Energy.WebEnergy.Pages.Contract
{
    public class EditModel : PageModel
    {
        private readonly IContractService _contractService;
        private readonly IHtmlHelper _htmlHelper;

        [BindProperty]
        public ContractMO Contract { get; set; }
        public IEnumerable<SelectListItem> ContractTypes { get; set; }

        public EditModel(IContractService contractService, IHtmlHelper htmlHelper)
        {
            _contractService = contractService;
            _htmlHelper = htmlHelper;
        }

        public async Task<IActionResult> OnGetAsync(int? code)
        {
            ContractTypes = _htmlHelper.GetEnumSelectList<ContractType>();
            Contract = code.HasValue ? await _contractService.Get(code.Value) : new ContractMO();
            if (Contract == null)
                return RedirectToPage("./NotFound");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ContractTypes = _htmlHelper.GetEnumSelectList<ContractType>();
                return Page();
            }
            Contract = await _contractService.Save(Contract);
            TempData["Message"] = "Contract Saved!";
            return RedirectToPage("./Detail", new { code = Contract.Code });
        }

        public async Task OnPostDeleteAsync(int code)
        {
            await _contractService.Get();
        }
    }
}