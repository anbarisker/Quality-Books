using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBooks.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        
        public string Name { get; set; }

        
        public string MobileNo { get; set; }

        public string StatusMessage { get; set; }

        public byte[] Photo { get; set; }

        public string UserPhoto { get; set; }




    }

}
