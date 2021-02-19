using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IArtWorkRepository
    {
        void Update(ArtWork artWork);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<ArtWork>> GetArtWorksAsync();
        Task<IEnumerable<ArtWork>> GetArtWorkByArtistAsync(string artist);
        Task<ArtWork> GetArtWorkByIdAsync(Guid id);
        void DeleteArtwork(Guid id);
        Task<ArtWork> CreateArtWorkAsync(ArtWork artWork);

    }
}