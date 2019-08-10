using System;

namespace TreasuryDepartment.Models
{
    public class Deal : Offer
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastStatusChangeDate { get; set; }

        public Deal()
        {
            LastStatusChangeDate = CreatedDate;
        }
    }
}