using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.StoredProcudureEntities
{
    [Table("dbo.InsertCategoryL1")]
    public class Sp_CategoryL1
    {
        public string CatL0_Id { get; set; }
        public string CategoryL1 { get;set; }
    }
}
