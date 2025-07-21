using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.StoredProcudureEntities
{
    [Table("dbo.InsertCategoryL2")]
    public class Sp_CategoryL2
    {
        public string CatL1_Id { get; set; }
        public string Category_L2 { get; set; }
    }
}
