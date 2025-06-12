using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Repositories
{
    public interface ICustomerRepository
    {
        public IEnumerable<CustomerMask> Get();
      
        public Task<IEnumerable<OrderMaskGift>> Get(int customerId);
    }
}
