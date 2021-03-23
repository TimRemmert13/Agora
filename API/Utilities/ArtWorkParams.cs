namespace API.Utilities
{
    public class ArtWorkParams : PaginationParams
    {
        public string OrderBy { get; set; } = "proximity";
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}