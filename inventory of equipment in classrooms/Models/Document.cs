using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_of_equipment_in_classrooms.Models
{
    [Table("document")]
    public class Document
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("doc_date")]
        public DateTime DocDate { get; set; }

        [Column("doc_type")]
        [MaxLength(50)]
        public string? DocType { get; set; }

        [Column("transfer_type")]
        [MaxLength(50)]
        public string? TransferType { get; set; } 

        [Column("sender_id")]
        public int? SenderId { get; set; }

        [Column("receiver_id")]
        public int? ReceiverId { get; set; }

        [Column("room_from_id")]
        public int? RoomFromId { get; set; }

        [Column("room_to_id")]
        public int? RoomToId { get; set; }

        [Column("inspection_conclusion")]
        public string? InspectionConclusion { get; set; }

        [Column("return_date")]
        public DateTime? ReturnDate { get; set; }

        [Column("status")]
        [MaxLength(50)]
        public string? Status { get; set; }

        [ForeignKey(nameof(SenderId))]
        public virtual User Sender { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public virtual User Receiver { get; set; }

        [ForeignKey(nameof(RoomFromId))]
        public virtual Room RoomFrom { get; set; }

        [ForeignKey(nameof(RoomToId))]
        public virtual Room RoomTo { get; set; }

        public virtual ICollection<DocumentItem> DocumentItems { get; set; } = new List<DocumentItem>();
    }
}