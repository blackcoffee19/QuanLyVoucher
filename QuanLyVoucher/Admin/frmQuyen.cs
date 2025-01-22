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
using Base.AppliBaseForm.Globals;
using Tool;
using Common.Globals;
using DevExpress.Utils;

namespace QuanLyVoucher.Admin
{
    public partial class frmQuyen : BaseFormOnGrid<sys_Quyen, sys_QuyenBusiness, QLVCDataContext>
    {
        public frmQuyen()
        {
            InitializeComponent();

            //..
            FunctionLoadData = "GetData";
            
            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cMa", "ID", "Ma", true, 10) { _isReadOnly = true },
                                new DataFieldColumn("cCode", "Mã quyền", "Code", true, 30),
                                new DataFieldColumn("cTen", "Tên quyền", "Ten", false, 50),
                                new DataFieldColumn("cTruyCapForm", "Truy cập form", "TruyCapForm", false, 20),
                                new DataFieldColumn("cMoTa", "Mô tả", "MoTa", false, 150),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 20));

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
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Code", "Bạn chưa nhập mã quyền", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("Code", "Mã quyền đã tồn tại", "", AutoGenExistedOnAddNew("Code", clsPublicVar.FieldCheckDelete)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập tên quyền", ""));
            
            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<sys_RoleQuyen>("Ma", "MaQuyen", false, clsPublicVar.FieldCheckDelete);
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
