using DataAccess.EntiitesConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [EntityTypeConfiguration(typeof(ProductConfiguration))]
    public class Product
    {
        /*Data Attributes
         * - Required
         * - MaxLength,MinLength
         * - Range
         * - Url
         * - Phone
         * - CreditCard
         * - EmailAddres
         * - Compare
         * - RegulatExpression
         */
        //[Key]
        public int Id { get; set; }
        //[Required, MinLength(3)]
        //[Required (ErrorMessage ="Name is required"), StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public string? Description { get; set; }
        //[Range(0,double.MaxValue, ErrorMessage="Price must be greater than or equal to 0")]
        public decimal Price { get; set; }
        //[Url]
        //[DisplayName("Image Path URL")]
        public string? ImagePath { get; set; }
       // [ForeignKey("Category")]
        public int CategoryId { get; set; }
        //navigation property
        public Category? Category { get; set; }
        public int? StorageId { get; set; }
        public Storage? Storage { get; set; }
    }
}
