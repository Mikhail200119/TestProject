using System.ComponentModel.DataAnnotations.Schema;
using Event.Dal.Entities.Interfaces;

namespace Event.Dal.Entities;

[Table("Organizer")]
public class OrganizerDbModel : IEntity
{
    [Column("OrganizerId")]
    public int Id { get; set; }
    public string Name { get; set; }
    public int EventId { get; set; }
    public EventDbModel Event { get; set; }
}