using Domain.Entities;
using Domain.Repositories;
namespace Domain.Services.EntityServices
{
    public class ItemsRepository : SQLGenericRepository<Items>, IItemsRepository
    {
        public ItemsRepository(IConnectionStringBuilder connectionStringBuilder) : base(connectionStringBuilder) 
        {

        }
    }
}
