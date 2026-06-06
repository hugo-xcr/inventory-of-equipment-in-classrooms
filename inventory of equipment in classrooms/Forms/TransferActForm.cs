using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using OxmlDocument = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace inventory_of_equipment_in_classrooms.Forms
{
    public partial class TransferActForm : Form
    {
        private readonly int _currentUserId;
        private List<InventoryItem> _allEquipment = new List<InventoryItem>();
        private List<(int ItemId, string Name, string InventoryNumber,
            string DateOnAccounting, string Defects, string CustodianName)>
            _selectedItems = new();

        public TransferActForm()
        {
            InitializeComponent();
            _currentUserId = 0;
        }

        public TransferActForm(int currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            this.Load += TransferActForm_Load;
        }

        private void TransferActForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadEquipment();
                ConfigureDataGridView();
                LoadActHistory();
                dtpDate.Value = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnSearchForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            var searchForm = new SearchForm(_currentUserId);
            searchForm.ShowDialog();
            this.Close();
        }



        private void LoadEquipment()
        {
            using (var db = new DatabaseContent())
            {
                _allEquipment = db.InventoryItems
                    .Where(i => i.CurrentState == "в наличии")
                    .OrderBy(i => i.Name)
                    .ToList();

                cmbEquipment.DataSource = new List<InventoryItem>(_allEquipment);
                cmbEquipment.DisplayMember = "Name";
                cmbEquipment.ValueMember = "Id";

                if (_allEquipment.Count > 0)
                    cmbEquipment.SelectedIndex = 0;
            }
        }

        private void ConfigureDataGridView()
        {
            if (dgvItems == null) return;
            dgvItems.Columns.Clear();

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "ItemId", HeaderText = "ID", Visible = false });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Name", HeaderText = "Наименование", Width = 250, ReadOnly = true });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "InvNumber", HeaderText = "Инв. номер", Width = 120, ReadOnly = true });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "DateAcc", HeaderText = "Дата учёта", Width = 100, ReadOnly = true });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Custodian", HeaderText = "МОЛ", Width = 150, ReadOnly = true });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Defects", HeaderText = "Дефекты", Width = 200, ReadOnly = true });

            dgvItems.AllowUserToAddRows = false;
            dgvItems.ReadOnly = true;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.BackgroundColor = System.Drawing.Color.White;
        }

        private void UpdateDataGridView()
        {
            dgvItems.Rows.Clear();
            foreach (var item in _selectedItems)
            {
                dgvItems.Rows.Add(
                    item.ItemId, item.Name, item.InventoryNumber,
                    item.DateOnAccounting, item.CustodianName, item.Defects);
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (cmbEquipment.SelectedValue == null ||
                !(cmbEquipment.SelectedValue is int itemId))
            {
                MessageBox.Show("Выберите оборудование из списка", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedItems.Any(x => x.ItemId == itemId))
            {
                MessageBox.Show("Это оборудование уже добавлено в акт", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new DatabaseContent();
            var item = db.InventoryItems
                .Include(i => i.Custodian)
                .FirstOrDefault(i => i.Id == itemId);

            if (item == null) return;

            string custodianName = item.Custodian != null
                ? $"{item.Custodian.Surname} {item.Custodian.Firstname[0]}." +
                  (item.Custodian.Patronymic != null ? $"{item.Custodian.Patronymic[0]}." : "")
                : "____________________";

            string dateStr = item.DateOnAccounting.HasValue
                ? item.DateOnAccounting.Value.ToString("dd.MM.yyyy")
                : "____________________";

            string defects = txtDefects.Text.Trim();

            _selectedItems.Add((
                itemId,
                item.Name,
                item.InventoryNumber ?? "____________________",
                dateStr,
                defects,
                custodianName
            ));

            UpdateDataGridView();
            txtDefects.Clear();
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите оборудование для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = dgvItems.SelectedRows[0].Index;

            if (selectedIndex >= 0 && selectedIndex < _selectedItems.Count)
            {
                _selectedItems.RemoveAt(selectedIndex);
                UpdateDataGridView();
            }
        }

        private void btnCreateAct_Click(object sender, EventArgs e)
        {
            if (_selectedItems.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одно оборудование в акт", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCommission.Text))
            {
                MessageBox.Show("Укажите комиссию (Ф.И.О.)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new DatabaseContent())
                {
                    var doc = new Models.Document
                    {
                        DocDate = DateTime.SpecifyKind(dtpDate.Value, DateTimeKind.Utc),
                        DocType = "inspection_act",
                        TransferType = "Списание",
                        InspectionConclusion = txtCommission.Text,
                        Status = "Исполнен",
                        SenderId = null,
                        ReceiverId = null,
                        RoomFromId = null,
                        RoomToId = null
                    };

                    db.Documents.Add(doc);
                    db.SaveChanges(); 

                    foreach (var item in _selectedItems)
                    {
                        db.DocumentItems.Add(new DocumentItem
                        {
                            DocumentId = doc.Id, 
                            ItemId = item.ItemId,
                            DefectDescription = item.Defects
                        });
                    }
                    db.SaveChanges();

                    var itemIds = _selectedItems.Select(x => x.ItemId).ToList();
                    var itemsToWriteOff = db.InventoryItems
                        .Where(i => itemIds.Contains(i.Id))
                        .ToList();

                    foreach (var item in itemsToWriteOff)
                    {
                        item.CurrentState = "Списан";
                        item.RoomId = null;
                        item.CustodianId = null;
                    }
                    db.SaveChanges();

                    string filePath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"Акт_осмотра_№{doc.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.docx");

                    GenerateInspectionActDocx(doc, _selectedItems, filePath, txtChairman.Text, txtConclusion.Text);

                    MessageBox.Show(
                        $"✓ Акт осмотра №{doc.Id} успешно создан!\n" +
                        $"Файл сохранён на рабочем столе:\n{System.IO.Path.GetFileName(filePath)}",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadActHistory();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при создании акта: {ex.InnerException?.Message ?? ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadActHistory()
        {
            if (dgvHistoryTransferAct == null) return;

            try
            {
                using var db = new DatabaseContent();

                var history = db.Documents
                    .Include(d => d.DocumentItems)
                        .ThenInclude(di => di.InventoryItem)
                    .Where(d => d.DocType == "inspection_act")
                    .OrderByDescending(d => d.DocDate)
                    .ToList()
                    .Select(d => new
                    {
                        d.Id,
                        Дата = d.DocDate.ToString("dd.MM.yyyy"),
                        Комиссия = d.InspectionConclusion ?? "—",
                        Оборудование = d.DocumentItems.Count == 1
                            ? (d.DocumentItems.First().InventoryItem?.Name ?? "—")
                            : d.DocumentItems.Count == 0
                                ? "—"
                                : $"{d.DocumentItems.Count} позиций",
                        Статус = d.Status ?? "—"
                    })
                    .ToList();

                dgvHistoryTransferAct.DataSource = history;

                if (dgvHistoryTransferAct.Columns.Count > 0)
                {
                    dgvHistoryTransferAct.Columns["Id"].HeaderText = "№";
                    dgvHistoryTransferAct.Columns["Id"].Width = 50;
                    dgvHistoryTransferAct.Columns["Дата"].Width = 100;
                    dgvHistoryTransferAct.Columns["Комиссия"].Width = 250;
                    dgvHistoryTransferAct.Columns["Оборудование"].Width = 200;
                    dgvHistoryTransferAct.Columns["Статус"].Width = 100;

                    dgvHistoryTransferAct.AllowUserToAddRows = false;
                    dgvHistoryTransferAct.ReadOnly = true;
                    dgvHistoryTransferAct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории актов: {ex.Message}");
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvHistoryTransferAct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvHistoryTransferAct.Rows[e.RowIndex].Cells["Id"]?.Value != null &&
                int.TryParse(dgvHistoryTransferAct.Rows[e.RowIndex].Cells["Id"].Value.ToString(), out int docId))
            {
                using var db = new DatabaseContent();
                var items = db.DocumentItems
                    .Include(di => di.InventoryItem)
                    .Where(di => di.DocumentId == docId)
                    .Select(di => "• " + di.InventoryItem.Name +
                                  (string.IsNullOrEmpty(di.DefectDescription) ? "" : $" — {di.DefectDescription}"))
                    .ToList();

                string details = items.Count > 0
                    ? string.Join("\n", items)
                    : "Позиции не найдены";

                MessageBox.Show($"Акт №{docId}\n\nСписываемое оборудование:\n{details}",
                    "Состав акта", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void GenerateInspectionActDocx(Models.Document document,
     List<(int ItemId, string Name, string InventoryNumber,
     string DateOnAccounting, string Defects, string CustodianName)> items,
     string filePath,
     string chairman,
     string conclusion)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                using var wordDoc = WordprocessingDocument.Create(filePath,
                    WordprocessingDocumentType.Document);

                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new OxmlDocument();
                var body = new Body();

                var sectPr = new SectionProperties(
                    new PageMargin { Top = 1134, Bottom = 1134, Left = 1701, Right = 850 }
                );

                var ru = new System.Globalization.CultureInfo("ru-RU");
                var date = DateTime.Now;
                string dateStr = $"«{date:dd}» {date.ToString("MMMM", ru)} {date:yyyy} г.";

                string teacherName = items.Count > 0 && items[0].CustodianName != "____________________"
                    ? items[0].CustodianName
                    : "не назначен";

                AddCenteredBoldParagraph(body, "АКТ ОСМОТРА", "28");
                AddEmptyLine(body);
                AddNormalParagraph(body, "Комиссия в составе:");
                AddNormalParagraph(body, "Председатель:");
                AddNormalParagraph(body, "Заместитель директора А.М. Чернега");
                AddNormalParagraph(body, "Члены комиссии:");
                AddNormalParagraph(body, $"Преподаватель {teacherName}");
                AddNormalParagraph(body, "Комендант Флотская Г.А. - МОЛ");
                AddNormalParagraph(body, "Зам. Директора Иванова Л.Н.");
                AddEmptyLine(body);
                AddNormalParagraph(body, "Провели осмотр технического состояния");
                AddEmptyLine(body);

                foreach (var item in items)
                {
                    AddNormalParagraph(body, $"Наименование объекта: {item.Name}");
                    AddNormalParagraph(body, $"Инвентарный номер: {item.InventoryNumber}");
                    AddNormalParagraph(body, $"Дата принятия к учету: {item.DateOnAccounting}");
                    AddNormalParagraph(body, "Количество: 1 шт.");
                    AddNormalParagraph(body,
                        $"Выявлены следующие дефекты: {(string.IsNullOrWhiteSpace(item.Defects) ? "не выявлены" : item.Defects)}");
                    AddEmptyLine(body);
                }

                AddNormalParagraph(body, "Заключение комиссии:");
                AddEmptyLine(body);
                AddNormalParagraph(body,
                    string.IsNullOrWhiteSpace(conclusion)
                        ? "Вследствие полной утраты потребительских свойств в ходе длительной эксплуатации " +
                          "вышли из строя, пришли в полную негодность и к дальнейшей эксплуатации не пригодны, " +
                          "ремонту и восстановлению не подлежат."
                        : conclusion);

                AddEmptyLine(body);
                CreateSignaturesSection(body, teacherName, dateStr);

                body.AppendChild(sectPr);
                mainPart.Document.AppendChild(body);
                mainPart.Document.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка создания акта: {ex.Message}", ex);
            }
        }
        private void CreateSignaturesSection(Body body, string teacherName, string dateStr)
        {
            AddEmptyLine(body);
            AddNormalParagraph(body, "Подписи:");
            AddEmptyLine(body);
            AddNormalParagraph(body, $"Председатель: А.М. Чернега  ____________  {dateStr}");
            AddEmptyLine(body);
            AddNormalParagraph(body, $"Член комиссии: {teacherName}  ____________  {dateStr}");
            AddEmptyLine(body);
            AddNormalParagraph(body, $"Комендант: Г.А. Флотская  ____________  {dateStr}");
            AddEmptyLine(body);
            AddNormalParagraph(body, $"Зам. директора: Л.Н. Иванова  ____________  {dateStr}");
        }


        private TableCell MakeSignatureCell(string width, string name)
        {
            return new TableCell(
                new TableCellProperties(
                    new TableCellWidth { Width = width, Type = TableWidthUnitValues.Dxa }
                ),
                new Paragraph(
                    new ParagraphProperties(
                        new SpacingBetweenLines { After = "0" }
                    ),
                    new Run(
                        new RunProperties(new FontSize { Val = "22" }),
                        new Text($"____________________________ {name}")
                        { Space = SpaceProcessingModeValues.Preserve }
                    )
                )
            );
        }

        private void AddSignatureRow(Body body, string name1, string name2)
        {
            var table = new Table();
            table.AppendChild(new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Nil },
                    new BottomBorder { Val = BorderValues.Nil },
                    new LeftBorder { Val = BorderValues.Nil },
                    new RightBorder { Val = BorderValues.Nil },
                    new InsideHorizontalBorder { Val = BorderValues.Nil },
                    new InsideVerticalBorder { Val = BorderValues.Nil }
                ),
                new TableWidth { Width = "9360", Type = TableWidthUnitValues.Dxa }
            ));
            table.AppendChild(new TableGrid(
                new GridColumn { Width = "4680" },
                new GridColumn { Width = "4680" }
            ));

            var row = new TableRow();

            // Ячейка 1
            row.AppendChild(new TableCell(
                new TableCellProperties(
                    new TableCellWidth { Width = "4680", Type = TableWidthUnitValues.Dxa }
                ),
                new Paragraph(new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(new FontSize { Val = "22" }),
                    new Text($"____________________________ {name1}")
                    { Space = SpaceProcessingModeValues.Preserve }
                ))
            ));

            // Ячейка 2
            row.AppendChild(new TableCell(
                new TableCellProperties(
                    new TableCellWidth { Width = "4680", Type = TableWidthUnitValues.Dxa }
                ),
                new Paragraph(new ParagraphProperties(
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(new FontSize { Val = "22" }),
                    new Text($"____________________________ {name2}")
                    { Space = SpaceProcessingModeValues.Preserve }
                ))
            ));

            table.AppendChild(row);
            body.AppendChild(table);
            AddEmptyLine(body);
        }

        private void AddBoldItalicParagraph(Body body, string text)
        {
            body.AppendChild(new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { After = "0" }),
                new Run(
                    new RunProperties(new Bold(), new Italic(), new FontSize { Val = "24" }),
                    new Text(text) { Space = SpaceProcessingModeValues.Preserve }
                )
            ));
        }

        private void AddBoldInlineParagraph(Body body, string label, string value)
        {
            var para = new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { After = "0" })
            );

            para.AppendChild(new Run(
                new RunProperties(new FontSize { Val = "24" }),
                new Text(label) { Space = SpaceProcessingModeValues.Preserve }
            ));
            para.AppendChild(new Run(
                new RunProperties(new Bold(), new FontSize { Val = "24" }),
                new Text(value) { Space = SpaceProcessingModeValues.Preserve }
            ));

            body.AppendChild(para);
        }

        private void CreateItemsTable(Body body,
            List<(int ItemId, string Name, string Defects)> items)
        {
            var table = new Table();

            table.AppendChild(new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new RightBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                ),
                new TableWidth { Width = "9360", Type = TableWidthUnitValues.Dxa }
            ));

            table.AppendChild(new TableGrid(
                new GridColumn { Width = "400" },
                new GridColumn { Width = "3500" },
                new GridColumn { Width = "5460" }
            ));

            // Заголовок
            var headerRow = new TableRow();
            AddHeaderCell(headerRow, "№", "400");
            AddHeaderCell(headerRow, "Наименование оборудования", "3500");
            AddHeaderCell(headerRow, "Выявленные дефекты", "5460");
            table.AppendChild(headerRow);

            // Строки
            for (int i = 0; i < items.Count; i++)
            {
                var row = new TableRow();
                AddDataCell(row, (i + 1).ToString(), "400", JustificationValues.Center);
                AddDataCell(row, items[i].Name, "3500", JustificationValues.Left);
                AddDataCell(row,
                    string.IsNullOrWhiteSpace(items[i].Defects) ? "—" : items[i].Defects,
                    "5460", JustificationValues.Left);
                table.AppendChild(row);
            }

            body.AppendChild(table);
        }

        private void CreateSignaturesTable(Body body, string[] commissionLines)
        {
            var table = new Table();
            table.AppendChild(new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Nil },
                    new BottomBorder { Val = BorderValues.Nil },
                    new LeftBorder { Val = BorderValues.Nil },
                    new RightBorder { Val = BorderValues.Nil },
                    new InsideHorizontalBorder { Val = BorderValues.Nil },
                    new InsideVerticalBorder { Val = BorderValues.Nil }
                ),
                new TableWidth { Width = "9360", Type = TableWidthUnitValues.Dxa }
            ));
            table.AppendChild(new TableGrid(
                new GridColumn { Width = "2500" },
                new GridColumn { Width = "4360" },
                new GridColumn { Width = "2500" }
            ));

            foreach (var line in commissionLines)
            {
                var row = new TableRow();
                AddSignatureCell(row, line.Trim(), "2500");
                AddSignatureCell(row, "____________________________", "4360");
                AddSignatureCell(row, "", "2500");
                table.AppendChild(row);
            }

            body.AppendChild(table);
        }

        private void AddHeaderCell(TableRow row, string text, string width)
        {
            var cell = new TableCell(
                new TableCellProperties(
                    new TableCellWidth { Width = width, Type = TableWidthUnitValues.Dxa },
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                ),
                new Paragraph(new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(new Bold(), new FontSize { Val = "20" }),
                    new Text(text)
                ))
            );
            row.AppendChild(cell);
        }

        private void AddDataCell(TableRow row, string text, string width, JustificationValues align)
        {
            var cell = new TableCell(
                new TableCellProperties(
                    new TableCellWidth { Width = width, Type = TableWidthUnitValues.Dxa },
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                ),
                new Paragraph(new ParagraphProperties(
                    new Justification { Val = align },
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(new FontSize { Val = "20" }),
                    new Text(text) { Space = SpaceProcessingModeValues.Preserve }
                ))
            );
            row.AppendChild(cell);
        }

        private void AddSignatureCell(TableRow row, string text, string width)
        {
            var cell = new TableCell(
                new TableCellProperties(
                    new TableCellWidth { Width = width, Type = TableWidthUnitValues.Dxa }
                ),
                new Paragraph(new ParagraphProperties(
                    new SpacingBetweenLines { After = "60" }
                ),
                new Run(
                    new RunProperties(new FontSize { Val = "20" }),
                    new Text(text) { Space = SpaceProcessingModeValues.Preserve }
                ))
            );
            row.AppendChild(cell);
        }

        private void AddCenteredBoldParagraph(Body body, string text, string fontSize)
        {
            body.AppendChild(new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center },
                    new SpacingBetweenLines { After = "0" }
                ),
                new Run(
                    new RunProperties(new Bold(), new FontSize { Val = fontSize }),
                    new Text(text)
                )
            ));
        }

        private void AddNormalParagraph(Body body, string text, bool bold = false)
        {
            var para = new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { After = "0" })
            );

            var runProperties = new RunProperties(new FontSize { Val = "24" });
            if (bold)
            {
                runProperties.AppendChild(new Bold());
            }

            para.AppendChild(new Run(
                runProperties,
                new Text(text) { Space = SpaceProcessingModeValues.Preserve }
            ));

            body.AppendChild(para);
        }

        private void AddNormalParagraph(Body body, string label, string value)
        {
            var para = new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { After = "0" })
            );

            para.AppendChild(new Run(
                new RunProperties(new FontSize { Val = "24" }),
                new Text(label) { Space = SpaceProcessingModeValues.Preserve }
            ));

            para.AppendChild(new Run(
                new RunProperties(new Bold(), new FontSize { Val = "24" }),
                new Text(value) { Space = SpaceProcessingModeValues.Preserve }
            ));

            body.AppendChild(para);
        }
        private void AddEmptyLine(Body body)
        {
            body.AppendChild(new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { After = "0" })
            ));
        }


        private void SetActiveButton(Guna.UI2.WinForms.Guna2Button activeBtn)
        {
            var inactiveColor = System.Drawing.Color.FromArgb(0, 51, 153);
            btnProfile.FillColor = btnEditCard.FillColor = btnTransfer.FillColor = inactiveColor;
            btnProfile.ForeColor = btnEditCard.ForeColor = btnTransfer.ForeColor = System.Drawing.Color.White;
            activeBtn.FillColor = System.Drawing.Color.White;
            activeBtn.ForeColor = System.Drawing.Color.Black;
        }

        private void BtnProfile_Click(object sender, EventArgs e) =>
    FormNavigator.ShowProfile();

        private void BtnEditCard_Click(object sender, EventArgs e) =>
            FormNavigator.ShowEditCard();

        private void BtnTransfer_Click(object sender, EventArgs e) =>
            FormNavigator.ShowTransfer();

        private void guna2Button1_Click(object sender, EventArgs e) =>  
            FormNavigator.ShowTransferAct();

        private void BtnImport_Click(object sender, EventArgs e) =>
            FormNavigator.ShowImport();
    }
}
