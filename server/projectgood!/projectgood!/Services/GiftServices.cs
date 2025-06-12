using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Repositories;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectChinesesActstion.Services
{
    public class GiftServices : IGiftServices
    {
        private readonly IGiftRepository _GiftRepository;
        private readonly IOrderRepository _OrderRepository;
        private static readonly Serilog.ILogger _logger = Log.ForContext<GiftServices>(); // Logger for GiftServices

        public GiftServices(IGiftRepository GiftRepository, IOrderRepository OrderRepository)
        {
            _GiftRepository = GiftRepository;
            _OrderRepository = OrderRepository;
        }

        public async Task<IEnumerable<Gift>> GetByPage(int page)
        {
            try
            {
                _logger.Information($"Fetching gifts on page: {page}");

                var gifts = await _GiftRepository.GetByPage(page);

                if (gifts != null && gifts.Any())
                {
                    _logger.Information($"Successfully fetched {gifts.Count()} gifts on page: {page}");
                    return gifts;
                }
                else
                {
                    _logger.Warning($"No gifts found on page: {page}");
                    return Enumerable.Empty<Gift>();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching gifts on page: {page}");
                return Enumerable.Empty<Gift>();
            }
        }

        public bool Add(Gift gift)
        {
            try
            {
                _logger.Information($"Adding gift with name: {gift.Name}");

                var existingGifts = _GiftRepository.Get();
                if (existingGifts.Any(i => i.Name == gift.Name))
                {
                    _logger.Warning($"Gift with name {gift.Name} already exists");
                    return false;
                }

                var result = _GiftRepository.Add(gift);
                if (result)
                {
                    _logger.Information($"Successfully added gift with name: {gift.Name}");
                }
                else
                {
                    _logger.Warning($"Failed to add gift with name: {gift.Name}");
                }

                return result;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while adding gift with name: {gift.Name}");
                return false;
            }
        }

        public bool Update(Gift gift)
        {
            try
            {
                _logger.Information($"Updating gift with ID: {gift.Id}");
                _GiftRepository.Update(gift);
                _logger.Information($"Successfully updated gift with ID: {gift.Id}");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while updating gift with ID: {gift.Id}");
                return false;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _logger.Information($"Deleting gift with ID: {id}");

                var gifts = _GiftRepository.Get();
                var giftToDelete = gifts.FirstOrDefault(i => i.Id == id);


                //var order = _OrderRepository.GetAll().Where(x => x.IsDraft == false && x.GiftId == id).ToList();
                //if (order != null)
                //{
                //    foreach (var item in order)
                //    {
                //        _OrderRepository.Delete(item.Gift.Name, item.CustomerId);
                //    }
                //}
                //else
                //{
                //    var ordoer = _OrderRepository.GetAll().Where(x => x.IsDraft == true && x.GiftId == id).ToList();
                //    if (ordoer != null)
                //        return;
                //    else return;
                //}

                if (giftToDelete != null)
                {
                    _GiftRepository.Delete(giftToDelete);
                    _logger.Information($"Successfully deleted gift with ID: {id}");
                }
                else
                {
                    _logger.Warning($"Gift with ID: {id} not found for deletion");
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting gift with ID: {id}");
            }
        }

        public void DeleteMany(int[] id)
        {
            try
            {
                _logger.Information($"Deleting multiple gifts with IDs: {string.Join(", ", id)}");

                var gifts = _GiftRepository.Get();
                List<Gift> giftsToDelete = new List<Gift>();

                foreach (var giftId in id)
                {
                    var gift = gifts.FirstOrDefault(g => g.Id == giftId);
                    if (gift != null)
                    {
                        giftsToDelete.Add(gift);
                    }
                }

                if (giftsToDelete.Any())
                {
                    _GiftRepository.DeleteMany(giftsToDelete);
                    _logger.Information($"Successfully deleted {giftsToDelete.Count} gifts");
                }
                else
                {
                    _logger.Warning($"No gifts found for deletion with the provided IDs");
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting multiple gifts with IDs: {string.Join(", ", id)}");
            }
        }

        public Task<IEnumerable<GiftMask>> GetByName(string name)
        {
            try
            {
                _logger.Information($"Fetching gift with name: {name}");
                return _GiftRepository.GetByName(name);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching gift with name: {name}");
                return Task.FromResult<IEnumerable<GiftMask>>(null);
            }
        }

        public Task<IEnumerable<GiftMask>> GetByDonator(string name)
        {
            try
            {
                _logger.Information($"Fetching gifts by donator: {name}");
                return _GiftRepository.GetByDonator(name);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching gifts by donator: {name}");
                return Task.FromResult<IEnumerable<GiftMask>>(null);
            }
        }

        public async Task<IEnumerable<GiftMask>> GetCheapGift(int page)
        {
            try
            {
                _logger.Information($"Fetching cheap gifts on page: {page}");

                var gifts = await _GiftRepository.GetCheapGift(page);

                if (gifts != null && gifts.Any())
                {
                    _logger.Information($"Successfully fetched cheap gifts on page: {page}");
                    return gifts;
                }
                else
                {
                    _logger.Warning($"No cheap gifts found on page: {page}");
                    return Enumerable.Empty<GiftMask>();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching cheap gifts on page: {page}");
                return Enumerable.Empty<GiftMask>();
            }
        }

        public async Task<IEnumerable<GiftMask>> GetExpensiveGift(int page)
        {
            try
            {
                _logger.Information($"Fetching expensive gifts on page: {page}");

                var gifts = await _GiftRepository.GetExpensiveGift(page);

                if (gifts != null && gifts.Any())
                {
                    _logger.Information($"Successfully fetched expensive gifts on page: {page}");
                    return gifts;
                }
                else
                {
                    _logger.Warning($"No expensive gifts found on page: {page}");
                    return Enumerable.Empty<GiftMask>();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching expensive gifts on page: {page}");
                return Enumerable.Empty<GiftMask>();
            }
        }

        public async Task<IEnumerable<GiftMask>> GetByCategory(int page)
        {
            try
            {
                _logger.Information($"Fetching gifts by category on page: {page}");

                var gifts = await _GiftRepository.GetByCategory(page);

                if (gifts != null && gifts.Any())
                {
                    _logger.Information($"Successfully fetched gifts by category on page: {page}");
                    return gifts;
                }
                else
                {
                    _logger.Warning($"No gifts found by category on page: {page}");
                    return Enumerable.Empty<GiftMask>();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching gifts by category on page: {page}");
                return Enumerable.Empty<GiftMask>();
            }
        }

        public int GetNumCustomer(string name)
        {
            try
            {
                _logger.Information($"Fetching number of customers for gift: {name}");
                return _GiftRepository.GetNumCustomer(name);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching number of customers for gift: {name}");
                return 0;
            }
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
            try
            {
                _logger.Information($"Fetching category.");

                var caterory = await _GiftRepository.GetCategory();

                if (caterory != null)
                {
                    _logger.Information($"Successfully fetched caterory.");
                    return caterory;
                }
                else
                {
                    _logger.Warning($"No caterory found.");
                    return Enumerable.Empty<Category>();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching caterory.");
                return Enumerable.Empty<Category>();
            }
        }
    }
}
