using Domain.Entities.StoredProcudureEntities;
using Domain.Repositories;
using Domain.Repositories.SP_Repositories;
using Domain.Services.EntityServices;

namespace Domain.Services.SPServices
{
    public class SpWholeSaleInvoiceItemRepository : SQLGenericRepository<Sp_WholesaleInvoiceItem>, ISPWholeSaleInvoiceItemRepository
    {
        public SpWholeSaleInvoiceItemRepository(IConnectionStringBuilder connectionStringBuilder) : base(connectionStringBuilder) { }
    }
}
