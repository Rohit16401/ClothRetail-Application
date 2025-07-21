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
    public class SPCategoryL0Repository : SQLGenericRepository<Sp_CategoryL0>,ISPCategoryL0Repository
    {
        public SPCategoryL0Repository(IConnectionStringBuilder connection) : base(connection) { }
    }
}
