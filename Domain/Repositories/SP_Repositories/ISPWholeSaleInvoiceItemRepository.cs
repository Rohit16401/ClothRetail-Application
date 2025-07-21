using Domain.Entities.StoredProcudureEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.SP_Repositories
{
    public interface ISPWholeSaleInvoiceItemRepository : ISQLGenericRepository<Sp_WholesaleInvoiceItem>
    {
    }
}
