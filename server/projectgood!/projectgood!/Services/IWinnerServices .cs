using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Services
{
    public interface IWinnerServices
    {
        public Task<CustomerMask> GetByGift(int giftId);
        public  Task<IEnumerable<WinnerMask>> Get();
        public int GetPayment();
    }
}
