using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
using Base.Common;
using Base.DevExpressEx.Extension;
using Base.DevExpressEx.Utility;
using Business;
using Business.Globals;
using Common.Globals;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using QuanLyVoucher.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;

namespace QuanLyVoucher.Admin
{
    public partial class frmPhanQuyen : BaseForm
    {
        private GridControl GRoleMenu = new GridControl();
        private GridViewEx GvRoleMenu = new GridViewEx();
        private GridControl GRoleQuyen = new GridControl();
        private GridViewEx GvRoleQuyen = new GridViewEx();
        private int m_MaRole = 0;
        private List<sp_DanhSachRoleMenuResult> ListRoleMenu = new List<sp_DanhSachRoleMenuResult>();
        private List<sp_DanhSachRoleQuyenResult> ListRoleQuyen = new List<sp_DanhSachRoleQuyenResult>();

        public frmPhanQuyen()
        {
            InitializeComponent();
        }

        public frmPhanQuyen(int MaRole)
        {
            InitializeComponent();

            //..
            m_MaRole = MaRole;

            //..
            ConfigureRoleMenu();
            ConfigureRoleQuyen();
        }

        private void ConfigureRoleMenu()
        {
            //..
            ActionsDeny.AddRange(new[] { ButtonAction.Add, ButtonAction.Delete });

            //Load data
            ListRoleMenu = new sys_RoleMenuBusiness().GetFollowRole4PhanQuyen(m_MaRole);
            ListRoleQuyen = new sys_RoleQuyenBusiness().GetFollowRoleMenu4PhanQuyen(m_MaRole);
            GRoleMenu.DataSource = ListRoleMenu;

            //..
            GvRoleMenu.ConfigDefault();
            GvRoleMenu.OptionsBehavior.Editable = true;

            GRoleMenu.MainView = GvRoleMenu;
            GRoleMenu.ViewCollection.AddRange(new BaseView[] { GvRoleMenu });
            GvRoleMenu.GridControl = GRoleMenu;
            panMenu.Controls.Add(GRoleMenu);
            GRoleMenu.Dock = DockStyle.Fill;

            GRoleMenu.KeyDown += (sender, e) =>
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    if (GvRoleMenu.GetRowCellValue(GvRoleMenu.FocusedRowHandle, GvRoleMenu.FocusedColumn) != null
                        && (!string.IsNullOrEmpty(GvRoleMenu.GetRowCellValue(GvRoleMenu.FocusedRowHandle, GvRoleMenu.FocusedColumn).ToString())))
                    {
                        Clipboard.SetText(GvRoleMenu.GetFocusedDisplayText());
                        e.Handled = true;
                    }
                    else
                    {
                        Clipboard.SetText(" ");
                        e.Handled = true;
                    }
                }
            };

