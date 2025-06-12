using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Services
{
    public interface IOrderServices
    {
        public bool Add(Order order);
        public IEnumerable<Order> GetByGift(int giftId);
        public Task<IEnumerable<OrderMaskAll>> Get(int page);
        public void Delete(string name, int customer);
        public bool Buy(int userId);
    }
}
