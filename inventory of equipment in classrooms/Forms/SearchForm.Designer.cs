using Guna.UI2.WinForms;
using System.Windows.Forms;
using System.Drawing;

namespace inventory_of_equipment_in_classrooms.Forms
{
    partial class SearchForm : Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            cmbSearchColumn = new Guna2ComboBox();
            txtSearchValue = new Guna2TextBox();
            btnExecuteSearch = new Guna2Button();
            lblTitle = new Label();
            guna2Panel1 = new Guna2Panel();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            guna2Button1 = new Guna2Button();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // cmbSearchColumn
            // 
            cmbSearchColumn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbSearchColumn.AutoRoundedCorners = true;
            cmbSearchColumn.BackColor = Color.Transparent;
            cmbSearchColumn.BorderColor = Color.Silver;
            cmbSearchColumn.BorderRadius = 17;
            cmbSearchColumn.CustomizableEdges = customizableEdges1;
            cmbSearchColumn.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSearchColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSearchColumn.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbSearchColumn.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbSearchColumn.Font = new Font("Segoe UI", 10F);
            cmbSearchColumn.ForeColor = Color.FromArgb(68, 88, 112);
            cmbSearchColumn.ItemHeight = 30;
            cmbSearchColumn.Location = new Point(51, 137);
            cmbSearchColumn.Name = "cmbSearchColumn";
            cmbSearchColumn.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbSearchColumn.Size = new Size(700, 36);
            cmbSearchColumn.TabIndex = 0;
            // 
            // txtSearchValue
            // 
            txtSearchValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearchValue.AutoRoundedCorners = true;
            txtSearchValue.BorderRadius = 17;
            txtSearchValue.Cursor = Cursors.IBeam;
            txtSearchValue.CustomizableEdges = customizableEdges3;
            txtSearchValue.DefaultText = "";
            txtSearchValue.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearchValue.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearchValue.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearchValue.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearchValue.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearchValue.Font = new Font("Segoe UI", 9F);
            txtSearchValue.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearchValue.Location = new Point(51, 218);
            txtSearchValue.Name = "txtSearchValue";
            txtSearchValue.PlaceholderText = "Напишите что вы ищите";
            txtSearchValue.SelectedText = "";
            txtSearchValue.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtSearchValue.Size = new Size(700, 36);
            txtSearchValue.TabIndex = 1;
            // 
            // btnExecuteSearch
            // 
            btnExecuteSearch.Anchor = AnchorStyles.Top;
            btnExecuteSearch.AutoRoundedCorners = true;
            btnExecuteSearch.BorderRadius = 19;
            btnExecuteSearch.CustomizableEdges = customizableEdges5;
            btnExecuteSearch.DisabledState.BorderColor = Color.DarkGray;
            btnExecuteSearch.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExecuteSearch.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExecuteSearch.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExecuteSearch.FillColor = Color.FromArgb(0, 51, 153);
            btnExecuteSearch.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnExecuteSearch.ForeColor = Color.White;
            btnExecuteSearch.Location = new Point(51, 295);
            btnExecuteSearch.Name = "btnExecuteSearch";
            btnExecuteSearch.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnExecuteSearch.Size = new Size(200, 40);
            btnExecuteSearch.TabIndex = 2;
            btnExecuteSearch.Text = "Поиск данных";
            btnExecuteSearch.Click += BtnExecuteSearch_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(327, 28);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(148, 25);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Поиск данных";
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.FromArgb(0, 51, 153);
            guna2Panel1.Controls.Add(pictureBox2);
            guna2Panel1.Controls.Add(lblTitle);
            guna2Panel1.CustomizableEdges = customizableEdges7;
            guna2Panel1.Dock = DockStyle.Top;
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2Panel1.Size = new Size(800, 76);
            guna2Panel1.TabIndex = 6;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Рисунок2Белый1;
            pictureBox2.Location = new Point(20, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(66, 67);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 25;
            pictureBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(51, 114);
            label1.Name = "label1";
            label1.Size = new Size(263, 15);
            label1.TabIndex = 7;
            label1.Text = "Выберите Столбец по которому будете искать";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(51, 195);
            label2.Name = "label2";
            label2.Size = new Size(144, 15);
            label2.TabIndex = 8;
            label2.Text = "Напишите что вы ищите";
            // 
            // guna2Button1
            // 
            guna2Button1.Anchor = AnchorStyles.Top;
            guna2Button1.AutoRoundedCorners = true;
            guna2Button1.BorderRadius = 19;
            guna2Button1.CustomizableEdges = customizableEdges9;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(0, 51, 153);
            guna2Button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(551, 295);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2Button1.Size = new Size(200, 40);
            guna2Button1.TabIndex = 9;
            guna2Button1.Text = "Отмена поиска";
            guna2Button1.Click += BtnClearSearch_Click;
            // 
            // SearchForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 380);
            Controls.Add(guna2Button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(guna2Panel1);
            Controls.Add(btnExecuteSearch);
            Controls.Add(txtSearchValue);
            Controls.Add(cmbSearchColumn);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(800, 380);
            Name = "SearchForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Поиск оборудования";
            Load += SearchForm_Load;
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ComboBox cmbSearchColumn;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchValue;
        private Guna.UI2.WinForms.Guna2Button btnExecuteSearch;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Guna2Button guna2Button1;
        // Элементы dgvSearchResults и lblStatus удалены
    }
}