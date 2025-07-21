using Domain.Entities.StoredProcudureEntities;
using Domain.Repositories;
using Domain.Repositories.SP_Repositories;
using Domain.Services.EntityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.SPServices
{
    public class SPCategoryL1Repository : SQLGenericRepository<Sp_CategoryL1>, ISPCategoryL1Repository
    {
        public SPCategoryL1Repository(IConnectionStringBuilder connectionStringBuilder) : base(connectionStringBuilder) { }
    }
}
