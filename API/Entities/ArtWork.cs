using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class ArtWork
    {
        // public ArtWork(Guid id, string title, string description, Image image, ICollection<Tag> tags, DateTime created, DateTime updated, AppUser artist, Guid appUserId)
        // {
        //     Id = id;
        //     Title = title;
        //     Description = description;
        //     Image = image;
        //     Tags = tags;
        //     Created = created;
        //     Updated = updated;
        //     Artist = artist;
        //     AppUserId = appUserId;
        // }

        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Image Image { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public AppUser Artist { get; set; }
        public Guid AppUserId { get; set; }
    }
}