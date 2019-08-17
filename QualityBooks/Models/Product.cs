using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBooks.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Image")]
        public Byte[] ProductImage { get; set; }

        public Category Category { get; set; }

        public Supplier Supplier { get; set; }
    }

}
