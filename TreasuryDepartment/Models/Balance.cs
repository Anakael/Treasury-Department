namespace TreasuryDepartment.Models
{
    public class Balance : UsersOffer
    {
        public decimal Sum { get; set; }

        protected Balance()
        {
        }


        public Balance(UsersOffer offer, decimal sum) : base(offer)
        {
            Sum = sum;
        }
    }
}