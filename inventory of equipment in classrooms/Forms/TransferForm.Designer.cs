using Guna.UI2.WinForms;
using System.Windows.Forms;
using System.Drawing; // Добавлено, если отсутствует

namespace inventory_of_equipment_in_classrooms.Forms
{
    partial class TransferForm
    {
        private System.ComponentModel.IContainer components = null;
        private Guna2Panel pnlTopMenu;

        private Guna2Button btnTransfer; // <--- Оставляем это объявление

        private Guna2ComboBox cmbTransferType;
        private Guna2ComboBox cmbRoomFrom;
        private Guna2ComboBox cmbRoomTo;
        private Guna2ComboBox cmbSender;
        private Guna2ComboBox cmbReceiver;
        private Label lblTransferType;
        private Label lblRoomFrom;
        private Label lblRoomTo;
        private Label lblSender;
        private Label lblReceiver;
        private Guna2Button btnCreateWaybill;
        private Guna2DataGridView dgvEquipment;
        private Guna2Button btnSelectEquipment;
        private Label lblSelectedCount;

        // Добавленные вами PictureBox и Buttons
        private PictureBox pictureBox1;
        private Guna2Button btnProfile;
        private PictureBox pictureBox3;
        private Guna2Button btnEditCard;
        private PictureBox pictureBox2;
        private PictureBox pictureBox4;


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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferForm));
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            pnlTopMenu = new Guna2Panel();
            pictureBox8 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox7 = new PictureBox();
            pictureBox2 = new PictureBox();
            guna2Button1 = new Guna2Button();
            btnImport = new Guna2Button();
            btnTransfer = new Guna2Button();
            pictureBox3 = new PictureBox();
            btnEditCard = new Guna2Button();
            pictureBox1 = new PictureBox();
            btnProfile = new Guna2Button();
            cmbTransferType = new Guna2ComboBox();
            cmbRoomFrom = new Guna2ComboBox();
            cmbRoomTo = new Guna2ComboBox();
            cmbSender = new Guna2ComboBox();
            cmbReceiver = new Guna2ComboBox();
            lblTransferType = new Label();
            lblRoomFrom = new Label();
            lblRoomTo = new Label();
            lblSender = new Label();
            lblReceiver = new Label();
            btnCreateWaybill = new Guna2Button();
            dgvEquipment = new Guna2DataGridView();
            btnSelectEquipment = new Guna2Button();
            lblSelectedCount = new Label();
            txtQuantity = new Guna2TextBox();
            label1 = new Label();
            dgvHistoryTransfer = new Guna2DataGridView();
            label2 = new Label();
            pnlTopMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistoryTransfer).BeginInit();
            SuspendLayout();
            // 
            // pnlTopMenu
            // 
            pnlTopMenu.BackColor = Color.FromArgb(0, 51, 153);
            pnlTopMenu.Controls.Add(pictureBox8);
            pnlTopMenu.Controls.Add(pictureBox4);
            pnlTopMenu.Controls.Add(pictureBox7);
            pnlTopMenu.Controls.Add(pictureBox2);
            pnlTopMenu.Controls.Add(guna2Button1);
            pnlTopMenu.Controls.Add(btnImport);
            pnlTopMenu.Controls.Add(btnTransfer);
            pnlTopMenu.Controls.Add(pictureBox3);
            pnlTopMenu.Controls.Add(btnEditCard);
            pnlTopMenu.Controls.Add(pictureBox1);
            pnlTopMenu.Controls.Add(btnProfile);
            pnlTopMenu.CustomizableEdges = customizableEdges11;
            pnlTopMenu.Dock = DockStyle.Top;
            pnlTopMenu.Location = new Point(0, 0);
            pnlTopMenu.Name = "pnlTopMenu";
            pnlTopMenu.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlTopMenu.Size = new Size(1301, 125);
            pnlTopMenu.TabIndex = 0;
            // 
            // pictureBox8
            // 
            pictureBox8.AccessibleRole = AccessibleRole.None;
            pictureBox8.Anchor = AnchorStyles.None;
            pictureBox8.BackColor = Color.FromArgb(0, 51, 153);
            pictureBox8.Image = Properties.Resources.Рисунок4;
            pictureBox8.Location = new Point(762, 50);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(30, 30);
            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox8.TabIndex = 36;
            pictureBox8.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.Рисунок2Белый1;
            pictureBox4.Location = new Point(20, 25);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(66, 67);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 30;
            pictureBox4.TabStop = false;
            // 
            // pictureBox7
            // 
            pictureBox7.AccessibleRole = AccessibleRole.None;
            pictureBox7.Anchor = AnchorStyles.None;
            pictureBox7.BackColor = Color.FromArgb(0, 51, 153);
            pictureBox7.Image = Properties.Resources.free_icon_google_docs_white;
            pictureBox7.Location = new Point(957, 50);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(30, 30);
            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox7.TabIndex = 35;
            pictureBox7.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.AccessibleRole = AccessibleRole.None;
            pictureBox2.Anchor = AnchorStyles.None;
            pictureBox2.BackColor = Color.White;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(545, 49);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(30, 30);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 29;
            pictureBox2.TabStop = false;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 10;
            guna2Button1.CustomizableEdges = customizableEdges1;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(0, 51, 153);
            guna2Button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(988, 36);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button1.Size = new Size(212, 56);
            guna2Button1.TabIndex = 34;
            guna2Button1.Text = "Списание";
            guna2Button1.Click += guna2Button1_Click;
            // 
            // btnImport
            // 
            btnImport.BorderRadius = 10;
            btnImport.CustomizableEdges = customizableEdges3;
            btnImport.DisabledState.BorderColor = Color.DarkGray;
            btnImport.DisabledState.CustomBorderColor = Color.DarkGray;
            btnImport.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnImport.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnImport.FillColor = Color.FromArgb(0, 51, 153);
            btnImport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnImport.ForeColor = Color.White;
            btnImport.Location = new Point(760, 36);
            btnImport.Name = "btnImport";
            btnImport.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnImport.Size = new Size(189, 56);
            btnImport.TabIndex = 33;
            btnImport.Text = "Импорт файла";
            btnImport.Click += BtnImport_Click;
            // 
            // btnTransfer
            // 
            btnTransfer.BorderRadius = 10;
            btnTransfer.CustomizableEdges = customizableEdges5;
            btnTransfer.DisabledState.BorderColor = Color.DarkGray;
            btnTransfer.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTransfer.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTransfer.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTransfer.FillColor = Color.White;
            btnTransfer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnTransfer.ForeColor = Color.Black;
            btnTransfer.Location = new Point(530, 36);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnTransfer.Size = new Size(205, 56);
            btnTransfer.TabIndex = 28;
            btnTransfer.Text = "Передача оборудования";
            // 
            // pictureBox3
            // 
            pictureBox3.AccessibleRole = AccessibleRole.None;
            pictureBox3.Anchor = AnchorStyles.None;
            pictureBox3.BackColor = Color.FromArgb(0, 51, 153);
            pictureBox3.Image = Properties.Resources.Рисунок4;
            pictureBox3.Location = new Point(264, 50);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(30, 30);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 27;
            pictureBox3.TabStop = false;
            // 
            // btnEditCard
            // 
            btnEditCard.BorderRadius = 10;
            btnEditCard.CustomizableEdges = customizableEdges7;
            btnEditCard.DisabledState.BorderColor = Color.DarkGray;
            btnEditCard.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEditCard.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEditCard.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEditCard.FillColor = Color.FromArgb(0, 51, 153);
            btnEditCard.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnEditCard.ForeColor = Color.White;
            btnEditCard.Location = new Point(281, 36);
            btnEditCard.Name = "btnEditCard";
            btnEditCard.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnEditCard.Size = new Size(243, 56);
            btnEditCard.TabIndex = 26;
            btnEditCard.Text = "Редактирование инвентарной карточки";
            btnEditCard.Click += btnEditCard_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.AccessibleRole = AccessibleRole.None;
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.BackColor = Color.FromArgb(0, 51, 153);
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(108, 49);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(30, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 23;
            pictureBox1.TabStop = false;
            // 
            // btnProfile
            // 
            btnProfile.BorderRadius = 10;
            btnProfile.CustomizableEdges = customizableEdges9;
            btnProfile.DisabledState.BorderColor = Color.DarkGray;
            btnProfile.DisabledState.CustomBorderColor = Color.DarkGray;
            btnProfile.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnProfile.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnProfile.FillColor = Color.FromArgb(0, 51, 153);
            btnProfile.Font = new Font("Segoe UI", 12F);
            btnProfile.ForeColor = Color.White;
            btnProfile.HoverState.FillColor = Color.White;
            btnProfile.HoverState.ForeColor = Color.Black;
            btnProfile.Location = new Point(95, 36);
            btnProfile.Name = "btnProfile";
            btnProfile.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnProfile.Size = new Size(161, 56);
            btnProfile.TabIndex = 22;
            btnProfile.Text = "Профиль";
            btnProfile.Click += BtnProfile_Click;
            // 
            // cmbTransferType
            // 
            cmbTransferType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbTransferType.BackColor = Color.Transparent;
            cmbTransferType.BorderRadius = 10;
            cmbTransferType.CustomizableEdges = customizableEdges13;
            cmbTransferType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTransferType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTransferType.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbTransferType.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbTransferType.Font = new Font("Segoe UI", 10F);
            cmbTransferType.ForeColor = Color.FromArgb(68, 88, 112);
            cmbTransferType.ItemHeight = 30;
            cmbTransferType.Items.AddRange(new object[] { "Постоянная передача", "Временная передача", "Списание" });
            cmbTransferType.Location = new Point(857, 167);
            cmbTransferType.Name = "cmbTransferType";
            cmbTransferType.ShadowDecoration.CustomizableEdges = customizableEdges14;
            cmbTransferType.Size = new Size(190, 36);
            cmbTransferType.TabIndex = 3;
            // 
            // cmbRoomFrom
            // 
            cmbRoomFrom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbRoomFrom.BackColor = Color.Transparent;
            cmbRoomFrom.BorderRadius = 10;
            cmbRoomFrom.CustomizableEdges = customizableEdges15;
            cmbRoomFrom.DrawMode = DrawMode.OwnerDrawFixed;
            cmbRoomFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoomFrom.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbRoomFrom.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbRoomFrom.Font = new Font("Segoe UI", 10F);
            cmbRoomFrom.ForeColor = Color.FromArgb(68, 88, 112);
            cmbRoomFrom.ItemHeight = 30;
            cmbRoomFrom.Location = new Point(1087, 167);
            cmbRoomFrom.Name = "cmbRoomFrom";
            cmbRoomFrom.ShadowDecoration.CustomizableEdges = customizableEdges16;
            cmbRoomFrom.Size = new Size(190, 36);
            cmbRoomFrom.TabIndex = 4;
            cmbRoomFrom.SelectedIndexChanged += CmbRoomFrom_SelectedIndexChanged;
            // 
            // cmbRoomTo
            // 
            cmbRoomTo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbRoomTo.BackColor = Color.Transparent;
            cmbRoomTo.BorderRadius = 10;
            cmbRoomTo.CustomizableEdges = customizableEdges17;
            cmbRoomTo.DrawMode = DrawMode.OwnerDrawFixed;
            cmbRoomTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoomTo.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbRoomTo.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbRoomTo.Font = new Font("Segoe UI", 10F);
            cmbRoomTo.ForeColor = Color.FromArgb(68, 88, 112);
            cmbRoomTo.ItemHeight = 30;
            cmbRoomTo.Location = new Point(1087, 237);
            cmbRoomTo.Name = "cmbRoomTo";
            cmbRoomTo.ShadowDecoration.CustomizableEdges = customizableEdges18;
            cmbRoomTo.Size = new Size(190, 36);
            cmbRoomTo.TabIndex = 6;
            cmbRoomTo.SelectedIndexChanged += CmbRoomTo_SelectedIndexChanged;
            // 
            // cmbSender
            // 
            cmbSender.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbSender.BackColor = Color.Transparent;
            cmbSender.BorderRadius = 10;
            cmbSender.CustomizableEdges = customizableEdges19;
            cmbSender.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSender.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbSender.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbSender.Font = new Font("Segoe UI", 10F);
            cmbSender.ForeColor = Color.FromArgb(68, 88, 112);
            cmbSender.ItemHeight = 30;
            cmbSender.Location = new Point(857, 237);
            cmbSender.Name = "cmbSender";
            cmbSender.ShadowDecoration.CustomizableEdges = customizableEdges20;
            cmbSender.Size = new Size(190, 36);
            cmbSender.TabIndex = 5;
            cmbSender.SelectedIndexChanged += cmbSender_SelectedIndexChanged;
            // 
            // cmbReceiver
            // 
            cmbReceiver.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbReceiver.BackColor = Color.Transparent;
            cmbReceiver.BorderRadius = 10;
            cmbReceiver.CustomizableEdges = customizableEdges21;
            cmbReceiver.DrawMode = DrawMode.OwnerDrawFixed;
            cmbReceiver.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReceiver.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbReceiver.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbReceiver.Font = new Font("Segoe UI", 10F);
            cmbReceiver.ForeColor = Color.FromArgb(68, 88, 112);
            cmbReceiver.ItemHeight = 30;
            cmbReceiver.Location = new Point(857, 307);
            cmbReceiver.Name = "cmbReceiver";
            cmbReceiver.ShadowDecoration.CustomizableEdges = customizableEdges22;
            cmbReceiver.Size = new Size(190, 36);
            cmbReceiver.TabIndex = 7;
            // 
            // lblTransferType
            // 
            lblTransferType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTransferType.AutoSize = true;
            lblTransferType.Font = new Font("Segoe UI", 9F);
            lblTransferType.Location = new Point(854, 149);
            lblTransferType.Name = "lblTransferType";
            lblTransferType.Size = new Size(86, 15);
            lblTransferType.TabIndex = 8;
            lblTransferType.Text = "Тип передачи:";
            // 
            // lblRoomFrom
            // 
            lblRoomFrom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblRoomFrom.AutoSize = true;
            lblRoomFrom.Font = new Font("Segoe UI", 9F);
            lblRoomFrom.Location = new Point(1084, 149);
            lblRoomFrom.Name = "lblRoomFrom";
            lblRoomFrom.Size = new Size(122, 15);
            lblRoomFrom.TabIndex = 9;
            lblRoomFrom.Text = "Из какой аудитории?";
            // 
            // lblRoomTo
            // 
            lblRoomTo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblRoomTo.AutoSize = true;
            lblRoomTo.Font = new Font("Segoe UI", 9F);
            lblRoomTo.Location = new Point(1084, 219);
            lblRoomTo.Name = "lblRoomTo";
            lblRoomTo.Size = new Size(120, 15);
            lblRoomTo.TabIndex = 10;
            lblRoomTo.Text = "В какую аудиторию?";
            // 
            // lblSender
            // 
            lblSender.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSender.AutoSize = true;
            lblSender.Font = new Font("Segoe UI", 9F);
            lblSender.Location = new Point(854, 219);
            lblSender.Name = "lblSender";
            lblSender.Size = new Size(81, 15);
            lblSender.TabIndex = 11;
            lblSender.Text = "Отправитель:";
            // 
            // lblReceiver
            // 
            lblReceiver.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblReceiver.AutoSize = true;
            lblReceiver.Font = new Font("Segoe UI", 9F);
            lblReceiver.Location = new Point(854, 289);
            lblReceiver.Name = "lblReceiver";
            lblReceiver.Size = new Size(76, 15);
            lblReceiver.TabIndex = 12;
            lblReceiver.Text = "Получатель:";
            // 
            // btnCreateWaybill
            // 
            btnCreateWaybill.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCreateWaybill.BorderRadius = 5;
            btnCreateWaybill.Cursor = Cursors.Hand;
            btnCreateWaybill.CustomizableEdges = customizableEdges23;
            btnCreateWaybill.FillColor = Color.FromArgb(0, 51, 153);
            btnCreateWaybill.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCreateWaybill.ForeColor = Color.White;
            btnCreateWaybill.Location = new Point(1087, 734);
            btnCreateWaybill.Name = "btnCreateWaybill";
            btnCreateWaybill.ShadowDecoration.CustomizableEdges = customizableEdges24;
            btnCreateWaybill.Size = new Size(190, 45);
            btnCreateWaybill.TabIndex = 13;
            btnCreateWaybill.Text = "Создать Накладную";
            btnCreateWaybill.Click += BtnCreateWaybill_Click;
            // 
            // dgvEquipment
            // 
            dgvEquipment.AllowUserToAddRows = false;
            dgvEquipment.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvEquipment.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvEquipment.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEquipment.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(0, 51, 153);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.5F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvEquipment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvEquipment.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.5F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvEquipment.DefaultCellStyle = dataGridViewCellStyle3;
            dgvEquipment.GridColor = Color.FromArgb(231, 229, 255);
            dgvEquipment.Location = new Point(12, 131);
            dgvEquipment.Name = "dgvEquipment";
            dgvEquipment.ReadOnly = true;
            dgvEquipment.RowHeadersVisible = false;
            dgvEquipment.RowHeadersWidth = 51;
            dgvEquipment.RowTemplate.Height = 28;
            dgvEquipment.Size = new Size(823, 304);
            dgvEquipment.TabIndex = 14;
            dgvEquipment.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvEquipment.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvEquipment.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvEquipment.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvEquipment.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvEquipment.ThemeStyle.BackColor = Color.White;
            dgvEquipment.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvEquipment.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(0, 51, 153);
            dgvEquipment.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvEquipment.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10.5F);
            dgvEquipment.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvEquipment.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvEquipment.ThemeStyle.HeaderStyle.Height = 40;
            dgvEquipment.ThemeStyle.ReadOnly = true;
            dgvEquipment.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvEquipment.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEquipment.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.5F);
            dgvEquipment.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvEquipment.ThemeStyle.RowsStyle.Height = 28;
            dgvEquipment.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvEquipment.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // btnSelectEquipment
            // 
            btnSelectEquipment.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSelectEquipment.BorderRadius = 5;
            btnSelectEquipment.Cursor = Cursors.Hand;
            btnSelectEquipment.CustomizableEdges = customizableEdges25;
            btnSelectEquipment.FillColor = Color.FromArgb(0, 51, 153);
            btnSelectEquipment.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSelectEquipment.ForeColor = Color.White;
            btnSelectEquipment.Location = new Point(877, 734);
            btnSelectEquipment.Name = "btnSelectEquipment";
            btnSelectEquipment.ShadowDecoration.CustomizableEdges = customizableEdges26;
            btnSelectEquipment.Size = new Size(190, 45);
            btnSelectEquipment.TabIndex = 15;
            btnSelectEquipment.Text = "Поиск оборудования";
            // 
            // lblSelectedCount
            // 
            lblSelectedCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSelectedCount.AutoSize = true;
            lblSelectedCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSelectedCount.Location = new Point(877, 706);
            lblSelectedCount.Name = "lblSelectedCount";
            lblSelectedCount.Size = new Size(196, 19);
            lblSelectedCount.TabIndex = 16;
            lblSelectedCount.Text = "Выбрано оборудования: 0";
            // 
            // txtQuantity
            // 
            txtQuantity.BorderRadius = 10;
            txtQuantity.CustomizableEdges = customizableEdges27;
            txtQuantity.DefaultText = "0";
            txtQuantity.Font = new Font("Segoe UI", 9F);
            txtQuantity.Location = new Point(1083, 307);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.PlaceholderText = "";
            txtQuantity.SelectedText = "";
            txtQuantity.ShadowDecoration.CustomizableEdges = customizableEdges28;
            txtQuantity.Size = new Size(190, 36);
            txtQuantity.TabIndex = 17;
            txtQuantity.Visible = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.Location = new Point(1087, 289);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 18;
            label1.Text = "Количество:";
            // 
            // dgvHistoryTransfer
            // 
            dgvHistoryTransfer.AllowUserToAddRows = false;
            dgvHistoryTransfer.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = Color.White;
            dgvHistoryTransfer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvHistoryTransfer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistoryTransfer.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(0, 51, 153);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 10.5F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvHistoryTransfer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvHistoryTransfer.ColumnHeadersHeight = 40;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10.5F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvHistoryTransfer.DefaultCellStyle = dataGridViewCellStyle6;
            dgvHistoryTransfer.GridColor = Color.FromArgb(231, 229, 255);
            dgvHistoryTransfer.Location = new Point(12, 459);
            dgvHistoryTransfer.Name = "dgvHistoryTransfer";
            dgvHistoryTransfer.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvHistoryTransfer.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvHistoryTransfer.RowHeadersVisible = false;
            dgvHistoryTransfer.RowHeadersWidth = 51;
            dgvHistoryTransfer.RowTemplate.Height = 28;
            dgvHistoryTransfer.Size = new Size(823, 320);
            dgvHistoryTransfer.TabIndex = 19;
            dgvHistoryTransfer.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvHistoryTransfer.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvHistoryTransfer.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvHistoryTransfer.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvHistoryTransfer.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvHistoryTransfer.ThemeStyle.BackColor = Color.White;
            dgvHistoryTransfer.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvHistoryTransfer.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(0, 51, 153);
            dgvHistoryTransfer.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistoryTransfer.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10.5F);
            dgvHistoryTransfer.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvHistoryTransfer.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHistoryTransfer.ThemeStyle.HeaderStyle.Height = 40;
            dgvHistoryTransfer.ThemeStyle.ReadOnly = true;
            dgvHistoryTransfer.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvHistoryTransfer.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistoryTransfer.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.5F);
            dgvHistoryTransfer.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvHistoryTransfer.ThemeStyle.RowsStyle.Height = 28;
            dgvHistoryTransfer.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvHistoryTransfer.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dgvHistoryTransfer.CellContentClick += dgvHistoryTransfer_CellContentClick;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.Location = new Point(10, 441);
            label2.Name = "label2";
            label2.Size = new Size(187, 15);
            label2.TabIndex = 20;
            label2.Text = "История передач оборудования:";
            // 
            // TransferForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1301, 795);
            Controls.Add(label2);
            Controls.Add(dgvHistoryTransfer);
            Controls.Add(label1);
            Controls.Add(lblSelectedCount);
            Controls.Add(btnSelectEquipment);
            Controls.Add(dgvEquipment);
            Controls.Add(btnCreateWaybill);
            Controls.Add(lblReceiver);
            Controls.Add(lblSender);
            Controls.Add(lblRoomTo);
            Controls.Add(lblRoomFrom);
            Controls.Add(lblTransferType);
            Controls.Add(cmbReceiver);
            Controls.Add(cmbRoomTo);
            Controls.Add(cmbSender);
            Controls.Add(cmbRoomFrom);
            Controls.Add(cmbTransferType);
            Controls.Add(pnlTopMenu);
            Controls.Add(txtQuantity);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1000, 620);
            Name = "TransferForm";
            Text = "Передача оборудования";
            pnlTopMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistoryTransfer).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }
        private PictureBox pictureBox8;
        private PictureBox pictureBox7;
        private Guna2Button guna2Button1;
        private Guna2Button btnImport;
        private Guna2TextBox txtQuantity;
        private Label label1;
        private Guna2DataGridView dgvHistoryTransfer;
        private Label label2;

        #endregion

        // УДАЛЕНЫ ДУБЛИКАТЫ ИЗ ЭТОЙ СЕКЦИИ
    }
}