using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Repositories
{
    public interface IWinnerRepository
    {
        public Task<CustomerMask> GetByGift(int giftId);
        public Task<IEnumerable<WinnerMask>> Get();
        public int GetPayment();

    }
}
