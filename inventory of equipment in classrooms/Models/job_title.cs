using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_of_equipment_in_classrooms.Models
{
    [Table("job_title")]
    public class JobTitle
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(255)]
        public string Name { get; set; }

       
    }
}