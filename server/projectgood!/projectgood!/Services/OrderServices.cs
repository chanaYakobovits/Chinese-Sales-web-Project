using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectChinesesActstion.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly ILogger<OrderServices> _logger;

        public OrderServices(IOrderRepository OrderRepository, ILogger<OrderServices> logger)
        {
            _OrderRepository = OrderRepository;
            _logger = logger;
        }

        public IEnumerable<Order> GetByGift(int giftId)
        {
            try
            {
                var orders = _OrderRepository.GetByGift(giftId);
                _logger.LogInformation("Fetched orders for giftId {GiftId}.", giftId);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for giftId {GiftId}.", giftId);
                throw; // rethrow the exception to handle it further upstream if necessary
            }
        }

        public bool Add(Order order)
        {
            try
            {
                //פה אני רוצה לשלוף את הUSERID
                bool isAdded = _OrderRepository.Add(order);
                if (isAdded)
                {
                    _logger.LogInformation("Order with Id {OrderId} was successfully added.", order.Id);
                }
                else
                {
                    _logger.LogWarning("Order with Id {OrderId} was not added due to an error.", order.Id);
                }
                return isAdded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order with Id {OrderId}.", order.Id);
                throw; // rethrow the exception to handle it further upstream if necessary
            }
        }

        public bool Buy(int userId)
        {
            try
            {
                //פה אני רוצה לשלוף את הUSERID
                bool isAdded = _OrderRepository.Buy(userId);
                if (isAdded)
                {
                    _logger.LogInformation("Order for Id {userId} was successfully buying.", userId);
                }
                else
                {
                    _logger.LogWarning("Order for Id {userId} was not buying due to an error.", userId);
                }
                return isAdded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buying order with Id {userId}.", userId);
                throw; // rethrow the exception to handle it further upstream if necessary
            }
        }
        public async Task<IEnumerable<OrderMaskAll>> Get(int page)
        {
            try
            {
                var orders = await _OrderRepository.Get(page);
                _logger.LogInformation("Fetched orders for page {Page}.", page);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for page {Page}.", page);
                throw; // rethrow the exception to handle it further upstream if necessary
            }
        }

        public void Delete(string name, int customer)
        {
            try
            {
                        _OrderRepository.Delete(name,customer);
                        _logger.LogInformation("Order with {name}  was successfully deleted.", name);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order with  {name}.", name);
                throw; // rethrow the exception to handle it further upstream if necessary
            }
        }
    }
}
