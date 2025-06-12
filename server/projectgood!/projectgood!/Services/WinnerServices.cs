using Serilog;
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjectChinesesActstion.Services
{
    public class WinnerServices : IWinnerServices
    {
        private readonly IWinnerRepository _WinnerRepository;

        public WinnerServices(IWinnerRepository WinnerRepository)
        {
            _WinnerRepository = WinnerRepository;
        }

        public async Task<CustomerMask> GetByGift(int giftId)
        {
            Log.Information("Attempting to fetch winner by giftId: {GiftId}", giftId);
            var result = await _WinnerRepository.GetByGift(giftId);
            if (result != null)
            {
                Log.Information("Successfully fetched winner for giftId: {GiftId}", giftId);
            }
            else
            {
                Log.Warning("No winner found for giftId: {GiftId}", giftId);
            }
            return result;
        }

        public async Task<IEnumerable<WinnerMask>> Get()
        {
            Log.Information("Fetching all winners.");
            var result = await _WinnerRepository.Get();
            if (result != null)
            {
                Log.Information("Successfully fetched all winners.");
            }
            else
            {
                Log.Warning("No winners found.");
            }
            return result;
        }

        public int GetPayment()
        {
            Log.Information("Fetching total payment.");
            var payment = _WinnerRepository.GetPayment();
            if (payment > 0)
            {
                Log.Information("Successfully fetched payment: {Payment}", payment);
            }
            else
            {
                Log.Warning("No payment data found.");
            }
            return payment;
        }
    }
}
