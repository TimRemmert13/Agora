using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Utilities;

namespace API.Interfaces
{
    public interface IArtWorkRepository
    {
        void Update(ArtWork artWork);
        Task<bool> SaveAllAsync();
        Task<PagedList<AllArtWorksDto>> GetArtWorksAsync(PaginationParams param);
        Task<IEnumerable<ArtWork>> GetArtWorkByArtistAsync(string artist);
        Task<ArtWork> GetArtWorkByIdAsync(Guid id);
        void DeleteArtworkAsync(Guid id);
        Task<ArtWork> CreateArtWorkAsync(ArtWork artWork);

    }
}