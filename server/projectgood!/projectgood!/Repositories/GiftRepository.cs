using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectChinesesActstion.Models;
using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Serilog;

namespace ProjectChinesesActstion.Repositories
{
    public class GiftRepository : IGiftRepository
    {
        private readonly ProjectContext _context;

        // Create a logger instance
        private static readonly Serilog.ILogger _logger = Log.ForContext<GiftRepository>();

        public GiftRepository(ProjectContext context)
        {
            _context = context;
        }

        public IEnumerable<Gift> Get()
        {
            try
            {
                _logger.Information("Attempting to get all gifts from the database.");
                var gifts = _context.Gift.ToList();
                _logger.Information($"Successfully retrieved {gifts.Count} gifts.");
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method.");
                return null;
            }
        }

        public async Task<IEnumerable<Gift>> GetByPage(int page)
        {
            try
            {
                _logger.Information($"Attempting to retrieve gifts for page {page}.");
                var giftWithUser = _context.Gift.Where(x => x.Id >= page * 10 - 9 && x.Id <= page * 10)
                    .Select(x => new Gift
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DonatorId = x.DonatorId,
                        Pay = x.Pay,
                        CategoryId = x.CategoryId,
                        Image = x.Image
                    });

                var result = await giftWithUser.ToListAsync();
                if (result.Any())
                {
                    _logger.Information($"Successfully retrieved {result.Count} gifts for page {page}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByPage method.");
                return null;
            }
        }

