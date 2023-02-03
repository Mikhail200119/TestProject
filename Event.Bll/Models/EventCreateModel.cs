namespace Event.Bll.Models;

public class EventCreateModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerCreateModel> Organizers { get; set; }
    public IEnumerable<SpeakerCreateModel> Speakers { get; set; }
}