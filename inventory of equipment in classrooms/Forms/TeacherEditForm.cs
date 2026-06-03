using System;
using System.Windows.Forms;
using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Models;

namespace inventory_of_equipment_in_classrooms.Forms
{
    public partial class TeacherEditForm : Form
    {
        private readonly int _teacherId;

        public TeacherEditForm(int teacherId)
        {
            InitializeComponent();
            _teacherId = teacherId;
            this.Load += TeacherEditForm_Load;
        }

        private void TeacherEditForm_Load(object sender, EventArgs e)
        {
            this.Text = _teacherId > 0 ? "Редактирование" : "Добавление";

            if (_teacherId > 0)
            {
                try
                {
                    using var db = new DatabaseContent();
                    var teacher = db.Users.Find(_teacherId);
                    if (teacher != null)
                    {
                        txtSurname.Text = teacher.Surname;
                        txtFirstname.Text = teacher.Firstname;
                        txtPatronymic.Text = teacher.Patronymic;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки: {ex.Message}");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSurname.Text) || string.IsNullOrWhiteSpace(txtFirstname.Text))
            {
                MessageBox.Show("Заполните обязательные поля (Фамилия и Имя)");
                return;
            }

            try
            {
                using var db = new DatabaseContent();
                if (_teacherId == 0)
                {
                    db.Users.Add(new User
                    {
                        Surname = txtSurname.Text,
                        Firstname = txtFirstname.Text,
                        Patronymic = txtPatronymic.Text,
                        JobTitleId = 1,
                        Email = null,
                        Password = null
                    });
                }
                else
                {
                    // Редактирование
                    var teacher = db.Users.Find(_teacherId);
                    if (teacher != null)
                    {
                        teacher.Surname = txtSurname.Text;
                        teacher.Firstname = txtFirstname.Text;
                        teacher.Patronymic = txtPatronymic.Text;
                    }
                }
                db.SaveChanges();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Ошибка сохранения: {innerMsg}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}