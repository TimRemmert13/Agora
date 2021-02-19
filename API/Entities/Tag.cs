using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Tag :
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        public Guid ArtWorkId { get; set; }
    }
}