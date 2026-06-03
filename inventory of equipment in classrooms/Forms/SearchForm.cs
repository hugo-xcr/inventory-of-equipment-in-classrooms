using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using inventory_of_equipment_in_classrooms.Models;
using inventory_of_equipment_in_classrooms.Data;

namespace inventory_of_equipment_in_classrooms.Forms
{
    public partial class SearchForm : Form
    {
        private readonly int _currentUserId;
        private readonly int? _filterRoomId;

        public List<int> SelectedInventoryItemIds { get; private set; } = new List<int>();
        public List<InventoryItem> SelectedInventoryItems { get; private set; } = new List<InventoryItem>();

        public SearchForm(int currentUserId) : this(currentUserId, null)
        {
        }

        public SearchForm(int currentUserId, int? filterRoomId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            _filterRoomId = filterRoomId;
            InitializeSearchColumns();
        }

        private static DatabaseContent GetDbContext()
        {
            return new DatabaseContent();
        }

        private void InitializeSearchColumns()
        {
            if (cmbSearchColumn?.Items.Count == 0)
            {
                var columns = new[]
                {
            new { Name = "Основное средство", Value = "Name" },
            new { Name = "Инвентарный номер", Value = "InventoryNumber" }, // Было Id, стало InventoryNumber
            new { Name = "Серийный номер", Value = "SerialNumber" },      // Добавили
            new { Name = "Ответственный", Value = "Custodian" },
            new { Name = "Текущее состояние", Value = "CurrentState" },
            new { Name = "Первоначальная стоимость", Value = "InitialCost" },
            new { Name = "Аудитория", Value = "Room" }
        };

                cmbSearchColumn.DataSource = columns;
                cmbSearchColumn.DisplayMember = "Name";
                cmbSearchColumn.ValueMember = "Value";
                cmbSearchColumn.SelectedIndex = 0;
            }
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
        }

        private void BtnExecuteSearch_Click(object sender, EventArgs e)
        {
            if (cmbSearchColumn?.SelectedValue == null) return;

            string searchValue = txtSearchValue.Text.Trim();
            string selectedColumn = cmbSearchColumn.SelectedValue.ToString();

            using var dbContext = GetDbContext();

            if (string.IsNullOrEmpty(searchValue))
            {
                ShowAllItems();
                return;
            }

            try
            {
                IQueryable<InventoryItem> query = dbContext.InventoryItems;

                if (_filterRoomId.HasValue)
                    query = query.Where(i => i.RoomId == _filterRoomId.Value);

                List<int> filteredItemIds = new();

                switch (selectedColumn)
                {
                    case "Name":
                        filteredItemIds = query.Where(i => i.Name.Contains(searchValue)).Select(i => i.Id).ToList();
                        break;

                    case "InventoryNumber": 
                        filteredItemIds = query.Where(i => i.InventoryNumber.Contains(searchValue)).Select(i => i.Id).ToList();
                        break;

                    case "SerialNumber": 
                        filteredItemIds = query.Where(i => i.SerialNumber != null && i.SerialNumber.Contains(searchValue)).Select(i => i.Id).ToList();
                        break;

                    case "Custodian":
                        filteredItemIds = query.Include(i => i.Custodian).AsEnumerable()
                            .Where(i => i.Custodian != null &&
                                ($"{i.Custodian.Surname} {i.Custodian.Firstname}").Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                            .Select(i => i.Id).ToList();
                        break;

                    case "CurrentState":
                        filteredItemIds = query.Where(i => i.CurrentState.Contains(searchValue)).Select(i => i.Id).ToList();
                        break;

                    case "Room":
                        filteredItemIds = query.Include(i => i.Room)
                            .Where(i => i.Room != null && i.Room.RoomName.Contains(searchValue))
                            .Select(i => i.Id).ToList();
                        break;

                    case "InitialCost":
                        if (decimal.TryParse(searchValue.Replace(".", ","), out decimal costValue))
                            filteredItemIds = query.Where(i => i.InitialCost == costValue).Select(i => i.Id).ToList();
                        break;

                    default:
                        MessageBox.Show("Метод поиска не реализован для данной колонки.");
                        return;
                }

                SelectedInventoryItemIds = filteredItemIds;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}");
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchValue.Text = string.Empty;
            if (cmbSearchColumn.Items.Count > 0)
            {
                cmbSearchColumn.SelectedIndex = 0;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ShowAllItems()
        {
            SelectedInventoryItemIds.Clear();
            SelectedInventoryItems.Clear();

            if (_filterRoomId.HasValue && _filterRoomId.Value > 0)
            {
                try
                {
                    using var dbContext = GetDbContext();
                    // Используем RoomId (PascalCase)
                    SelectedInventoryItemIds = dbContext.InventoryItems
                                              .Where(i => i.RoomId == _filterRoomId.Value)
                                              .Select(i => i.Id)
                                              .ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке всех элементов для комнаты: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Abort;
                    Close();
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Label2_Click(object sender, EventArgs e)
        {
        }

        private void Guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
