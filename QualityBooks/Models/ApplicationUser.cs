using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QualityBooks.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool Enabled { get; set; }
        public String Address { get; set; }
        public int MobileNo { get; set; }
        public int PostalCode { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public int HomeNo { get; set; }
        public Byte[] Photo { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public String Gender { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        
    }
}
