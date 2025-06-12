using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Repositories;
using Serilog;

namespace ProjectChinesesActstion.Services
{
    public class DonatorServices : IDonatorServices
    {
        private readonly IDonatorRepository _DonatorRepository;
        private static readonly Serilog.ILogger _logger = Log.ForContext<DonatorServices>(); // Logger for DonatorServices

        public DonatorServices(IDonatorRepository DonatorRepository)
        {
            _DonatorRepository = DonatorRepository;
        }
        public async Task<IEnumerable<DonatorMask>> Get()
        {
            try
            {
                _logger.Information($"Fetching donators ");

                var donators = await _DonatorRepository.Get();

                if (donators != null && donators.Any())
                {
                    _logger.Information($"Successfully fetched {donators.Count()} donators");
                    return donators;
                }
                else
                {
                    _logger.Warning($"No donators found");
                    return Enumerable.Empty<DonatorMask>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching donators");
                return Enumerable.Empty<DonatorMask>(); // Return an empty list if error occurs
            }
        }
        public async Task<IEnumerable<DonatorMask>> GetByPage(int page)
        {
            try
            {
                _logger.Information($"Fetching donators on page: {page}");

                var donators = await _DonatorRepository.GetByPage(page);

                if (donators != null && donators.Any())
                {
                    _logger.Information($"Successfully fetched {donators.Count()} donators on page: {page}");
                    return donators;
                }
                else
                {
                    _logger.Warning($"No donators found on page: {page}");
                    return Enumerable.Empty<DonatorMask>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching donators on page: {page}");
                return Enumerable.Empty<DonatorMask>(); // Return an empty list if error occurs
            }
        }

        public bool Add(User user)
        {
            try
            {
                _logger.Information($"Adding new donator with email: {user.Email}");
                var result = _DonatorRepository.Add(user);

                if (result)
                {
                    _logger.Information($"Successfully added new donator with email: {user.Email}");
                }
                else
                {
                    _logger.Warning($"Failed to add new donator with email: {user.Email}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while adding new donator with email: {user.Email}");
                return false; // Return false if error occurs
            }
        }

        public bool Update(User user)
        {
            try
            {
                _logger.Information($"Updating donator with ID: {user.Name}");
                _DonatorRepository.Update(user);
                _logger.Information($"Successfully updated donator with ID: {user.Name}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while updating donator with ID: {user.Name}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                _logger.Information($"Deleting donator with ID: {id}");
                var donator = _DonatorRepository.Getall().FirstOrDefault(x => x.Id == id);

                if (donator != null)
                {
                    _DonatorRepository.Delete(donator);
                    _logger.Information($"Successfully deleted donator with ID: {id}");
                    return true;
                }
                else
                {
                    _logger.Warning($"Donator with ID: {id} not found for deletion");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting donator with ID: {id}");
                return false;
            }
        }

        public async Task<IEnumerable<DonatorMask>> GetByName(string name)
        {
            try
            {
                _logger.Information($"Fetching donators with name: {name}");

                var donators = await _DonatorRepository.GetByName(name);

                if (donators != null && donators.Any())
                {
                    _logger.Information($"Successfully fetched donators with name: {name}");
                    return donators;
                }
                else
                {
                    _logger.Warning($"No donators found with name: {name}");
                    return Enumerable.Empty<DonatorMask>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching donators with name: {name}");
                return Enumerable.Empty<DonatorMask>();
            }
        }

        public async Task<DonatorMask> GetByEmail(string email)
        {
            try
            {
                _logger.Information($"Fetching donator with email: {email}");

                var donator = await _DonatorRepository.GetByEmail(email);

                if (donator != null)
                {
                    _logger.Information($"Successfully fetched donator with email: {email}");
                    return donator;
                }
                else
                {
                    _logger.Warning($"No donator found with email: {email}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching donator with email: {email}");
                return null;
            }
        }

        public async Task<IEnumerable<DonatorMask>> GetByGift(string gift)
        {
            try
            {
                _logger.Information($"Fetching donators for gift: {gift}");

                var donators = await _DonatorRepository.GetByGift(gift);

                if (donators != null && donators.Any())
                {
                    _logger.Information($"Successfully fetched donators for gift: {gift}");
                    return donators;
                }
                else
                {
                    _logger.Warning($"No donators found for gift: {gift}");
                    return Enumerable.Empty<DonatorMask>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching donators for gift: {gift}");
                return Enumerable.Empty<DonatorMask>();
            }
        }
    }
}
