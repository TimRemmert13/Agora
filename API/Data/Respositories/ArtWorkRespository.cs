using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Respositories
{
    public class ArtWorkRespository : IArtWorkRepository
    {
        private readonly DataContext _context;
        public ArtWorkRespository(DataContext context)
        {
            _context = context;
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

        public async Task<IEnumerable<ArtWork>> GetArtWorkByArtistAsync(string artist)
        {
            return await _context.ArtWorks.Where(x => x.AppUserEmail == artist).ToListAsync();
        }

        public async Task<ArtWork> GetArtWorkByIdAsync(Guid id)
        {
            return await _context.ArtWorks.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ArtWork>> GetArtWorksAsync()
        {
            return await _context.ArtWorks.ToListAsync();
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