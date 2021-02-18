using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class ArtWork
    {
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
        public string AppUserId { get; set; }
    }
}