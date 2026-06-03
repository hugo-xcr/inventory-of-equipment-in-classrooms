using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InventorySystem.Tests
{

    public class TransferService
    {
        private readonly DatabaseContent _db;

        public TransferService(DatabaseContent db)
        {
            _db = db;
        }

        public (bool success, string error) TransferEquipment(
            List<int> itemIds,
            int roomFromId,
            int roomToId,
            int? senderId,
            int? receiverId,
            string transferType)
        {
            if (itemIds == null || itemIds.Count == 0)
                return (false, "Не выбрано оборудование");

            if (roomFromId <= 0)
                return (false, "Не указана исходная аудитория");

            if (string.IsNullOrWhiteSpace(transferType))
                return (false, "Не указан тип передачи");

            var items = _db.InventoryItems
                .Where(i => itemIds.Contains(i.Id))
                .ToList();

            if (items.Count != itemIds.Count)
                return (false, "Одно или несколько наименований оборудования не найдены в базе");

            var wrongRoom = items.FirstOrDefault(i => i.RoomId != roomFromId);
            if (wrongRoom != null)
                return (false, $"Оборудование '{wrongRoom.Name}' не находится в указанной аудитории");

            var document = new Document
            {
                DocDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                DocType = transferType == "Списание" ? "inspection_act" : "waybill_permanent",
                TransferType = transferType,
                SenderId = senderId,
                ReceiverId = receiverId,
                RoomFromId = roomFromId,
                RoomToId = roomToId <= 0 ? null : roomToId,
                Status = "Исполнен"
            };

            _db.Documents.Add(document);
            _db.SaveChanges();

            foreach (var itemId in itemIds)
            {
                _db.DocumentItems.Add(new DocumentItem
                {
                    DocumentId = document.Id,
                    ItemId = itemId,
                    DefectDescription = transferType == "Списание" ? "Требуется списание" : "Передача"
                });
            }

            foreach (var item in items)
            {
                if (roomToId > 0)
                {
                    item.RoomId = roomToId;
                    if (receiverId.HasValue && receiverId > 0)
                        item.CustodianId = receiverId;
                }
                else
                {
                    item.CurrentState = "Списан";
                    item.RoomId = null;
                    item.CustodianId = null;
                }
            }

            _db.SaveChanges();
            return (true, null);
        }

        public (bool success, string error) AddInventoryItem(string name, decimal price, string state, int roomId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Название не может быть пустым");

            if (roomId <= 0)
                return (false, "Некорректный ID аудитории");

            var newItem = new InventoryItem
            {
                Name = name,
                InitialCost = price,
                CurrentState = state,
                RoomId = roomId,
                Quantity = 1,
                InventoryNumber = "INV-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                DateOnAccounting = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc) 
            };

            _db.InventoryItems.Add(newItem);
            _db.SaveChanges();

            return (true, null);
        }

        public class TestDatabaseContent : DatabaseContent
        {
            private readonly string _dbName;

            public TestDatabaseContent(string dbName)
            {
                _dbName = dbName;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // Переопределяем подключение, чтобы тесты не лезли в реальную СУБД
                optionsBuilder.UseInMemoryDatabase(_dbName);
            }
        }

        public class TransferEquipmentTests : IDisposable
        {
            private readonly TestDatabaseContent _db;
            private readonly TransferService _service;

            public TransferEquipmentTests()
            {
                // Генерируем уникальное имя для изолированной БД текущего теста
                string dbName = Guid.NewGuid().ToString();
                _db = new TestDatabaseContent(dbName);
                _service = new TransferService(_db);

                PrepareTestData();
            }

            private void PrepareTestData()
            {
                var roomFrom = new Room { Id = 1, RoomName = "104" };
                var roomTo = new Room { Id = 2, RoomName = "105" };

                var sender = new User
                {
                    Id = 1,
                    Surname = "Иванов",
                    Firstname = "Иван",
                    Patronymic = "Иванович"
                };

                var receiver = new User
                {
                    Id = 2,
                    Surname = "Петров",
                    Firstname = "Пётр",
                    Patronymic = "Петрович"
                };

                var item1 = new InventoryItem
                {
                    Id = 1,
                    Name = "Компьютер",
                    InventoryNumber = "INV001",
                    RoomId = 1,
                    CustodianId = 1,
                    CurrentState = "в наличии",
                    Quantity = 1
                };
                var item2 = new InventoryItem
                {
                    Id = 2,
                    Name = "Монитор",
                    InventoryNumber = "INV002",
                    RoomId = 1,
                    CustodianId = 1,
                    CurrentState = "в наличии",
                    Quantity = 1
                };

                _db.Rooms.AddRange(roomFrom, roomTo);
                _db.Users.AddRange(sender, receiver);
                _db.InventoryItems.AddRange(item1, item2);
                _db.SaveChanges();
            }

            // ── ПОЗИТИВНЫЕ ТЕСТЫ ──────────────────────────────

            [Fact]
            public void Transfer_ValidData_CreatesDocument()
            {
                var requestedItems = new List<int> { 1 };
                int roomFrom = 1;
                int roomTo = 2;
                int senderId = 1;
                int receiverId = 2;
                string transferType = "Постоянная передача";

                var result = _service.TransferEquipment(
                    requestedItems, roomFrom, roomTo, senderId, receiverId, transferType);

                Assert.True(result.success);
                Assert.Equal(1, _db.Documents.Count());
            }

            [Fact]
            public void Transfer_ValidData_UpdatesItemRoom()
            {
                _service.TransferEquipment(
                    new List<int> { 1 }, 1, 2, 1, 2, "Постоянная передача");

                var item = _db.InventoryItems.Find(1);
                Assert.Equal(2, item.RoomId);
            }

            [Fact]
            public void Transfer_ValidData_UpdatesItemCustodian()
            {
                _service.TransferEquipment(
                    new List<int> { 1 }, 1, 2, 1, 2, "Постоянная передача");

                var item = _db.InventoryItems.Find(1);
                Assert.Equal(2, item.CustodianId);
            }

            [Fact]
            public void Transfer_MultipleItems_AllItemsMoved()
            {
                _service.TransferEquipment(
                    new List<int> { 1, 2 }, 1, 2, 1, 2, "Постоянная передача");

                var item1 = _db.InventoryItems.Find(1);
                var item2 = _db.InventoryItems.Find(2);

                Assert.Equal(2, item1.RoomId);
                Assert.Equal(2, item2.RoomId);
            }

            [Fact]
            public void Transfer_MultipleItems_CreatesDocumentItems()
            {
                _service.TransferEquipment(
                    new List<int> { 1, 2 }, 1, 2, 1, 2, "Постоянная передача");

                Assert.Equal(2, _db.DocumentItems.Count());
            }

            [Fact]
            public void WriteOff_ValidData_SetsStatusSpredan()
            {
                _service.TransferEquipment(
                    new List<int> { 1 }, 1, 0, 1, null, "Списание");

                var item = _db.InventoryItems.Find(1);
                Assert.Equal("Списан", item.CurrentState);
            }

            [Fact]
            public void WriteOff_ValidData_ClearsRoomAndCustodian()
            {
                _service.TransferEquipment(
                    new List<int> { 1 }, 1, 0, 1, null, "Списание");

                var item = _db.InventoryItems.Find(1);
                Assert.Null(item.RoomId);
                Assert.Null(item.CustodianId);
            }

            [Fact]
            public void Transfer_DocumentHasCorrectType_ForPermanent()
            {
                _service.TransferEquipment(
                    new List<int> { 1 }, 1, 2, 1, 2, "Постоянная передача");

                var doc = _db.Documents.First();
                Assert.Equal("waybill_permanent", doc.DocType);
            }

            [Fact]
            public void Transfer_DocumentHasCorrectType_ForWriteOff()
            {
                _service.TransferEquipment(
                    new List<int> { 1 }, 1, 0, 1, null, "Списание");

                var doc = _db.Documents.First();
                Assert.Equal("inspection_act", doc.DocType);
            }

            // ── НЕГАТИВНЫЕ И ГРАНИЧНЫЕ ТЕСТЫ ──────────────────

            [Fact]
            public void Transfer_EmptyItemList_ReturnsFalse()
            {
                var result = _service.TransferEquipment(
                    new List<int>(), 1, 2, 1, 2, "Постоянная передача");

                Assert.False(result.success);
                Assert.Equal("Не выбрано оборудование", result.error);
            }

            [Fact]
            public void Transfer_NullItemList_ReturnsFalse()
            {
                var result = _service.TransferEquipment(
                    null, 1, 2, 1, 2, "Постоянная передача");

                Assert.False(result.success);
                Assert.Equal("Не выбрано оборудование", result.error);
            }

            [Fact]
            public void Transfer_InvalidRoomFrom_ReturnsFalse()
            {
                var result = _service.TransferEquipment(
                    new List<int> { 1 }, 0, 2, 1, 2, "Постоянная передача");

                Assert.False(result.success);
                Assert.Equal("Не указана исходная аудитория", result.error);
            }

            [Fact]
            public void Transfer_EmptyTransferType_ReturnsFalse()
            {
                var result = _service.TransferEquipment(
                    new List<int> { 1 }, 1, 2, 1, 2, "");

                Assert.False(result.success);
                Assert.Equal("Не указан тип передачи", result.error);
            }

            [Fact]
            public void Transfer_ItemFromWrongRoom_ReturnsFalse()
            {
                var result = _service.TransferEquipment(
                    new List<int> { 1 }, 2, 1, 1, 2, "Постоянная передача");

                Assert.False(result.success);
                Assert.Contains("не находится в указанной аудитории", result.error);
            }

            [Fact]
            public void Transfer_NonExistentItem_ReturnsFalse()
            {
                var result = _service.TransferEquipment(
                    new List<int> { 999 }, 1, 2, 1, 2, "Постоянная передача");

                Assert.False(result.success);
                Assert.Equal("Одно или несколько наименований оборудования не найдены в базе", result.error);
            }

            [Fact]
            public void Transfer_PartiallyNonExistentItems_DoesNotModifyData()
            {
                var result = _service.TransferEquipment(
                    new List<int> { 1, 999 }, 1, 2, 1, 2, "Постоянная передача");

                Assert.False(result.success);
                Assert.Equal("Одно или несколько наименований оборудования не найдены в базе", result.error);

                var item1 = _db.InventoryItems.Find(1);
                Assert.Equal(1, item1.RoomId); // Данные не должны измениться, так как транзакция/проверка отклонена
            }

            [Fact]
            public void AddItem_ValidData_SavesToDatabase()
            {
                string itemName = "Ноутбук Dell";
                decimal itemPrice = 45000.00m;
                string itemState = "Новое";
                int targetRoomId = 1;

                int initialCount = _db.InventoryItems.Count();

                var result = _service.AddInventoryItem(itemName, itemPrice, itemState, targetRoomId);

                Assert.True(result.success);
                Assert.Equal(initialCount + 1, _db.InventoryItems.Count());

                var addedItem = _db.InventoryItems.FirstOrDefault(i => i.Name == "Ноутбук Dell");
                Assert.NotNull(addedItem);
                Assert.Equal("Новое", addedItem.CurrentState);
                Assert.Equal(targetRoomId, addedItem.RoomId);
            }

            public void Dispose() => _db.Dispose();
        }
    }
}