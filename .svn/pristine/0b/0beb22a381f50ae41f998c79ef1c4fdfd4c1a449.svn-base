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
using DevExpress.XtraEditors;
using Base.AppliBase;
using QuanLyVoucher.Globals;
using Tool;
using Common.Globals;
using DevExpress.Utils;

namespace QuanLyVoucher.Admin
{
    public partial class frmUserRole : BaseFormOnGrid<sys_UserRole, sys_UserRoleBusiness, QLVCDataContext>
    {
        public int m_CurrentUserID { get; set; }
        private readonly int m_MenuID = clsPublicVar.MainMenuID;

        public frmUserRole()
        {
            InitializeComponent();

            //..
            SetActionForForm.SetAction(this, m_MenuID, clsPublicVar.QuyenCuaUser);

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cMaRole", "Role", "MaRole", true, 70),
                                new DataFieldColumn("cMaChiNhanh", "Chi nhánh", "MaChiNhanh", true, 70),
                                new DataFieldColumn("cMaNganh", "Ngành", "MaNganh", true, 70),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 10));

            //..specify field what use combobox
            var cboRole = GvMain.ExSetSearchColumnCombobox("MaRole", "Ten", "Ma", new sys_RoleBusiness().Get().ToList(), 350, 350
                , new LookUpColumnInfo("Code", 50, "Mã role")
                , new LookUpColumnInfo("Ten", 100, "Tên role"));

            cboRole.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new sys_RoleBusiness().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboRole.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new sys_RoleBusiness().Get().ToList();
            };

            var cboChiNhanh = GvMain.ExSetSearchColumnCombobox("MaChiNhanh", "Ten", "Ma", new tbl_ChiNhanhBusiness().Get().ToList(), 350, 350
                , new LookUpColumnInfo("Ten", 100, "Tên chi nhánh"));

            cboChiNhanh.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new tbl_ChiNhanhBusiness().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboChiNhanh.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new tbl_ChiNhanhBusiness().Get().ToList();
            };

            var cboNganh = GvMain.ExSetSearchColumnCombobox("MaNganh", "Ten", "Ma", new tbl_NganhBusiness().Get().ToList(), 350, 350
                , new LookUpColumnInfo("Ten", 100, "Tên ngành"));

            cboNganh.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                int iMaChiNhanh = 0;

                sys_UserRole dr = GvMain.ExGetCurrentDataRow() as sys_UserRole;
                if (dr != null)
                    iMaChiNhanh = dr.MaChiNhanh;
                
                cboEdit.Properties.DataSource = new tbl_NganhBusiness().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false)
                    && (!GridUtil._view.IsFilterRow(GvMain.FocusedRowHandle) ? t.MaChiNhanh == iMaChiNhanh : true))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboNganh.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new tbl_NganhBusiness().Get().ToList();
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
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("MaRole", "Bạn chưa chọn role", "",
                dr => (dr != null && ((sys_UserRole)dr).MaRole > 0) ? true : false));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("MaChiNhanh", "Bạn chưa chọn chi nhánh", "",
                dr => (dr != null && ((sys_UserRole)dr).MaChiNhanh > 0) ? true : false));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("MaNganh", "Bạn chưa chọn ngành", "",
                dr => (dr != null && ((sys_UserRole)dr).MaNganh > 0) ? true : false));

            //Validate primary key
            Func<sys_UserRole, bool> FuExist = dr => _dExec.Exist(dr, new string[] { "MaUser", "MaRole", "MaChiNhanh", "MaNganh" }, clsPublicVar.FieldCheckDelete);
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("MaRole", "Role, chi nhánh và ngành đã tồn tại", ""
                , dr => GridUtil.DataMode == DataModeType.AddNew
                    && FuExist.Invoke(new sys_UserRole
                    {
                        MaUser  = m_CurrentUserID,
                        MaRole = ((sys_UserRole)dr).MaRole,
                        MaChiNhanh = ((sys_UserRole)dr).MaChiNhanh,
                        MaNganh = ((sys_UserRole)dr).MaNganh
                    })));

            //..define delete
            AutoGenFunctionDelete();
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
