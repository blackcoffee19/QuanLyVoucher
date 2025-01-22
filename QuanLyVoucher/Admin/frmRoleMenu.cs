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
using Base.AppliBaseForm.Globals;
using Business.Globals;

namespace QuanLyVoucher.Admin
{
    public partial class frmRoleMenu : BaseFormOnGrid<sys_RoleMenu, sys_RoleMenuBusiness, QLVCDataContext>
    {
        public int m_CurrentRoleID { get; set; }
        private readonly int m_MenuID = clsPublicVar.MainMenuID;
        public frmRole m_frmRole { get; set; }

        public frmRoleMenu()
        {
            InitializeComponent();

            //..
            SetActionForForm.SetAction(this, m_MenuID, clsPublicVar.QuyenCuaUser);

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cMaMenu", "Menu", "MaMenu", true, 90),
                                new DataFieldColumn("cChoPhepTruyCap", "Cho phép truy cập", "ChoPhepTruyCap", false, 30),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 30));

            //..specify field what use combobox
            var cboMenu = GvMain.ExSetSearchColumnCombobox("MaMenu", "Ten", "Ma", new Base<sys_Menu>().Get().ToList(), 400, 350
                , new LookUpColumnInfo("Ten", 300, "Tên menu")
                , new LookUpColumnInfo("HienMenu", 50, "Hiện thị menu"));

            cboMenu.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new sys_MenuBusiness().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboMenu.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                cboEdit.Properties.DataSource = new sys_MenuBusiness().Get().ToList();
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
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("ChoPhepTruyCap", true, dr => GridUtil.DataMode == DataModeType.AddNew));

            //..set validate
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("MaMenu", "Bạn chưa chọn menu", ""));

            //Validate primary key           
            Func<sys_RoleMenu, bool> FuExist = dr => _dExec.Exist(dr, new string[] { "MaRole", "MaMenu" }, clsPublicVar.FieldCheckDelete);
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("MaMenu", "Menu đã tồn tại", ""
                , dr => GridUtil.DataMode == DataModeType.AddNew
                    && FuExist.Invoke(new sys_RoleMenu
                    {
                        MaRole = m_CurrentRoleID,
                        MaMenu = ((sys_RoleMenu)dr).MaMenu
                    })));

            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<sys_RoleQuyen>("MaRole,MaMenu", "MaRole,MaMenu", false, clsPublicVar.FieldCheckDelete);
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
