using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Repositories
{
    public interface IOrderRepository
    {
        public bool Add(Order order);
        public IEnumerable<Order> GetAll();
        public IEnumerable<Order> GetByGift(int giftId);
        public Task<IEnumerable<OrderMaskAll>> Get(int page);
        public void Delete(string name, int customer);
        public bool Buy(int userId);

    }
}
