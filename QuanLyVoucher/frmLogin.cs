﻿using Base.AppliBaseForm;
using Base.Common;
using Base.Connection;
using Base.Connection.OLEDB;
using Base.DevExpressEx.Utility;
using Business;
using Common.Globals;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using QuanLyVoucher.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVoucher
{
    public partial class frmLogin : BaseForm
    {
        private sys_UserBusiness _user;
        private ValidateUtis Valid = new ValidateUtis();
        private Remember rm = new Remember();
        private List<tbl_ChiNhanh> lChiNhanh = new List<tbl_ChiNhanh>();
        private List<tbl_Nganh> lNganh = new List<tbl_Nganh>();
        private bool m_IsLoginAs = false, m_ChonChiNhanh= false;
        private string m_Username = "", chiNhanh = "", nganh = "", XMLPath = QLVCConst.ConfigurePath;

        public frmLogin(string[] arg)
        {
            InitializeComponent();

            m_IsLoginAs = false;
            m_Username = "";
            ConfigLogin();
           // arg = new string[] { "1", "233DE45275D3CCA63554164D487BA51DC2121237F081E09B3AB46E9D2684AB8D86559A15BDDF14FE4A130E839BF1728B42A174ADD95D8388D4355B9094068FC6" };
            if (arg != null && arg.Length > 1)
            {
                string token = arg[1].Split('/').Where(t=>t != "").Last();
                handleTokenLogin(token);
            }
        }

        private void handleTokenLogin(string token)
        {
            if (GetConnection())
            {
                //clsPublicVar.User = new sys_User_CRM_SPA_HHHBusiness().GetUserAsSys_User(token);
                //if(clsPublicVar.User == null)
                //{
                //    SetFailure("Liên kết đăng nhập không hợp lệ. "+ token);
                //    return;
                //}
                _user = new sys_UserBusiness();

                //Set chi nhanh va nganh mac dinh
                clsPublicVar.ChiNhanh = new tbl_ChiNhanh();
                clsPublicVar.Nganh = new tbl_Nganh();
                cboChiNhanh.EditValue = null;
                cboNganh.EditValue = null;

                List<sys_UserRole> lUserRole = new Base<sys_UserRole>().Get(t => t.MaUser == clsPublicVar.User.Ma && (t.TrangThai ?? false)
                    && !(t.DaXoa ?? false)).ToList();
                sys_UserRole drUR = lUserRole.FirstOrDefault();
                cboChiNhanh.Properties.DropDownRows = 1;
                if (drUR != null)
                {
                    //Chi nhanh
                    tbl_ChiNhanh dfChiNhanh = _user.DC.GetTable<tbl_ChiNhanh>().FirstOrDefault(t => t.Ma == drUR.MaChiNhanh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (dfChiNhanh != null)
                    {
                        clsPublicVar.ChiNhanh = dfChiNhanh;
                        cboChiNhanh.EditValue = dfChiNhanh.Ma;
                    }

                    //Nganh
                    tbl_Nganh dfNganh = _user.DC.GetTable<tbl_Nganh>().FirstOrDefault(t => t.Ma == drUR.MaNganh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (dfNganh != null)
                    {
                        clsPublicVar.Nganh = dfNganh;
                        cboNganh.EditValue = dfNganh.Ma;
                    }
                }

                //Lay tat ca chi nhanh va nganh ma user co the dang nhap
                lChiNhanh = new List<tbl_ChiNhanh>();
                lNganh = new List<tbl_Nganh>();

                //Chen chi nhanh va nganh vao list combobox
                foreach (sys_UserRole item in lUserRole)
                {
                    //Chi nhanh
                    tbl_ChiNhanh chiNhanh = _user.DC.GetTable<tbl_ChiNhanh>().FirstOrDefault(t => t.Ma == item.MaChiNhanh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));

                    if (chiNhanh != null && !lChiNhanh.Any(t => t.Ma == chiNhanh.Ma))
                        lChiNhanh.Add(chiNhanh);

                    //Nganh
                    tbl_Nganh nganh = _user.DC.GetTable<tbl_Nganh>().FirstOrDefault(t => t.Ma == item.MaNganh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));

                    if (nganh != null && !lNganh.Any(t => t.Ma == nganh.Ma))
                        lNganh.Add(nganh);
                }

                //Binding data
                if ((lChiNhanh != null && lChiNhanh.Count > 1 && lNganh != null && lNganh.Count > 0)
                    || (lChiNhanh != null && lChiNhanh.Count > 0 && lNganh != null && lNganh.Count > 1))
                {
                    //Chi nhanh
                    cboChiNhanh.Properties.Columns.Clear();
                    cboChiNhanh.Properties.DataSource = lChiNhanh
                        .Select(t => new { t.Ma, t.Ten }).OrderBy(t => t.Ma).ToList();
                    cboChiNhanh.Properties.DropDownRows = lChiNhanh.Count;

                    tbl_ChiNhanh drChiNhanh = lChiNhanh.FirstOrDefault(t => t.Ma == Cast.ToInt(chiNhanh)
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (drChiNhanh != null)
                        cboChiNhanh.EditValue = chiNhanh;

                    //Nganh
                    cboNganh.Properties.Columns.Clear();
                    cboNganh.Properties.DataSource = lNganh
                        .Select(t => new { t.Ma, t.Ten }).ToList();

                    tbl_Nganh drNganh = lNganh.FirstOrDefault(t => t.Ma == Cast.ToInt(nganh)
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (drNganh != null)
                        cboNganh.EditValue = nganh;

                    m_ChonChiNhanh = true;
                    SetVisible4Branch(true);
                    SetReadOnlyControl(true);
                    ConfigureVisible(grbConfigure.Visible, m_ChonChiNhanh);
                }
                else
                {
                    GetPermissionForUser();
                    DialogResult = DialogResult.OK;
                }
            }
        }

        public frmLogin(bool IsLoginAs, string Username)
        {
            InitializeComponent();

            m_IsLoginAs = IsLoginAs;
            m_Username = Username;
            ConfigLogin();
        }

        private void ConfigLogin()
        {
            grbConfigure.Visible = false;
            ConfigureVisible(grbConfigure.Visible, m_ChonChiNhanh);

            //..
            cboDatabaseSQL.Build("Text", "Value", QLVCConst.ListDatabase.ToList()
                , new LookUpColumnInfo("Text", 100, "Tên CSDL"));

            cboChiNhanh.Build("Ten", "Ma", lChiNhanh
                , new LookUpColumnInfo("Ten", 200, "Tên chi nhánh"));

            cboNganh.Build("Ten", "Ma", lNganh
                , new LookUpColumnInfo("Ten", 200, "Tên ngành"));

            cboNganh.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as LookUpEdit;

                var list = lNganh.Where(t => t.MaChiNhanh == Cast.ToInt(cboChiNhanh.EditValue))
                    .Select(t => new { t.Ma, t.Ten }).OrderBy(t => t.Ma).ToList();

                cboEdit.Properties.DataSource = list;
                cboEdit.Properties.DropDownRows = list.Count;
            };

            cboNganh.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as LookUpEdit;
                cboEdit.Properties.DataSource = lNganh.ToList();
            };

            //..
            cboChiNhanh.EditValueChanged += (sender, e) => cboNganh.EditValue = null;

            //..
            LoadData();

            //get version
            if (ApplicationDeployment.IsNetworkDeployed)
                clsPublicVar.currentVersion = string.Format("Version [{0}]", ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString());
            txtVersion.Text = clsPublicVar.currentVersion;
        }

        private void LoadData()
        {
            //..
            SetVisible4Branch(false);

            //get remember me
            string userID = "";
            string passWord = "";
            string serverSQL = "";
            string userSQL = "";
            string passwordSQL = "";
            string databaseSQL = "";

            if (rm.GetRemember(ref XMLPath, ref userID, ref passWord, ref chiNhanh, ref nganh
                , ref serverSQL, ref userSQL, ref passwordSQL, ref databaseSQL).Success)
            {
                if (m_IsLoginAs)
                    txtUsername.Text = m_Username;
                else
                    txtUsername.Text = userID;

                txtPassword.Text = passWord;
                txtServerSQL.Text = serverSQL;
                txtUserSQL.Text = userSQL;
                txtPasswordSQL.Text = passwordSQL;
                cboDatabaseSQL.EditValue = databaseSQL;
                chkRemember.Checked = userID != "" ? true : false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (GetConnection())
            {
                if (Accessible())
                {
                    GetPermissionForUser();
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            grbConfigure.Visible = !grbConfigure.Visible;
            ConfigureVisible(grbConfigure.Visible, m_ChonChiNhanh);
        }

        private void ConfigureVisible(bool ConfigureVisible, bool BranchVisible)
        {
            if (ConfigureVisible && BranchVisible)
            {
                this.panLogin.Size = new System.Drawing.Size(788, 340);
                this.panTitle.Size = new System.Drawing.Size(778, 50);
                this.lbDangNhap.Location = new System.Drawing.Point(323, 8);
                this.lbDangNhap.Size = new System.Drawing.Size(128, 31);
                this.ClientSize = new System.Drawing.Size(795, 347);
            }
            else if (ConfigureVisible)
            {
                this.panLogin.Size = new System.Drawing.Size(788, 254);
                this.panTitle.Size = new System.Drawing.Size(778, 50);
                this.lbDangNhap.Location = new System.Drawing.Point(323, 8);
                this.lbDangNhap.Size = new System.Drawing.Size(128, 31);
                this.ClientSize = new System.Drawing.Size(795, 261);
            }
            else if (BranchVisible)
            {
                this.panLogin.Size = new System.Drawing.Size(505, 340);
                this.panTitle.Size = new System.Drawing.Size(495, 50);
                this.lbDangNhap.Location = new System.Drawing.Point(183, 8);
                this.lbDangNhap.Size = new System.Drawing.Size(128, 31);
                this.ClientSize = new System.Drawing.Size(511, 347);
            }
            else
            {
                this.panLogin.Size = new System.Drawing.Size(505, 254);
                this.panTitle.Size = new System.Drawing.Size(495, 50);
                this.lbDangNhap.Location = new System.Drawing.Point(183, 8);
                this.lbDangNhap.Size = new System.Drawing.Size(128, 31);
                this.ClientSize = new System.Drawing.Size(511, 261);
            }
        }

        private void GetPermissionForUser()
        {
            clsPublicVar.QuyenCuaUser = new Base<vQuyen>().Get(t => t.MaChiNhanh == clsPublicVar.ChiNhanh.Ma
                && t.MaNganh == clsPublicVar.Nganh.Ma && t.MaUser == clsPublicVar.User.Ma).ToList();
        }

        private bool Accessible()
        {
            try
            {
                Valid.AddValidNotBlank(txtUsername, "Bạn vui lòng nhập tên đăng nhập");
                Valid.AddValidNotBlank(txtPassword, "Bạn vui lòng nhập mật khẩu");

                _user = new sys_UserBusiness();

                if (!Valid.ValidateOK())
                    return false;

                sys_User u = null;

                //Login as
                if (m_IsLoginAs)
                {
                    u = _user.First(t => t.TenDangNhap == txtUsername.Text && (t.TrangThai ?? false));
                    if (u == null)
                    {
                        SetFailure("Tên đăng nhập hoặc mật khẩu không chính xác", "Thông báo");
                        return false;
                    }
                }
                else//Authenticate QLVC
                {
                    u = _user.First(t => t.TenDangNhap == txtUsername.Text && t.Password == Utility.GetSHA512(txtPassword.Text)
                        && (t.TrangThai ?? false));
                    if (u == null)
                    {
                        SetFailure("Tên đăng nhập hoặc mật khẩu không chính xác", "Thông báo");
                        return false;
                    }
                }

                //..apply log in successfull
                clsPublicVar.User = u;

                //Set chi nhanh va nganh mac dinh
                clsPublicVar.ChiNhanh = new tbl_ChiNhanh();
                clsPublicVar.Nganh = new tbl_Nganh();
                cboChiNhanh.EditValue = null;
                cboNganh.EditValue = null;

                List<sys_UserRole> lUserRole = new Base<sys_UserRole>().Get(t => t.MaUser == u.Ma && (t.TrangThai ?? false)
                    && !(t.DaXoa ?? false)).ToList();
                sys_UserRole drUR = lUserRole.FirstOrDefault();
                cboChiNhanh.Properties.DropDownRows = 1;
                if (drUR != null)
                {
                    //Chi nhanh
                    tbl_ChiNhanh dfChiNhanh = _user.DC.GetTable<tbl_ChiNhanh>().FirstOrDefault(t => t.Ma == drUR.MaChiNhanh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (dfChiNhanh != null)
                    {
                        clsPublicVar.ChiNhanh = dfChiNhanh;
                        cboChiNhanh.EditValue = dfChiNhanh.Ma;
                    }

                    //Nganh
                    tbl_Nganh dfNganh = _user.DC.GetTable<tbl_Nganh>().FirstOrDefault(t => t.Ma == drUR.MaNganh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (dfNganh != null)
                    {
                        clsPublicVar.Nganh = dfNganh;
                        cboNganh.EditValue = dfNganh.Ma;
                    }
                }

                //Lay tat ca chi nhanh va nganh ma user co the dang nhap
                lChiNhanh = new List<tbl_ChiNhanh>();
                lNganh = new List<tbl_Nganh>();
                
                //Chen chi nhanh va nganh vao list combobox
                foreach (sys_UserRole item in lUserRole)
                {
                    //Chi nhanh
                    tbl_ChiNhanh chiNhanh = _user.DC.GetTable<tbl_ChiNhanh>().FirstOrDefault(t => t.Ma == item.MaChiNhanh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));

                    if (chiNhanh != null && !lChiNhanh.Any(t => t.Ma == chiNhanh.Ma))
                        lChiNhanh.Add(chiNhanh);

                    //Nganh
                    tbl_Nganh nganh = _user.DC.GetTable<tbl_Nganh>().FirstOrDefault(t => t.Ma == item.MaNganh
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));

                    if (nganh != null && !lNganh.Any(t => t.Ma == nganh.Ma))
                        lNganh.Add(nganh);
                }

                //Binding data
                if ((lChiNhanh != null && lChiNhanh.Count > 1 && lNganh != null && lNganh.Count > 0)
                    || (lChiNhanh != null && lChiNhanh.Count > 0 && lNganh != null && lNganh.Count > 1))
                {
                    //Chi nhanh
                    cboChiNhanh.Properties.Columns.Clear();
                    cboChiNhanh.Properties.DataSource = lChiNhanh
                        .Select(t => new { t.Ma, t.Ten }).OrderBy(t => t.Ma).ToList();
                    cboChiNhanh.Properties.DropDownRows = lChiNhanh.Count;

                    tbl_ChiNhanh drChiNhanh = lChiNhanh.FirstOrDefault(t => t.Ma == Cast.ToInt(chiNhanh)
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (drChiNhanh != null)
                        cboChiNhanh.EditValue = chiNhanh;

                    //Nganh
                    cboNganh.Properties.Columns.Clear();
                    cboNganh.Properties.DataSource = lNganh
                        .Select(t => new { t.Ma, t.Ten }).ToList();

                    tbl_Nganh drNganh = lNganh.FirstOrDefault(t => t.Ma == Cast.ToInt(nganh)
                        && (t.TrangThai ?? false) && !(t.DaXoa ?? false));
                    if (drNganh != null)
                        cboNganh.EditValue = nganh;

                    m_ChonChiNhanh = true;
                    SetVisible4Branch(true);
                    SetReadOnlyControl(true);
                    ConfigureVisible(grbConfigure.Visible, m_ChonChiNhanh);
                    return false;
                }
                else
                    SetReadOnlyControl(true);

                //..check remember me
                if (!m_IsLoginAs)
                    SetRememberMe();

                return (u != null);
            }
            catch (Exception ex)
            {
                SetFailure(ex.Message);
            }
            return false;
        }

        private void SetRememberMe()
        {
            if (chkRemember.Checked)
                rm.SetRemember(txtUsername.Text, txtPassword.Text, Cast.ToString(clsPublicVar.ChiNhanh.Ma), Cast.ToString(clsPublicVar.Nganh.Ma)
                    , txtServerSQL.Text, txtUserSQL.Text, txtPasswordSQL.Text, Cast.ToString(cboDatabaseSQL.EditValue));
            else
                rm.SetRemember("", "", "", ""
                    , txtServerSQL.Text, txtUserSQL.Text, txtPasswordSQL.Text, Cast.ToString(cboDatabaseSQL.EditValue));
        }

        private bool GetConnection()
        {
            bool isConnectOK = true;

            //Set connection
            QLVCConst.SetConnectionString(Cast.ToString(txtServerSQL.EditValue), Cast.ToString(txtUserSQL.EditValue)
                , Cast.ToString(txtPasswordSQL.EditValue), Cast.ToString(cboDatabaseSQL.EditValue));

            clsPublicVar.Conn = new ConnectionSQLServer(QLVCConst.ConnectionStrings);

            if (clsPublicVar.Conn == null || !clsPublicVar.Conn.ConnectOK())
            {
                isConnectOK = false;
                XtraMessageBox.Show("Kết nối thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Constant.ConnectionString = QLVCConst.ConnectionStrings;
            }

            return isConnectOK;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            Valid.AddValidNotBlank(cboChiNhanh, "Bạn vui lòng chọn Chi nhánh");
            Valid.AddValidNotBlank(cboNganh, "Bạn vui lòng chọn Ngành");

            if (!Valid.ValidateOK())
                return;

            clsPublicVar.ChiNhanh = new tbl_ChiNhanhBusiness().First(t => t.Ma == Cast.ToInt(cboChiNhanh.EditValue));
            clsPublicVar.Nganh = new tbl_NganhBusiness().First(t => t.Ma == Cast.ToInt(cboNganh.EditValue));

            //..check remember me
            if (!m_IsLoginAs)
                SetRememberMe();

            GetPermissionForUser();
            DialogResult = DialogResult.OK;
        }

        private void SetVisible4Branch(bool visible)
        {
            lbChiNhanh.Visible = visible;
            cboChiNhanh.Visible = visible;
            lbNganh.Visible = visible;
            cboNganh.Visible = visible;
            btnSelect.Visible = visible;

            txtUsername.Enabled = !visible;
            txtPassword.Enabled = !visible;
            btnLogin.Enabled = !visible;

            if (visible)
            {
                this.AcceptButton = btnSelect;
                cboChiNhanh.Focus();
            }
            else
                this.AcceptButton = btnLogin;
        }

        private void SetReadOnlyControl(bool bReadOnly)
        {
            txtServerSQL.Properties.ReadOnly = bReadOnly;
            txtUserSQL.Properties.ReadOnly = bReadOnly;
            txtPasswordSQL.Properties.ReadOnly = bReadOnly;
            cboDatabaseSQL.Properties.ReadOnly = bReadOnly;
        }
    }
}
