using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_of_equipment_in_classrooms.Models
{
    [Table("document_item")]
    public class DocumentItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("document_id")]
        public int DocumentId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("defect_description")]
        public string DefectDescription { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public virtual Document Document { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem InventoryItem { get; set; }
    }
}