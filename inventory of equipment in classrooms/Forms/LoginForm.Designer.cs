
namespace inventory_of_equipment_in_classrooms // Убедитесь, что это пространство имен вашего проекта
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            labelTitle = new Label();
            labelSubtitle = new Label();
            labelEmail = new Label();
            labelPassword = new Label();
            pictureBoxIcon = new PictureBox();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            SuspendLayout();
            // 
            // labelTitle
            // 
            labelTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelTitle.ForeColor = Color.FromArgb(0, 49, 163);
            labelTitle.Location = new Point(180, 9);
            labelTitle.Margin = new Padding(4, 0, 4, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(327, 35);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Добро пожаловать!";
            labelTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelSubtitle
            // 
            labelSubtitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelSubtitle.ForeColor = Color.FromArgb(0, 49, 163);
            labelSubtitle.Location = new Point(180, 50);
            labelSubtitle.Margin = new Padding(4, 0, 4, 0);
            labelSubtitle.Name = "labelSubtitle";
            labelSubtitle.Size = new Size(327, 33);
            labelSubtitle.TabIndex = 1;
            labelSubtitle.Text = "Войдите в свой аккаунт";
            labelSubtitle.TextAlign = ContentAlignment.TopCenter;
            labelSubtitle.Click += labelSubtitle_Click;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            labelEmail.Location = new Point(393, 152);
            labelEmail.Margin = new Padding(4, 0, 4, 0);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(183, 25);
            labelEmail.TabIndex = 2;
            labelEmail.Text = "Введите свой email";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            labelPassword.Location = new Point(393, 221);
            labelPassword.Margin = new Padding(4, 0, 4, 0);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(200, 25);
            labelPassword.TabIndex = 4;
            labelPassword.Text = "Введите свой пароль";
            // 
            // pictureBoxIcon
            // 
            pictureBoxIcon.Image = (Image)resources.GetObject("pictureBoxIcon.Image");
            pictureBoxIcon.Location = new Point(52, 110);
            pictureBoxIcon.Margin = new Padding(4, 3, 4, 3);
            pictureBoxIcon.Name = "pictureBoxIcon";
            pictureBoxIcon.Size = new Size(233, 231);
            pictureBoxIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxIcon.TabIndex = 7;
            pictureBoxIcon.TabStop = false;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 10;
            guna2Button1.CustomizableEdges = customizableEdges1;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(0, 49, 163);
            guna2Button1.Font = new Font("Segoe UI", 9F);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(381, 332);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button1.Size = new Size(218, 45);
            guna2Button1.TabIndex = 8;
            guna2Button1.Text = "Авторизация";
            guna2Button1.Click += BtnLogin_Click;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.White;
            txtEmail.BorderColor = Color.Silver;
            txtEmail.BorderRadius = 10;
            txtEmail.CustomizableEdges = customizableEdges3;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FillColor = Color.Silver;
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.ForeColor = Color.Black;
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(381, 180);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtEmail.Size = new Size(218, 36);
            txtEmail.TabIndex = 9;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.White;
            txtPassword.BorderColor = Color.Silver;
            txtPassword.BorderRadius = 10;
            txtPassword.CustomizableEdges = customizableEdges5;
            txtPassword.DefaultText = "";
            txtPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.FillColor = Color.Silver;
            txtPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.ForeColor = Color.Black;
            txtPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.Location = new Point(381, 249);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "";
            txtPassword.SelectedText = "";
            txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtPassword.Size = new Size(218, 36);
            txtPassword.TabIndex = 10;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(700, 438);
            Controls.Add(txtPassword);
            Controls.Add(txtEmail);
            Controls.Add(guna2Button1);
            Controls.Add(pictureBoxIcon);
            Controls.Add(labelPassword);
            Controls.Add(labelEmail);
            Controls.Add(labelSubtitle);
            Controls.Add(labelTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private void labelSubtitle_Click(object sender, EventArgs e)
        {
        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSubtitle;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
    }
}