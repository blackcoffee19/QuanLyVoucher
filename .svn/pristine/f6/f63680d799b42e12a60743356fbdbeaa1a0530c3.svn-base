using Base.AppliBase;
using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
using Base.DevExpressEx.Utility;
using Business;
using Business.Globals;
using Common.Globals;
using QuanLyVoucher.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;

namespace QuanLyVoucher.Category
{
    public partial class frmChiNhanh : BaseFormOnGrid<tbl_ChiNhanh, tbl_ChiNhanhBusiness, QLVCDataContext>
    {
        private frmNganh m_frmNganh;
        private readonly int m_MenuID = clsPublicVar.MainMenuID;

        public frmChiNhanh()
        {
            InitializeComponent();

            ConfigureChiNhanh();
            ConfigureNganh();
        }

        private void ConfigureChiNhanh()
        {
            //..
            FunctionLoadData = "GetData";

            //..
            btnChenLogo.Enabled = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID
                && t.MaQuyen == (int)Quyen.ChenHinhAnh);

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cTen", "Tên", "Ten", false, 100),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 80));

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..
            panChiNhanh.Controls.Add(GMain);

            //..set auto values
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiTao", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgayTao", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiSua", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgaySua", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("TrangThai", true, dr => GridUtil.DataMode == DataModeType.AddNew));

            //..set validate
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập tên chi nhánh", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("Ten", "Tên chi nhánh đã tồn tại", "", AutoGenExistedOnAddNew("Ten", clsPublicVar.FieldCheckDelete)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("Ten", "Tên chi nhánh đã tồn tại", "", dr =>
            {
                bool valid = true;

                if (dr != null && GridUtil.DataMode == DataModeType.Edit)
                {
                    tbl_ChiNhanh drCN = new Base<tbl_ChiNhanh>().First(t => t.Ten == ((tbl_ChiNhanh)dr).Ten && !(t.DaXoa ?? false)
                        && t.Ma != ((tbl_ChiNhanh)dr).Ma);

                    if (drCN != null)
                        valid = false;
                }
                return valid;
            }));

            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<sys_UserRole>("Ma", "MaChiNhanh", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_Nganh>("Ma", "MaChiNhanh", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_VoucherRole>("Ma", "MaChiNhanh", false, clsPublicVar.FieldCheckDelete);

            //just allow edit detail when master in view mode only.
            GridUtil.DataModeChanged += (modType) =>
            {
                m_frmNganh.GMain.Enabled = (modType == DataModeType.View);
            };
        }

        private void ConfigureNganh()
        {
            m_frmNganh = new frmNganh() { Name = "frmNganh" };
            m_frmNganh.FunctionLoadData = "GetFollowChiNhanh";
            m_frmNganh.FuLoadDataParas = () =>
            {
                tbl_ChiNhanh focusedChiNhanh = GvMain.ExGetCurrentDataRow() as tbl_ChiNhanh;
                if (focusedChiNhanh != null)
                    return new[] { (object)focusedChiNhanh.Ma };
                return new[] { (object)int.MinValue };
            };

            //..set auto values
            m_frmNganh.GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("MaChiNhanh", dr => ((tbl_ChiNhanh)GvMain.ExGetCurrentDataRow()).Ma));

            //include child form to master form            
            m_frmNganh.TopLevel = false;
            m_frmNganh.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            m_frmNganh.ControlBox = false;
            m_frmNganh.Dock = DockStyle.Fill;
            panNganh.Controls.Add(m_frmNganh);

            //config child form behavior            
            this.GvMain.FocusedRowChanged += (sender, e) =>
            {
                if (this.GridUtil.DataMode == DataModeType.View)
                {
                    m_frmNganh.Browse();

                    this.GridUtil.DataMode = this.GridUtil.DataMode;

                    LoadLogo();
                }
            };

            m_frmNganh.GMain.Enter += (sender, e) =>
            {
                tbl_ChiNhanh focusedChiNhanh = GvMain.ExGetCurrentDataRow() as tbl_ChiNhanh;
                m_frmNganh.m_MaChiNhanh = (focusedChiNhanh != null ? focusedChiNhanh.Ma : short.MinValue);

                this.SetChildFormDeactivate();
                BaseFormFocus = m_frmNganh;
                m_frmNganh.SetChildFormActivated();
            };

            m_frmNganh.GMain.Leave += (sender, e) =>
            {
                m_frmNganh.SetChildFormDeactivate();
                BaseFormFocus = this;
                this.SetChildFormActivated();
            };

            //just allow edit detail when master in view mode only.
            m_frmNganh.GridUtil.DataModeChanged += (modType) =>
            {
                this.GMain.Enabled = (modType == DataModeType.View);
            };
        }

        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            m_frmNganh._parent = this._parent;
            m_frmNganh.Show();

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvMain, QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }

        protected override void RaiseEventToChild(ButtonAction action)
        {
            //make sure toolbar mode use for master when grid control focused only
            if (GMain.Focused || !m_frmNganh.GMain.Enabled)
                base.RaiseEventToChild(action);
        }

        public override void Browse()
        {
            base.Browse();

            m_frmNganh.Browse();

            //..
            GridUtil.DataMode = GridUtil.DataMode;
        }

        public override void Delete()
        {
            UpdateDelete(clsPublicVar.FieldUpdateDelete());
        }

        private void btnChenLogo_Click(object sender, EventArgs e)
        {
            tbl_ChiNhanh drCN = GvMain.ExGetCurrentDataRow() as tbl_ChiNhanh;

            if (drCN != null)
            {
                OpenFileDialog cdlg = new OpenFileDialog();
                cdlg.Multiselect = false;

                if (cdlg.ShowDialog() == DialogResult.OK)
                {
                    FileStream stream = new FileStream(
                        cdlg.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(stream);

                    byte[] photo = reader.ReadBytes((int)stream.Length);

                    drCN.Logo = photo;

                    new Base<tbl_ChiNhanh>().Update(drCN, t => t.Ma == drCN.Ma, true);

                    if (State.Success)
                    {
                        SetSuccess("Thêm thành công");
                        LoadLogo();
                    }
                    else
                        SetFailure("Thêm thất bại");
                }
            }
            else
                SetFailure("Bạn vui lòng chọn chi nhánh bạn muốn thêm");
        }

        private void LoadLogo()
        {
            try
            {
                tbl_ChiNhanh dr = GvMain.ExGetCurrentDataRow() as tbl_ChiNhanh;
                if (dr != null && dr.Logo != null)
                {
                    MemoryStream ms = new MemoryStream(dr.Logo.ToArray(), true);
                    ms.Write(dr.Logo.ToArray(), 0, dr.Logo.ToArray().Length);
                    Image image = Image.FromStream(ms, true);
                    picLogo.Image = image;
                    picLogo.SizeMode = PictureBoxSizeMode.CenterImage;
                    picLogo.BackColor = Color.White;
                }
                else
                {
                    picLogo.Image = QuanLyVoucher.Properties.Resources.no_image;
                    picLogo.SizeMode = PictureBoxSizeMode.CenterImage;
                    picLogo.BackColor = Color.White;
                }
            }
            catch
            {
                picLogo.Image = null;
            }
        }
    }
}
