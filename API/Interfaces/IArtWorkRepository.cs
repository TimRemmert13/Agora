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
        Task<PagedList<AllArtWorksDto>> GetArtWorksAsync(ArtWorkParams param);
        Task<PagedList<ArtWorkDto>> GetArtWorkByArtistAsync(string artist, ArtWorkParams artWorkParams);
        Task<ArtWorkDto> GetArtWorkByIdAsync(Guid id);
        void DeleteArtworkAsync(Guid id);
        Task<ArtWorkDto> CreateArtWorkAsync(ArtWork artWork);

    }
}