using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AppUser
    {
        [Key]
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        [Required]
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        [Required]
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string ImageUrl { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public IEnumerable<ArtWork> ArtWorks { get; set; }
    }
}