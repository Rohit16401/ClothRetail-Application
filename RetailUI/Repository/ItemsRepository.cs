using Domain.Entities;
using RetailUI.DBContext;

namespace RetailUI.Repository
{
    public class ItemsRepository
    {
        private readonly RetailUIDbContext _context;

        public ItemsRepository()
        {
            _context = new RetailUIDbContext();
        }

        public async Task AddItem(Items item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }
    }
}
