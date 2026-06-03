using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_of_equipment_in_classrooms.Models
{
    [Table("item_category")]
    public class ItemCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}