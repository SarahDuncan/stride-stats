namespace Domain.Responses.Models
{
    public class SummaryGear
    {
        public string Id { get; set; }
        public int ResourceState { get; set; }
        public bool Primary { get; set; }
        public string Name { get; set; }
        public float Distance { get; set; }
    }
}
