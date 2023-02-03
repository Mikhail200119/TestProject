namespace Event.Api.Models.Request;

public class EventUpdateRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerUpdateRequest> Organizers { get; set; }
    public IEnumerable<SpeakerUpdateRequest> Speakers { get; set; }
}