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
    public class DonatorRepository : IDonatorRepository
    {
        private readonly ProjectContext _context;
        private static readonly Serilog.ILogger _logger = Log.ForContext<DonatorRepository>(); // Create a logger instance

        public DonatorRepository(ProjectContext context)
        {
            _context = context;
        }
        public IEnumerable<DonatorMask> Getall()
        {
            try
            {
                _logger.Information("Attempting to retrieve all donators.");
                var donatorWithgift = _context.Donator.Include(x => x.User)
                    .Select(x => new DonatorMask
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        Email = x.User.Email,
                        Adress = x.User.Adress,
                        Phone = x.User.Phone,
                        Gifts = x.gifts.Select(x => x.Name).ToList()
                    });

                var result = donatorWithgift.ToList();
                _logger.Information($"Successfully retrieved donators.");
                return  result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method of DonatorRepository.");
                return null;
            }
        }

        public async  Task<IEnumerable<DonatorMask>> Get()
        {
            try
            {
                _logger.Information("Attempting to retrieve all donators.");
                var donatorWithgift = _context.Donator.Include(x => x.User)
                    .Select(x => new DonatorMask
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        Email = x.User.Email,
                        Adress = x.User.Adress,
                        Phone = x.User.Phone,
                        Gifts = x.gifts.Select(x => x.Name).ToList()
                    });

                var result = donatorWithgift.ToListAsync();
                _logger.Information($"Successfully retrieved donators.");
                return await result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method of DonatorRepository.");
                return null;
            }
        }

        public async Task<IEnumerable<DonatorMask>> GetByPage(int page)
        {
            try
            {
                _logger.Information($"Attempting to retrieve donators on page {page}.");
                var donatorWithgift = _context.Donator.Include(x => x.User)
                    .Where(x => x.Id >= page * 10 - 9 && x.Id <= page * 10)
                    .Select(x => new DonatorMask
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        Email = x.User.Email,
                        Adress = x.User.Adress,
                        Phone = x.User.Phone,
                        Gifts = x.gifts.Select(x => x.Name).ToList()
                    });

                var result = await donatorWithgift.ToListAsync();
                _logger.Information($"Successfully retrieved {result.Count} donators for page {page}.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByPage method of DonatorRepository.");
                return null;
            }
        }

        public bool Add(User user)
        {
            try
            {
                _logger.Information("Attempting to add a new donator.");
                _context.User.Add(user);
                _context.SaveChanges();
                Donator d = new Donator { UserId = user.Id };
                _context.Donator.Add(d);
                _context.SaveChanges();

                if (d != null)
                {
                    _logger.Information("Successfully added a new donator.");
                    return true;
                }
                _logger.Warning("Donator was not added.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Add method of DonatorRepository.");
                return false;
            }
        }

        public void Update(User user)
        {
            try
            {
                _logger.Information($"Attempting to update donator with ID {user.Name}.");
                var donetor = _context.Donator.Where(d => d.Id == user.Id).FirstOrDefault();
                User u = new User
                {
                    Id= donetor.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Adress= user.Adress,
                    Phone= user.Phone
                };
               
                if (u != null)
                { 
                    _context.User.Update(u);
                    _context.SaveChanges();
                    _logger.Information($"Successfully updated donator with ID {user.Name}.");
                }
                else
                {
                    _logger.Warning($"Donator with ID {user.Name} not found for update.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Update method of DonatorRepository.");
            }
        }

        public bool Delete(DonatorMask donator)
        {
            try
            {
                _logger.Information($"Attempting to delete donator with ID {donator.Id}.");

                // חיפוש המשתמש בתורם
                User user = _context.User.FirstOrDefault(u => u.Email == donator.Email);
                if (user == null)
                {
                    _logger.Error($"User with email {donator.Email} not found.");
                    return false;
                }

                // חיפוש התורם
                Donator don = _context.Donator.FirstOrDefault(d => d.Id == donator.Id);
                if (don == null)
                {
                    _logger.Error($"Donator with ID {donator.Id} not found.");
                    return false;
                }

                // חיפוש המתנה הקשורה לתורם
                Gift gift = _context.Gift.FirstOrDefault(g => g.DonatorId == donator.Id);

                if (gift != null)
                {
                    // אם יש מתנה, מחק את ההזמנות הקשורות
                    var ord = _context.Order.Where(o => o.GiftId == gift.Id).ToList();
                    if (ord != null && ord.Any(o => o.IsDraft == true))
                    {
                        return false;


                    }
                    else
                     foreach (var o in ord)
                     {
                          _context.Order.Remove(o);
                     }

                    // מחק את המתנה
                    _context.Gift.Remove(gift);
                    _context.SaveChanges();
                }

                // מחק את התורם גם אם אין מתנה
                _context.Donator.Remove(don);
                _context.SaveChanges();

                // מחק את המשתמש
                _context.User.Remove(user);
                _context.SaveChanges();

                _logger.Information($"Successfully deleted donator with ID {donator.Id}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Delete method of DonatorRepository.");
                return false;
            }
        }


        public async Task<IEnumerable<DonatorMask>> GetByName(string name)
        {
            try
            {
                _logger.Information($"Attempting to retrieve donator with name {name}.");
                var donator = _context.Donator.Include(x => x.User)
                    .Where(x => x.User.Name == name)
                    .Select(x => new DonatorMask
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        Email = x.User.Email,
                        Adress = x.User.Adress,
                        Phone = x.User.Phone,
                        Gifts = x.gifts.Select(x => x.Name).ToList()
                    });

                var result = await donator.ToListAsync();
                _logger.Information($"Successfully retrieved {result.Count} donators by name {name}.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByName method of DonatorRepository.");
                return null;
            }
        }

        public async Task<DonatorMask> GetByEmail(string email)
        {
            try
            {
                _logger.Information($"Attempting to retrieve donator by email {email}.");
                var donator = _context.Donator.Include(x => x.User)
                    .Where(x => x.User.Email == email)
                    .Select(x => new DonatorMask
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        Email = x.User.Email,
                        Adress = x.User.Adress,
                        Phone = x.User.Phone,
                        Gifts = x.gifts.Select(x => x.Name).ToList()
                    });

                var result = await donator.SingleOrDefaultAsync();
                _logger.Information($"Successfully retrieved donator with email {email}.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByEmail method of DonatorRepository.");
                return null;
            }
        }

        public async Task<IEnumerable<DonatorMask>> GetByGift(string gift)
        {
            try
            {
                _logger.Information($"Attempting to retrieve donators by gift name {gift}.");
                var don = _context.Gift.Include(x => x.Donator)
                    .Where(x => x.Name == gift)
                    .Select(x => new DonatorMask
                    {
                        Id = x.Donator.Id,
                        Name = x.Donator.User.Name,
                        Email = x.Donator.User.Email,
                        Adress = x.Donator.User.Adress,
                        Phone = x.Donator.User.Phone,
                        Gifts = x.Donator.gifts.Select(x => x.Name).ToList()
                    });

                var result = await don.ToListAsync();
                _logger.Information($"Successfully retrieved {result.Count} donators by gift {gift}.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByGift method of DonatorRepository.");
                return null;
            }
        }
    }
}
