// EditCardForm.cs
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Guna.UI2.WinForms;
using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Forms;
using inventory_of_equipment_in_classrooms.Models;

namespace inventory_of_equipment_in_classrooms
{
    public partial class EditCardForm : Form
    {
        private readonly Color DefaultBlueColor = Color.FromArgb(0, 51, 153);
        private readonly Color HoverPanelColor = Color.FromArgb(10, 61, 173);
        private TransferActForm _transferActForm;
        private readonly int _currentUserId;
        private List<int> _searchFilterIds = new List<int>();
        private int? _selectedRoomId;
        private int _selectedEquipmentId = 0;

        public EditCardForm(int currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            this.FormClosing += EditCardForm_FormClosing;
            this.Load += EditCardForm_Load;

            dgvEquipment.AllowUserToAddRows = false;
            dgvEquipment.ReadOnly = true;
            dgvEquipment.CellClick += dgvEquipment_CellClick;
        }

        private void EditCardForm_Load(object sender, EventArgs e)
        {
            var toolTip = new ToolTip();
            toolTip.SetToolTip(txtOkeiCode,
                "Коды ОКЕИ:\n" +
                "796 — штука\n" +
                "006 — метр\n" +
                "112 — литр\n" +
                "166 — килограмм\n" +
                "839 — комплект\n" +
                "778 — упаковка");
            SetActiveButton(btnEditCard);
            try
            {
                UnsubscribeFilterEvents();
                LoadFilterData();
                LoadEquipmentData();
                SubscribeFilterEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке формы: {ex.InnerException?.Message ?? ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private static DatabaseContent GetDbContext() => new DatabaseContent();

        private void LoadFilterData()
        {
            using var dbContext = GetDbContext();

            var teachers = dbContext.Users
                .Select(u => new UserReference
                {
                    Id = u.Id,
                    FullName = u.Surname + " " + u.Firstname + (u.Patronymic != null ? " " + u.Patronymic : "")
                })
                .OrderBy(u => u.FullName)
                .ToList();
            teachers.Insert(0, new UserReference { Id = 0, FullName = "Все преподаватели" });
            teachers.Insert(1, new UserReference { Id = -1, FullName = "Не назначен" });
            cmbTeachers.DataSource = teachers;
            cmbTeachers.DisplayMember = "FullName";
            cmbTeachers.ValueMember = "Id";

            var classrooms = dbContext.Rooms
                .Select(r => new RoomReference { Id = r.Id, Name = r.RoomName })
                .OrderBy(r => r.Name)
                .ToList();
            classrooms.Insert(0, new RoomReference { Id = 0, Name = "Все аудитории" });
            classrooms.Insert(1, new RoomReference { Id = -1, Name = "Не назначена" });
            cmbClassrooms.DataSource = classrooms;
            cmbClassrooms.DisplayMember = "Name";
            cmbClassrooms.ValueMember = "Id";

            cmbTeachers.SelectedValue = 0;
            cmbClassrooms.SelectedValue = 0;
        }

        private void LoadEquipmentData()
        {
            using var dbContext = GetDbContext();
            var data = dbContext.InventoryItems
                .Include(i => i.Room)
                .Include(i => i.Custodian)
                .Select(i => new
                {
                    i.Id,
                    Аудитория = i.Room != null ? i.Room.RoomName : "Н/Д",
                    Ответственный = i.Custodian != null ? $"{i.Custodian.Surname} {i.Custodian.Firstname}" : "Н/Д",
                    Название = i.Name,
                    Инв_Номер = i.InventoryNumber,
                    Серийный_Номер = i.SerialNumber,
                    Дата_Учета = i.DateOnAccounting,
                    Стоимость = i.InitialCost,
                    Состояние = i.CurrentState,
                    Ед_Изм = i.UnitName,
                    Код_ОКЕИ = i.OkeiCode,
                    RoomId = i.RoomId,
                    CustodianId = i.CustodianId
                }).ToList();

            dgvEquipment.DataSource = data;

            string[] hiddenColumns = { "Id", "RoomId", "CustodianId" };
            foreach (var col in hiddenColumns)
            {
                if (dgvEquipment.Columns.Contains(col))
                    dgvEquipment.Columns[col].Visible = false;
            }
        }

        private void CmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEquipmentData();
        }

        private bool TryParseInput(out decimal initialCost, out decimal amortizationAmount, out decimal residualValue, out DateTime? dateOnAccount, out string equipmentName, out string currentState)
        {
            initialCost = amortizationAmount = residualValue = 0;
            dateOnAccount = null;
            equipmentName = txtEquipmentName.Text.Trim();
            currentState = txtCurrentState.Text.Trim();

            if (string.IsNullOrWhiteSpace(equipmentName))
            {
                MessageBox.Show("Заполните поле 'Основное средство'.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(currentState)) currentState = "Н/Д";

            if (!decimal.TryParse(txtInitialCost.Text, out initialCost))
            {
                MessageBox.Show("Введите корректные числовые значения для стоимостных полей.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (initialCost < 0 || amortizationAmount < 0 || residualValue < 0)
            {
                MessageBox.Show("Стоимостные поля не могут быть отрицательными.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtDateOnAccount.Text))
            {
                if (DateTime.TryParse(txtDateOnAccount.Text, out DateTime dt))
                    dateOnAccount = dt;
                else
                {
                    MessageBox.Show("Введите корректную дату (ДД.ММ.ГГГГ).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private void dgvEquipment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvEquipment.Rows[e.RowIndex];

            _selectedEquipmentId = Convert.ToInt32(row.Cells["Id"].Value);

            txtEquipmentName.Text = row.Cells["Название"].Value?.ToString();
            txtInventoryNumber.Text = row.Cells["Инв_Номер"].Value?.ToString();
            txtDateOnAccount.Text = row.Cells["Дата_Учета"].Value?.ToString();
            txtInitialCost.Text = row.Cells["Стоимость"].Value?.ToString();
            txtCurrentState.Text = row.Cells["Состояние"].Value?.ToString();
            txtUnitName.Text = row.Cells["Ед_Изм"].Value?.ToString();
            txtOkeiCode.Text = row.Cells["Код_ОКЕИ"].Value?.ToString();

            int roomId = row.Cells["RoomId"].Value as int? ?? -1;
            int custodianId = row.Cells["CustodianId"].Value as int? ?? -1;

            UpdateCombos(roomId, custodianId);
        }
        private void UpdateCombos(int roomId, int custodianId)
        {
            UnsubscribeFilterEvents();

            if (cmbClassrooms.Items.Cast<object>().Any())
            {
                cmbClassrooms.SelectedValue = roomId > 0 ? roomId : 0;
            }

            if (cmbTeachers.Items.Cast<object>().Any())
            {
                cmbTeachers.SelectedValue = custodianId > 0 ? custodianId : 0;
            }

            SubscribeFilterEvents();
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            try
            {
                using var dbContext = GetDbContext();

                if (!TryParseInput(out decimal initialCost, out _, out _, out DateTime? dateOnAccount, out string equipmentName, out string currentState))
                    return;

                string inventoryNumber = txtInventoryNumber.Text.Trim();
                if (!string.IsNullOrWhiteSpace(inventoryNumber))
                {
                    bool exists = dbContext.InventoryItems.Any(i => i.InventoryNumber == inventoryNumber);
                    if (exists)
                    {
                        MessageBox.Show("Оборудование с таким инвентарным номером уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                int selectedTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
                int selectedClassroomId = Convert.ToInt32(cmbClassrooms.SelectedValue);

                var newItem = new InventoryItem
                {
                    Name = equipmentName,
                    InventoryNumber = string.IsNullOrWhiteSpace(inventoryNumber) ? null : inventoryNumber,
                    UnitName = txtUnitName.Text.Trim(),
                    OkeiCode = txtOkeiCode.Text.Trim(),
                    DateOnAccounting = dateOnAccount.HasValue ? DateTime.SpecifyKind(dateOnAccount.Value, DateTimeKind.Utc) : DateTime.UtcNow,
                    InitialCost = initialCost,
                    CurrentState = currentState,
                    RoomId = selectedClassroomId > 0 ? selectedClassroomId : (int?)null,
                    CustodianId = selectedTeacherId > 0 ? selectedTeacherId : (int?)null
                };

                dbContext.InventoryItems.Add(newItem);
                dbContext.SaveChanges();

                MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEquipmentData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    MessageBox.Show("Оборудование с таким инвентарным номером уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Ошибка: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
        }
        private void ClearInputs()
        {
            _selectedEquipmentId = 0;
            txtEquipmentName.Clear();
            txtInventoryNumber.Clear();
            txtInitialCost.Clear();
            txtUnitName.Clear();
            txtOkeiCode.Clear();
            txtCurrentState.Text = "в наличии";
            cmbClassrooms.SelectedIndex = 0;
            cmbTeachers.SelectedIndex = 0;
        }
        private void BtnEditData_Click(object sender, EventArgs e)
        {
            if (_selectedEquipmentId <= 0)
            {
                MessageBox.Show("Выберите запись для редактирования.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryParseInput(out decimal initialCost, out _, out _, out DateTime? dateOnAccount, out string equipmentName, out string currentState))
                return;

            try
            {
                using var dbContext = GetDbContext();
                var item = dbContext.InventoryItems.Find(_selectedEquipmentId);

                if (item == null)
                {
                    MessageBox.Show("Запись не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string inputInvNumber = txtInventoryNumber.Text.Trim();

                if (!string.IsNullOrWhiteSpace(inputInvNumber))
                {
                    bool exists = dbContext.InventoryItems.Any(i => i.InventoryNumber == inputInvNumber && i.Id != _selectedEquipmentId);
                    if (exists)
                    {
                        MessageBox.Show("Этот инвентарный номер уже присвоен другому оборудованию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                item.InventoryNumber = string.IsNullOrWhiteSpace(inputInvNumber) ? null : inputInvNumber;
                item.Name = equipmentName;
                item.InitialCost = initialCost;
                item.CurrentState = currentState;
                item.UnitName = txtUnitName.Text.Trim();
                item.OkeiCode = txtOkeiCode.Text.Trim();

                if (dateOnAccount.HasValue)
                {
                    item.DateOnAccounting = DateTime.SpecifyKind(dateOnAccount.Value, DateTimeKind.Utc);
                }

                int selectedTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
                int selectedClassroomId = Convert.ToInt32(cmbClassrooms.SelectedValue);

                item.CustodianId = (selectedTeacherId <= 0) ? null : (int?)selectedTeacherId;
                item.RoomId = (selectedClassroomId <= 0) ? null : (int?)selectedClassroomId;

                dbContext.SaveChanges();

                MessageBox.Show("Запись успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _selectedEquipmentId = 0;
                ClearInputs();
                LoadEquipmentData();
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    MessageBox.Show("Этот инвентарный номер уже присвоен другому оборудованию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Ошибка: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
        }

        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            if (dgvEquipment.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите окончательно удалить выбранную запись?",
                                         "Подтверждение удаления",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedId = (int)dgvEquipment.SelectedRows[0].Cells["Id"].Value;

                    using var dbContext = GetDbContext();
                    var item = dbContext.InventoryItems.Find(selectedId);

                    if (item != null)
                    {
                        dbContext.InventoryItems.Remove(item);
                        dbContext.SaveChanges();

                        MessageBox.Show("Запись успешно удалена.");
                        LoadEquipmentData(); // Обновляем таблицу
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (SearchForm searchForm = new SearchForm(_currentUserId, _selectedRoomId))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    var foundIds = searchForm.SelectedInventoryItemIds;

                    if (foundIds.Count > 0)
                    {
                        DisplaySearchResults(foundIds);
                    }
                    else
                    {
                        MessageBox.Show("Записей не найдено.");
                        LoadEquipmentData();
                    }
                }
            }
        }
        private void AutoSelectTeacherByClassroom()
        {
            if (cmbClassrooms.SelectedValue != null)
            {
                int selectedRoomId = Convert.ToInt32(cmbClassrooms.SelectedValue);

                if (selectedRoomId <= 0) return;

                try
                {
                    using var dbContext = new DatabaseContent();

                    var room = dbContext.Rooms.FirstOrDefault(r => r.Id == selectedRoomId);

                    UnsubscribeFilterEvents();

                    if (room != null && room.TeacherId.HasValue && room.TeacherId.Value > 0)
                    {
                        int linkedTeacherId = room.TeacherId.Value;

                        cmbTeachers.SelectedValue = linkedTeacherId;
                    }
                    else
                    {
                        cmbTeachers.SelectedValue = -1;
                    }

                    SubscribeFilterEvents();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка автовыбора: {ex.Message}");
                    SubscribeFilterEvents();
                }
            }
        }
        private void DisplaySearchResults(List<int> ids)
        {
            using var dbContext = GetDbContext();

            var data = dbContext.InventoryItems
                .Include(i => i.Room)
                .Include(i => i.Custodian)
                .Where(i => ids.Contains(i.Id))
                .Select(i => new
                {
                    i.Id,
                    Аудитория = i.Room != null ? i.Room.RoomName : "Н/Д",
                    Ответственный = i.Custodian != null ? $"{i.Custodian.Surname} {i.Custodian.Firstname}" : "Н/Д",
                    Название = i.Name,
                    Инв_Номер = i.InventoryNumber,
                    Серийный_Номер = i.SerialNumber,
                    Дата_Учета = i.DateOnAccounting,
                    Стоимость = i.InitialCost,
                    Состояние = i.CurrentState,
                    Ед_Изм = i.UnitName,
                    Код_ОКЕИ = i.OkeiCode,
                    CustodianId = i.CustodianId,
                    RoomId = i.RoomId
                }).ToList();

            dgvEquipment.DataSource = data;
        }
        private void BtnResetSearch_Click(object sender, EventArgs e)
        {
            _searchFilterIds.Clear();
            LoadEquipmentData();
            MessageBox.Show("Фильтр поиска сброшен.", "Сброс", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EditCardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void SetActiveButton(Guna2Button activeBtn)
        {
            btnProfile.FillColor = btnEditCard.FillColor = btnTransfer.FillColor = DefaultBlueColor;
            btnProfile.ForeColor = btnEditCard.ForeColor = btnTransfer.ForeColor = Color.White;
            pictureBox1.BackColor = pictureBox3.BackColor = pictureBox4.BackColor = DefaultBlueColor;

            activeBtn.FillColor = Color.White;
            activeBtn.ForeColor = Color.Black;
            if (activeBtn == btnProfile) pictureBox1.BackColor = Color.White;
            else if (activeBtn == btnEditCard) pictureBox3.BackColor = Color.White;
            else if (activeBtn == btnTransfer) pictureBox4.BackColor = Color.White;
        }

   
        private void PBoxLogo_MouseEnter(object sender, EventArgs e) => pnlTopBar.BackColor = HoverPanelColor;
        private void PBoxLogo_MouseLeave(object sender, EventArgs e) => pnlTopBar.BackColor = DefaultBlueColor;
        private void cmbClassrooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            AutoSelectTeacherByClassroom();
            LoadEquipmentData();
        }
        private void SubscribeFilterEvents()
        {
            if (cmbTeachers != null) cmbTeachers.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;
            if (cmbClassrooms != null) cmbClassrooms.SelectedIndexChanged += cmbClassrooms_SelectedIndexChanged;
        }

        private void UnsubscribeFilterEvents()
        {
            if (cmbTeachers != null) cmbTeachers.SelectedIndexChanged -= CmbFilter_SelectedIndexChanged;
            if (cmbClassrooms != null) cmbClassrooms.SelectedIndexChanged -= cmbClassrooms_SelectedIndexChanged;
        }
        private void CmbTeachers_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var form = new TeacherEditForm(0);
            if (form.ShowDialog() == DialogResult.OK)
            {
                UnsubscribeFilterEvents();
                LoadFilterData();
                SubscribeFilterEvents();
                LoadEquipmentData();
                MessageBox.Show("Преподаватель добавлен.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            int selectedTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
            if (selectedTeacherId <= 0)
            {
                MessageBox.Show("Выберите конкретного преподавателя для удаления.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранного преподавателя?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using var dbContext = GetDbContext();

            bool hasItems = dbContext.InventoryItems.Any(i => i.CustodianId == selectedTeacherId);
            if (hasItems)
            {
                if (MessageBox.Show(
                    "За преподавателем числится оборудование. Снять его ответственность и удалить?",
                    "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                    != DialogResult.Yes)
                    return;
                var items = dbContext.InventoryItems
                    .Where(i => i.CustodianId == selectedTeacherId).ToList();
                items.ForEach(i => i.CustodianId = null);
            }

            var teacher = dbContext.Users.Find(selectedTeacherId);
            if (teacher != null)
            {
                dbContext.Users.Remove(teacher);
                dbContext.SaveChanges();
                UnsubscribeFilterEvents();
                LoadFilterData();
                SubscribeFilterEvents();
                LoadEquipmentData();
                MessageBox.Show("Преподаватель удалён.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEditTeacher_Click(object sender, EventArgs e)
        {
            int selectedTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
            if (selectedTeacherId <= 0)
            {
                MessageBox.Show("Выберите конкретного преподавателя для редактирования.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new TeacherEditForm(selectedTeacherId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                UnsubscribeFilterEvents();
                LoadFilterData();
                if (cmbTeachers.Items.Cast<UserReference>().Any(u => u.Id == selectedTeacherId))
                    cmbTeachers.SelectedValue = selectedTeacherId;
                SubscribeFilterEvents();
                LoadEquipmentData();
                MessageBox.Show("Данные преподавателя обновлены.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void LoadRoomsData()
        {
            try
            {
                UnsubscribeFilterEvents();

                using var dbContext = GetDbContext();
                var classrooms = dbContext.Rooms
                    .Select(r => new RoomReference { Id = r.Id, Name = r.RoomName })
                    .OrderBy(r => r.Name)
                    .ToList();

                classrooms.Insert(0, new RoomReference { Id = 0, Name = "Все аудитории" });
                classrooms.Insert(1, new RoomReference { Id = -1, Name = "Не назначена" });

                cmbClassrooms.DataSource = classrooms;
                cmbClassrooms.DisplayMember = "Name";
                cmbClassrooms.ValueMember = "Id";

                SubscribeFilterEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении списка аудиторий: {ex.Message}");
            }
        }
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            using var editForm = new RoomEditForm(); 
            if (editForm.ShowDialog() != DialogResult.OK) return;

            string newName = editForm.RoomName;
            if (string.IsNullOrWhiteSpace(newName)) return;

            try
            {
                using var db = new DatabaseContent();

                if (db.Rooms.Any(r => r.RoomName == newName))
                {
                    MessageBox.Show("Аудитория с таким номером уже существует.");
                    return;
                }

                var newRoom = new Room
                {
                    RoomName = newName,
                    TeacherId = editForm.SelectedTeacherId 
                };
                db.Rooms.Add(newRoom);
                db.SaveChanges();

                UnsubscribeFilterEvents();
                LoadFilterData();
                SubscribeFilterEvents();

                MessageBox.Show($"Аудитория {newName} успешно добавлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void btnEditRoom_Click(object sender, EventArgs e)
        {
            if (!(cmbClassrooms.SelectedItem is RoomReference selectedRef) || selectedRef.Id <= 0)
            {
                MessageBox.Show("Выберите аудиторию из списка для редактирования.");
                return;
            }

            try
            {
                using var db = new DatabaseContent();
                var roomToUpdate = db.Rooms.FirstOrDefault(r => r.Id == selectedRef.Id);
                if (roomToUpdate == null) return;

                using var editForm = new RoomEditForm(roomToUpdate.RoomName, roomToUpdate.TeacherId);
                if (editForm.ShowDialog() != DialogResult.OK) return;

                roomToUpdate.RoomName = editForm.RoomName;
                roomToUpdate.TeacherId = editForm.SelectedTeacherId; 
                db.SaveChanges();

                UnsubscribeFilterEvents();
                LoadFilterData(); 
                SubscribeFilterEvents();

                MessageBox.Show("Аудитория успешно обновлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void btnDeleteRoom_Click(object sender, EventArgs e)
        {
            if (cmbClassrooms.SelectedItem is RoomReference selectedRef)
            {
                if (selectedRef.Id <= 0) return;

                var confirm = MessageBox.Show($"Вы уверены, что хотите удалить аудиторию {selectedRef.Name}?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        using (var db = new DatabaseContent())
                        {
                            var room = db.Rooms.Find(selectedRef.Id);
                            if (room != null)
                            {
                                db.Rooms.Remove(room);
                                db.SaveChanges();
                                LoadFilterData();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Невозможно удалить аудиторию, так как в ней числится оборудование.");
                    }
                }
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите удалить ВСЕ записи об оборудовании?",
                "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var dbContext = GetDbContext())
                    {

                        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE savchenko_dm.inventory_item CASCADE");

                        MessageBox.Show("База данных успешно очищена.", "Успех");
                        LoadEquipmentData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        public class UserReference
        {
            public int Id { get; set; }
            public string FullName { get; set; }
        }

        public class RoomReference
        {
            public int Id { get; set; }
            public string Name { get; set; }
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

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}