namespace Event.Api.Models.Response;

public class EventCreateResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerCreateResponse> Organizers { get; set; }
    public IEnumerable<SpeakerCreateResponse> Speakers { get; set; }
}