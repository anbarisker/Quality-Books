using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBooks.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Categroy Name")]
        public string CategroyName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
