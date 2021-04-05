using System;

namespace API.Entities
{
    public class Like
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }

        public ArtWork LikedArt { get; set; }
        public Guid LikedArtId { get; set; }
    }
}