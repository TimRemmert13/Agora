using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Data;
using API.Data.Respositories;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper.Mappers;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Moq;
using NuGet.Frameworks;
using Xunit;
using Xunit.Abstractions;

namespace API.Tests
{
    public class ArtWorkRespositoryTests : SetUpTests
    {
        private readonly IArtWorkRepository _repository;
        private static ITestOutputHelper output;

        public ArtWorkRespositoryTests(ITestOutputHelper output) : base(output)
        {
            _repository = new ArtWorkRespository(GetTestDbContext());
        }

        public static IEnumerable<object[]> GetArtWorkPositive()
        {
            var newArtWork = new ArtWork
            {
                AppUserEmail = "tremmert93@gmail.com",
                Title = "My new Art Work",
                Description = "new description"
            };
            yield return new object[] {newArtWork};
        }
        public static IEnumerable<object[]> GetArtWorkNegative()
        {
            var newArtWork = new ArtWork
            {
                AppUserEmail = "tremmert93@gmail.com",
                Description = "new description"
            };
            yield return new object[] {newArtWork};
        }

        [Theory]
        [MemberData(nameof(GetArtWorkPositive))]
        public async Task CreateArtWorkAsync_ShouldCreateNewArtwork(ArtWork artWork)
        {
            // arrange
            var result = await _repository.CreateArtWorkAsync(artWork);
            await _repository.SaveAllAsync();
            var query = await Context.ArtWorks.Where(x => x.Id == artWork.Id).SingleOrDefaultAsync();
            Assert.Equal(result, query);
        }
        
        [Theory]
        [MemberData(nameof(GetArtWorkNegative))]
        public async Task CreateArtWorkAsync_ShouldRejectNewArtwork(ArtWork artWork)
        {
            // arrange
            await _repository.CreateArtWorkAsync(artWork);
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.SaveAllAsync());
        }

        [Theory]
        [InlineData("tremmert@gmail.com")]
        public async Task GetArtWorkByArtistAsync_ShouldReturnArtWorkListOfArtist(string artist)
        {
            var artworks = await _repository.GetArtWorkByArtistAsync(artist);
            Assert.All(artworks, x => x.AppUserEmail.Equals(artist));
        }

        [Fact]
        public async Task GetArtWorksAsync_ShouldReturnArtWork()
        {
            var artworks = await _repository.GetArtWorksAsync();
            Assert.False(artworks.IsNullOrEmpty());
        }

        [Fact]
        public async Task GetArtWorkByIdAsync_ShouldReturnArtWork()
        {
            // Arrange
            var artworks = await _repository.GetArtWorksAsync();
            // Action
            var selected = await _repository.GetArtWorkByIdAsync(artworks.First().Id);
            // Assert
            Assert.True(selected.Id == artworks.First().Id);
        }

        [Fact]
        public async Task DeleteArtWorkAsync_ShouldDeleteArtWork()
        {
            var artworks = await _repository.GetArtWorksAsync();
            var toBeDeleted = artworks.First();
            _repository.DeleteArtworkAsync(toBeDeleted.Id);
            var result = await _repository.SaveAllAsync();
            var newList = await _repository.GetArtWorksAsync();
            Assert.True(result);
            Assert.DoesNotContain(toBeDeleted, newList);
        }

        [Fact]
        public async Task Update_ShouldUpdateArtWork()
        {
            var artworks = await _repository.GetArtWorksAsync();
            var toBeUpdated = artworks.First();
            var id = toBeUpdated.Id;
            toBeUpdated.Title = "Updated Title";
            _repository.Update(toBeUpdated);
            var result = await _repository.SaveAllAsync();
            var updated = await _repository.GetArtWorkByIdAsync(id);
            Assert.True(result);
            Assert.True(updated.Title.Equals("Updated Title"));
        }
        
    }
    
   
}