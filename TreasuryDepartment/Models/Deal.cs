using System;
using TreasuryDepartment.Models.Enums;

namespace TreasuryDepartment.Models
{
    public class Deal : Offer
    {
        public decimal Sum { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastStatusChangeDate { get; set; } = DateTime.Now;
    }
}