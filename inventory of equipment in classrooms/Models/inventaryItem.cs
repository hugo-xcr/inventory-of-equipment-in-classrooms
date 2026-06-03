using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_of_equipment_in_classrooms.Models
{
    [Table("inventory_item")]
    public class InventoryItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("inventory_number")]
        public string InventoryNumber { get; set; } = string.Empty;

        [Column("serial_number")] // Добавлено из скриншота БД
        public string? SerialNumber { get; set; }

        [Column("id_category")]
        public int? CategoryId { get; set; }

        [Column("date_on_accounting")] // Исправлено под реальное имя в БД
        public DateTime? DateOnAccounting { get; set; }

        [Column("initial_cost")]
        public decimal? InitialCost { get; set; }

        [Column("current_state")]
        public string CurrentState { get; set; } = "в наличии";

        [Column("room_id")]
        public int? RoomId { get; set; }

        [Column("custodian_id")]
        public int? CustodianId { get; set; }

        [Column("unit_name")] // Поле из БД
        public string? UnitName { get; set; }

        [Column("okei_code")] // Поле из БД
        public string? OkeiCode { get; set; }

        // Навигационные свойства
        [ForeignKey("CategoryId")]
        public virtual ItemCategory? Category { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }

        [ForeignKey("CustodianId")]
        public virtual User? Custodian { get; set; }
        [Column("quantity")]
        public double Quantity { get; set; } = 1;
    }
}