using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        public int SquareFt { get; set; }
        public int Ocuupancy { get; set; }
        public string? Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
        public static implicit operator VillaDTO(Villa vdto)
        {
            return new VillaDTO()
            {
                Id = vdto.Id,
                Name = vdto.Name,
                SquareFt = vdto.SquareFt,
                Ocuupancy = vdto.Ocuupancy,
                Details = vdto.Details,
                Rate = vdto.Rate,
                ImageUrl = vdto.ImageUrl,
                Amenity = vdto.Amenity
            };
        }
    }
}
