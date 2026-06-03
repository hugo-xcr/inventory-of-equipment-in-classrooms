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

            this.Load += (s, e) =>
            {
                LoadTeachersToComboBox();
            };
        }
        public RoomEditForm(string initialName, int? initialTeacherId) : this()
        {
            txtRoom.Text = initialName;

            this.Load += (s, e) =>
            {
                if (initialTeacherId.HasValue)
                {
                    cmbTeacher.SelectedValue = initialTeacherId.Value;
                }
                else
                {
                    cmbTeacher.SelectedValue = -1; 
                }
            };
        }

        private void LoadTeachersToComboBox()
        {
            try
            {
                var dbContext = DatabaseContent.GetContext();

                var teachersFromDb = dbContext.Users
                    .Select(u => new TeacherComboItem
                    {
                        Id = u.Id,
                        Name = (u.Surname + " " + u.Firstname + " " + u.Patronymic).Trim()
                    })
                    .ToList();

                var comboSource = new List<TeacherComboItem>
                {
                    new TeacherComboItem { Id = -1, Name = "Не назначен" }
                };
                comboSource.AddRange(teachersFromDb);

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