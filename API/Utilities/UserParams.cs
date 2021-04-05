namespace API.Utilities
{
    public class UserParams : PaginationParams
    {
        /// <summary>
        /// How to sort the response. "proximity" is by artists closest to you, "popular" is by amount of like
        /// an  artwork gets. Proximity is the default value
        /// </summary>
        public string OrderBy { get; set; } = "proximity";
        /// <summary>
        /// The users current longitude to search from
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// The users current latitude to search from
        /// </summary>
        public double Latitude { get; set; }
    }
}