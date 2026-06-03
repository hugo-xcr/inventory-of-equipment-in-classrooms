using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OxmlDocument = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace inventory_of_equipment_in_classrooms.Forms
{
    public partial class TransferForm : Form
    {
        private readonly int _currentUserId;
        private List<int> _selectedEquipmentIds = new List<int>();
        private List<int> _filteredEquipmentIds = null;
        private bool _isProcessing = false;

        public class UserReference
        {
            public int Id { get; set; }
            public string FullName { get; set; }
        }

        public class RoomReference
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? ResponsibleTeacherId { get; set; }
        }

        public TransferForm(int currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            this.Load += TransferForm_Load;

            if (dgvEquipment != null)
            {
                dgvEquipment.AllowUserToAddRows = false;
                dgvEquipment.ReadOnly = true;
                dgvEquipment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvEquipment.MultiSelect = true;
                dgvEquipment.SelectionChanged += DgvEquipment_SelectionChanged;
            }

            if (btnSelectEquipment != null) btnSelectEquipment.Click += BtnSelectEquipment_Click;
            if (btnCreateWaybill != null) btnCreateWaybill.Click += BtnCreateWaybill_Click;
            if (cmbRoomFrom != null) cmbRoomFrom.SelectedIndexChanged += CmbRoomFrom_SelectedIndexChanged;
            if (cmbRoomTo != null) cmbRoomTo.SelectedIndexChanged += CmbRoomTo_SelectedIndexChanged;
            if (btnProfile != null) btnProfile.Click += BtnProfile_Click;
            if (btnEditCard != null) btnEditCard.Click += BtnEditCard_Click;
        }

        private static DatabaseContent GetDbContext() => DatabaseContent.GetContext();

        private void DgvEquipment_SelectionChanged(object sender, EventArgs e)
        {
            CollectSelectedEquipment();
        }

        private void TransferForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                LoadEquipmentData();
                LoadTransferHistory();
                UpdateSelectedCountLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке формы: {ex.InnerException?.Message ?? ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadTransferHistory()
        {
            if (dgvHistoryTransfer == null) return;

            try
            {
                using var db = new DatabaseContent();

                var history = db.Documents
                    .Include(d => d.Sender)
                    .Include(d => d.Receiver)
                    .Include(d => d.RoomFrom)
                    .Include(d => d.RoomTo)
                    .Include(d => d.DocumentItems)
                        .ThenInclude(di => di.InventoryItem)
                    .OrderByDescending(d => d.DocDate)
                    .ToList()
                    .Select(d => new
                    {
                        d.Id,
                        Дата = d.DocDate.ToString("dd.MM.yyyy"),
                        Тип = d.TransferType ?? "—",
                        Откуда = d.RoomFrom?.RoomName ?? "—",
                        Куда = d.RoomTo?.RoomName ?? "Списание",
                        Отправитель = d.Sender != null
                            ? d.Sender.Surname + " " + d.Sender.Firstname
                            : "—",
                        Получатель = d.Receiver != null
                            ? d.Receiver.Surname + " " + d.Receiver.Firstname
                            : "—",
                        // Если одна позиция — показываем название, если больше — "N позиций"
                        Оборудование = d.DocumentItems.Count == 1
                            ? (d.DocumentItems.First().InventoryItem?.Name ?? "—")
                            : d.DocumentItems.Count == 0
                                ? "—"
                                : $"{d.DocumentItems.Count} позиций",
                        Статус = d.Status ?? "—"
                    })
                    .ToList();

                dgvHistoryTransfer.DataSource = history;

                if (dgvHistoryTransfer.Columns.Count > 0)
                {
                    dgvHistoryTransfer.Columns["Id"].HeaderText = "№";
                    dgvHistoryTransfer.Columns["Id"].Width = 40;
                    dgvHistoryTransfer.Columns["Дата"].Width = 90;
                    dgvHistoryTransfer.Columns["Тип"].Width = 130;
                    dgvHistoryTransfer.Columns["Откуда"].Width = 70;
                    dgvHistoryTransfer.Columns["Куда"].Width = 70;
                    dgvHistoryTransfer.Columns["Отправитель"].Width = 130;
                    dgvHistoryTransfer.Columns["Получатель"].Width = 130;
                    dgvHistoryTransfer.Columns["Оборудование"].Width = 160;
                    dgvHistoryTransfer.Columns["Статус"].Width = 80;

                    dgvHistoryTransfer.AllowUserToAddRows = false;
                    dgvHistoryTransfer.ReadOnly = true;
                    dgvHistoryTransfer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}");
            }
        }
        private void LoadData()
        {
            if (cmbRoomFrom != null) cmbRoomFrom.SelectedIndexChanged -= CmbRoomFrom_SelectedIndexChanged;
            if (cmbRoomTo != null) cmbRoomTo.SelectedIndexChanged -= CmbRoomTo_SelectedIndexChanged;

            using var dbContext = new DatabaseContent();

            // 1. Загрузка пользователей с правильным склеиванием ФИО
            var users = dbContext.Users
                .Select(u => new UserReference
                {
                    Id = u.Id,
                    FullName = u.Surname + " " + u.Firstname + (u.Patronymic != null ? " " + u.Patronymic : "")
                })
                .OrderBy(u => u.FullName)
                .ToList();

            // 2. Загрузка комнат (используем правильное имя RoomName)
            var rooms = dbContext.Rooms
                .Select(r => new RoomReference
                {
                    Id = r.Id,
                    Name = r.RoomName,
                    ResponsibleTeacherId = null // Оставляем null, раз поле временно отключено
                })
                .OrderBy(r => r.Name)
                .ToList();

            var transferTypes = new List<string> { "Постоянная передача", "Временная передача", "Списание" };

            if (cmbTransferType != null) { cmbTransferType.DataSource = transferTypes; cmbTransferType.SelectedIndex = 0; }

            // 3. Настройка выпадающего списка Отправителей
            if (cmbSender != null)
            {
                cmbSender.DataSource = users.ToList();
                cmbSender.DisplayMember = "FullName";
                cmbSender.ValueMember = "Id";
                cmbSender.SelectedValue = _currentUserId;
            }

            // 4. Настройка выпадающего списка Получателей
            if (cmbReceiver != null)
            {
                var receivers = users.ToList();
                receivers.Insert(0, new UserReference { Id = -1, FullName = "Не назначен / Списание" });
                cmbReceiver.DataSource = receivers;
                cmbReceiver.DisplayMember = "FullName";
                cmbReceiver.ValueMember = "Id";
                cmbReceiver.SelectedValue = -1;
            }

            // 5. Настройка выпадающего списка Исходных комнат
            if (cmbRoomFrom != null)
            {
                cmbRoomFrom.DataSource = rooms.ToList();
                cmbRoomFrom.DisplayMember = "Name";
                cmbRoomFrom.ValueMember = "Id";
                cmbRoomFrom.SelectedIndex = -1; // Чтобы форма открывалась пустой, без предвыбора
            }

            // 6. Настройка выпадающего списка Конечных комнат
            if (cmbRoomTo != null)
            {
                var toRooms = rooms.ToList();
                toRooms.Insert(0, new RoomReference { Id = -1, Name = "Не назначена / Списание", ResponsibleTeacherId = null });
                cmbRoomTo.DataSource = toRooms;
                cmbRoomTo.DisplayMember = "Name";
                cmbRoomTo.ValueMember = "Id";
                cmbRoomTo.SelectedValue = -1;
            }
            if (cmbRoomFrom != null) cmbRoomFrom.SelectedIndexChanged += CmbRoomFrom_SelectedIndexChanged;
            if (cmbRoomTo != null) cmbRoomTo.SelectedIndexChanged += CmbRoomTo_SelectedIndexChanged;
        }

        private void LoadEquipmentData()
        {
            if (_isProcessing) return;

            if (cmbRoomFrom?.SelectedItem == null)
            {
                dgvEquipment.DataSource = null;
                return;
            }

            if (!(cmbRoomFrom.SelectedItem is RoomReference selectedRoom) || selectedRoom.Id <= 0)
            {
                return;
            }

            try
            {
                int roomId = selectedRoom.Id;
                int? senderId = (cmbSender?.SelectedItem as UserReference)?.Id;

                using (var db = new DatabaseContent())
                {
                    var list = db.InventoryItems
                        .Where(x => x.RoomId == roomId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.InventoryNumber,
                            x.Quantity,
                            x.CurrentState
                        }).ToList();

                    dgvEquipment.DataSource = list;

                    if (dgvEquipment.Columns.Count > 0)
                    {
                        dgvEquipment.Columns["Id"].Visible = false;
                        dgvEquipment.Columns["Name"].HeaderText = "Наименование";
                        dgvEquipment.Columns["InventoryNumber"].HeaderText = "Инв. номер";
                        dgvEquipment.Columns["Quantity"].HeaderText = "Кол-во";
                        dgvEquipment.Columns["CurrentState"].HeaderText = "Статус";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке оборудования: {ex.Message}\n\nInner: {ex.InnerException?.Message}");
            }
        }

        private void UpdateSelectedCountLabel()
        {
            if (lblSelectedCount != null)
            {
                lblSelectedCount.Text = $"Выбрано оборудования: {_selectedEquipmentIds.Count}";
            }
            if (txtQuantity != null)
            {
                txtQuantity.Text = _selectedEquipmentIds.Count.ToString();
            }
        }

        private void CollectSelectedEquipment()
        {
            if (dgvEquipment == null) return;

            _selectedEquipmentIds.Clear();
            foreach (DataGridViewRow row in dgvEquipment.SelectedRows)
            {
                if (dgvEquipment.Columns["Id"] != null &&
                    row.Cells["Id"]?.Value != null &&
                    int.TryParse(row.Cells["Id"].Value.ToString(), out int id))
                {
                    _selectedEquipmentIds.Add(id);
                }
            }
            UpdateSelectedCountLabel();
        }

        private void BtnSelectEquipment_Click(object sender, EventArgs e)
        {
            int? filterRoomId = null;
            if (cmbRoomFrom?.SelectedValue != null && int.TryParse(cmbRoomFrom.SelectedValue.ToString(), out int fromRoomId) && fromRoomId > 0)
            {
                filterRoomId = fromRoomId;
            }

            using var searchForm = new SearchForm(_currentUserId, filterRoomId);

            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                _filteredEquipmentIds = searchForm.SelectedInventoryItemIds;
                LoadEquipmentData();

                if (_filteredEquipmentIds != null)
                {
                    dgvEquipment.ClearSelection();
                    _selectedEquipmentIds.Clear();

                    foreach (DataGridViewRow row in dgvEquipment.Rows)
                    {
                        if (row.Cells["Id"]?.Value != null && int.TryParse(row.Cells["Id"].Value.ToString(), out int id))
                        {
                            if (_filteredEquipmentIds.Contains(id))
                            {
                                row.Selected = true;
                            }
                        }
                    }
                    CollectSelectedEquipment();
                }
            }
        }

        private void BtnCreateWaybill_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;
            _isProcessing = true;
            if (btnCreateWaybill != null) btnCreateWaybill.Enabled = false;

            CollectSelectedEquipment();

            if (_selectedEquipmentIds.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите оборудование.", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetProcessing();
                return;
            }

            if (cmbTransferType?.SelectedItem == null || cmbSender?.SelectedValue == null || cmbRoomFrom?.SelectedValue == null)
            {
                MessageBox.Show("Необходимо выбрать тип передачи, отправителя и исходную аудиторию.", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetProcessing();
                return;
            }

            string transferType = cmbTransferType.SelectedItem.ToString();

            if (!int.TryParse(cmbSender.SelectedValue.ToString(), out int senderId) ||
                !int.TryParse(cmbRoomFrom.SelectedValue.ToString(), out int roomFromId) ||
                !int.TryParse(cmbReceiver.SelectedValue.ToString(), out int receiverId) ||
                !int.TryParse(cmbRoomTo.SelectedValue.ToString(), out int roomToId))
            {
                ResetProcessing();
                return;
            }

            try
            {
                using var dbContext = new DatabaseContent();

                var newDocument = new Models.Document
                {
                    DocDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    DocType = (transferType == "Списание") ? "inspection_act" : "waybill_permanent",
                    TransferType = transferType,
                    SenderId = (senderId <= 0) ? (int?)null : senderId,
                    ReceiverId = (receiverId <= 0) ? (int?)null : receiverId,
                    RoomFromId = (roomFromId <= 0) ? (int?)null : roomFromId,
                    RoomToId = (roomToId <= 0) ? (int?)null : roomToId,
                    Status = "Исполнен"
                };

                dbContext.Documents.Add(newDocument);
                dbContext.SaveChanges();

                foreach (int itemId in _selectedEquipmentIds)
                {
                    dbContext.DocumentItems.Add(new DocumentItem
                    {
                        DocumentId = newDocument.Id,
                        ItemId = itemId
                    });
                }
                dbContext.SaveChanges();
                if (roomToId > 0)
                {
                    var itemsToUpdate = dbContext.InventoryItems
                        .Where(i => _selectedEquipmentIds.Contains(i.Id))
                        .ToList();

                    foreach (var item in itemsToUpdate)
                    {
                        item.RoomId = roomToId;
                        if (receiverId > 0)
                            item.CustodianId = receiverId;
                    }
                }
                else
                {
                    var itemsToUpdate = dbContext.InventoryItems
                        .Where(i => _selectedEquipmentIds.Contains(i.Id))
                        .ToList();

                    foreach (var item in itemsToUpdate)
                    {
                        item.CurrentState = "Списан";
                        item.RoomId = null;
                        item.CustodianId = null;
                    }
                }
                dbContext.SaveChanges();

                var docData = dbContext.Documents
                    .Include(d => d.DocumentItems)
                    .Include(d => d.Sender)
                    .Include(d => d.Receiver)
                    .Include(d => d.RoomFrom)
                    .Include(d => d.RoomTo)
                    .FirstOrDefault(d => d.Id == newDocument.Id);

                var itemsData = dbContext.InventoryItems
                    .Where(i => _selectedEquipmentIds.Contains(i.Id))
                    .ToList();

                string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string docxFilePath = Path.Combine(downloadsPath, $"Документ_{newDocument.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.docx");

                if (docData != null)
                    GenerateWaybillDocxFromScratch(docData, itemsData, docxFilePath);

                MessageBox.Show($"Документ №{newDocument.Id} успешно создан.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _selectedEquipmentIds.Clear();
                UpdateSelectedCountLabel();
                LoadEquipmentData();
                LoadTransferHistory();
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message
                            ?? ex.InnerException?.Message
                            ?? ex.Message;
                MessageBox.Show($"Ошибка: {inner}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ResetProcessing();
            }
        }

        private void ResetProcessing()
        {
            _isProcessing = false;
            if (btnCreateWaybill != null) btnCreateWaybill.Enabled = true;
        }

        // Принимает Models.Document (наша модель), не OpenXML Document
        public void GenerateWaybillDocxFromScratch(Models.Document document, List<InventoryItem> items, string filePath)
        {
            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new OxmlDocument();
                    Body body = new Body();

                    AddParagraphLarge(body, "Накладная № " + document.Id.ToString().PadLeft(12, '0'), true, false, JustificationValues.Center);
                    AddParagraphLarge(body, "на внутреннее перемещение нефинансовых активов", false, false, JustificationValues.Center);

                    var date = document.DocDate;
                    var russianCulture = new CultureInfo("ru-RU");
                    AddParagraphLarge(body, $"от \"{date:dd}\" {date.ToString("MMMM", russianCulture)} {date:yyyy} г.", false, false, JustificationValues.Center);

                    body.AppendChild(new Paragraph(new Run(new Text(""))));

                    CreateCompactVerticalHeaderTable(body, document);

                    body.AppendChild(new Paragraph(new Run(new Text(""))));
                    AddParagraphNormal(body, "Сведения о передаваемых объектах:", false, false, JustificationValues.Left);

                    CreateCorrectItemsTable(body, items);

                    mainPart.Document.AppendChild(body);
                    mainPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании Word-файла: {ex.Message}", ex);
            }
        }

        private class SignatoryData
        {
            public string Type { get; set; }
            public string Position { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
        }

        private const string STATIC_EMAIL = "s252734@std.novsu.ru";
        private const string STATIC_JOB_TITLE = "Преподаватель";
        private const string RESPONSIBLE_JOB_TITLE = "Бухгалтер";
        private const string RESPONSIBLE_FALLBACK_FULL_NAME = "А. С. Ильина";

        private void CreateSignatureLine(Body body, string type, string positionText, string nameText, string phone, bool showContactRow, bool isResponsible = false)
        {
            Table table = new Table();

            TableProperties tableProperties = new TableProperties(
                new TableWidth() { Width = "100%", Type = TableWidthUnitValues.Pct },
                new TableLayout() { Type = TableLayoutValues.Fixed }
            );

            TableBorders noBorders = new TableBorders(
                new TopBorder() { Val = BorderValues.Nil },
                new BottomBorder() { Val = BorderValues.Nil },
                new LeftBorder() { Val = BorderValues.Nil },
                new RightBorder() { Val = BorderValues.Nil },
                new InsideHorizontalBorder() { Val = BorderValues.Nil },
                new InsideVerticalBorder() { Val = BorderValues.Nil }
            );
            tableProperties.Append(noBorders);

            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "2000" },
                new GridColumn() { Width = "2000" },
                new GridColumn() { Width = "2000" },
                new GridColumn() { Width = "3000" }
            );
            table.Append(tableProperties, tableGrid);

            TableRow dataRow = new TableRow();
            AddTableCellNoParagraphIndent(dataRow, type, JustificationValues.Left, "10", false, false);
            AddTableCellNoParagraphIndent(dataRow, positionText, JustificationValues.Center, "10", true, true);
            AddTableCellNoParagraphIndent(dataRow, "_____________________", JustificationValues.Center, "10", true, true);
            AddTableCellNoParagraphIndent(dataRow, nameText, JustificationValues.Center, "10", true, true);
            table.Append(dataRow);

            TableRow captionRow = new TableRow();
            AddTableCellNoParagraphIndent(captionRow, "", JustificationValues.Left, "10", false, false);
            AddTableCellNoParagraphIndent(captionRow, "(должность)", JustificationValues.Center, "10", false, false);
            AddTableCellNoParagraphIndent(captionRow, "(подпись)", JustificationValues.Center, "10", false, false);
            AddTableCellNoParagraphIndent(captionRow, "(расшифровка подписи)", JustificationValues.Center, "10", false, false);
            table.Append(captionRow);

            if (showContactRow)
            {
                TableRow contactLineRow = new TableRow();
                AddMergedTableCellNoParagraphIndent(contactLineRow, "______________________________", JustificationValues.Center, "10", true, 2);
                AddMergedTableCellNoParagraphIndent(contactLineRow, "s252734@std.novsu.ru", JustificationValues.Center, "10", true, 2);
                table.Append(contactLineRow);

                TableRow contactCaptionRow = new TableRow();
                AddMergedTableCellNoParagraphIndent(contactCaptionRow, "(номер контактного телефона)", JustificationValues.Center, "10", false, 2);
                AddMergedTableCellNoParagraphIndent(contactCaptionRow, "(электронный адрес)", JustificationValues.Center, "10", false, 2);
                table.Append(contactCaptionRow);

                if (isResponsible)
                {
                    TableRow dateRow = new TableRow();
                    AddMergedTableCellNoParagraphIndent(dateRow, "\"12\"  11  2025 г.", JustificationValues.Center, "10", true, 4);
                    table.Append(dateRow);
                }
            }

            body.AppendChild(table);
            body.AppendChild(new Paragraph(new Run(new Text(" "))));
        }

        private void AddTableCellNoParagraphIndent(TableRow row, string text, JustificationValues alignment, string fontSize, bool underline, bool addSpaceForInput)
        {
            TableCell cell = new TableCell(new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
            Paragraph paragraph = new Paragraph(new ParagraphProperties(
                new Justification() { Val = alignment },
                new SpacingBetweenLines() { After = "0" },
                new Indentation() { Left = "0", Right = "0", FirstLine = "0" }
            ));

            Run run = new Run();
            RunProperties props = new RunProperties(new FontSize() { Val = fontSize });
            if (underline) props.Underline = new Underline() { Val = UnderlineValues.Single };
            run.RunProperties = props;
            run.AppendChild(new Text(text));

            paragraph.Append(run);
            cell.Append(paragraph);
            row.Append(cell);
        }

        private void AddMergedTableCellNoParagraphIndent(TableRow row, string text, JustificationValues alignment, string fontSize, bool underline, int colSpan)
        {
            TableCell cell = new TableCell();
            TableCellProperties props = new TableCellProperties(new GridSpan() { Val = colSpan });
            props.Append(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
            cell.Append(props);

            Paragraph paragraph = new Paragraph(new ParagraphProperties(
                new Justification() { Val = alignment },
                new SpacingBetweenLines() { After = "0" }
            ));

            Run run = new Run();
            RunProperties runProps = new RunProperties(new FontSize() { Val = fontSize });
            if (underline) runProps.Underline = new Underline() { Val = UnderlineValues.Single };
            run.RunProperties = runProps;
            run.Append(new Text(text));

            paragraph.Append(run);
            cell.Append(paragraph);
            row.Append(cell);
        }

        // Принимает Models.Document
        private void CreateCompactSignaturesSection(Body body, Models.Document document)
        {
            // Используем PascalCase свойства навигации
            string senderName = document.Sender != null
                ? $"{document.Sender.Surname} {document.Sender.Firstname[0]}. {document.Sender.Patronymic?[0]}."
                : "________________";

            string receiverName = document.Receiver != null
                ? $"{document.Receiver.Surname} {document.Receiver.Firstname[0]}. {document.Receiver.Patronymic?[0]}."
                : "________________";

            string senderJob = document.Sender?.JobTitle?.Name ?? "Ответственное лицо";
            string receiverJob = document.Receiver?.JobTitle?.Name ?? "Принимающее лицо";

            CreateSignatureLine(body, "Передал", senderJob, senderName, "", false);
            CreateSignatureLine(body, "Принял", receiverJob, receiverName, "", false);
            CreateSignatureLine(body, "Ответственный Исполнитель", "Бухгалтер", "А. С. ИЛЬИНА", "(номер контактного телефона)", true, true);
        }

        private void CreateCorrectItemsTable(Body body, List<InventoryItem> items)
        {
            Table table = new Table();

            TableProperties tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder() { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder() { Val = BorderValues.Single, Size = 4 },
                    new RightBorder() { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder() { Val = BorderValues.Single, Size = 4 }
                ),
                new TableWidth() { Width = "100%", Type = TableWidthUnitValues.Pct },
                new TableLayout() { Type = TableLayoutValues.Fixed }
            );

            table.Append(tableProperties);
            CreateCorrectTableHeader(table);

            decimal totalCost = 0;
            int totalQuantity = 0;

            foreach (var item in items)
            {
                // Используем PascalCase: InitialCost, Name, CurrentState, Id, InventoryNumber
                decimal itemCost = item.InitialCost.HasValue ? item.InitialCost.Value : 0;
                totalCost += itemCost;
                totalQuantity += 1;

                TableRow dataRow = new TableRow();

                AddTableCellNormalNoIndent(dataRow, Safe(TruncateString(item.Name, 30)), JustificationValues.Left);
                AddTableCellNormalNoIndent(dataRow, Safe(item.CurrentState ?? ""), JustificationValues.Left);

                AddTableCellNormalNoIndent(dataRow, "", JustificationValues.Center);
                AddTableCellNormalNoIndent(dataRow, Safe(item.InventoryNumber ?? item.Id.ToString().PadLeft(5, '0')), JustificationValues.Center);
                AddTableCellNormalNoIndent(dataRow, item.Id.ToString().PadLeft(12, '0'), JustificationValues.Center);

                AddTableCellNormalNoIndent(dataRow, GetUnitName(item), JustificationValues.Center);
                AddTableCellNormalNoIndent(dataRow, GetOKEICode(item), JustificationValues.Center);

                AddTableCellNormalNoIndent(dataRow, "1", JustificationValues.Right);
                AddTableCellNormalNoIndent(dataRow, itemCost.ToString("F2").Replace('.', ','), JustificationValues.Right);

                AddTableCellNormalNoIndent(dataRow, itemCost.ToString("F2").Replace('.', ','), JustificationValues.Right);

                table.Append(dataRow);
            }

            TableRow finalTotalRow = new TableRow();
            AddMergedTableCellNormalNoIndent(finalTotalRow, "Итого", JustificationValues.Left, 7);
            AddTableCellNormalNoIndent(finalTotalRow, totalQuantity.ToString(), JustificationValues.Right);
            AddTableCellNormalNoIndent(finalTotalRow, "x", JustificationValues.Right);
            AddTableCellNormalNoIndent(finalTotalRow, totalCost.ToString("F2").Replace('.', ','), JustificationValues.Right);

            table.Append(finalTotalRow);
            body.Append(table);
        }

        private string GetUnitName(InventoryItem item)
        {
            string itemName = item.Name?.ToLower() ?? "";

            if (itemName.Contains("компьютер") || itemName.Contains("ноутбук") || itemName.Contains("монитор") ||
                itemName.Contains("проектор") || itemName.Contains("принтер") || itemName.Contains("сканер"))
                return "штука";
            else if (itemName.Contains("стол") || itemName.Contains("стул") || itemName.Contains("шкаф") ||
                     itemName.Contains("доска") || itemName.Contains("парта"))
                return "штука";
            else if (itemName.Contains("кабель") || itemName.Contains("провод"))
                return "метр";
            else if (itemName.Contains("краска") || itemName.Contains("лак") || itemName.Contains("клей"))
                return "килограмм";
            else if (itemName.Contains("комплект") || itemName.Contains("набор"))
                return "комплект";
            else
                return "штука";
        }

        private string GetOKEICode(InventoryItem item)
        {
            string unitName = GetUnitName(item).ToLower();

            switch (unitName)
            {
                case "штука": return "796";
                case "метр": return "006";
                case "литр": return "112";
                case "килограмм": return "166";
                case "комплект": return "839";
                case "упаковка": return "778";
                default: return "";
            }
        }

        private void CreateCorrectTableHeader(Table table)
        {
            TableGrid tableGrid = new TableGrid(
                new GridColumn() { Width = "2500" },
                new GridColumn() { Width = "2000" },
                new GridColumn() { Width = "1200" },
                new GridColumn() { Width = "1500" },
                new GridColumn() { Width = "1800" },
                new GridColumn() { Width = "1200" },
                new GridColumn() { Width = "1200" },
                new GridColumn() { Width = "1200" },
                new GridColumn() { Width = "1800" },
                new GridColumn() { Width = "1800" }
            );

            table.Append(tableGrid);

            TableRow headerRow1 = new TableRow();

            AddMergedHeaderCell(headerRow1, "Материальные ценности", 2, 1, "10", true);
            AddMergedHeaderCell(headerRow1, "Код строки", 1, 2, "10", true);
            AddMergedHeaderCell(headerRow1, "Учетный номер", 2, 1, "10", true);
            AddMergedHeaderCell(headerRow1, "Единица измерения", 2, 1, "10", true);
            AddMergedHeaderCell(headerRow1, "Количество", 1, 2, "10", true);
            AddMergedHeaderCell(headerRow1, "Цена/\nПервоначальная\n(балансовая)\nстоимость", 1, 2, "10", true);
            AddMergedHeaderCell(headerRow1, "Стоимость объекта\n(группы объектов)", 1, 2, "10", true);

            table.Append(headerRow1);

            TableRow headerRow2 = new TableRow();

            AddNormalHeaderCellNoNumber(headerRow2, "наименование основное", "10", true);
            AddNormalHeaderCellNoNumber(headerRow2, "наименование дополнительное\n(код) (при наличии)", "10", true);
            AddVerticalMergeContinueCell(headerRow2);
            AddNormalHeaderCellNoNumber(headerRow2, "инвентарный/\nноменклатурный", "10", true);
            AddNormalHeaderCellNoNumber(headerRow2, "паспорта\n(иной)", "10", true);
            AddNormalHeaderCellNoNumber(headerRow2, "наименование", "10", true);
            AddNormalHeaderCellNoNumber(headerRow2, "код по\nОКЕИ", "10", true);
            AddVerticalMergeContinueCell(headerRow2);
            AddVerticalMergeContinueCell(headerRow2);
            AddVerticalMergeContinueCell(headerRow2);

            table.Append(headerRow2);

            TableRow headerRow3 = new TableRow();

            for (int i = 1; i <= 10; i++)
                AddNumberHeaderCell(headerRow3, i.ToString(), "10", true);

            table.Append(headerRow3);
        }

        private void AddVerticalMergeContinueCell(TableRow row)
        {
            TableCell cell = new TableCell();
            cell.Append(new TableCellProperties(
                new VerticalMerge() { Val = MergedCellValues.Continue },
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            ));
            cell.Append(new Paragraph());
            row.Append(cell);
        }

        private void AddMergedHeaderCell(TableRow row, string text, int columnSpan, int rowSpan, string fontSize, bool bold)
        {
            TableCell cell = new TableCell();
            TableCellProperties cellProps = new TableCellProperties(
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            );

            if (columnSpan > 1)
                cellProps.Append(new GridSpan() { Val = columnSpan });

            if (rowSpan == 2)
                cellProps.Append(new VerticalMerge() { Val = MergedCellValues.Restart });

            cell.Append(cellProps);

            Paragraph p = new Paragraph(
                new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center },
                    new SpacingBetweenLines() { After = "0" },
                    new Indentation() { Left = "0", Right = "0", FirstLine = "0" }
                )
            );

            Run r = new Run();
            RunProperties rp = new RunProperties();
            if (bold) rp.Bold = new Bold();
            rp.FontSize = new FontSize() { Val = fontSize };
            r.Append(rp);

            foreach (var line in text.Split('\n'))
            {
                r.Append(new Text(line));
                r.Append(new Break());
            }

            p.Append(r);
            cell.Append(p);
            row.Append(cell);
        }

        private void AddNormalHeaderCellNoNumber(TableRow row, string text, string fontSize, bool bold)
        {
            TableCell cell = new TableCell();
            cell.Append(new TableCellProperties(
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            ));

            Paragraph p = new Paragraph(
                new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center },
                    new SpacingBetweenLines() { After = "0" }
                )
            );

            Run r = new Run();
            RunProperties rp = new RunProperties();
            if (bold) rp.Bold = new Bold();
            rp.FontSize = new FontSize() { Val = fontSize };
            r.Append(rp);

            foreach (var line in text.Split('\n'))
            {
                r.Append(new Text(line));
                r.Append(new Break());
            }

            p.Append(r);
            cell.Append(p);
            row.Append(cell);
        }

        private void AddNumberHeaderCell(TableRow row, string number, string fontSize, bool bold)
        {
            TableCell cell = new TableCell();
            cell.Append(new TableCellProperties(
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            ));

            Paragraph p = new Paragraph(
                new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center },
                    new SpacingBetweenLines() { After = "0" }
                )
            );

            Run r = new Run();
            RunProperties rp = new RunProperties();
            if (bold) rp.Bold = new Bold();
            rp.FontSize = new FontSize() { Val = fontSize };
            r.Append(rp);

            r.Append(new Text(number));

            p.Append(r);
            cell.Append(p);
            row.Append(cell);
        }

        private void AddTableCellNormalNoIndent(TableRow row, string text, JustificationValues align)
        {
            TableCell cell = new TableCell();
            cell.Append(new TableCellProperties(
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            ));

            Paragraph p = new Paragraph(
                new ParagraphProperties(
                    new Justification() { Val = align },
                    new SpacingBetweenLines() { After = "0" },
                    new Indentation() { Left = "0", Right = "0", FirstLine = "0" }
                )
            );

            Run r = new Run(
                new RunProperties(new FontSize() { Val = "10" })
            );

            foreach (var line in text.Split('\n'))
            {
                r.Append(new Text(line));
                r.Append(new Break());
            }

            p.Append(r);
            cell.Append(p);
            row.Append(cell);
        }

        private void AddMergedTableCellNormalNoIndent(TableRow row, string text, JustificationValues align, int span)
        {
            TableCell cell = new TableCell();
            cell.Append(new TableCellProperties(
                new GridSpan() { Val = span },
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            ));

            Paragraph p = new Paragraph(
                new ParagraphProperties(
                    new Justification() { Val = align },
                    new SpacingBetweenLines() { After = "0" }
                )
            );

            Run r = new Run(
                new RunProperties(new FontSize() { Val = "10" })
            );

            foreach (var line in text.Split('\n'))
            {
                r.Append(new Text(line));
                r.Append(new Break());
            }

            p.Append(r);
            cell.Append(p);
            row.Append(cell);
        }

        // Исправлено: принимает Models.Document вместо удалённого Waybill
        private void CreateCompactVerticalHeaderTable(Body body, Models.Document document)
        {
            string senderName = FormatUser(document.Sender);
            string receiverName = FormatUser(document.Receiver);

            Table table = new Table();

            TableProperties tblProps = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.None },
                    new BottomBorder { Val = BorderValues.None },
                    new LeftBorder { Val = BorderValues.None },
                    new RightBorder { Val = BorderValues.None },
                    new InsideHorizontalBorder { Val = BorderValues.None },
                    new InsideVerticalBorder { Val = BorderValues.None }
                )
            );

            tblProps.Append(new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct });
            table.AppendChild(tblProps);

            AddRow(table, "Учреждение:",
                "Федеральное государственное бюджетное образовательное учреждение высшего образования \"Новгородский государственный университет имени Ярослава Мудрого\"");
            AddRow(table, "Обособленное подразделение:", "");
            AddRow(table, "Структурное подразделение:", "Отдел материального учета");
            AddRow(table, "Главный администратор бюджетных средств (Учредитель):",
                "Министерство науки и высшего образования Российской Федерации");
            AddRow(table, "Наименование бюджета:", "Федеральный бюджет");
            // Используем навигационные свойства RoomFrom/RoomTo и их поле RoomNumber
            AddRow(table, "Отправитель:", document.RoomFrom?.RoomName ?? "—");
            AddRow(table, "Ответственное лицо - отправитель:", senderName);
            AddRow(table, "Получатель:", document.RoomTo?.RoomName ?? "—");
            AddRow(table, "Ответственное лицо - получатель:", receiverName);
            AddRow(table, "Единица измерения:", "руб. (с точностью до второго десятичного знака)");
            AddRow(table, "Документ-основание:", "");

            body.Append(table);
        }

        private void AddRow(Table table, string label, string value)
        {
            TableRow row = new TableRow();

            Run labelRun = new Run(new Text(label))
            {
                RunProperties = new RunProperties(
                    new FontSize() { Val = "10" }
                )
            };

            Paragraph labelParagraph = new Paragraph(labelRun)
            {
                ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Left })
            };

            TableCell labelCell = new TableCell(labelParagraph);
            labelCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "35" }
            ));

            if (string.IsNullOrEmpty(value))
            {
                value = new string(' ', 80);
            }
            else
            {
                value = value.PadRight(80);
            }

            Run underlineRun = new Run(new Text(value))
            {
                RunProperties = new RunProperties(
                    new Underline() { Val = UnderlineValues.Single },
                    new FontSize() { Val = "10" }
                )
            };

            Paragraph valueParagraph = new Paragraph(underlineRun)
            {
                ParagraphProperties = new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Left },
                    new SpacingBetweenLines() { After = "0", Before = "0", Line = "240" }
                )
            };

            TableCell valueCell = new TableCell(valueParagraph);
            valueCell.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "65" }
            ));

            row.Append(labelCell, valueCell);
            table.Append(row);
        }

        private string FormatUser(User? user)
        {
            if (user == null) return "—";
            return $"{user.Surname} {user.Firstname} {user.Patronymic}".Trim();
        }

        private void AddParagraphLarge(Body body, string text, bool bold, bool underline, JustificationValues alignment)
        {
            Paragraph paragraph = new Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();
            paragraphProperties.Justification = new Justification() { Val = alignment };
            paragraphProperties.Append(new Indentation() { Left = "0", Right = "0", FirstLine = "0" });
            paragraph.AppendChild(paragraphProperties);

            Run run = new Run();
            RunProperties runProperties = new RunProperties();
            runProperties.FontSize = new FontSize() { Val = "16" };

            if (bold)
                runProperties.Bold = new Bold();
            if (underline)
                runProperties.Underline = new Underline() { Val = UnderlineValues.Single };

            run.RunProperties = runProperties;
            run.AppendChild(new Text(text));

            paragraph.AppendChild(run);
            body.AppendChild(paragraph);
        }

        private void AddParagraphNormal(Body body, string text, bool bold, bool underline, JustificationValues alignment)
        {
            Paragraph paragraph = new Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();
            paragraphProperties.Justification = new Justification() { Val = alignment };
            paragraphProperties.Append(new Indentation() { Left = "0", Right = "0", FirstLine = "0" });
            paragraph.AppendChild(paragraphProperties);

            Run run = new Run();
            RunProperties runProperties = new RunProperties();
            runProperties.FontSize = new FontSize() { Val = "10" };

            if (bold)
                runProperties.Bold = new Bold();
            if (underline)
                runProperties.Underline = new Underline() { Val = UnderlineValues.Single };

            run.RunProperties = runProperties;
            run.AppendChild(new Text(text));

            paragraph.AppendChild(run);
            body.AppendChild(paragraph);
        }

        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
        }

        private string Safe(string? value)
        {
            return string.IsNullOrEmpty(value) ? "—" : value.Trim();
        }

        private void CmbRoomTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRoomTo?.SelectedValue is int roomToId && roomToId > 0)
            {
                if (cmbRoomTo.DataSource is List<RoomReference> rooms)
                {
                    var selectedRoom = rooms.FirstOrDefault(r => r.Id == roomToId);
                    if (selectedRoom?.ResponsibleTeacherId.HasValue == true)
                    {
                        cmbReceiver.SelectedValue = selectedRoom.ResponsibleTeacherId.Value;
                        return;
                    }
                }
            }
            if (cmbReceiver != null)
                cmbReceiver.SelectedValue = -1;
        }

        private void CmbRoomFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isProcessing || cmbRoomFrom?.SelectedItem == null) return;

            if (cmbRoomFrom.SelectedItem is RoomReference selectedRoom && selectedRoom.Id > 0)
            {
                using var db = new DatabaseContent();
                var custodianId = db.InventoryItems
                    .Where(x => x.RoomId == selectedRoom.Id && x.CustodianId != null)
                    .Select(x => x.CustodianId)
                    .FirstOrDefault();

                if (cmbSender != null)
                {
                    if (custodianId.HasValue && custodianId.Value > 0)
                        cmbSender.SelectedValue = custodianId.Value;
                    else
                        cmbSender.SelectedValue = _currentUserId;
                }
            }

            LoadEquipmentData();
        }

        private void BtnProfile_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnEditCard_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void cmbSender_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.Hide();
            var importForm = new ImportForm(_currentUserId);
            importForm.ShowDialog();
            this.Close();
        }


        private void dgvHistoryTransfer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvHistoryTransfer.Rows[e.RowIndex].Cells["Id"]?.Value != null &&
                int.TryParse(dgvHistoryTransfer.Rows[e.RowIndex].Cells["Id"].Value.ToString(), out int docId))
            {
                using var db = new DatabaseContent();
                var items = db.DocumentItems
                    .Include(di => di.InventoryItem)
                    .Where(di => di.DocumentId == docId)
                    .Select(di => "• " + di.InventoryItem.Name + " [" + di.InventoryItem.InventoryNumber + "]")
                    .ToList();

                string details = items.Count > 0
                    ? string.Join("\n", items)
                    : "Позиции не найдены";

                MessageBox.Show($"Документ №{docId}\n\nСостав передачи:\n{details}",
                    "Состав документа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var transferActForm = new TransferActForm(_currentUserId);

                transferActForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть форму акта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditCard_Click(object sender, EventArgs e)
        {
            this.Hide();
            var editForm = new EditCardForm(_currentUserId);
            editForm.ShowDialog();
            this.Close();
        }
    }
}
