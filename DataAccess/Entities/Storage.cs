using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Storage
    {
        [Key]
        public int Id { get; set; }
        public int ProductQuantity { get; set; }
        public int? ProductId { get; set; }
        public Product? Products { get; set; }
    }
}
