using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.Customer")]
    public class Customer
    {
        [Key]
        public string Customer_Id { get; set; }
        public string Name { get; set; }
        public string Phone_No { get; set; }
        public string City { get; set; }

        public string Type { get; set; }
    }
}
