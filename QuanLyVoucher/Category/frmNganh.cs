﻿using Base.AppliBase;
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
    public partial class frmNganh : BaseFormOnGrid<tbl_Nganh, tbl_NganhBusiness, QLVCDataContext>
    {
        public int m_MaChiNhanh { get; set; }
        private readonly int m_MenuID = clsPublicVar.MainMenuID;

        public frmNganh()
        {
            InitializeComponent();

            //..
            SetActionForForm.SetAction(this, m_MenuID, clsPublicVar.QuyenCuaUser);

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cTen", "Tên", "Ten", false, 100),
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
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập tên ngành", ""));

            //Validate primary key           
            Func<tbl_Nganh, bool> FuExist = dr => _dExec.Exist(dr, new string[] { "MaChiNhanh", "Ten" }, clsPublicVar.FieldCheckDelete);
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("Ten", "Ngành đã tồn tại", ""
                , dr => GridUtil.DataMode == DataModeType.AddNew
                    && FuExist.Invoke(new tbl_Nganh
                    {
                        MaChiNhanh = m_MaChiNhanh,
                        Ten = ((tbl_Nganh)dr).Ten
                    })));

            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("Ten", "Ngành đã tồn tại", "", dr =>
            {
                bool valid = true;

                if (dr != null && GridUtil.DataMode == DataModeType.Edit)
                {
                    tbl_Nganh drN = new Base<tbl_Nganh>().First(t => t.Ten == ((tbl_Nganh)dr).Ten && !(t.DaXoa ?? false)
                        && t.MaChiNhanh == ((tbl_Nganh)dr).MaChiNhanh && t.Ma != ((tbl_Nganh)dr).Ma);

                    if (drN != null)
                        valid = false;
                }
                return valid;
            }));

            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<sys_UserRole>("Ma", "MaNganh", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_VoucherRole>("Ma", "MaNganh", false, clsPublicVar.FieldCheckDelete);
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
