namespace Domain.Responses.Models
{
    public class SummaryClub
    {
        public long Id { get; set; }
        public int ResourceState { get; set; }
        public string Name { get; set; }
        public string ProfileMedium { get; set; }
        public string CoverPhoto { get; set; }
        public string CoverPhotoSmall { get; set; }
        public string SportType { get; set; }
        public ActivityTypeEnum ActivityTypes { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Private { get; set; }
        public int MemberCount { get; set; }
        public bool Featured { get; set; }
        public bool Verified { get; set; }
        public string Url { get; set; }
    }
}
