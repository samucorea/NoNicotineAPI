namespace NoNicotineAPI.Models
{
    public class EntryResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool TherapistAllowed { get; set; }

        public string PatientId { get; set; } = string.Empty;
        public List<string> Symptoms { get; set; } = new List<string>();
        public List<string> Feelings { get; set; } = new List<string>();
    }
}
