using System;
using System.Collections.Generic;
using API.Entities;
using Auth0.ManagementApi.Models;

namespace API.DTOs
{
    public class AllArtWorksDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string AppUserId { get; set; }
        public UserDto artist { get; set; }
    }
}