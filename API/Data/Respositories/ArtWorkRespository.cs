using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Respositories
{
    public class ArtWorkRespository : IArtWorkRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ArtWorkRespository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArtWork> CreateArtWorkAsync(ArtWork artWork)
        {
            //_context.ArtWorks.Where(x => x.Tags.Any(y => y.Name == "name"));
            await _context.ArtWorks.AddAsync(artWork);
            return artWork;
        }

        public async void DeleteArtworkAsync(Guid id)
        {
            _context.ArtWorks.Remove(await _context.ArtWorks.Where(x => x.Id == id).SingleOrDefaultAsync());
        }

        public async Task<IEnumerable<ArtWork>> GetArtWorkByArtistAsync(string artistId)
        {
            return await _context.ArtWorks.Where(x => x.AppUserId == artistId).ToListAsync();
        }

        public async Task<ArtWork> GetArtWorkByIdAsync(Guid id)
        {
            return await _context.ArtWorks.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<PagedList<AllArtWorksDto>> GetArtWorksAsync(PaginationParams param)
        {
            var query = _context.ArtWorks.AsQueryable();
            query = query.Include(a => a.Artist);
            await _context.ArtWorks.Include(a => a.Artist).ToListAsync();
            return await PagedList<AllArtWorksDto>.CreateAsync(query
                .ProjectTo<AllArtWorksDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(), param.PageNumber, param.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(ArtWork artWork)
        {
            _context.Entry(artWork).State = EntityState.Modified;
        }
    }
}