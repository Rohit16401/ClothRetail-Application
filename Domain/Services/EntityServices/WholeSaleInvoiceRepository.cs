using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.EntityServices
{
    public  class WholeSaleInvoiceRepository : SQLGenericRepository<WholeSale_Invoice> , IWholeSaleInvoiceRepository
    {
        public WholeSaleInvoiceRepository(IConnectionStringBuilder connectionStringBuilder) : base(connectionStringBuilder) { }
    }
}
