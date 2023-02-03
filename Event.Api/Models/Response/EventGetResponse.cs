namespace Event.Api.Models.Response;

public class EventGetResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerGetResponse> Organizers { get; set; }
    public IEnumerable<SpeakerGetResponse> Speakers { get; set; }
}