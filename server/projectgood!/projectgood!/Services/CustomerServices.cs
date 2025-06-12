using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Repositories;
using Serilog;

namespace ProjectChinesesActstion.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _CustomerRepository;
        private static readonly Serilog.ILogger _logger = Log.ForContext<CustomerServices>(); // Create a logger instance

        public CustomerServices(ICustomerRepository CustomerRepository)
        {
            _CustomerRepository = CustomerRepository;
        }

        public async Task<IEnumerable<OrderMaskGift>> Get(int customerId)
        {
            try
            {
                _logger.Information($"Fetching orders for customer with ID: {customerId}");
           
                // Fetch orders from the repository
                var orders = await _CustomerRepository.Get(customerId);

                if (orders != null && orders.Any())
                {
                    _logger.Information($"Successfully fetched {orders.Count()} orders for customer ID: {customerId}.");
                    return orders;
                }
                else
                {
                    _logger.Warning($"No orders found for customer ID: {customerId}");
                    return Enumerable.Empty<OrderMaskGift>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching orders for customer ID: {customerId}");
                return Enumerable.Empty<OrderMaskGift>(); // Returning an empty list in case of an error
            }
        }
    }
}
