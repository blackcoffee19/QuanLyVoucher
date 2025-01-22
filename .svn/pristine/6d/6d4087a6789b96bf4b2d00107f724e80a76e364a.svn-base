using Base.AppliBase;
using Base.AppliBaseForm;
using Base.DevExpressEx.Utility;
using Business;
using Common.Globals;
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
    public partial class frmNhomVoucher : BaseFormOnGrid<tbl_NhomVoucher, tbl_NhomVoucherBusiness, QLVCDataContext>
    {
        public frmNhomVoucher()
        {
            InitializeComponent();

            //..
            FunctionLoadData = "GetData";

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cTen", "Tên nhóm", "Ten", false, 100),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 80));

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
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập tên nhóm", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("Ten", "Tên nhóm đã tồn tại", "", AutoGenExistedOnAddNew("Ten", clsPublicVar.FieldCheckDelete)));

            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<tbl_Voucher>("Ma", "NhomVoucherID", false, clsPublicVar.FieldCheckDelete);
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
