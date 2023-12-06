using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Versioning;
namespace ContactManager.Models
{
    public class Contact
    {
        public int ContactId { get; set; }//Primary Key

        [Required]
        public string Prenom { get; set; }
        [Required]
        public string Nom { get; set; }

        public string Adresse { get; set; }
        public string Ville { get; set; }

        public string Province { get; set; }

        public string CodePostal { get; set; }


        public string Telephone { get; set; }

        public string Courriel { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreation { get; set; }

        [Required]
        public int CategorieID { get; set; }//Foreign Key

        [StringLength(450)]//meme taille que reference dans DB
        public string? UserId { get; set; }//pour authorisation AspNet user(Id)

        [Required]
        public string? UserName { get; set; }
        //proprietes de navigation
        public Categorie? Categorie { get; set;}

    }
}
