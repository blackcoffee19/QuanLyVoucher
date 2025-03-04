﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Base.AppliBaseForm;
using Service;
using Business;
using Base.DevExpressEx.Utility;
using DevExpress.XtraEditors.Controls;
using Base.AppliBase;
using QuanLyVoucher.Globals;
using Base.AppliBase.Items;
using DevExpress.XtraEditors;
using Base.AppliBaseForm.Globals;
using Tool;
using Common.Globals;
using Business.Globals;
using Base.Common;
using DevExpress.Utils;
using Base.Common.Items;
using System.IO;
using DevExpress.XtraGrid.Views.Grid;
using Base.DevExpressEx.Extension;
using DevExpress.XtraGrid;

namespace QuanLyVoucher.Admin
{
    public partial class frmUser : BaseFormOnGrid<sys_User, sys_UserBusiness, QLVCDataContext>
    {
        private frmUserRole m_frmUserRole;
        private readonly int m_MenuID = clsPublicVar.MainMenuID;
        private tbl_ChiNhanhBusiness tbl_ChiNhanhB = new tbl_ChiNhanhBusiness();

        public frmUser()
        {
            InitializeComponent();

            //..
            btnResetPassword.Enabled = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID
                && t.MaQuyen == (int)Quyen.ResetPassword);
            btnInsertPhoto.Enabled = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID
                && t.MaQuyen == (int)Quyen.ChenHinhAnh);

            ConfigUser();
            ConfigUserRole();
            btnUserRoleThemNhanh.Enabled = false;
            cboChiNhanhNganh.Enabled = btnUserRoleThemNhanh.Enabled;
            cboRole.Enabled = btnUserRoleThemNhanh.Enabled;

