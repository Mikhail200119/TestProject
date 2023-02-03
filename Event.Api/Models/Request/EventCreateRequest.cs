using System.ComponentModel.DataAnnotations;

namespace Event.Api.Models.Request;

public class EventCreateRequest
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public IEnumerable<OrganizerCreateRequest> Organizers { get; set; }
    public IEnumerable<SpeakerCreateRequest> Speakers { get; set; }
}