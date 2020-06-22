using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.Energy.Model
{
    public class ContractMO
    {
        public int Code { get; set; }
        [Required(ErrorMessage = "Client name is required.")]
        public string ClientName { get; set; }
        public ContractType Type { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MonthsDuration 
        { 
            get
            {
                DateTime endDate = EndDate.HasValue ? EndDate.Value : DateTime.Now;
                return (int)Math.Floor((endDate - StartDate).TotalDays / 30.4);
            }
        }
    }
}
