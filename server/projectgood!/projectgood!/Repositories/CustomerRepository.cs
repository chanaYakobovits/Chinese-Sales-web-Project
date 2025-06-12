using Effortless.Net.Encryption;
using Microsoft.EntityFrameworkCore;
using ProjectChinesesActstion.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectChinesesActstion.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ProjectContext _context;

        // Create a logger instance
        private static readonly Serilog.ILogger _logger = Log.ForContext<CustomerRepository>();

        public CustomerRepository(ProjectContext context)
        {
            _context = context;
        }

        public IEnumerable<CustomerMask> Get()
        {
            try
            {
                _logger.Information("Attempting to retrieve all customers.");
                var CustomerWithGift = _context.Customer.Include(x => x.User)
                    .Select(x => new CustomerMask
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        Email = x.User.Email,
                        Adress = x.User.Adress,
                        Phone = x.User.Phone,
                        Password = x.Password
                    });

                var result = CustomerWithGift.ToList();
                if (result.Any())
                {
                    _logger.Information($"Successfully retrieved {result.Count} customers.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method of CustomerRepository.");
                return null;
            }
        }

        public async Task<IEnumerable<OrderMaskGift>> Get(int customerId)
        {
            try
            {
                var customer = _context.Customer.FirstOrDefault(c => c.UserId == customerId);
                if (customer == null)
                {
                    _logger.Warning($"Customer with ID {customerId} not found.");
                    return Enumerable.Empty<OrderMaskGift>();
                }

                _logger.Information($"Attempting to retrieve orders for customer with ID: {customerId}.");

                // קבלת רשימת מתנות עם ספירה
                var giftsWithCount = await _context.Order
                    .Where(x => x.CustomerId == customer.Id && !x.IsDraft)
                    .GroupBy(x => new { x.Gift.Name, x.Gift.Image, x.Gift.Pay })
                    .Select(g => new OrderMaskGift
                    {
                        Name = g.Key.Name,
                        Image = g.Key.Image,
                        Pay = g.Key.Pay,
                        Count = g.Count() // מספר הפעמים שהמתנה מופיעה
                    })
                    .ToListAsync();

                _logger.Information($"Successfully retrieved {giftsWithCount.Count} gifts for customer {customerId}.");
                return giftsWithCount;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method of CustomerRepository for orders.");
                return null;
            }
        }


        //public int Count(int giftId)
        //{
        //    var count = _context.Order.Where(x => x.GiftId == giftId).ToList().Count();
        //    if (count != 0)
        //        return   count;
        //    return 0;

        //}

    }
}
