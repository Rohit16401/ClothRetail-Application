using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.StoredProcudureEntities
{
    [Table("dbo.InsertCategoryL3")]
    public class Sp_CategoryL3
    {
        public string CatL2_Id { get; set; }
        public string Category_L3 { get; set; }
    }
}
