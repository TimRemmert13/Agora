using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using API.Data;
using API.Data.Respositories;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace API.Tests
{
    internal class SetUpTests : IDisposable, ISetupTests
    {

        private DataContext _context;

        internal SetUpTests() {}

        public DataContext GetContext()
        {
            return _context;
        }
        public DataContext GetTestDbContext(string testClass)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite($"data source={testClass}.db").Options;
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