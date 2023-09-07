using System.ComponentModel.DataAnnotations;
namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        public int? SquareFt { get; set;}
        public int Ocuupancy { get; set; }
    }
}
