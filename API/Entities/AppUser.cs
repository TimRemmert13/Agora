using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string ImageUrl { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public ICollection<ArtWork> ArtWorks { get; set; }
        
        public ICollection<AppUserRole> UserRoles { get; set; }
        
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