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
    public class SPCategoryL3Repository : SQLGenericRepository<Sp_CategoryL3>, ISPCategoryL3Repository
    {
        public SPCategoryL3Repository(IConnectionStringBuilder connectionStringBuilder):base(connectionStringBuilder) { }
    }
}
