namespace Event.Api.Models.Response;

public class EventUpdateResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerUpdateResponse> Organizers { get; set; }
    public IEnumerable<SpeakerUpdateResponse> Speakers { get; set; }
}