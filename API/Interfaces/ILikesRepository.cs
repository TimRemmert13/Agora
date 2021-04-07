using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<ICollection<LikeDto>> GetLikedArtWorkAsync(int userId);
        Task<Like> GetLikedArtByIdAsync(Guid id, int sourceUserId);
        Task CreateLikeAsync(Like like);
        Task<ICollection<LikeDto>> GetArtWorkLikesAsync(Guid id);
        Task<ICollection<LikeDto>> GetUserLikesAsync(int userId);
        void DeleteLike(Like like);
    }
}