using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Models;

namespace inventory_of_equipment_in_classrooms.Forms
{
    public partial class RoomEditForm : Form
    {
        // Вспомогательный класс, чтобы комбобокс железно прочитал свойства
        public class TeacherComboItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public string RoomName => txtRoom.Text.Trim();

        public int? SelectedTeacherId
        {
            get
            {
                if (cmbTeacher.SelectedValue != null)
                {
                    int id = Convert.ToInt32(cmbTeacher.SelectedValue);
                    return id > 0 ? id : null;
                }
                return null;
            }
        }

        public RoomEditForm()
        {
            InitializeComponent();
            SetupButtons();

            // Привязываем загрузку данных к событию Load формы, чтобы она срабатывала ВСЕГДА
            this.Load += (s, e) =>
            {
                LoadTeachersToComboBox();
            };
        }
        public RoomEditForm(string initialName, int? initialTeacherId) : this()
        {
            // Заполняем текстовое поле (оно заполнится сразу)
            txtRoom.Text = initialName;

            // Расширяем событие Load, чтобы после загрузки списка выбрать нужного человека
            this.Load += (s, e) =>
            {
                if (initialTeacherId.HasValue)
                {
                    cmbTeacher.SelectedValue = initialTeacherId.Value;
                }
                else
                {
                    cmbTeacher.SelectedValue = -1; // Пункт "Не назначен"
                }
            };
        }

        private void LoadTeachersToComboBox()
        {
            try
            {
                // КРИТИЧЕСКИЙ МОМЕНТ: Используем GetContext(), а не new DatabaseContent()!
                var dbContext = DatabaseContent.GetContext();

                // Загружаем список из базы данных
                var teachersFromDb = dbContext.Users
                    .Select(u => new TeacherComboItem
                    {
                        Id = u.Id,
                        Name = (u.Surname + " " + u.Firstname + " " + u.Patronymic).Trim()
                    })
                    .ToList();

                // Создаем итоговый список и добавляем пункт "Не назначен"
                var comboSource = new List<TeacherComboItem>
                {
                    new TeacherComboItem { Id = -1, Name = "Не назначен" }
                };
                comboSource.AddRange(teachersFromDb);

                // Привязываем данные к комбобоксу
                cmbTeacher.DataSource = comboSource;
                cmbTeacher.DisplayMember = "Name";
                cmbTeacher.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка преподавателей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupButtons()
        {
            btnSaveRoom.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(RoomName))
                {
                    MessageBox.Show("Введите номер или название аудитории.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            btnCancelRoom.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
        }

        private void cmbTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}