using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Tests
{
    public interface ISetupTests
    {
        DataContext GetTestDbContext(string testClass);
        DataContext GetContext();
        void Dispose();
    }
}