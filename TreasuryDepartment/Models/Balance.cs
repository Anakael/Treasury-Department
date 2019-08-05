using Microsoft.EntityFrameworkCore;

namespace TreasuryDepartment.Models
{
    public class Balance : Offer
    {
        public decimal Sum { get; set; }

        public Balance()
        {
        }

        public Balance(Offer offer) : base(offer)
        {
        }
    }
}