        public bool Add(Gift gift)
        {
            try
            {
                _logger.Information($"Attempting to add gift with name: {gift.Name}");
                var donator = _context.Donator.Where(d => d.Id == gift.DonatorId);
                var category = _context.Category.Where(c => c.Id == gift.CategoryId);
                if (donator.Count() > 0 && category.Count() > 0)
                {
                    _context.Gift.Add(gift);
                    _context.SaveChanges();
                    _logger.Information($"Successfully added gift: {gift.Name}");
                    return true;
                }
                else
                {
                    _logger.Warning("Donator or Category not found for gift: {GiftName}.", gift.Name);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Add method.");
                return false;
            }
        }

        public bool Update(Gift gift)
        {
            try
            {
                _logger.Information($"Attempting to update gift with ID: {gift.Id}");
                _context.Gift.Update(gift);
                _context.SaveChanges();
                _logger.Information($"Successfully updated gift with ID: {gift.Id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Update method.");
                return false;
            }
        }

        public void Delete(Gift gift)
        {
            try
            {
                var order = _context.Order.FirstOrDefault(p => p.GiftId == gift.Id && p.IsDraft == true);
               var order2 = _context.Order.Where(p => p.GiftId == gift.Id && p.IsDraft == false);
                if (order == null && order2 == null)
                {
                    if (gift != null)
                    {
                        _context.Gift.Remove(gift);
                        _context.SaveChanges();
                    }
                }
                else if (order != null)
                {
                    return;
                }
                else if(order2.Any())
                {
                    foreach (var item in order2)
                    {
                        _context.Order.Remove(item);
                    }
                    _context.Gift.Remove(gift);
                    _context.SaveChanges();
                }
               
                _logger.Information($"Attempting to delete gift with ID: {gift.Id}");
                _context.Gift.Remove(gift);
                _context.SaveChanges();
                _logger.Information($"Successfully deleted gift with ID: {gift.Id}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Delete method.");
            }
        }

        public void DeleteMany(List<Gift> gift)
        {
            try
            {
                _logger.Information("Attempting to delete multiple gifts.");
                foreach (var g in gift)
                {
                    _context.Gift.Remove(g);
                    _context.SaveChanges();
                    _logger.Information($"Successfully deleted gift with ID: {g.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in DeleteMany method.");
            }
        }

        public async Task<IEnumerable<GiftMask>> GetByName(string name)
        {
            try
            {
                _logger.Information($"Attempting to retrieve gift by name: {name}");
                var gift = _context.Gift.Include(x => x.Donator)
                    .Where(x => x.Name == name)
                    .Select(x => new GiftMask
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Donator = x.Donator.User.Name,
                        Pay = x.Pay,
                        Category = x.Category.Name,
                        Image = x.Image
                    });

                var result = await gift.ToListAsync();
                if (result != null)
                {
                    _logger.Information($"Successfully retrieved gift with name: {name}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByName method.");
                return null;
            }
        }

        public async Task<IEnumerable<GiftMask>> GetByDonator(string name)
        {
            try
            {
                _logger.Information($"Attempting to retrieve gifts by donator: {name}");
                var gift = _context.Gift.Include(x => x.Donator)
                    .Where(x => x.Donator.User.Name == name)
                    .Select(x => new GiftMask
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Donator = x.Donator.User.Name,
                        Pay = x.Pay,
                        Category = x.Category.Name,
                        Image = x.Image
                    });

                var result = await gift.ToListAsync();
                if (result.Any())
                {
                    _logger.Information($"Successfully retrieved {result.Count} gifts for donator: {name}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByDonator method.");
                return null;
            }
        }

        public async Task<IEnumerable<GiftMask>> GetCheapGift(int page)
        {
            try
            {
                _logger.Information($"Attempting to retrieve cheap gifts for page {page}.");
                var giftCheap = _context.Gift.Include(x => x.Donator)
                    .Where(x => x.Id >= page * 10 - 9 && x.Id <= page * 10)
                    .OrderBy(x => x.Pay)
                    .Select(x => new GiftMask
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Donator = x.Donator.User.Name,
                        Pay = x.Pay,
                        Category = x.Category.Name,
                        Image = x.Image
                    });

                var result = await giftCheap.ToListAsync();
                if (result.Any())
                {
                    _logger.Information($"Successfully retrieved {result.Count} cheap gifts for page {page}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetCheapGift method.");
                return null;
            }
        }

        public async Task<IEnumerable<GiftMask>> GetByCategory(int page)
        {
            try
            {
                _logger.Information($"Attempting to retrieve gifts by category for page {page}.");
                var giftCategory = _context.Gift.Include(x => x.Donator)
                    .Where(x => x.Id >= page * 10 - 9 && x.Id <= page * 10)
                    .OrderBy(x => x.Category.Name)
                    .Select(x => new GiftMask
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Donator = x.Donator.User.Name,
                        Pay = x.Pay,
                        Category = x.Category.Name,
                        Image = x.Image
                    });

                var result = await giftCategory.ToListAsync();
                if (result.Any())
                {
                    _logger.Information($"Successfully retrieved {result.Count} gifts for page {page}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetByCategory method.");
                return null;
            }
        }

        public async Task<IEnumerable<GiftMask>> GetExpensiveGift(int page)
        {
            try
            {
                _logger.Information($"Attempting to retrieve expensive gifts for page {page}.");
                var giftExpensive = _context.Gift.Include(x => x.Donator)
                    .Where(x => x.Id >= page * 10 - 9 && x.Id <= page * 10)
                    .OrderByDescending(x => x.Pay)
                    .Select(x => new GiftMask
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Donator = x.Donator.User.Name,
                        Pay = x.Pay,
                        Category = x.Category.Name,
                        Image = x.Image
                    });

                var result = await giftExpensive.ToListAsync();
                if (result.Any())
                {
                    _logger.Information($"Successfully retrieved {result.Count} expensive gifts for page {page}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetExpensiveGift method.");
                return null;
            }
        }

        public int GetNumCustomer(string name)
        {
            try
            {
                _logger.Information($"Attempting to get number of customers for gift: {name}");
                var gift = _context.Order.Include(g => g.Gift).Where(g => g.Gift.Name == name).Count();
                _logger.Information($"Number of customers for gift '{name}': {gift}");
                return gift;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetNumCustomer method.");
                return 0;
            }
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
            try
            {
                _logger.Information($"Attempting to retrieve category.");
                var category = _context.Category.ToListAsync(); 
                if (category!= null)
                {
                    _logger.Information($"Successfully retrieved category.");
                }
                return await category;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetCategory method.");
                return null;
            }
        }
    }
}
