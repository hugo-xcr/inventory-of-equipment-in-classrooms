using inventory_of_equipment_in_classrooms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("room")]
public class Room
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("room_number")]
    [MaxLength(50)]
    public string RoomName { get; set; }

    [Column("teacher_id")]
    public int? TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public virtual User? Teacher { get; set; }
}