namespace DVLDWinForms___Presentation_Layer.Users
{
    partial class frmListUsers
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListUsers));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilterBy = new System.Windows.Forms.TextBox();
            this.cmbFilterBy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblNumOfRecords = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvManageUsers = new System.Windows.Forms.DataGridView();
            this.cmUsers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmShowDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.cmAddNewUser = new System.Windows.Forms.ToolStripMenuItem();
            this.cmEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSendEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.cmPhoneCall = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.cmbIsActive = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageUsers)).BeginInit();
            this.cmUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(380, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(193, 140);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(374, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Manage Users";
            // 
            // txtFilterBy
            // 
            this.txtFilterBy.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilterBy.Location = new System.Drawing.Point(325, 248);
            this.txtFilterBy.Name = "txtFilterBy";
            this.txtFilterBy.Size = new System.Drawing.Size(216, 26);
            this.txtFilterBy.TabIndex = 31;
            this.txtFilterBy.Visible = false;
            this.txtFilterBy.TextChanged += new System.EventHandler(this.txtFilterBy_TextChanged);
            this.txtFilterBy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilterBy_KeyPress);
            // 
            // cmbFilterBy
            // 
            this.cmbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterBy.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFilterBy.FormattingEnabled = true;
            this.cmbFilterBy.Items.AddRange(new object[] {
            "None",
            "User ID",
            "UserName",
            "Person ID",
            "Full Name",
            "Is Active"});
            this.cmbFilterBy.Location = new System.Drawing.Point(117, 248);
            this.cmbFilterBy.Name = "cmbFilterBy";
            this.cmbFilterBy.Size = new System.Drawing.Size(184, 26);
            this.cmbFilterBy.TabIndex = 30;
            this.cmbFilterBy.SelectedIndexChanged += new System.EventHandler(this.cmbFilterBy_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 18);
            this.label3.TabIndex = 29;
            this.label3.Text = "Filter By:";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(807, 533);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 36);
            this.btnClose.TabIndex = 37;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblNumOfRecords
            // 
            this.lblNumOfRecords.AutoSize = true;
            this.lblNumOfRecords.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumOfRecords.Location = new System.Drawing.Point(204, 541);
            this.lblNumOfRecords.Name = "lblNumOfRecords";
            this.lblNumOfRecords.Size = new System.Drawing.Size(26, 18);
            this.lblNumOfRecords.TabIndex = 36;
            this.lblNumOfRecords.Text = "??";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 541);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 18);
            this.label2.TabIndex = 35;
            this.label2.Text = "Number Of Records: ";
            // 
            // dgvManageUsers
            // 
            this.dgvManageUsers.AllowUserToAddRows = false;
            this.dgvManageUsers.AllowUserToDeleteRows = false;
            this.dgvManageUsers.AllowUserToResizeRows = false;
            this.dgvManageUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvManageUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManageUsers.ContextMenuStrip = this.cmUsers;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvManageUsers.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvManageUsers.Location = new System.Drawing.Point(37, 290);
            this.dgvManageUsers.Name = "dgvManageUsers";
            this.dgvManageUsers.Size = new System.Drawing.Size(875, 227);
            this.dgvManageUsers.TabIndex = 34;
            this.dgvManageUsers.DoubleClick += new System.EventHandler(this.dgvManageUsers_DoubleClick);
            // 
            // cmUsers
            // 
            this.cmUsers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmShowDetails,
            this.cmAddNewUser,
            this.cmEdit,
            this.cmDelete,
            this.cmChangePassword,
            this.cmSendEmail,
            this.cmPhoneCall});
            this.cmUsers.Name = "cmUsers";
            this.cmUsers.Size = new System.Drawing.Size(185, 270);
            // 
            // cmShowDetails
            // 
            this.cmShowDetails.Image = ((System.Drawing.Image)(resources.GetObject("cmShowDetails.Image")));
            this.cmShowDetails.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cmShowDetails.Name = "cmShowDetails";
            this.cmShowDetails.Size = new System.Drawing.Size(184, 38);
            this.cmShowDetails.Text = "&Show Details";
            this.cmShowDetails.Click += new System.EventHandler(this.cmShowDetails_Click);
            // 
            // cmAddNewUser
            // 
            this.cmAddNewUser.Image = ((System.Drawing.Image)(resources.GetObject("cmAddNewUser.Image")));
            this.cmAddNewUser.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cmAddNewUser.Name = "cmAddNewUser";
            this.cmAddNewUser.Size = new System.Drawing.Size(184, 38);
            this.cmAddNewUser.Text = "&Add New User";
            this.cmAddNewUser.Click += new System.EventHandler(this.cmAddNewUser_Click);
            // 
            // cmEdit
            // 
            this.cmEdit.Image = ((System.Drawing.Image)(resources.GetObject("cmEdit.Image")));
            this.cmEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cmEdit.Name = "cmEdit";
            this.cmEdit.Size = new System.Drawing.Size(184, 38);
            this.cmEdit.Text = "&Edit";
            this.cmEdit.Click += new System.EventHandler(this.cmEdit_Click);
            // 
            // cmDelete
            // 
            this.cmDelete.Image = ((System.Drawing.Image)(resources.GetObject("cmDelete.Image")));
            this.cmDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cmDelete.Name = "cmDelete";
            this.cmDelete.Size = new System.Drawing.Size(184, 38);
            this.cmDelete.Text = "&Delete";
            this.cmDelete.Click += new System.EventHandler(this.cmDelete_Click);
            // 
            // cmChangePassword
            // 
            this.cmChangePassword.Image = ((System.Drawing.Image)(resources.GetObject("cmChangePassword.Image")));
            this.cmChangePassword.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cmChangePassword.Name = "cmChangePassword";
            this.cmChangePassword.Size = new System.Drawing.Size(184, 38);
            this.cmChangePassword.Text = "&Change Password";
            this.cmChangePassword.Click += new System.EventHandler(this.cmChangePassword_Click);
            // 
            // cmSendEmail
            // 
            this.cmSendEmail.Image = ((System.Drawing.Image)(resources.GetObject("cmSendEmail.Image")));
            this.cmSendEmail.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cmSendEmail.Name = "cmSendEmail";
            this.cmSendEmail.Size = new System.Drawing.Size(184, 38);
            this.cmSendEmail.Text = "Send Email";
            // 
            // cmPhoneCall
            // 
            this.cmPhoneCall.Image = ((System.Drawing.Image)(resources.GetObject("cmPhoneCall.Image")));
            this.cmPhoneCall.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cmPhoneCall.Name = "cmPhoneCall";
            this.cmPhoneCall.Size = new System.Drawing.Size(184, 38);
            this.cmPhoneCall.Text = "Phone Call";
            // 
            // btnAddUser
            // 
            this.btnAddUser.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddUser.BackgroundImage")));
            this.btnAddUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddUser.Location = new System.Drawing.Point(842, 237);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(70, 49);
            this.btnAddUser.TabIndex = 38;
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // cmbIsActive
            // 
            this.cmbIsActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIsActive.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbIsActive.FormattingEnabled = true;
            this.cmbIsActive.Items.AddRange(new object[] {
            "All",
            "Yes",
            "No"});
            this.cmbIsActive.Location = new System.Drawing.Point(325, 249);
            this.cmbIsActive.Name = "cmbIsActive";
            this.cmbIsActive.Size = new System.Drawing.Size(96, 26);
            this.cmbIsActive.TabIndex = 39;
            this.cmbIsActive.Visible = false;
            this.cmbIsActive.SelectedIndexChanged += new System.EventHandler(this.cmbIsActive_SelectedIndexChanged);
            // 
            // frmListUsers
            // 
            this.AcceptButton = this.btnAddUser;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(940, 596);
            this.Controls.Add(this.cmbIsActive);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblNumOfRecords);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvManageUsers);
            this.Controls.Add(this.txtFilterBy);
            this.Controls.Add(this.cmbFilterBy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmListUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Users";
            this.Load += new System.EventHandler(this.frmListUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageUsers)).EndInit();
            this.cmUsers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilterBy;
        private System.Windows.Forms.ComboBox cmbFilterBy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblNumOfRecords;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvManageUsers;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.ComboBox cmbIsActive;
        private System.Windows.Forms.ContextMenuStrip cmUsers;
        private System.Windows.Forms.ToolStripMenuItem cmShowDetails;
        private System.Windows.Forms.ToolStripMenuItem cmAddNewUser;
        private System.Windows.Forms.ToolStripMenuItem cmEdit;
        private System.Windows.Forms.ToolStripMenuItem cmDelete;
        private System.Windows.Forms.ToolStripMenuItem cmChangePassword;
        private System.Windows.Forms.ToolStripMenuItem cmSendEmail;
        private System.Windows.Forms.ToolStripMenuItem cmPhoneCall;
    }
}