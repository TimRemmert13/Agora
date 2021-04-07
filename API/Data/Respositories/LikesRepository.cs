using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Respositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LikesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateLikeAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
        }

        public void DeleteLike(Like like)
        {
            _context.Likes.Remove(like);
        }

        public async Task<ICollection<LikeDto>> GetArtWorkLikesAsync(Guid id)
        {
            return _mapper.Map<ICollection<Like>, ICollection<LikeDto>>(
                await _context.Likes.Where(l => l.LikedArtId == id).ToListAsync()
                );
        }

        public async Task<ICollection<LikeDto>> GetUserLikesAsync(int userId)
        {
            return _mapper.Map<ICollection<Like>, ICollection<LikeDto>>(
                await _context.Likes
                    .Include(l => l.LikedArt)
                    .Where(l => l.LikedArt.ArtistId == userId)
                    .ToListAsync()
            );
        }

        public async Task<ICollection<LikeDto>> GetLikedArtWorkAsync(int userId)
        {
            var likes = await _context.Likes
                .Include(l => l.LikedArt)
                .Where(l => l.SourceUserId == userId).ToListAsync();
            return _mapper.Map<ICollection<Like>, ICollection<LikeDto>>(likes);
        }

        public async Task<Like> GetLikedArtByIdAsync(Guid id, int sourceUserId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.LikedArtId == id && l.SourceUserId == sourceUserId);
        }
    }
}