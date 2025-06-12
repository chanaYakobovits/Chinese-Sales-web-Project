using static System.Reflection.Metadata.BlobBuilder;
using System.Data;
using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Services
{
    public interface IGiftServices
    {
        public Task<IEnumerable<Gift>> GetByPage(int page);
        public bool Add(Gift gift);
        public bool Update(Gift gift);
        public void Delete(int id);
        public void DeleteMany(int[] id);
        public Task<IEnumerable<GiftMask>> GetByName(string name);
        public Task<IEnumerable<GiftMask>> GetByDonator(string name);
        public  Task<IEnumerable<GiftMask>> GetCheapGift(int page);
        public  Task<IEnumerable<GiftMask>> GetExpensiveGift(int page);
        public  Task<IEnumerable<GiftMask>> GetByCategory(int page);
        public int GetNumCustomer(string name);
        public  Task<IEnumerable<Category>> GetCategory();
    }
}

