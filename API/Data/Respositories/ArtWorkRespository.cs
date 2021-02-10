using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;

namespace API.Data.Respositories
{
    public class ArtWorkRespository : IArtWorkRepository
    {
        public Task<ArtWork> CreateArtWork(ArtWork artWork)
        {
            throw new NotImplementedException();
        }

        public Task<ArtWork> DeleteArtwork(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtWork>> GetArtWorkByArtistAsync(string artist)
        {
            throw new NotImplementedException();
        }

        public Task<ArtWork> GetArtWorkByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtWork>> GetArtWorksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        public ArtWork Update(ArtWork artWork)
        {
            throw new NotImplementedException();
        }
    }
}