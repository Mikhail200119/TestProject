namespace Event.Bll.Models;

public class EventModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerModel> Organizers { get; set; }
    public IEnumerable<SpeakerModel> Speakers { get; set; }
}