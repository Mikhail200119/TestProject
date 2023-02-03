using System.ComponentModel.DataAnnotations.Schema;
using Event.Dal.Entities.Interfaces;

namespace Event.Dal.Entities;

[Table("Event")]
public class EventDbModel : IEntity
{
    [Column("EventId")]
    public int Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public string CreatedBy { get; set; }
    
    public IEnumerable<OrganizerDbModel> Organizers { get; set; }
    public IEnumerable<SpeakerDbModel> Speakers { get; set; }
}