using Microsoft.EntityFrameworkCore;
using ProjectChinesesActstion.Models;
using System.Data;
using System.Threading.Tasks;

namespace ProjectChinesesActstion.Repositories
{
    public interface IGiftRepository
    {
        public IEnumerable<Gift> Get();
        public Task<IEnumerable<Gift>> GetByPage(int page);
        public bool Add(Gift gift);
        public bool Update(Gift gift);
        public void Delete(Gift gift);
        public void DeleteMany(List<Gift> gift);
        public Task<IEnumerable<GiftMask>> GetByName(string name);
        public Task<IEnumerable<GiftMask>> GetByDonator(string name);
        public int GetNumCustomer(string name);
        public  Task<IEnumerable<GiftMask>> GetCheapGift(int page);
        public  Task<IEnumerable<GiftMask>> GetExpensiveGift(int page);
        public Task<IEnumerable<GiftMask>> GetByCategory(int page);
        public Task<IEnumerable<Category>> GetCategory();

    }
}
