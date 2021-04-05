using System;

namespace API.DTOs
{
    public class LikeDto
    {
        public int SourceUserId { get; set; }
        public Guid LikedArtId { get; set; }
    }
}