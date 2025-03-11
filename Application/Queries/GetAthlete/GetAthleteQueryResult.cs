namespace Application.Queries.GetAthlete
{
    public class GetAthleteQueryResult
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public int ResourceState { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string Sex { get; set; }
        public bool Premium { get; set; }
        public bool Summit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int BadgeTypeId { get; set; }
        public float? Weight { get; set; }
        public string ProfileMedium { get; set; }
        public string Profile { get; set; }
        public int? Friend { get; set; }
        public int? Follower { get; set; }
    }
}
