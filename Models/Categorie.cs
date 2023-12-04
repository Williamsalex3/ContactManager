using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Categorie
    {
        public int CategorieID { get; set; }
        [Required]
        public string? CategorieName { get; set; }


        /*Proprietes de navigation*/
        public ICollection<Contact>? Contacts { get; set; }//plusieurs contacts a une categorie
    }
}
