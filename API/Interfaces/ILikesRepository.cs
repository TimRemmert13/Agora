using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<ICollection<LikeDto>> GetLikedArtWork(int userId);
        Task<Like> GetLikedArtByIdAsync(Guid id, int sourceUserId);
        Task CreateLikeAsync(Like like);
        void DeleteLike(Like like);
    }
}