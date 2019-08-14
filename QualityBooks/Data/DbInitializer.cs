using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QualityBooks.Models;

namespace QualityBooks.Data
{
    public static class DbInitializer
    {
        public static void Initialize(QualityBooksContext context)
        {
            context.Database.EnsureCreated();
            // Look for any students.
            if (context.Categories.Any())
            {
                return; // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category{CategroyName="Arts & Music"},
                new Category{CategroyName="Business"},
                new Category{CategroyName="Sports"},
                new Category{CategroyName="Maori Culture"},
            };

            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var suppliers = new Supplier[]
            {
                new Supplier{FirstName="John",LastName="Liew",HomePhoneNo="210856677",MobileNo="210856677",Email="john@email.com"},
                new Supplier{FirstName="Zane",LastName="Chau",HomePhoneNo="210856577",MobileNo="210856577",Email="zane@email.com"},
                new Supplier{FirstName="Peter",LastName="Tan",HomePhoneNo="210856477",MobileNo="210856477",Email="peter@email.com"},
            };

            foreach (Supplier s in suppliers)
            {
                context.Suppliers.Add(s);
            }
            context.SaveChanges();
        }
    }
}