            //..
            cboRole.Build("Ten", "Ma", new Base<sys_Role>().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false)).ToList(), 300, 350
                , new LookUpColumnInfo("Ten", 300, "Role"));

            var Nganh = tbl_ChiNhanhB.Get()
                .Join(tbl_ChiNhanhB.DC.GetTable<tbl_Nganh>(), cn => cn.Ma, n => n.MaChiNhanh, (cn, n) => new { cn, n })
                .Where(t => (t.cn.TrangThai ?? false) && !(t.cn.DaXoa ?? false)
                    && (t.n.TrangThai ?? false) && !(t.n.DaXoa ?? false))
                .Select(t => new { Ma = t.n.Ma, Ten = t.cn.Ten + " - " + t.n.Ten })
                .OrderBy(t => t.Ten).ToList();
            cboChiNhanhNganh.Properties.DropDownRows = 20;
            cboChiNhanhNganh.Properties.DataSource = Nganh;
            cboChiNhanhNganh.Properties.ValueMember = "Ma";
            cboChiNhanhNganh.Properties.DisplayMember = "Ten";
        }

        private void ConfigUser()
        {
            //..
            FunctionLoadData = "GetData";

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cTenDangNhap", "Tên đăng nhập", "TenDangNhap", true, 30),
                                new DataFieldColumn("cPasswordEx", "Mật khẩu", "PasswordEx", false, 30),
                                new DataFieldColumn("cHoTen", "Họ và tên", "HoTen", false, 50),
                                new DataFieldColumn("cGioiTinhEx", "Giới tính", "GioiTinhEx", false, 20),
                                new DataFieldColumn("cMsnv", "MSNV", "MSNV", false, 30),
                                new DataFieldColumn("cTuChoiNhanEmail", "Từ chối nhận email", "TuChoiNhanEmail", false, 10),
                                new DataFieldColumn("cEmail", "Email", "Email", false, 50),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 10),
                                new DataFieldColumn("cSpaUSER", "UserCRM", "CRMUser", false, 10),
                                new DataFieldColumn("cLoginAs", "Login as", "LoginAs", false, 10));

            //..specify field what use combobox
            List<BasicItem> lGioiTinh = new List<BasicItem>(new[]
            {
                new BasicItem("Nam", Enum.GetName(typeof(GioiTinh), GioiTinh.Nam)),
                new BasicItem("Nữ", Enum.GetName(typeof(GioiTinh), GioiTinh.Nu))
            });
            GvMain.ExSetSearchColumnCombobox("GioiTinhEx", "Text", "Value", lGioiTinh.ToList(), 350, 350
                , new LookUpColumnInfo("Text", 100, "Giới tính"));

            GvMain.ExSetColumnButton("LoginAs", "Login as", (sender, e) =>
            {
                sys_User drU = GvMain.ExGetCurrentDataRow() as sys_User;

                if (drU != null)
                {
                    Program.m_IsLoginAs = true;
                    Program.m_Username = drU.TenDangNhap;
                    Program.fMain.CloseAllChildren();
                    Program.fMain.bSignOut = true;
                    Program.fMain.Close();
                }
            });

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..
            panUser.Controls.Add(GMain);

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..set auto values
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("CRMUser", false));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiTao", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgayTao", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiSua", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgaySua", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.Edit));
            
            //..set validate
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("TenDangNhap", "Bạn chưa nhập tên đăng nhập", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("TenDangNhap", "Tên đăng nhập đã tồn tại", "", AutoGenExistedOnAddNew("TenDangNhap", clsPublicVar.FieldCheckDelete)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("PasswordEx", "Bạn chưa nhập mật khẩu", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("GioiTinhEx", "Bạn chưa chọn giới tính", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("HoTen", "Bạn chưa nhập họ tên", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("Email", "Email không đúng định dạng", "", dr => dr != null && Utility.IsEmail(((sys_User)dr).Email, true)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("MSNV", "Vui lòng nhập mã số nhân viên", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("MSNV", "Mã số nhân viên đã tồn tại", "", AutoGenExistedOnAddNew("MSNV", clsPublicVar.FieldCheckDelete)));


            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<tbl_BanHang>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_ChiNhanh>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_KhachHang>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_Nganh>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_ThanhToan>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_Voucher>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_Menu>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_Quyen>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_Role>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_RoleMenu>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_RoleQuyen>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_UserRole>("ID", "NguoiTao", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_UserRole>("ID", "MaUser", false, clsPublicVar.FieldCheckDelete);

            //just allow edit detail when master in view mode only.
            GridUtil.DataModeChanged += (modType) =>
            {
                m_frmUserRole.GMain.Enabled = (modType == DataModeType.View);
            };
        }

        private void ConfigUserRole()
        {
            m_frmUserRole = new frmUserRole() { Name = "frmUserRole" };
            m_frmUserRole.FunctionLoadData = "GetFollowUser";
            m_frmUserRole.FuLoadDataParas = () =>
            {
                sys_User focusedUser = GvMain.ExGetCurrentDataRow() as sys_User;
                if (focusedUser != null)
                    return new[] { (object)focusedUser.Ma };
                return new[] { (object)int.MinValue };
            };

            //..set auto values
            m_frmUserRole.GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("MaUser", dr => ((sys_User)this.GvMain.ExGetCurrentDataRow()).Ma));

            //include child form to master form            
            m_frmUserRole.TopLevel = false;
            m_frmUserRole.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            m_frmUserRole.ControlBox = false;
            m_frmUserRole.Dock = DockStyle.Fill;
            panRole.Controls.Add(m_frmUserRole);

            //config child form behavior            
            this.GvMain.FocusedRowChanged += (sender, e) =>
            {
                if (this.GridUtil.DataMode == DataModeType.View)
                {
                    m_frmUserRole.Browse();

                    //..
                    this.GridUtil.DataMode = GridUtil.DataMode;

                    LoadPhoto();
                }
            };

            m_frmUserRole.GMain.Enter += (sender, e) =>
            {
                sys_User currentUser = GvMain.ExGetCurrentDataRow() as sys_User;
                m_frmUserRole.m_CurrentUserID = (currentUser != null ? currentUser.Ma : 0);

                this.SetChildFormDeactivate();
                BaseFormFocus = m_frmUserRole;
                m_frmUserRole.SetChildFormActivated();
            };

            m_frmUserRole.GMain.Leave += (sender, e) =>
            {
                m_frmUserRole.SetChildFormDeactivate();
                BaseFormFocus = this;
                this.SetChildFormActivated();
            };

            //just allow edit detail when master in view mode only.
            m_frmUserRole.GridUtil.DataModeChanged += (modType) =>
            {
                this.GMain.Enabled = (modType == DataModeType.View);
                btnUserRoleThemNhanh.Enabled = modType == DataModeType.AddNew;
                cboChiNhanhNganh.Enabled = btnUserRoleThemNhanh.Enabled;
                cboRole.Enabled = btnUserRoleThemNhanh.Enabled;

                if (!btnUserRoleThemNhanh.Enabled)
                {
                    cboChiNhanhNganh.EditValue = null;
                    cboChiNhanhNganh.RefreshEditValue();
                    cboRole.EditValue = null;
                }
            };
        }

        protected override void BaseForm_Load(object sender, System.EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            m_frmUserRole._parent = this._parent;
            m_frmUserRole.Show();

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvMain, QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }


        public override void Save()
        {
            sys_User drU = GvMain.ExGetCurrentDataRow() as sys_User;
            if (drU != null && (drU.CRMUser ?? false)) 
            {
                SetFailure("Không thể chỉnh sửa USER CRM");
                return;
            }

            BindingList<sys_User> user = (GvMain.DataSource as BindingList<sys_User>);
            foreach (sys_User item in userCRM)
            {
                if (item.CRMUser ?? false)
                {
                    user.Remove(item);
                }
            }
            base.Save();
            foreach (sys_User item in userCRM)
            {
               user.Add(item);
            }
        }

        protected override void RaiseEventToChild(ButtonAction action)
        {
            //make sure toolbar mode use for master when grid control focused only
            if (GMain.Focused || !m_frmUserRole.GMain.Enabled)
                base.RaiseEventToChild(action);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            sys_User drU = GvMain.ExGetCurrentDataRow() as sys_User;

            if (drU != null)
            {
                //Set password new
                string Pass = string.Format("QLVC{0}!", DataUtils.GetDate().Year);

                //Update password new
                drU.Password = Utility.GetSHA512(Pass);
                State = new Base<sys_User>().Update(drU, t => t.Ma == drU.Ma, new string[] { "Password" }, false, true);

                if (State.Success)
                    SetSuccess(string.Format("Reset thành công, mật khẩu mới của {0} là: {1}", drU.TenDangNhap, Pass));
                else
                    SetFailure("Reset thất bại");
            }
            else
                SetFailure("Bạn vui lòng chọn người dùng muốn reset mật khẩu");
        }

        public override void Delete()
        {
            sys_User drU = GvMain.ExGetCurrentDataRow() as sys_User;

            if (drU != null && (drU.CRMUser ?? false))
            {
                SetFailure("Không thể xóa USER CRM");
                return;
            }
            if (drU != null)
            {
                if (drU.TenDangNhap.ToLower() == "admin")
                {
                    SetFailure("Bạn không thể xóa người dùng này");
                    return;
                }
            }

            UpdateDelete(clsPublicVar.FieldUpdateDelete());
        }
        List<sys_User> userCRM = new List<sys_User>();
        public override void Browse()
        {
            base.Browse();

            m_frmUserRole.Browse();

            GridUtil.DataMode = GridUtil.DataMode;

           // userCRM = new sys_User_CRM_SPA_HHHBusiness().GetDataAsSys_User();

            foreach(sys_User user in userCRM)
            {
                (GvMain.DataSource as BindingList<sys_User>).Add(user);
            }
        }

        private void btnInsertPhoto_Click(object sender, EventArgs e)
        {
            sys_User drU = GvMain.ExGetCurrentDataRow() as sys_User;

            if (drU != null)
            {
                OpenFileDialog cdlg = new OpenFileDialog();
                cdlg.Multiselect = false;

                if (cdlg.ShowDialog() == DialogResult.OK)
                {
                    FileStream stream = new FileStream(
                        cdlg.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(stream);

                    byte[] photo = reader.ReadBytes((int)stream.Length);

                    drU.Photo = photo;

                    new Base<sys_User>().Update(drU, t => t.Ma == drU.Ma, true);

                    if (State.Success)
                        SetSuccess("Thêm thành công");
                    else
                        SetFailure("Thêm thất bại");
                }
            }
            else
                SetFailure("Bạn vui lòng chọn nhân viên bạn muốn thêm");
        }

        private void LoadPhoto()
        {
            try
            {
                sys_User dr = GvMain.ExGetCurrentDataRow() as sys_User;
                if (dr != null && dr.Photo != null)
                {
                    MemoryStream ms = new MemoryStream(dr.Photo.ToArray(), true);
                    ms.Write(dr.Photo.ToArray(), 0, dr.Photo.ToArray().Length);
                    Image image = Image.FromStream(ms, true);
                    picPhoto.Image = image;
                    picPhoto.SizeMode = PictureBoxSizeMode.CenterImage;
                    picPhoto.BackColor = Color.White;
                }
                else
                {
                    picPhoto.Image = QuanLyVoucher.Properties.Resources.no_image;
                    picPhoto.SizeMode = PictureBoxSizeMode.CenterImage;
                    picPhoto.BackColor = Color.White;
                }
            }
            catch
            {
                picPhoto.Image = null;
            }
        }

        private void btnUserRoleThemNhanh_Click(object sender, EventArgs e)
        {
            if (cboChiNhanhNganh.EditValue != null && cboRole.EditValue != null)
            {
                sys_UserRoleBusiness sys_UserRoleB = new sys_UserRoleBusiness();
                List<sys_UserRole> lUserRole = (m_frmUserRole.GvMain.GridControl.DataSource as IQueryable<sys_UserRole>).ToList();
                string[] Nganh = Cast.ToString(cboChiNhanhNganh.EditValue).Split(';');

                sys_User drU = GvMain.ExGetCurrentDataRow() as sys_User;

                if (drU != null)
                {
                    foreach (string n in Nganh)
                    {
                        tbl_Nganh drN = sys_UserRoleB.DC.GetTable<tbl_Nganh>().FirstOrDefault(t => t.Ma == Cast.ToInt(n.Trim()));

                        if (drN != null && !lUserRole.Any(t => t.MaUser == drU.Ma && t.MaRole == Cast.ToInt(cboRole.EditValue)
                            && t.MaChiNhanh == drN.MaChiNhanh && t.MaNganh == drN.Ma))
                        {
                            sys_UserRole userRole = new sys_UserRole()
                            {
                                MaUser = drU.Ma,
                                MaRole = Cast.ToInt(cboRole.EditValue),
                                MaChiNhanh = drN.MaChiNhanh,
                                MaNganh = drN.Ma,
                                TrangThai = true,
                                NguoiTao = clsPublicVar.User.Ma,
                                NgayTao = DataUtils.GetDate()
                            };
                            sys_UserRoleB.Add(userRole, true);
                        };
                    }
                }

                m_frmUserRole.Browse();
                cboChiNhanhNganh.EditValue = null;
                cboChiNhanhNganh.RefreshEditValue();
                cboRole.EditValue = null;
            }
        }
    }
}
