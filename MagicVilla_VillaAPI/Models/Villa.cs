﻿using System.ComponentModel.DataAnnotations;
namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SquareFt { get; set;}
        public int Ocuupancy { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public static implicit operator Villa (VillaDTO vdto)
        {
            return new Villa()
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
