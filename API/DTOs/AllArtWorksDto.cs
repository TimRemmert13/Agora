using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class AllArtWorksDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string AppUserUsername { get; set; }
        public UserInternalDto Artist { get; set; }

        public class UserInternalDto
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public bool EmailVerified { get; set; }
            public string Username { get; set; }
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public string Name { get; set; }
            public string Nickname { get; set; }
            public string ImageUrl { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
    }
}