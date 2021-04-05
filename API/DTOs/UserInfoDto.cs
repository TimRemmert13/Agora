using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserInfoDto
    {
        [Required]
        public int Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberConfirmed { get; set; }
        public int TwoFactorEnabled { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}