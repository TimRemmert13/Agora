using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        public Guid ArtWorkId { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Tag tag && this.Id.Equals(tag.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}