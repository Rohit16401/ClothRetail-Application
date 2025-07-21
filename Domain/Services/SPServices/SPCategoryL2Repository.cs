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
    public class SPCategoryL2Repository : SQLGenericRepository<Sp_CategoryL2>,ISPCategoryL2Repository
    {
        public SPCategoryL2Repository(IConnectionStringBuilder connectionStringBuilder) : base(connectionStringBuilder) { }
    }
}
