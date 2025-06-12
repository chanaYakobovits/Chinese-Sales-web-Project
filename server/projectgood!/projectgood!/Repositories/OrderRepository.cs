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
    public class OrderRepository : IOrderRepository
    {
        private readonly ProjectContext _context;
        private static readonly Serilog.ILogger _logger = Log.ForContext<OrderRepository>(); // Create a logger instance

        public OrderRepository(ProjectContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAll()
        {
            try
            {
                _logger.Information("Attempting to retrieve all orders.");
                var orders = _context.Order.ToList();
                _logger.Information($"Successfully retrieved {orders.Count} orders.");
                return orders;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetAll method of OrderRepository.");
                return null;
            }
        }

        public IEnumerable<Order> GetByGift(int giftId)
        {
            try
            {
                _logger.Information($"Attempting to retrieve orders for gift ID {giftId}.");
                var orders = _context.Order.Where(x => x.GiftId == giftId && x.IsDraft == true).ToList();
                _logger.Information($"Successfully retrieved {orders.Count} orders for gift ID {giftId}.");
                return orders;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByGift method of OrderRepository.");
                return null;
            }
        }

        public async Task<IEnumerable<OrderMaskAll>> Get(int page)
        {
            try
            {
                _logger.Information($"Attempting to retrieve orders on page {page}.");

                var orders = await _context.Order
                    .Include(x => x.Gift)
                    .Include(x => x.Customer)
                    .ThenInclude(c => c.User)
                    .Where(x => x.Gift.Id >= (page - 1) * 10 + 1 && x.Gift.Id <= page * 10)
                    .Where(x => x.IsDraft == true)
                    .GroupBy(x => x.Gift.Name) // קיבוץ לפי Gift.Id (שדה ייחודי)
                    .Select(group => new OrderMaskAll
                    {
                        Name = group.First().Gift.Name, // שם המתנה
                        Customers = group
                            .Select(o => new User
                            {
                                Name = o.Customer.User.Name,
                                Email = o.Customer.User.Email,
                                Adress = o.Customer.User.Adress,
                                Phone = o.Customer.User.Phone
                            })
                            .Distinct() // שימוש ב-Distinct כדי להסיר כפילויות
                            .ToList(), // המרה לרשימה
                        Count = group.Count() // מספר ההזמנות עבור מתנה זו
                    })
                    .ToListAsync(); // ביצוע השאילתה ב-SQL

                _logger.Information($"Successfully retrieved {orders.Count} orders for page {page}.");
                return orders;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method of OrderRepository.");
                return null;
            }
        }




        public bool Add(Order order)
        {
            try
            { 
                _logger.Information("Attempting to add a new order.");
                var gift = _context.Gift.Where(g => g.Id == order.GiftId).FirstOrDefault();
                var customer = _context.Customer.Where(c => c.UserId == order.CustomerId).FirstOrDefault();
                order.CustomerId = customer.Id;

                if (gift != null && customer != null)
                {
                    _context.Order.Add(order);
                    _context.SaveChanges();
                    _logger.Information("Successfully added a new order.");
                    return true;
                }
                else
                {
                    _logger.Warning("Failed to add order. Either gift or customer was not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Add method of OrderRepository.");
                return false;
            }
        }

        public void Delete(string name,int customer)
        {
            
            try
            {
                var c = _context.Customer.FirstOrDefault(c => c.UserId == customer);
                if (c == null)
                {
                    _logger.Warning($"Customer with ID {customer} not found.");
                    return ;
                }

                _logger.Information($"Attempting to retrieve orders for customer with ID: {customer}.");

                var o= _context.Order.Include(x => x.Gift).FirstOrDefault(x => x.Gift.Name == name && x.CustomerId == c.Id);
                //_logger.Information($"Attempting to delete order with ID {o.Id}.");
                _context.Order.Remove(o);
                _context.SaveChanges();
                _logger.Information($"Successfully deleted order with ID {o.Id}.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Delete method of OrderRepository.");
            }
        }

        public bool Buy(int userId)
        {
            try
            {
                _logger.Information("Attempting payment for order.");
                var customer = _context.Customer.Where(c => c.UserId == userId).FirstOrDefault();
                var ord = _context.Order.Where(o => o.CustomerId == customer.Id).ToList();
               
                if (ord != null )
                {
                    foreach (var item in ord)
                    {
                        item.IsDraft = true;
                        _context.Order.Update(item);
                        _context.SaveChanges();
                    }
                    _logger.Information("Successfully payment for order.");
                    return true;
                }
                else
                {
                    _logger.Warning("Failed to payment for order. Either gift or customer was not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Buy method of OrderRepository.");
                return false;
            }
        }
    }
}