            //..define fields what showed on grid
            GvRoleMenu.ExAddColumns(new DataFieldColumn("cTenMenu", "Menu", "TenMenu", true, 90),
                                    new DataFieldColumn("cChoPhepTruyCap", "Cho phép truy cập", "ChoPhepTruyCap", false, 30),
                                    new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 30));

            //..
            GvRoleMenu.ViewRowNumber = true;

            //..
            GvRoleMenu.Columns["TenMenu"].OptionsColumn.AllowEdit = false;

            //..
            GvRoleMenu.FocusedRowChanged += (sender, e) => LoadQuyen();

            //..
            GvRoleMenu.CellValueChanged += (sender, e) =>
            {
                GvRoleMenu.PostEditor();

                sp_DanhSachRoleMenuResult dr = GvRoleMenu.ExGetCurrentDataRow() as sp_DanhSachRoleMenuResult;
                if (dr != null)
                {
                    sp_DanhSachRoleMenuResult item = ListRoleMenu.FirstOrDefault(t => t.MaMenu == dr.MaMenu);
                    if (item != null)
                    {
                        item.ChoPhepTruyCap = dr.ChoPhepTruyCap;
                        item.TrangThai = dr.TrangThai;
                        item.Modified = true;
                    }
                }
            };
        }

        private void ConfigureRoleQuyen()
        {
            //..
            ActionsDeny.AddRange(new[] { ButtonAction.Add, ButtonAction.Delete });

            //..
            GvRoleQuyen.ConfigDefault();
            GvRoleQuyen.OptionsBehavior.Editable = true;

            GRoleQuyen.MainView = GvRoleQuyen;
            GRoleQuyen.ViewCollection.AddRange(new BaseView[] { GvRoleQuyen });
            GvRoleQuyen.GridControl = GRoleQuyen;
            panQuyen.Controls.Add(GRoleQuyen);
            GRoleQuyen.Dock = DockStyle.Fill;

            GRoleQuyen.KeyDown += (sender, e) =>
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    if (GvRoleQuyen.GetRowCellValue(GvRoleQuyen.FocusedRowHandle, GvRoleQuyen.FocusedColumn) != null
                        && (!string.IsNullOrEmpty(GvRoleQuyen.GetRowCellValue(GvRoleQuyen.FocusedRowHandle, GvRoleQuyen.FocusedColumn).ToString())))
                    {
                        Clipboard.SetText(GvRoleQuyen.GetFocusedDisplayText());
                        e.Handled = true;
                    }
                    else
                    {
                        Clipboard.SetText(" ");
                        e.Handled = true;
                    }
                }
            };

            //..define fields what showed on grid
            GvRoleQuyen.ExAddColumns(new DataFieldColumn("cMaQuyen", "Quyền", "MaQuyen", true, 50),
                                     new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 30));

            //..specify field what use combobox
            GvRoleQuyen.ExSetSearchColumnCombobox("MaQuyen", "Ten", "Ma", new Base<sys_Quyen>().Get().ToList(), 350, 350
                , new LookUpColumnInfo("Ten", 70, "Tên quyền")
                , new LookUpColumnInfo("MoTa", 100, "Mô tả"));

            //..
            GvRoleQuyen.ViewRowNumber = true;

            //..
            GvRoleQuyen.Columns["MaQuyen"].OptionsColumn.AllowEdit = false;

            //..
            GvRoleQuyen.CellValueChanged += (sender, e) =>
            {
                GvRoleQuyen.PostEditor();

                sp_DanhSachRoleQuyenResult dr = GvRoleQuyen.ExGetCurrentDataRow() as sp_DanhSachRoleQuyenResult;
                if (dr != null)
                {
                    sp_DanhSachRoleQuyenResult item = ListRoleQuyen.FirstOrDefault(t => t.MaMenu == dr.MaMenu && t.MaQuyen == dr.MaQuyen);
                    if (item != null)
                    {
                        item.TrangThai = dr.TrangThai;
                        item.Modified = true;
                    }
                }
            };
        }

        private void LoadQuyen()
        {
            //Load data
            sp_DanhSachRoleMenuResult dr = GvRoleMenu.ExGetCurrentDataRow() as sp_DanhSachRoleMenuResult;

            if (dr != null)
                GRoleQuyen.DataSource = ListRoleQuyen.Where(t => t.MaMenu == dr.MaMenu).ToList();
            else
                GRoleQuyen.DataSource = null;
        }

        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvRoleMenu, QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (SetConfirm("Bạn thật sự muốn lưu phân quyền này?", "Thông báo") == DialogResult.OK)
            {
                List<sp_DanhSachRoleMenuResult> ListRoleMenuLuu = ListRoleMenu.Where(t => (t.Modified ?? false)).ToList();
                List<sp_DanhSachRoleQuyenResult> ListRoleQuyenLuu = ListRoleQuyen.Where(t => (t.Modified ?? false)).ToList();
                sys_RoleMenuBusiness sys_RoleMenuB = new sys_RoleMenuBusiness();
                sys_RoleQuyenBusiness sys_RoleQuyenB = new sys_RoleQuyenBusiness();

                foreach (sp_DanhSachRoleMenuResult item in ListRoleMenuLuu)
                {
                    if (item.Ma == null)//Them moi
                    {
                        sys_RoleMenu rmAdd = new sys_RoleMenu
                        {
                            MaRole = m_MaRole,
                            MaMenu = item.MaMenu ?? 0,
                            ChoPhepTruyCap = item.ChoPhepTruyCap,
                            TrangThai = item.TrangThai,
                            NguoiTao = clsPublicVar.User.Ma,
                            NgayTao = DataUtils.GetDate()
                        };

                        sys_RoleMenuB.Add(rmAdd, true);
                    }
                    else//Chinh sua
                    {
                        sys_RoleMenu dr = sys_RoleMenuB.First(t => t.Ma == item.Ma);
                        if (dr != null)
                        {
                            dr.ChoPhepTruyCap = item.ChoPhepTruyCap;
                            dr.TrangThai = item.TrangThai;
                            dr.NguoiSua = clsPublicVar.User.Ma;
                            dr.NgaySua = DataUtils.GetDate();

                            sys_RoleMenuB.Update(dr, t => t.Ma == dr.Ma, true);
                        }
                    }
                }

                foreach (sp_DanhSachRoleQuyenResult item in ListRoleQuyenLuu)
                {
                    if (item.Ma == null)//Them moi
                    {
                        sys_RoleQuyen rqAdd = new sys_RoleQuyen
                        {
                            MaRole = m_MaRole,
                            MaMenu = item.MaMenu,
                            MaQuyen = item.MaQuyen,
                            TrangThai = item.TrangThai,
                            NguoiTao = clsPublicVar.User.Ma,
                            NgayTao = DataUtils.GetDate()
                        };

                        sys_RoleQuyenB.Add(rqAdd, true);
                    }
                    else//Chinh sua
                    {
                        sys_RoleQuyen dr = sys_RoleQuyenB.First(t => t.Ma == item.Ma);
                        if (dr != null)
                        {
                            dr.TrangThai = item.TrangThai;
                            dr.NguoiSua = clsPublicVar.User.Ma;
                            dr.NgaySua = DataUtils.GetDate();

                            sys_RoleQuyenB.Update(dr, t => t.Ma == dr.Ma, true);
                        }
                    }
                }
            }
            else
                return;

            //Done
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
