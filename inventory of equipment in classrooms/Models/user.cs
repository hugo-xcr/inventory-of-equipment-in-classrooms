using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_of_equipment_in_classrooms.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Column("password")]
        [MaxLength(255)]
        public string? Password { get; set; }

        [Column("firstname")]
        [MaxLength(100)]
        public string Firstname { get; set; }

        [Column("surname")]
        [MaxLength(100)]
        public string Surname { get; set; }

        [Column("patronymic")]
        [MaxLength(100)]
        public string Patronymic { get; set; }

        [Column("id_job")]
        public int? JobTitleId { get; set; }

        [ForeignKey(nameof(JobTitleId))]
        public virtual JobTitle JobTitle { get; set; }
    }
}