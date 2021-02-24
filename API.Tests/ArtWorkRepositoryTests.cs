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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Moq;
using NuGet.Frameworks;
using Xunit;
using Xunit.Abstractions;

namespace API.Tests
{
    public class ArtWorkRespositoryTests : IDisposable
    {
        private IArtWorkRepository _repository;

        private ITestOutputHelper _output;

        private DataContext _context;
        public ArtWorkRespositoryTests(ITestOutputHelper output)
        {
            _output = output;
            _repository = new ArtWorkRespository(GetTestDbContextAsync());
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
            var query = await _context.ArtWorks.Where(x => x.Id == artWork.Id).SingleOrDefaultAsync();
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

        private DataContext GetTestDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("data source=Test.db").Options;
            _context = new DataContext(options);
            _context.Database.EnsureCreated();
            if (_context.Users.Count() <= 0)
            {
                // read in users data
                using (StreamReader r = new StreamReader($"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}/TestData/UsersData.json"))
                {
                    string json = r.ReadToEnd();
                    IEnumerable<AppUser> users = JsonSerializer.Deserialize<IEnumerable<AppUser>>(json);
                    _context.Users.AddRange(users);
                }
                // read in art works data
                using (StreamReader r = new StreamReader($"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}/TestData/ArtWorksData.json"))
                {
                    string json = r.ReadToEnd();
                    IEnumerable<ArtWork> artworks = JsonSerializer.Deserialize<IEnumerable<ArtWork>>(json);
                    _context.ArtWorks.AddRange(artworks);
                }
            }

            _context.SaveChanges();
            return _context;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
    
   
}