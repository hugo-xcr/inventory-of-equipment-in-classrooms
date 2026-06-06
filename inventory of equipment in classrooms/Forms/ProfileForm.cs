using System;
using System.Linq;
using System.Windows.Forms;
using inventory_of_equipment_in_classrooms.Data;
using inventory_of_equipment_in_classrooms.Forms;
using inventory_of_equipment_in_classrooms.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_of_equipment_in_classrooms
{
    public partial class ProfileForm : Form
    {
        private readonly int _currentUserId;
        private User _currentUser;

        public ProfileForm(int currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            SetActiveButton(btnProfile);
            LoadUserData();
        }

        private void LoadUserData()
        {
            using var db = new DatabaseContent();
            _currentUser = db.Users.Include(u => u.JobTitle).FirstOrDefault(u => u.Id == _currentUserId);
            if (_currentUser != null)
            {
                txtFirstName.Text = _currentUser.Firstname;
                txtSurname.Text = _currentUser.Surname;
                txtEmail.Text = _currentUser.Email;
                txtJobTitle.Text = _currentUser.JobTitle?.Name ?? "Не указана";
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            string newFirstName = txtFirstName.Text.Trim();
            string newSurname = txtSurname.Text.Trim();
            string newEmail = txtEmail.Text.Trim(); 

            if (string.IsNullOrEmpty(newFirstName) || string.IsNullOrEmpty(newSurname))
            {
                MessageBox.Show("Имя и фамилия не могут быть пустыми.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new DatabaseContent();
                var user = db.Users.FirstOrDefault(u => u.Id == _currentUserId);

                if (user != null)
                {
                    user.Firstname = newFirstName;
                    user.Surname = newSurname;
                    user.Email = newEmail;

                    db.Entry(user).State = EntityState.Modified;

                    db.SaveChanges();

                    MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден в базе данных.", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetActiveButton(Guna.UI2.WinForms.Guna2Button activeBtn)
        {
            var inactiveColor = System.Drawing.Color.FromArgb(0, 51, 153);
            btnProfile.FillColor = btnEditCard.FillColor = btnTransfer.FillColor = inactiveColor;
            btnProfile.ForeColor = btnEditCard.ForeColor = btnTransfer.ForeColor = System.Drawing.Color.White;
            activeBtn.FillColor = System.Drawing.Color.White;
            activeBtn.ForeColor = System.Drawing.Color.Black;
        }

        private TransferActForm _transferActForm;

        private void btnTransferActForm_Click(object sender, EventArgs e)
        {
            try
            {
                if (_transferActForm == null || _transferActForm.IsDisposed)
                {
                    _transferActForm = new TransferActForm(_currentUserId);
                }

                if (!_transferActForm.Visible)
                    _transferActForm.ShowDialog();
                else
                    _transferActForm.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть форму акта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void BtnEditCard_Click(object sender, EventArgs e) =>
            FormNavigator.ShowEditCard();

        private void BtnTransfer_Click(object sender, EventArgs e) =>
            FormNavigator.ShowTransfer();

        private void guna2Button1_Click(object sender, EventArgs e) =>  // Списание
            FormNavigator.ShowTransferAct();

        private void BtnImport_Click(object sender, EventArgs e) =>
            FormNavigator.ShowImport();
    }
}
