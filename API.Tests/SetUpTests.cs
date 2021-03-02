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
    public abstract class SetUpTests : IDisposable
    {
        private ITestOutputHelper _output;

        private protected DataContext Context;

        protected SetUpTests(ITestOutputHelper output)
        {
            _output = output;
        }
        private protected DataContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("data source=Test.db").Options;
            Context = new DataContext(options);
            Context.Database.EnsureCreated();
            if (Context.Users.Count() <= 0)
            {
                // read in users data
                using (StreamReader r = new StreamReader($"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}/TestData/UsersData.json"))
                {
                    string json = r.ReadToEnd();
                    IEnumerable<AppUser> users = JsonSerializer.Deserialize<IEnumerable<AppUser>>(json);
                    Context.Users.AddRange(users);
                }
                // read in art works data
                using (StreamReader r = new StreamReader($"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}/TestData/ArtWorksData.json"))
                {
                    string json = r.ReadToEnd();
                    IEnumerable<ArtWork> artworks = JsonSerializer.Deserialize<IEnumerable<ArtWork>>(json);
                    Context.ArtWorks.AddRange(artworks);
                }
            }

            Context.SaveChanges();
            return Context;
        }
        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}