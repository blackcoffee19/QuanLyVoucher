using System;
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
using Base.Common;
using Base.AppliBaseForm.Globals;
using Tool;
using Common.Globals;
using DevExpress.Utils;

namespace QuanLyVoucher.Admin
{
    public partial class frmRole : BaseFormOnGrid<sys_Role, sys_RoleBusiness, QLVCDataContext>
    {
        private frmRoleMenu m_frmRoleMenu;
        private frmRoleQuyen m_frmRoleQuyen;

        public frmRole()
        {
            InitializeComponent();

            ConfigureRoleGrid();
            ConfigueRoleMenu();
            ConfigueRoleQuyen();
            ConfigureControls();
        }

        private void ConfigureRoleGrid()
        {
            //..
            FunctionLoadData = "GetData";

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cCode", "Mã role", "Code", true, 50),
                                new DataFieldColumn("cTen", "Tên role", "Ten", false, 100),
                                new DataFieldColumn("cMoTa", "Mô tả", "MoTa", false, 150),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 20));

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..
            panRole.Controls.Add(GMain);

            //..set auto values
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiTao", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgayTao", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiSua", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgaySua", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("TrangThai", true, dr => GridUtil.DataMode == DataModeType.AddNew));

            //..set validate
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Code", "Bạn chưa nhập mã role", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập tên role", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("Code", "Mã role đã tồn tại", "", AutoGenExistedOnAddNew("Code", clsPublicVar.FieldCheckDelete)));

            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<sys_UserRole>("Ma", "MaRole", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_RoleQuyen>("Ma", "MaRole", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_RoleMenu>("Ma", "MaRole", false, clsPublicVar.FieldCheckDelete);

            //just allow edit detail when master in view mode only.
            GridUtil.DataModeChanged += (modType) =>
            {
                m_frmRoleMenu.GMain.Enabled = (modType == DataModeType.View);
                m_frmRoleQuyen.GMain.Enabled = (modType == DataModeType.View);
            };
        }

        private void ConfigueRoleMenu()
        {
            m_frmRoleMenu = new frmRoleMenu() { Name = "frmRoleMenu" };
            m_frmRoleMenu.FunctionLoadData = "GetFollowRole";
            m_frmRoleMenu.FuLoadDataParas = () =>
            {
                sys_Role focusedRole = GvMain.ExGetCurrentDataRow() as sys_Role;
                if (focusedRole != null)
                    return new[] { (object)focusedRole.Ma };
                return new[] { (object)int.MinValue };
            };

            //..set auto values
            m_frmRoleMenu.GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("MaRole", dr => ((sys_Role)GvMain.ExGetCurrentDataRow()).Ma));

            //include child form to master form            
            m_frmRoleMenu.TopLevel = false;
            m_frmRoleMenu.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            m_frmRoleMenu.ControlBox = false;
            m_frmRoleMenu.Dock = DockStyle.Fill;
            panMenu.Controls.Add(m_frmRoleMenu);

            //config child form behavior            
            this.GvMain.FocusedRowChanged += (sender, e) =>
            {
                if (this.GridUtil.DataMode == DataModeType.View)
                {
                    m_frmRoleMenu.Browse();
                    m_frmRoleQuyen.Browse();

                    this.GridUtil.DataMode = this.GridUtil.DataMode;
                }
            };

            m_frmRoleMenu.GMain.Enter += (sender, e) =>
            {
                sys_Role focusedRole = GvMain.ExGetCurrentDataRow() as sys_Role;
                m_frmRoleMenu.m_CurrentRoleID = (focusedRole != null ? focusedRole.Ma : short.MinValue);
                m_frmRoleMenu.m_frmRole = this;
                
                this.SetChildFormDeactivate();
                BaseFormFocus = m_frmRoleMenu;
                m_frmRoleMenu.SetChildFormActivated();
            };

            m_frmRoleMenu.GMain.Leave += (sender, e) =>
            {
                m_frmRoleMenu.SetChildFormDeactivate();
                BaseFormFocus = this;
                this.SetChildFormActivated();
            };

            //just allow edit detail when master in view mode only.
            m_frmRoleMenu.GridUtil.DataModeChanged += (modType) =>
            {
                this.GMain.Enabled = (modType == DataModeType.View);
                m_frmRoleQuyen.GMain.Enabled = (modType == DataModeType.View);
            };
        }

        private void ConfigueRoleQuyen()
        {
            m_frmRoleQuyen = new frmRoleQuyen() { Name = "frmRoleQuyen" };
            m_frmRoleQuyen.FunctionLoadData = "GetFollowRoleMenu";
            m_frmRoleQuyen.FuLoadDataParas = () =>
            {
                sys_RoleMenu focusedRoleMenu = m_frmRoleMenu.GvMain.ExGetCurrentDataRow() as sys_RoleMenu;
                if (focusedRoleMenu != null)
                    return new[] { (object)focusedRoleMenu.MaRole, (object)focusedRoleMenu.MaMenu };
                return new[] { (object)int.MinValue, (object)int.MinValue };
            };

            //..set auto values
            m_frmRoleQuyen.GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("MaRole", dr => ((sys_RoleMenu)m_frmRoleMenu.GvMain.ExGetCurrentDataRow()).MaRole));
            m_frmRoleQuyen.GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("MaMenu", dr => ((sys_RoleMenu)m_frmRoleMenu.GvMain.ExGetCurrentDataRow()).MaMenu));

            //include child form to master form            
            m_frmRoleQuyen.TopLevel = false;
            m_frmRoleQuyen.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            m_frmRoleQuyen.ControlBox = false;
            m_frmRoleQuyen.Dock = DockStyle.Fill;
            panPermission.Controls.Add(m_frmRoleQuyen);

            //config child form behavior            
            m_frmRoleMenu.GvMain.FocusedRowChanged += (sender, e) =>
            {
                if (m_frmRoleMenu.GridUtil.DataMode == DataModeType.View)
                {
                    m_frmRoleQuyen.Browse();

                    m_frmRoleMenu.GridUtil.DataMode = m_frmRoleMenu.GridUtil.DataMode;
                }
            };

            m_frmRoleQuyen.GMain.Enter += (sender, e) =>
            {
                sys_RoleMenu focusedRole = m_frmRoleMenu.GvMain.ExGetCurrentDataRow() as sys_RoleMenu;
                m_frmRoleQuyen.m_CurrentMenuID = (focusedRole != null ? focusedRole.MaMenu : 0);
                m_frmRoleQuyen.m_CurrentRoleID = (focusedRole != null ? focusedRole.MaRole : short.MinValue);
                m_frmRoleQuyen.m_frmRole = this;

                this.SetChildFormDeactivate();
                BaseFormFocus = m_frmRoleQuyen;
                m_frmRoleQuyen.SetChildFormActivated();
            };

            m_frmRoleQuyen.GMain.Leave += (sender, e) =>
            {
                m_frmRoleQuyen.SetChildFormDeactivate();
                BaseFormFocus = this;
                this.SetChildFormActivated();
            };

            //just allow edit detail when master in view mode only.
            m_frmRoleQuyen.GridUtil.DataModeChanged += (modType) =>
            {
                this.GMain.Enabled = (modType == DataModeType.View);
                m_frmRoleMenu.GMain.Enabled = (modType == DataModeType.View);
            };
        }

        private void ConfigureControls()
        {
            cboMenu.Properties.ReadOnly = true;
            cboPermission.Properties.ReadOnly = true;
            btnMenuQuickAdd.Enabled = false;
            btnPermissionQuickAdd.Enabled = false;

            sys_MenuBusiness sysMenuB = new sys_MenuBusiness();
            List<sys_Menu> results = new List<sys_Menu>();
            List<sys_Menu> ListMenuChild = new List<sys_Menu>();

            List<sys_Menu> ListMenuParent = sysMenuB.Get(t => (t.MenuCha ?? 0) == 0 && (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                .OrderBy(t => t.SoThuTu).ToList();

            foreach (sys_Menu menuParent in ListMenuParent)
            {
                results.Add(menuParent);

                ListMenuChild = sysMenuB.Get(t => (t.MenuCha ?? 0) == menuParent.Ma && (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderBy(t => t.SoThuTu).ToList();

                foreach (sys_Menu menuChild in ListMenuChild)
                    results.Add(menuChild);
            }

            var Menus = results.ToList();

            cboMenu.Properties.DropDownRows = 20;
            cboMenu.Properties.DataSource = Menus;
            cboMenu.Properties.ValueMember = "Ma";
            cboMenu.Properties.DisplayMember = "Ten";

            var Permissions = new Base<sys_Quyen>().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false)).OrderBy(t => t.Ten).ToList();
            cboPermission.Properties.DropDownRows = 20;
            cboPermission.Properties.DataSource = Permissions;
            cboPermission.Properties.ValueMember = "Ma";
            cboPermission.Properties.DisplayMember = "Ten";

            m_frmRoleMenu.GridUtil.DataModeChanged += (ModeType) =>
            {
                bool enableControls = ModeType == DataModeType.AddNew || ModeType == DataModeType.Edit;

                cboMenu.Properties.ReadOnly = !enableControls;
                btnMenuQuickAdd.Enabled = enableControls;

                if (ModeType == DataModeType.View)
                {
                    cboMenu.EditValue = null;

                    //clear Permission checked combobox editor
                    for (int i = 0; i < cboMenu.Properties.Items.Count; i++)
                    {
                        cboMenu.Properties.Items[i].CheckState = CheckState.Unchecked;
                    }
                }
            };

            m_frmRoleQuyen.GridUtil.DataModeChanged += (ModeType) =>
            {
                bool enableControls = ModeType == DataModeType.AddNew || ModeType == DataModeType.Edit;

                cboPermission.Properties.ReadOnly = !enableControls;
                btnPermissionQuickAdd.Enabled = enableControls;

                if (ModeType == DataModeType.View)
                {
                    cboPermission.EditValue = null;

                    //clear Permission checked combobox editor
                    for (int i = 0; i < cboPermission.Properties.Items.Count; i++)
                    {
                        cboPermission.Properties.Items[i].CheckState = CheckState.Unchecked;
                    }
                }
            };
        }

        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            m_frmRoleMenu._parent = this._parent;
            m_frmRoleMenu.Show();

            //..
            m_frmRoleQuyen._parent = this._parent;
            m_frmRoleQuyen.Show();

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvMain,QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }

        protected override void RaiseEventToChild(ButtonAction action)
        {
            //make sure toolbar mode use for master when grid control focused only
            if (GMain.Focused || !m_frmRoleMenu.GMain.Enabled || !m_frmRoleQuyen.GMain.Enabled)
                base.RaiseEventToChild(action);
        }

        public override void Browse()
        {
            base.Browse();

            m_frmRoleMenu.Browse();
            m_frmRoleQuyen.Browse();

            //..
            GridUtil.DataMode = GridUtil.DataMode;
        }

        public override void Delete()
        {
            UpdateDelete(clsPublicVar.FieldUpdateDelete());
        }

        private void btnMenuQuickAdd_Click(object sender, EventArgs e)
        {
            if (cboMenu.EditValue != null)
            {
                sys_RoleMenuBusiness sysRoleMenuB = new sys_RoleMenuBusiness();
                List<sys_RoleMenu> lRoleMenu = (m_frmRoleMenu.GvMain.GridControl.DataSource as IQueryable<sys_RoleMenu>).ToList();
                string[] Menu = Cast.ToString(cboMenu.EditValue).Split(';');

                sys_Role drR = GvMain.ExGetCurrentDataRow() as sys_Role;

                if (drR != null)
                {
                    foreach (string m in Menu)
                    {
                        if (!lRoleMenu.Any(t => t.MaMenu == Cast.ToInt(m.Trim().TrimStart()) && t.MaRole == drR.Ma))
                        {
                            sys_RoleMenu rolemenu = new sys_RoleMenu()
                            {
                                MaRole = drR.Ma,
                                MaMenu = Cast.ToInt(m.Trim().TrimStart()),
                                ChoPhepTruyCap = true,
                                TrangThai = true,
                                NguoiTao = clsPublicVar.User.Ma,
                                NgayTao = DataUtils.GetDate()
                            };
                            sysRoleMenuB.Add(rolemenu, true);
                        }
                    }
                }

                m_frmRoleMenu.Browse();
                m_frmRoleQuyen.Browse();
            }
        }

        private void btnPermissionQuickAdd_Click(object sender, EventArgs e)
        {
            if (cboPermission.EditValue != null)
            {
                sys_RoleQuyenBusiness sysRolePermissionB = new sys_RoleQuyenBusiness();
                List<sys_RoleQuyen> lRolePermission = (m_frmRoleQuyen.GvMain.GridControl.DataSource as IQueryable<sys_RoleQuyen>).ToList();
                int menu = 0;
                int role = 0;

                sys_RoleMenu drRM = m_frmRoleMenu.GvMain.ExGetCurrentDataRow() as sys_RoleMenu;

                if (drRM != null)
                {
                    menu = drRM.MaMenu;
                    role = drRM.MaRole;

                    string[] permissions = Cast.ToString(cboPermission.EditValue).Split(';');
                    foreach (string p in permissions)
                    {
                        short per = Cast.ToShort(p);
                        if (!lRolePermission.Any(t => t.MaMenu == menu && t.MaRole == role && t.MaQuyen == per))
                        {
                            sys_RoleQuyen rolepermission = new sys_RoleQuyen()
                            {
                                MaRole = role,
                                MaMenu = menu,
                                MaQuyen = per,
                                TrangThai = true,
                                NguoiTao = clsPublicVar.User.Ma,
                                NgayTao = DataUtils.GetDate()
                            };
                            sysRolePermissionB.Add(rolepermission, true);
                        }
                    }
                }

                m_frmRoleQuyen.Browse();
            }
        }
    }
}
