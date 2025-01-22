using Base.AppliBase;
using Base.AppliBaseForm;
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;

namespace QuanLyVoucher.Category
{
    public partial class frmVoucherRole : BaseFormOnGrid<tbl_VoucherRole, tbl_VoucherRoleBusiness, QLVCDataContext>
    {
        public int m_MaVoucher { get; set; }
        private readonly int m_MenuID = clsPublicVar.MainMenuID;

        public frmVoucherRole()
        {
            InitializeComponent();

            //..
            SetActionForForm.SetAction(this, m_MenuID, clsPublicVar.QuyenCuaUser);

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cMaChiNhanh", "Chi nhánh", "MaChiNhanh", true, 90),
                                new DataFieldColumn("cMaNganh", "Ngành", "MaNganh", true, 90),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 30));

            //..specify field what use combobox
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

                tbl_VoucherRole dr = GvMain.ExGetCurrentDataRow() as tbl_VoucherRole;
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

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..
            GvMain.ViewRowNumber = true;

            //..set auto values
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiTao", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgayTao", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiSua", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgaySua", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("TrangThai", true, dr => GridUtil.DataMode == DataModeType.AddNew));
            
            //..set validate
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("MaChiNhanh", "Bạn chưa chọn chi nhánh", "",
                dr => (dr != null && ((tbl_VoucherRole)dr).MaChiNhanh > 0) ? true : false));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("MaNganh", "Bạn chưa chọn ngành", "",
                dr => (dr != null && ((tbl_VoucherRole)dr).MaNganh > 0) ? true : false));

            //Validate primary key           
            Func<tbl_VoucherRole, bool> FuExist = dr => _dExec.Exist(dr, new string[] { "MaVoucher", "MaChiNhanh", "MaNganh" }, clsPublicVar.FieldCheckDelete);
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("MaChiNhanh", "Chi nhánh - Ngành đã tồn tại", ""
                , dr => GridUtil.DataMode == DataModeType.AddNew
                    && FuExist.Invoke(new tbl_VoucherRole
                    {
                        MaVoucher = m_MaVoucher,
                        MaChiNhanh = ((tbl_VoucherRole)dr).MaChiNhanh,
                        MaNganh = ((tbl_VoucherRole)dr).MaNganh
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
