using System;
using TreasuryDepartment.Models.Enums;
using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class Deal : Offer
    {
        public decimal Sum { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastStatusChangeDate { get; set; } = DateTime.Now;

        public Deal()
        {
        }

        public Deal(Offer offer, decimal sum) : base(offer)
        {
            Sum = sum;
        }
    }
}