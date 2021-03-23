using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AppUser
    {
        [Key]
        public string Id { get; set; }
        [Required]
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
        public ICollection<ArtWork> ArtWorks { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is AppUser user && this.Id.Equals(user.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}