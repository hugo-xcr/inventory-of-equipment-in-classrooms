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


        private void BtnProfile_Click(object sender, EventArgs e)
        {
            this.Hide();
            var profileForm = new ProfileForm(_currentUserId);
            profileForm.ShowDialog();
            this.Close();
        }

        private void BtnTransfer_Click(object sender, EventArgs e)
        {
            this.Hide();
            var transferForm = new TransferForm(_currentUserId);
            transferForm.ShowDialog();
            this.Close();
        }

        private void BtnSearchForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            var searchForm = new SearchForm(_currentUserId);
            searchForm.ShowDialog();
            this.Close();
        }

        private void BtnEditCard_Click(object sender, EventArgs e)
        {
            this.Hide();
            var editForm = new EditCardForm(_currentUserId);
            editForm.ShowDialog();
            this.Close();
        }

        private void BtnActForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            var actForm = new TransferActForm(_currentUserId);
            actForm.ShowDialog();
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

                        var item = new InventoryItem
                        {
                            Name = cellName,
                            InventoryNumber = invNum,
                            CurrentState = "в наличии",
                            UnitName = "шт.",
                            DateOnAccounting = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                            CustodianId = null
                        };
                        string priceRaw = row.Cell(13).GetValue<string>().Replace(" ", "").Replace(",", ".");
                        if (decimal.TryParse(priceRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                            item.InitialCost = price;
                        string qtyRaw = row.Cell(14).GetValue<string>().Trim().Replace(",", ".");
                        if (double.TryParse(qtyRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out double qty))
                            item.Quantity = qty;
                        else
                            item.Quantity = 1;

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

                dgvPreview.DataSource = _previewData.Take(100).Select(i => new
                {
                    Инв_Номер = i.InventoryNumber,
                    Наименование = i.Name,
                    Кол_во = i.Quantity,
                    Ед_Изм = i.UnitName,
                    Стоимость = i.InitialCost?.ToString("N2"),
                    Владелец_ID = i.CustodianId
                }).ToList();

                lblStatus.Text = $"Загружено {_previewData.Count} строк";
                btnImport.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DatabaseContent())
                {
                    int addedCount = 0;
                    foreach (var item in _previewData)
                    {
                        if (!db.InventoryItems.Any(x => x.InventoryNumber == item.InventoryNumber))
                        {
                            item.CustodianId = _currentUserId > 0 ? _currentUserId : (int?)null;

                            db.InventoryItems.Add(item);
                            addedCount++;
                        }
                    }
                    db.SaveChanges();
                    MessageBox.Show($"Импорт завершен! Добавлено: {addedCount}");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            this.Hide();
            var profileForm = new ProfileForm(_currentUserId);
            profileForm.ShowDialog();
            this.Close();
        }

        private void btnEditCard_Click(object sender, EventArgs e)
        {
            this.Hide();
            var editForm = new EditCardForm(_currentUserId);
            editForm.ShowDialog();
            this.Close();
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            this.Hide();
            var transferForm = new TransferForm(_currentUserId);
            transferForm.ShowDialog();
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var actForm = new TransferActForm(_currentUserId);
            actForm.ShowDialog();
            this.Close();
        }
    }
}

