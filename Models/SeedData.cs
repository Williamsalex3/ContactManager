using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContactManager.Data;
using System;
using System.Linq;
namespace ContactManager.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Categorie.Any())
                {
                    return;  // DB has been seeded.
                }
                context.Categorie.AddRange(
                    new Categorie
                    {
                        CategorieName="Famille"
                    },
                    new Categorie
                    {
                        CategorieName = "Ami"
                    },
                    new Categorie
                    {
                        CategorieName = "Travail"
                    },
                    new Categorie
                    {
                        CategorieName = "Autre"
                    });
                //Foreign Key error
                //if (context.Contact.Any())
                //{
                //    return;  // DB has been seeded.
                //}
                //context.Contact.AddRange(
                //    new Contact
                //    {
                //        Nom = "Nom",
                //        Prenom = "Prenom",
                //        Adresse = "32 rue rouge",
                //        Ville = "Moncton",
                //        Province = "Moncton",
                //        CodePostal = "Moncton",
                //        Telephone = "Moncton",
                //        Courriel = "Moncton",
                //        DateCreation = new DateTime(),
                //        CategorieID=2,
                //        UserName="Username"
                //    }
                //   );
                context.SaveChanges();
            }
        }
    }
}
