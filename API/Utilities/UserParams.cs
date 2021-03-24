namespace API.Utilities
{
    public class UserParams : PaginationParams
    {
        public string OrderBy { get; set; } = "proximity";
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}