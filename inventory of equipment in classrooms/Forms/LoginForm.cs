using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using inventory_of_equipment_in_classrooms.Models;
using inventory_of_equipment_in_classrooms.Data;

namespace inventory_of_equipment_in_classrooms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

            private void BtnLogin_Click(object sender, EventArgs e)
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Пожалуйста, введите Email и Пароль.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using var dbContext = new DatabaseContent();

                    User user = dbContext.Users
                        .FirstOrDefault(u => u.Email == email);

                    if (user != null)
                    {
                        if (user.Password == password)
                        {
                            MessageBox.Show($"Добро пожаловать, {user.Firstname}!", "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var profileForm = new ProfileForm(user.Id);

                            this.Hide();
                            profileForm.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неверный Email или Пароль.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}