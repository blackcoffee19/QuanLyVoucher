using Base.AppliBase;
using Base.AppliBaseForm;
using Base.Common;
using Base.Common.Items;
using Base.DevExpressEx.Utility;
using Business;
using Business.Globals;
using Common.Globals;
using DevExpress.XtraEditors.Controls;
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
    public partial class frmKhachHang : BaseFormOnGrid<tbl_KhachHang, tbl_KhachHangBusiness, QLVCDataContext>
    {
        private readonly int m_MenuID = clsPublicVar.MainMenuID;

        public frmKhachHang()
        {
            InitializeComponent();

            //..
            ConfigSearchBox();

            btnChenHinh.Enabled = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID
                && t.MaQuyen == (int)Quyen.ChenHinhAnh);
            //..
            FunctionLoadData = "GetData";
            FuLoadDataParas = () => new[]
            {
                (object)uSearch.GetValue("dptTuNgay"),
                (object)uSearch.GetValue("dptDenNgay"),
                (object)uSearch.GetValue("txtHoTen"),
                (object)uSearch.GetValue("txtSoDienThoai"),
                (object)uSearch.GetValue("txtCMND"),
                (object)uSearch.GetValue("txtDiaChi")
            };

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cTen", "Họ và tên", "Ten", false, 150),
                                new DataFieldColumn("cCMND", "CMND", "CMND", false, 50),
                                new DataFieldColumn("cDienThoai", "Điện thoại", "DienThoai", false, 50),
                                new DataFieldColumn("cGioiTinhEx", "Giới tính", "GioiTinhEx", false, 50),
                                new DataFieldColumn("cDiaChi", "Địa chỉ", "DiaChi", false, 150),
                                new DataFieldColumn("cEmail", "Email", "Email", false, 100),
                                new DataFieldColumn("cGhichu", "Ghi chú", "Ghichu", false, 150),
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 30));

            //..specify field what use combobox
            List<BasicItem> lGioiTinh = new List<BasicItem>(new[]
            {
                new BasicItem("Nam", Enum.GetName(typeof(GioiTinh), GioiTinh.Nam)),
                new BasicItem("Nữ", Enum.GetName(typeof(GioiTinh), GioiTinh.Nu))
            });
            GvMain.ExSetSearchColumnCombobox("GioiTinhEx", "Text", "Value", lGioiTinh.ToList(), 350, 350
                , new LookUpColumnInfo("Text", 100, "Giới tính"));

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..
            panKhachHang.Controls.Add(GMain);

            //..set auto values
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiTao", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgayTao", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiSua", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgaySua", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("TrangThai", true, dr => GridUtil.DataMode == DataModeType.AddNew));

            //..set validate
            //GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("CMND", "Bạn chưa nhập số CMND", ""));
            //GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("CMND", "Số CMND đã tồn tại", "", AutoGenExistedOnAddNew("CMND", clsPublicVar.FieldCheckDelete)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("DienThoai", "Bạn chưa nhập số điện thoại", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("DienThoai", "Số điện thoại đã tồn tại", "", AutoGenExistedOnAddNew("DienThoai", clsPublicVar.FieldCheckDelete)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập họ và tên", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("GioiTinhEx", "Bạn chưa chọn giới tính", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("Email", "Email không đúng định dạng", "", dr => Utility.IsEmail(((tbl_KhachHang)dr).Email, true)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("DienThoai", "Số điện thoại đã tồn tại", "", dr =>
            {
                bool valid = true;

                if (dr != null && GridUtil.DataMode == DataModeType.Edit)
                {
                    tbl_KhachHang drKH = new Base<tbl_KhachHang>().First(t => t.DienThoai == ((tbl_KhachHang)dr).DienThoai
                        && !(t.DaXoa ?? false) && t.Ma != ((tbl_KhachHang)dr).Ma);

                    if (drKH != null)
                        valid = false;
                }
                return valid;
            }));

            //..define delete
            AutoGenFunctionDelete();

            //config child form behavior            
            this.GvMain.FocusedRowChanged += (sender, e) =>
            {
                if (this.GridUtil.DataMode == DataModeType.View)
                {                  

                    this.GridUtil.DataMode = this.GridUtil.DataMode;

                    LoadHinh();
                }
            };

            //.x. set children
            SetChildren<tbl_BanHang>("Ma", "MaKH", false, clsPublicVar.FieldCheckDelete);
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

        private void LoadHinh()
        {
            try
            {
                tbl_KhachHang dr = GvMain.ExGetCurrentDataRow() as tbl_KhachHang;
                if (dr != null && dr.Hinh != null)
                {
                    MemoryStream ms = new MemoryStream(dr.Hinh.ToArray(), true);
                    ms.Write(dr.Hinh.ToArray(), 0, dr.Hinh.ToArray().Length);
                    Image image = Image.FromStream(ms, true);
                    picHinh.Image = image;
                    picHinh.SizeMode = PictureBoxSizeMode.Zoom;
                    picHinh.BackColor = Color.White;
                }
                else
                {
                    picHinh.Image = QuanLyVoucher.Properties.Resources.no_image;
                    picHinh.SizeMode = PictureBoxSizeMode.Zoom;
                    picHinh.BackColor = Color.White;
                }
            }
            catch
            {
                picHinh.Image = null;
            }
        }  

        private void btnChenHinh_Click(object sender, EventArgs e)
        {
            tbl_KhachHang drKH = GvMain.ExGetCurrentDataRow() as tbl_KhachHang;

            if (drKH != null)
            {
                OpenFileDialog cdlg = new OpenFileDialog();
                cdlg.Multiselect = false;

                if (cdlg.ShowDialog() == DialogResult.OK)
                {
                    FileStream stream = new FileStream(cdlg.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(stream);

                    byte[] photo = reader.ReadBytes((int)stream.Length);

                    drKH.Hinh = photo;

                    new Base<tbl_KhachHang>().Update(drKH, t => t.Ma == drKH.Ma, true);

                    if (State.Success)
                    {
                        SetSuccess("Thêm thành công");
                        LoadHinh();
                    }
                    else
                        SetFailure("Thêm thất bại");
                }
            }
            else
                SetFailure("Bạn vui lòng chọn chi nhánh bạn muốn thêm");
        }
    }
}
