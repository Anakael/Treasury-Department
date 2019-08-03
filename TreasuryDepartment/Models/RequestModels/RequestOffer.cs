using TreasuryDepartment.Models.Enums;

namespace TreasuryDepartment.Models.RequestModels
{
    public class RequestOffer : RequestUsersOffer
    {
        public Status Status { get; set; } = Status.Pending;
    }
}