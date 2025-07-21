using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.EntityServices
{
    public class RetailnvoiceRepository : SQLGenericRepository<Retail_Invoice>, IRetailInvoiceRepository
    {
        public RetailnvoiceRepository(IConnectionStringBuilder connectionStringBuilder) : base(connectionStringBuilder)
        {
        }
    }
}
