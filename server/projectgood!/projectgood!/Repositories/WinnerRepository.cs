using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectChinesesActstion.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectChinesesActstion.Repositories
{
    public class WinnerRepository : IWinnerRepository
    {
        private readonly ProjectContext _context;
        private readonly IOrderRepository _OrderRepository;
        private static readonly Serilog.ILogger _logger = Log.ForContext<WinnerRepository>(); // Create a logger instance

        public WinnerRepository(ProjectContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _OrderRepository = orderRepository;
        }

        public async Task<CustomerMask> GetByGift(int giftId)
        {
            try
            {
                _logger.Information($"Attempting to retrieve winner for gift ID {giftId}.");

                // Step 1: Retrieve customer data
                var customer = _OrderRepository.GetByGift(giftId);
                var customerList = customer.ToList(); // Convert to list if needed

                if (customerList.Count == 0)
                {
                    _logger.Warning($"No customers found for gift ID {giftId}.");
                    return null;
                }

                // Step 2: Randomly select a winner
                Random random = new Random();
                int win = random.Next(0, customerList.Count);
                var winner = customerList[win];  // Randomly selected winner

                // Step 3: Create Winner object to save
                Winner w = new Winner
                {
                    GiftId = giftId,
                    CustomerId = winner.CustomerId
                };

                // Step 4: Save the winner in the database
                _context.Winner.Add(w);
                await _context.SaveChangesAsync();

                _logger.Information($"Winner selected and saved for gift ID {giftId}, Customer ID {winner.CustomerId}.");

                // Step 5: Retrieve and return the customer details asynchronously
                var winnerCus = await _context.Customer
                    .Include(x => x.User)
                    .Where(x => x.Id == winner.CustomerId)
                    .Select(x => new CustomerMask
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        Email = x.User.Email,
                        Adress = x.User.Adress,
                        Phone = x.User.Phone,
                        Password = x.Password
                    })
                    .SingleOrDefaultAsync();

                return winnerCus;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByGift method of WinnerRepository.");
                return null;
            }
        }

        public async Task<IEnumerable<WinnerMask>> Get()
        {
            try
            {
                var g = _context.Gift.ToArray();
                foreach (var item in g)
                {
                    await GetByGift(item.Id);
                }
                _logger.Information("Attempting to retrieve all winners.");
                var winner = _context.Winner.Include(x => x.Customer)
                    .Select(x => new WinnerMask
                    {
                        Id = x.Id,
                        CustName = x.Customer.User.Name,
                        Email = x.Customer.User.Email,
                        Adress = x.Customer.User.Adress,
                        Phone = x.Customer.User.Phone,
                        Gift = x.Gift.Name
                    });

                var result = await winner.ToListAsync();

                _logger.Information($"Successfully retrieved {result.Count} winners.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method of WinnerRepository.");
                return null;
            }
        }

        public int GetPayment()
        {
            try
            {
                _logger.Information("Attempting to retrieve the total payment for orders.");
                var pay = _context.Order.Include(x => x.Gift).Where(x => x.IsDraft == true).Select(x => x.Gift.Pay).Sum();

                _logger.Information($"Total payment retrieved: {pay}");

                return pay;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetPayment method of WinnerRepository.");
                return 0;
            }
        }
    }
}
