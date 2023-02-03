namespace Event.Bll.Models;

public class EventUpdateModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerUpdateModel> Organizers { get; set; }
    public IEnumerable<SpeakerUpdateModel> Speakers { get; set; }
}