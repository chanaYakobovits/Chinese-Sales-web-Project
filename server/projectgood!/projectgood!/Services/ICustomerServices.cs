using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Services
{
    public interface ICustomerServices
    {
        public Task<IEnumerable<OrderMaskGift>> Get(int customerId);
    }
}
