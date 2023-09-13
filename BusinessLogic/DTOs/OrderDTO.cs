using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string IdsProduct { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? Users { get; set; }
    }
}
