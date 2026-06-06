using ClosedXML.Excel;
using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace inventory_of_equipment_in_classrooms.Forms
{
    public partial class ImportForm : Form
    {
        private List<InventoryItem> _previewData = new List<InventoryItem>();
        private string _selectedFilePath = string.Empty;
        private readonly int _currentUserId;

        public ImportForm()
        {
            InitializeComponent();
            _currentUserId = 0;
            this.Load += ImportForm_Load;
        }

        public ImportForm(int currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            this.Load += ImportForm_Load;
        }

        private void ImportForm_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            if (dgvPreview == null) return;

            dgvPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPreview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPreview.AllowUserToAddRows = false;
            dgvPreview.AllowUserToDeleteRows = false;
            dgvPreview.ReadOnly = true;
            dgvPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private void BtnSearchForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            var searchForm = new SearchForm(_currentUserId);
            searchForm.ShowDialog();
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files (*.xlsx)|*.xlsx|Excel Files (*.xls)|*.xls|All Files (*.*)|*.*";
                ofd.Title = "Выберите файл Excel для импорта";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedFilePath = ofd.FileName;
                    txtFilePath.Text = ofd.SafeFileName;
                    LoadPreview();
                }
            }
        }
        public static class ExcelImportService
        {
            public static List<InventoryItem> ImportFromExcel(string filePath)
            {
                var items = new List<InventoryItem>();
                var uniqueInventoryNumbers = new HashSet<string>();

                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet(1);
                    int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;

                    for (int i = 9; i <= lastRow; i++)
                    {
                        var row = worksheet.Row(i);
                        string invNum = row.Cell(10).GetValue<string>().Trim();
                        string cellName = row.Cell(3).GetValue<string>().Trim();

                        if (string.IsNullOrEmpty(invNum) || cellName.Contains("Итого") || cellName.StartsWith("101."))
                            continue;

                        if (uniqueInventoryNumbers.Contains(invNum))
                            continue;

                        uniqueInventoryNumbers.Add(invNum);

                        var item = new InventoryItem
                        {
                            Name = cellName,
                            InventoryNumber = invNum,
                            CurrentState = "в наличии",
                            UnitName = "шт.",
                            DateOnAccounting = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                            CustodianId = null,
                            Quantity = 1
                        };

                        string priceRaw = row.Cell(13).GetValue<string>().Replace(" ", "").Replace(",", ".");
                        if (decimal.TryParse(priceRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                            item.InitialCost = price;

                        string qtyRaw = row.Cell(14).GetValue<string>().Trim().Replace(",", ".");
                        if (double.TryParse(qtyRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out double qty))
                            item.Quantity = qty;

                        items.Add(item);
                    }
                }
                return items;
            }
        }
        private void LoadPreview()
        {
            if (string.IsNullOrEmpty(_selectedFilePath)) return;

            try
            {
                _previewData = ExcelImportService.ImportFromExcel(_selectedFilePath);

                var preview = _previewData.Take(100).Select(i => new
                {
                    Инв_Номер = i.InventoryNumber,
                    Наименование = i.Name,
                    Кол_во = i.Quantity,
                    Ед_Изм = i.UnitName,
                    Стоимость = i.InitialCost?.ToString("N2"),
                    Состояние = i.CurrentState
                }).ToList();

                dgvPreview.DataSource = preview;

                lblStatus.Text = $"Загружено {_previewData.Count} строк для импорта";
                btnImport.Enabled = _previewData.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnImport.Enabled = false;
            }
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnEditCard_Click(object sender, EventArgs e)
        {
            this.Hide();
            var editForm = new EditCardForm(_currentUserId);
            editForm.ShowDialog();
            this.Close();
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

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageBox.Show("Выберите файл для импорта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_previewData == null || _previewData.Count == 0)
            {
                MessageBox.Show("Нет данных для импорта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Будет импортировано {_previewData.Count} записей. Продолжить?",
                "Подтверждение импорта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using var dbContext = DatabaseContent.GetContext();

                int importedCount = 0;
                int skippedCount = 0;
                int errorCount = 0;

                foreach (var item in _previewData)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(item.InventoryNumber))
                        {
                            skippedCount++;
                            continue;
                        }

                        bool exists = dbContext.InventoryItems.Any(i => i.InventoryNumber == item.InventoryNumber);
                        if (exists)
                        {
                            skippedCount++;
                            continue;
                        }

                        var newItem = new InventoryItem
                        {
                            Name = item.Name,
                            InventoryNumber = item.InventoryNumber,
                            CurrentState = "в наличии",
                            UnitName = string.IsNullOrEmpty(item.UnitName) ? "шт." : item.UnitName,
                            Quantity = item.Quantity > 0 ? item.Quantity : 1,
                            InitialCost = item.InitialCost,
                            DateOnAccounting = DateTime.UtcNow,
                            CustodianId = null,
                            RoomId = null
                        };

                        dbContext.InventoryItems.Add(newItem);
                        importedCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"Ошибка импорта строки: {ex.Message}");
                    }
                }

                dbContext.SaveChanges();

                MessageBox.Show($"Импорт завершен!\n" +
                    $"Успешно импортировано: {importedCount}\n" +
                    $"Пропущено (нет инв. номера или дубликат): {skippedCount}\n" +
                    $"Ошибок: {errorCount}",
                    "Результат импорта", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (importedCount > 0)
                {
                    _previewData.Clear();
                    dgvPreview.DataSource = null;
                    txtFilePath.Clear();
                    _selectedFilePath = string.Empty;
                    btnImport.Enabled = false;
                    lblStatus.Text = "Импорт завершен";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

