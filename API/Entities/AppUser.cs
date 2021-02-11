using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AppUser
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Email { get; set; }
        [Required]
        public bool EmailVerified { get; set; }
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string PictureUrl { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public ICollection<ArtWork> MyProperty { get; set; }

    }
}