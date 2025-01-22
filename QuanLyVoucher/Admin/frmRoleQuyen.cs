using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;
using Business;
using Base.AppliBaseForm;
using Base.DevExpressEx.Utility;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using Base.Common;
using Base.AppliBase;
using QuanLyVoucher.Globals;
using Tool;
using Common.Globals;
using DevExpress.Utils;
using Base.AppliBaseForm.Globals;
using Business.Globals;

namespace QuanLyVoucher.Admin
{
    public partial class frmRoleQuyen : BaseFormOnGrid<sys_RoleQuyen, sys_RoleQuyenBusiness, QLVCDataContext>
    {
        public int m_CurrentRoleID { get; set; }
        public int m_CurrentMenuID { get; set; }
        private readonly int m_MenuID = clsPublicVar.MainMenuID;
        public frmRole m_frmRole { get; set; }

        public frmRoleQuyen()
        {
            InitializeComponent();

            //..
            SetActionForForm.SetAction(this, m_MenuID, clsPublicVar.QuyenCuaUser);

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cMaQuyen", "Quyền", "MaQuyen", true, 50),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 30));

            //..specify field what use combobox
            var cboPermission = GvMain.ExSetSearchColumnCombobox("MaQuyen", "Ten", "Ma", new Base<sys_Quyen>().Get().ToList(), 350, 350
                , new LookUpColumnInfo("Ten", 70, "Tên quyền")
                , new LookUpColumnInfo("MoTa", 100, "Mô tả"));

            cboPermission.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new Base<sys_Quyen>().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboPermission.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new Base<sys_Quyen>().Get().ToList();
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
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("MaQuyen", "Bạn chưa chọn quyền", "",
                dr => (dr != null && ((sys_RoleQuyen)dr).MaQuyen > 0) ? true : false));

            //Validate primary key           
            Func<sys_RoleQuyen, bool> FuExist = dr => _dExec.Exist(dr, new string[] { "MaRole", "MaMenu", "MaQuyen" }, clsPublicVar.FieldCheckDelete);
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("MaQuyen", "Quyền đã tồn tại", ""
                , dr => GridUtil.DataMode == DataModeType.AddNew
                    && FuExist.Invoke(new sys_RoleQuyen
                    {
                        MaRole = m_CurrentRoleID,
                        MaMenu = m_CurrentMenuID,
                        MaQuyen = ((sys_RoleQuyen)dr).MaQuyen
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

        public override void Add()
        {
            if (m_CurrentRoleID == (int)Role.Admin)
                base.Add();
            else
            {
                if (m_CurrentRoleID > 0)
                {
                    new frmPhanQuyen(m_CurrentRoleID).ShowDialog();
                    m_frmRole.Browse();
                    SetChildFormActivated();
                }
                else
                    SetFailure("Bạn vui lòng chọn Role cần phân quyền");
            }
        }

        public override void Edit()
        {
            if (m_CurrentRoleID == (int)Role.Admin)
                base.Edit();
            else
            {
                if (m_CurrentRoleID > 0)
                {
                    new frmPhanQuyen(m_CurrentRoleID).ShowDialog();
                    m_frmRole.Browse();
                    SetChildFormActivated();
                }
                else
                    SetFailure("Bạn vui lòng chọn Role cần phân quyền");
            }
        }

        public override void Delete()
        {
            if (m_CurrentRoleID == (int)Role.Admin)
                UpdateDelete(clsPublicVar.FieldUpdateDelete());
            else
                SetFailure("Chỉ duy nhất role Admin mới có chức năng này");
        }
    }
}
