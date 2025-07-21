using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services.EntityServices
{
    public class CartRepository : SQLGenericRepository<Cart_View>,ICartRepository
    {
        public CartRepository(IConnectionStringBuilder connection) : base(connection) { }
    }
}
