using System;

namespace TreasureDepartment.Data.Dbo
{
    public class DealDbo : UsersOfferDbo
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastStatusChangeDate { get; set; }

        public DealDbo()
        {
            LastStatusChangeDate = CreatedDate;
        }
    }
}