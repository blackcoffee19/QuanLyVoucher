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
using Base.AppliBase;
using QuanLyVoucher.Globals;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using Tool;
using Common.Globals;
using Base.AppliBaseForm.Globals;
using DevExpress.Utils;

namespace QuanLyVoucher.Admin
{
    public partial class frmMenu : BaseFormOnGrid<sys_Menu, sys_MenuBusiness, QLVCDataContext>
    {
        public frmMenu()
        {
            InitializeComponent();

            //..
            FunctionLoadData = "GetData";

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cMenuCha", "Menu cha", "MenuCha", false, 40),
                                new DataFieldColumn("cCode", "Mã menu", "Code", true, 60),
                                new DataFieldColumn("cTen", "Tên menu", "Ten", false, 90),
                                new DataFieldColumn("cDuongDan", "Đường dẫn", "DuongDan", false, 150),
                                new DataFieldColumn("cMenuCon", "Con của menu", "MenuCon", false, 70),
                                new DataFieldColumn("cIcon", "Icon", "Icon", false, 40),
                                new DataFieldColumn("cSoThuTu", "Số thứ tự", "SoThuTu", false, 20),
                                new DataFieldColumn("cThamSo", "Tham số", "ThamSo", false, 50),
                                new DataFieldColumn("cHienMenu", "Hiện menu", "HienMenu", false, 30),
                                new DataFieldColumn("cLuonLuonHien", "Luôn hiển thị", "LuonLuonHien", false, 30),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 30));

            //..specify field what use combobox
            var cboMenu = GvMain.ExSetSearchColumnCombobox("MenuCha", "Ten", "Ma", new Base<sys_Menu>().Get(t => (t.MenuCha ?? 0) == 0
                && (t.HienMenu ?? false)).ToList(), 300, 350
                , new LookUpColumnInfo("Code", 50, "Mã menu")
                , new LookUpColumnInfo("Ten", 100, "Tên menu"));

            cboMenu.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new Base<sys_Menu>().Get(t => (t.MenuCha ?? 0) == 0
                    && (t.HienMenu ?? false) && (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboMenu.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new Base<sys_Menu>().Get(t => (t.MenuCha ?? 0) == 0
                    && (t.HienMenu ?? false)).ToList();
            };

            var cboMenuChild = GvMain.ExSetSearchColumnCombobox("MenuCon", "Ten", "Ma", new Base<sys_Menu>().Get(t => (t.MenuCha ?? 0) != 0
                && (t.HienMenu ?? false)).ToList(), 300, 350
                , new LookUpColumnInfo("Code", 50, "Mã menu")
                , new LookUpColumnInfo("Ten", 100, "Tên menu"));

            cboMenuChild.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new Base<sys_Menu>().Get(t => (t.MenuCha ?? 0) != 0
                    && (t.HienMenu ?? false) && (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboMenuChild.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new Base<sys_Menu>().Get(t => (t.MenuCha ?? 0) != 0
                    && (t.HienMenu ?? false)).ToList();
            };

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..set auto values
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiTao", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgayTao", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiSua", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgaySua", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("TrangThai", true, dr => GridUtil.DataMode == DataModeType.AddNew));

            //..set validate
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Code", "Bạn chưa nhập mã menu", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("Code", "Mã menu đã tồn tại", "", AutoGenExistedOnAddNew("Code", clsPublicVar.FieldCheckDelete)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập tên menu", ""));
            //GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("AssemplyName", "Bạn chưa nhập assemply", ""));
            //GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Icon", "Bạn chưa nhập icon", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("SoThuTu", "Bạn chưa nhập số thứ tự", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("SoThuTu", "Số thứ tự phải lớn hơn 0", "",
                dr => (dr != null && ((sys_Menu)dr).SoThuTu > 0) ? true : false));

            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<sys_RoleMenu>("Ma", "MaMenu", false, clsPublicVar.FieldCheckDelete);
            SetChildren<sys_RoleQuyen>("Ma", "MaMenu", false, clsPublicVar.FieldCheckDelete);
        }

        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvMain, QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }

        public override void Delete()
        {
            UpdateDelete(clsPublicVar.FieldUpdateDelete());
        }
    }
}
