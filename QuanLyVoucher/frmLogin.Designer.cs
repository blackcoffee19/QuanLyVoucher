namespace QuanLyVoucher
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.grbConfigure = new System.Windows.Forms.GroupBox();
            this.cboDatabaseSQL = new Base.DevExpressEx.Extension.LookUpEditEx();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtServerSQL = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtUserSQL = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtPasswordSQL = new DevExpress.XtraEditors.TextEdit();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtVersion = new DevExpress.XtraEditors.LabelControl();
            this.btnConfigure = new DevExpress.XtraEditors.SimpleButton();
            this.panTitle = new DevExpress.XtraEditors.PanelControl();
            this.lbDangNhap = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkRemember = new DevExpress.XtraEditors.CheckEdit();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.panLogin = new DevExpress.XtraEditors.PanelControl();
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.cboNganh = new Base.DevExpressEx.Extension.LookUpEditEx();
            this.cboChiNhanh = new Base.DevExpressEx.Extension.LookUpEditEx();
            this.lbNganh = new DevExpress.XtraEditors.LabelControl();
            this.lbChiNhanh = new DevExpress.XtraEditors.LabelControl();
            this.grbConfigure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDatabaseSQL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerSQL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserSQL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordSQL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panTitle)).BeginInit();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemember.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panLogin)).BeginInit();
            this.panLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboNganh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboChiNhanh.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grbConfigure
            // 
            this.grbConfigure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.grbConfigure.Controls.Add(this.cboDatabaseSQL);
            this.grbConfigure.Controls.Add(this.labelControl3);
            this.grbConfigure.Controls.Add(this.labelControl7);
            this.grbConfigure.Controls.Add(this.txtServerSQL);
            this.grbConfigure.Controls.Add(this.labelControl6);
            this.grbConfigure.Controls.Add(this.txtUserSQL);
            this.grbConfigure.Controls.Add(this.labelControl5);
            this.grbConfigure.Controls.Add(this.txtPasswordSQL);
            this.grbConfigure.Location = new System.Drawing.Point(505, 91);
            this.grbConfigure.Name = "grbConfigure";
            this.grbConfigure.Size = new System.Drawing.Size(277, 129);
            this.grbConfigure.TabIndex = 21;
            this.grbConfigure.TabStop = false;
            this.grbConfigure.Text = "Cấu hình";
            // 
            // cboDatabaseSQL
            // 
            this.cboDatabaseSQL.Location = new System.Drawing.Point(90, 101);
            this.cboDatabaseSQL.Name = "cboDatabaseSQL";
            this.cboDatabaseSQL.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDatabaseSQL.Size = new System.Drawing.Size(181, 20);
            this.cboDatabaseSQL.TabIndex = 21;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(6, 20);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(61, 13);
            this.labelControl3.TabIndex = 17;
            this.labelControl3.Text = "Tên máy chủ";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(6, 101);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(66, 13);
            this.labelControl7.TabIndex = 20;
            this.labelControl7.Text = "Tên database";
            // 
            // txtServerSQL
            // 
            this.txtServerSQL.Location = new System.Drawing.Point(90, 17);
            this.txtServerSQL.Name = "txtServerSQL";
            this.txtServerSQL.Size = new System.Drawing.Size(181, 20);
            this.txtServerSQL.TabIndex = 13;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(6, 74);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(44, 13);
            this.labelControl6.TabIndex = 19;
            this.labelControl6.Text = "Mật khẩu";
            // 
            // txtUserSQL
            // 
            this.txtUserSQL.Location = new System.Drawing.Point(90, 44);
            this.txtUserSQL.Name = "txtUserSQL";
            this.txtUserSQL.Size = new System.Drawing.Size(181, 20);
            this.txtUserSQL.TabIndex = 14;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(6, 47);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 13);
            this.labelControl5.TabIndex = 18;
            this.labelControl5.Text = "Tên đăng nhập";
            // 
            // txtPasswordSQL
            // 
            this.txtPasswordSQL.Location = new System.Drawing.Point(90, 71);
            this.txtPasswordSQL.Name = "txtPasswordSQL";
            this.txtPasswordSQL.Properties.PasswordChar = '0';
            this.txtPasswordSQL.Properties.UseSystemPasswordChar = true;
            this.txtPasswordSQL.Size = new System.Drawing.Size(181, 20);
            this.txtPasswordSQL.TabIndex = 15;
            // 
            // panelControl3
            // 
            this.panelControl3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panelControl3.Appearance.Options.UseBackColor = true;
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.ContentImage = global::QuanLyVoucher.Properties.Resources.login;
            this.panelControl3.Location = new System.Drawing.Point(4, 91);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(182, 147);
            this.panelControl3.TabIndex = 12;
            // 
            // txtVersion
            // 
            this.txtVersion.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVersion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.txtVersion.Location = new System.Drawing.Point(360, 190);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(130, 13);
            this.txtVersion.TabIndex = 8;
            this.txtVersion.Text = "Version [99.99.99.99]";
            // 
            // btnConfigure
            // 
            this.btnConfigure.Location = new System.Drawing.Point(210, 220);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(75, 23);
            this.btnConfigure.TabIndex = 22;
            this.btnConfigure.Text = "Cấu hình";
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // panTitle
            // 
            this.panTitle.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.panTitle.Appearance.Options.UseBackColor = true;
            this.panTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panTitle.Controls.Add(this.lbDangNhap);
            this.panTitle.Location = new System.Drawing.Point(5, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(778, 50);
            this.panTitle.TabIndex = 11;
            // 
            // lbDangNhap
            // 
            this.lbDangNhap.Appearance.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDangNhap.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbDangNhap.Location = new System.Drawing.Point(323, 8);
            this.lbDangNhap.Name = "lbDangNhap";
            this.lbDangNhap.Size = new System.Drawing.Size(128, 31);
            this.lbDangNhap.TabIndex = 10;
            this.lbDangNhap.Text = "Đăng nhập";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(213, 135);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Tên đăng nhập";
            // 
            // groupControl1
            // 
            this.groupControl1.Location = new System.Drawing.Point(213, 91);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(277, 23);
            this.groupControl1.TabIndex = 9;
            this.groupControl1.Text = "Thông tin đăng nhập";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(303, 132);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(187, 20);
            this.txtUsername.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(213, 164);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(44, 13);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Mật khẩu";
            // 
            // chkRemember
            // 
            this.chkRemember.Location = new System.Drawing.Point(301, 187);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Properties.Caption = "Ghi nhớ";
            this.chkRemember.Size = new System.Drawing.Size(75, 19);
            this.chkRemember.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(303, 161);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(187, 20);
            this.txtPassword.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(409, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Thoát";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(303, 220);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(88, 23);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // panLogin
            // 
            this.panLogin.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.panLogin.Appearance.Options.UseBackColor = true;
            this.panLogin.Controls.Add(this.btnSelect);
            this.panLogin.Controls.Add(this.grbConfigure);
            this.panLogin.Controls.Add(this.cboNganh);
            this.panLogin.Controls.Add(this.cboChiNhanh);
            this.panLogin.Controls.Add(this.lbNganh);
            this.panLogin.Controls.Add(this.lbChiNhanh);
            this.panLogin.Controls.Add(this.btnConfigure);
            this.panLogin.Controls.Add(this.txtVersion);
            this.panLogin.Controls.Add(this.panelControl3);
            this.panLogin.Controls.Add(this.panTitle);
            this.panLogin.Controls.Add(this.labelControl1);
            this.panLogin.Controls.Add(this.groupControl1);
            this.panLogin.Controls.Add(this.txtUsername);
            this.panLogin.Controls.Add(this.labelControl2);
            this.panLogin.Controls.Add(this.chkRemember);
            this.panLogin.Controls.Add(this.txtPassword);
            this.panLogin.Controls.Add(this.btnCancel);
            this.panLogin.Controls.Add(this.btnLogin);
            this.panLogin.Location = new System.Drawing.Point(3, 3);
            this.panLogin.Name = "panLogin";
            this.panLogin.Size = new System.Drawing.Size(788, 340);
            this.panLogin.TabIndex = 12;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(303, 310);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(88, 23);
            this.btnSelect.TabIndex = 27;
            this.btnSelect.Text = "Chọn";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cboNganh
            // 
            this.cboNganh.Location = new System.Drawing.Point(303, 283);
            this.cboNganh.Name = "cboNganh";
            this.cboNganh.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboNganh.Size = new System.Drawing.Size(187, 20);
            this.cboNganh.TabIndex = 26;
            // 
            // cboChiNhanh
            // 
            this.cboChiNhanh.Location = new System.Drawing.Point(303, 255);
            this.cboChiNhanh.Name = "cboChiNhanh";
            this.cboChiNhanh.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboChiNhanh.Size = new System.Drawing.Size(187, 20);
            this.cboChiNhanh.TabIndex = 25;
            // 
            // lbNganh
            // 
            this.lbNganh.Location = new System.Drawing.Point(213, 286);
            this.lbNganh.Name = "lbNganh";
            this.lbNganh.Size = new System.Drawing.Size(31, 13);
            this.lbNganh.TabIndex = 24;
            this.lbNganh.Text = "Ngành";
            // 
            // lbChiNhanh
            // 
            this.lbChiNhanh.Location = new System.Drawing.Point(213, 258);
            this.lbChiNhanh.Name = "lbChiNhanh";
            this.lbChiNhanh.Size = new System.Drawing.Size(48, 13);
            this.lbChiNhanh.TabIndex = 23;
            this.lbChiNhanh.Text = "Chi nhánh";
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(795, 347);
            this.Controls.Add(this.panLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.grbConfigure.ResumeLayout(false);
            this.grbConfigure.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboDatabaseSQL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerSQL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserSQL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordSQL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panTitle)).EndInit();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemember.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panLogin)).EndInit();
            this.panLogin.ResumeLayout(false);
            this.panLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboNganh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboChiNhanh.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbConfigure;
        private Base.DevExpressEx.Extension.LookUpEditEx cboDatabaseSQL;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit txtServerSQL;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtUserSQL;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtPasswordSQL;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl txtVersion;
        private DevExpress.XtraEditors.SimpleButton btnConfigure;
        private DevExpress.XtraEditors.PanelControl panTitle;
        private DevExpress.XtraEditors.LabelControl lbDangNhap;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit txtUsername;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit chkRemember;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.PanelControl panLogin;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
        private Base.DevExpressEx.Extension.LookUpEditEx cboNganh;
        private Base.DevExpressEx.Extension.LookUpEditEx cboChiNhanh;
        private DevExpress.XtraEditors.LabelControl lbNganh;
        private DevExpress.XtraEditors.LabelControl lbChiNhanh;
    }
}