using Base.AppliBase;
using Base.AppliBase.Items;
using Base.AppliBaseForm;
using Base.Common;
using Base.Common.Items;
using Base.DevExpressEx.Utility;
using Business;
using Business.Globals;
using Common.Globals;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using QuanLyVoucher.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;

namespace QuanLyVoucher.Transaction
{
    public partial class frmBanHang : BaseFormOnControls<vBanHang, vBanHangBusiness, QLVCDataContext>
    {
        private tbl_VoucherBusiness tbl_VoucherB = new tbl_VoucherBusiness();
        private readonly int m_MenuID = clsPublicVar.MainMenuID;
        private bool bKichHoat = false, bHuyKichHoat = false, bLuuVaInHoaDon = false;

        public frmBanHang()
        {
            InitializeComponent();
            
            //..
            ConfigSearchBox();

            //..
            bKichHoat = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID && t.MaQuyen == (int)Quyen.KichHoat);
            bHuyKichHoat = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID && t.MaQuyen == (int)Quyen.HuyKichHoat);
            bLuuVaInHoaDon = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID && t.MaQuyen == (int)Quyen.LuuVaInHoaDon);

            //..
            cboDanhSachBH.Build("TenVoucher", "Ma", new Base<vBanHang>().Get().ToList(), 350, 300
                , new LookUpColumnInfo("TenVoucher", 250, "Tên voucher")
                , new LookUpColumnInfo("SoDu", 50, "Số dư")
                , new LookUpColumnInfo("NgayHieuLuc", 50, "Ngày hiệu lực"));

            cboDanhSachBH.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;

                tbl_Voucher drV = new Base<tbl_Voucher>().First(t => t.Ma == Cast.ToInt(cboVoucher.EditValue));

                if (drV != null)
                    cboEdit.Properties.DataSource = new Base<vBanHang>().Get(t => t.MaKH == Cast.ToInt(cboKhachHang.EditValue)
                        //(t.MaThe == Cast.ToString(txtMaThe.EditValue)
                        //|| (t.MaKH == Cast.ToInt(cboKhachHang.EditValue) && t.MaVoucher == Cast.ToInt(cboVoucher.EditValue))
                        //|| t.MaVach == Cast.ToString(txtMaVach.EditValue))
                        && t.MaChiNhanh == clsPublicVar.ChiNhanh.Ma && t.MaNganh == clsPublicVar.Nganh.Ma
                        && t.NhomVoucherID == drV.NhomVoucherID && (t.TrangThai ?? false))
                        .OrderByDescending(t => t.NgayHieuLuc).ToList();
                else
                    cboEdit.Properties.DataSource = new Base<vBanHang>().Get(t => t.MaKH == Cast.ToInt(cboKhachHang.EditValue)
                        //(t.MaThe == Cast.ToString(txtMaThe.EditValue)
                        //|| (t.MaKH == Cast.ToInt(cboKhachHang.EditValue) && t.MaVoucher == Cast.ToInt(cboVoucher.EditValue))
                        //|| t.MaVach == Cast.ToString(txtMaVach.EditValue))
                        && t.MaChiNhanh == clsPublicVar.ChiNhanh.Ma && t.MaNganh == clsPublicVar.Nganh.Ma && (t.TrangThai ?? false))
                        .OrderByDescending(t => t.NgayHieuLuc).ToList();
            };

            cboDanhSachBH.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;

                cboEdit.Properties.DataSource = new Base<vBanHang>().Get().ToList();
            };

            cboDanhSachBH.EditValueChanged += (sender, e) =>
            {
                vBanHang dr = new Base<vBanHang>().First(t => t.Ma == Cast.ToInt(cboDanhSachBH.EditValue));
                if (dr != null)
                {
                    if (Cast.ToInt(cboVoucher.EditValue) == 0)
                        cboVoucher.EditValue = dr.MaVoucher;
                    else
                    {
                        tbl_Voucher drV = new Base<tbl_Voucher>().First(t => t.Ma == Cast.ToInt(cboVoucher.EditValue));

                        if (drV != null && dr.NhomVoucherID != drV.NhomVoucherID)
                            cboVoucher.EditValue = null;
                    }

                    cboKhachHang.EditValue = dr.MaKH;
                    txtMaThe.EditValue = dr.MaThe;
                    txtMaVach.EditValue = dr.MaVach;
                    SetReadOnlyControl(true);
                }
                else
                {
                    cboVoucher.EditValue = null;
                    cboKhachHang.EditValue = null;
                    txtMaThe.EditValue = null;
                    txtMaVach.EditValue = null;
                    SetReadOnlyControl(false);
                }
            };

            //..
            FunctionLoadData = "GetData";
            FuLoadDataParas = () => new[]
            {
                (object)clsPublicVar.ChiNhanh.Ma,
                (object)clsPublicVar.Nganh.Ma,
                (object)uSearch.GetValue("dptTuNgay"),
                (object)uSearch.GetValue("dptDenNgay"),
                (object)uSearch.GetValue("cboKhachHang"),
                (object)uSearch.GetValue("cboVoucher"),
                (object)uSearch.GetValue("txtMaThe"),
                (object)uSearch.GetValue("chkKichHoat")
            };

            //Add Column to gridcontrol
            AddDataColumnAndControl("cMaVoucher", "Voucher", "MaVoucher", true, 80, cboVoucher);
            AddDataColumnAndControl("cMaKH", "Khách hàng", "MaKH", true, 70, cboKhachHang);
            AddDataColumnAndControl("cMaThe", "Mã thẻ", "MaThe", false, 30, txtMaThe);
            AddDataColumnAndControl(new DataFieldColumn("cGiaTri", "Giá trị", "GiaTri", true, 25) { _isReadOnly = true, _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat }, null);
            AddDataColumnAndControl(new DataFieldColumn("cSoDu", "Số dư", "SoDu", false, 25) { _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat }, null);
            AddDataColumnAndControl(new DataFieldColumn("cThanhTien", "Thành tiền", "ThanhTien", false, 25) { _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat }, null);
            AddDataColumnAndControl(new DataFieldColumn("cNgayHieuLuc", "Ngày hiệu lực", "NgayHieuLuc", false, 25) { _formatType = FormatType.DateTime, _formatString = QLVCConst.DateFormat }, dptNgayHieuLuc);
            AddDataColumnAndControl(new DataFieldColumn("cNgayHetHan", "Ngày hết hạn", "NgayHetHan", false, 25) { _formatType = FormatType.DateTime, _formatString = QLVCConst.DateFormat }, dptNgayHetHan);
            AddDataColumnAndControl("cNguoiKichHoat", "Người kích hoạt", "NguoiKichHoat", false, 45, null);
            AddDataColumnAndControl("cNgayKichHoat", "Ngày kích hoạt", "NgayKichHoat", false, 30, null);
            AddDataColumnAndControl("cTrangThai", "Trạng thái", "TrangThai", false, 10, chkTrangThai, true);
            AddDataColumnAndControl("cChonEx", "Chọn", "ChonEx", false, 10, null);

            //..
            GridUtil.BCU.AddControls(new ControlBind(txtMaVach, "MaVach", false));
            GridUtil.BCU.AddControls(new ControlBind(txtGiaTri, true, "GiaTri", 30, true));
            GridUtil.BCU.AddControls(new ControlBind(txtThanhTien, "ThanhTien", false));
            GridUtil.BCU.AddControls(new ControlBind(txtGhiChu, false, "GhiChu", 500, false));
            GridUtil.BCU.AutoDetectMaxlength(typeof(vBanHang));

            //..specify field what use combobox
            cboVoucher.Build("Ten", "Ma", tbl_VoucherB.Get().ToList(), 350, 300
                , new LookUpColumnInfo("Ten", 250, "Tên voucher")
                , new LookUpColumnInfo("GiaTri", 100, "Giá trị voucher"));
            GvMain.ExSetSearchColumnCombobox("MaVoucher", "Ten", "Ma", tbl_VoucherB.Get().ToList(), 350, 300
                , new LookUpColumnInfo("Ten", 250, "Tên voucher")
                , new LookUpColumnInfo("GiaTri", 100, "Giá trị voucher"));

            cboVoucher.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                tbl_VoucherB = new tbl_VoucherBusiness();

                vBanHang drBH = tbl_VoucherB.DC.GetTable<vBanHang>().FirstOrDefault(t => t.Ma == Cast.ToInt(cboDanhSachBH.EditValue));

                if (drBH != null)
                    cboEdit.Properties.DataSource = tbl_VoucherB.Get()
                        .Join(tbl_VoucherB.DC.GetTable<tbl_VoucherRole>(), v => v.Ma, vr => vr.MaVoucher, (v, vr) => new { v, vr })
                        .Where(t => (t.v.TrangThai ?? false) && !(t.v.DaXoa ?? false) && (t.vr.TrangThai ?? false) && !(t.vr.DaXoa ?? false)
                            && t.vr.MaChiNhanh == clsPublicVar.ChiNhanh.Ma && t.vr.MaNganh == clsPublicVar.Nganh.Ma
                            && t.v.NhomVoucherID == drBH.NhomVoucherID)
                        .Select(t => new { t.v.Ma, t.v.Ten, t.v.GiaTri, t.v.NgayTao })
                        .OrderByDescending(t => t.NgayTao).ToList();
                else
                    cboEdit.Properties.DataSource = tbl_VoucherB.Get()
                        .Join(tbl_VoucherB.DC.GetTable<tbl_VoucherRole>(), v => v.Ma, vr => vr.MaVoucher, (v, vr) => new { v, vr })
                        .Where(t => (t.v.TrangThai ?? false) && !(t.v.DaXoa ?? false) && (t.vr.TrangThai ?? false) && !(t.vr.DaXoa ?? false)
                            && t.vr.MaChiNhanh == clsPublicVar.ChiNhanh.Ma && t.vr.MaNganh == clsPublicVar.Nganh.Ma)
                        .Select(t => new { t.v.Ma, t.v.Ten, t.v.GiaTri, t.v.NgayTao })
                        .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboVoucher.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;

                cboEdit.Properties.DataSource = tbl_VoucherB.Get().ToList();
            };

            cboKhachHang.Build("Ten", "Ma", new Base<tbl_KhachHang>().Get().ToList(), 450, 300
                , new LookUpColumnInfo("Ten", 200, "Tên khách hàng")
                , new LookUpColumnInfo("DienThoai", 125, "Số điện thoại")
                , new LookUpColumnInfo("CMND", 125, "CMND")
                //, new LookUpColumnInfo("DiaChi", 150, "Địa chỉ")
                );
            GvMain.ExSetSearchColumnCombobox("MaKH", "Ten", "Ma", new Base<tbl_KhachHang>().Get().ToList(), 450, 300
                , new LookUpColumnInfo("Ten", 200, "Tên khách hàng")
                , new LookUpColumnInfo("DienThoai", 125, "Số điện thoại")
                , new LookUpColumnInfo("CMND", 125, "CMND")
                //, new LookUpColumnInfo("DiaChi", 200, "Địa chỉ")
                );

            cboKhachHang.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                tbl_VoucherB = new tbl_VoucherBusiness();

                cboEdit.Properties.DataSource = tbl_VoucherB.DC.GetTable<tbl_KhachHang>()
                    .Where(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.NgayTao).ToList();
            };

            cboKhachHang.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;

                cboEdit.Properties.DataSource = tbl_VoucherB.DC.GetTable<tbl_KhachHang>().ToList();
            };

            List<BasicItem> lLoaiThoiHan = new List<BasicItem>(new[]
            {
                new BasicItem("Ngày", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Ngay)),
                new BasicItem("Tuần", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Tuan)),
                new BasicItem("Tháng", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Thang)),
                new BasicItem("Năm", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Nam))
            });
            cboLoaiThoiGian.Build("Text", "Value", lLoaiThoiHan.ToList()
                , new LookUpColumnInfo("Text", 100, "Loại thời hạn"));

            GvMain.ExSetSearchColumnCombobox("NguoiKichHoat", "HoTen", "Ma", new Base<sys_User>().Get().ToList(), 250, 300
                , new LookUpColumnInfo("HoTen", 250, "Tên nhân viên"));

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..
            panContent.Controls.Add(GMain);

            //..base automatically
            AutoGenAllDataImplementation(clsPublicVar.FieldCheckDelete);

            //..set validate
            GridUtil.Valid.AddValidNotBlank(cboVoucher, "Bạn chưa chọn loại Voucher");
            GridUtil.Valid.AddValidNotBlank(cboKhachHang, "Bạn chưa chọn Khách hàng");
            GridUtil.Valid.AddValidNotBlank(txtGiaTri, "Bạn chưa nhập giá trị");
            GridUtil.Valid.AddValidNotBlank(txtThanhTien, "Bạn chưa nhập thành tiền");
            GridUtil.Valid.AddValidNotBlank(cboLoaiThoiGian, "Bạn chưa chọn loại thời hạn");
            GridUtil.Valid.AddValidNotBlank(txtMaThe, "Bạn chưa nhập mã thẻ");
            GridUtil.Valid.AddValidNotBlank(txtMaVach, "Bạn chưa nhập mã vạch");
            GridUtil.Valid.AddValidKeysExist(txtMaVach, "Mã vạch bị trùng", dr =>
            {
                if (chkBanMoi.Checked)
                {
                    int iCount = 0;
                    vBanHang drBH = GvMain.ExGetCurrentDataRow() as vBanHang;

                    if (GridUtil.DataMode == DataModeType.Edit && drBH != null)
                        iCount = new Base<tbl_BanHang>().Count(t => t.MaVach == Cast.ToString(txtMaVach.EditValue)
                            && t.Ma != drBH.Ma && !(t.DaXoa ?? false));
                    else if (GridUtil.DataMode == DataModeType.AddNew)
                        iCount = new Base<tbl_BanHang>().Count(t => t.MaVach == Cast.ToString(txtMaVach.EditValue)
                            && !(t.DaXoa ?? false));

                    if (iCount >= 1)
                        return true;
                }
                return false;
            });
            GridUtil.Valid.AddValidNotBlank(txtGiaTriThoiHan, "Bạn chưa nhập giá trị thời hạn");
            GridUtil.Valid.AddValidNotBlank(dptNgayHieuLuc, "Bạn chưa chọn ngày hiệu lực");
            GridUtil.Valid.AddValidNotBlank(dptNgayHetHan, "Bạn chưa chọn ngày hết hạn");
            GridUtil.Valid.AddValidKeysExist(cboDanhSachBH, "Bạn chưa chọn phiếu bán hàng bổ sung", dr =>
            {
                if (chkBanBoSung.Checked && Cast.ToString(cboDanhSachBH.EditValue) == "")
                    return true;
                return false;
            });

            //..
            cboVoucher.EditValueChanged += (sender, e) =>
            {
                tbl_Voucher drV = new Base<tbl_Voucher>().First(t => t.Ma == Cast.ToInt(cboVoucher.EditValue));
                if (drV != null)
                {
                    txtGiaTri.EditValue = drV.GiaTri;
                    cboLoaiThoiGian.EditValue = drV.LoaiThoiHan;
                    txtGiaTriThoiHan.EditValue = drV.GiaTriThoiHan;
                    txtThanhTien.EditValue = drV.DonGia;
                }
                else
                {
                    txtGiaTri.EditValue = 0;
                    cboLoaiThoiGian.EditValue = null;
                    txtGiaTriThoiHan.EditValue = 0;
                    txtThanhTien.EditValue = 0;
                }
            };

            //..
            dptNgayHieuLuc.EditValueChanged += (sender, e) =>
            {
                DateTime dtdate = DateTime.Now;

                if (Cast.ToString(cboLoaiThoiGian.EditValue) != "")
                {
                    if (Cast.ToString(cboLoaiThoiGian.EditValue) == Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Ngay))
                        dtdate = Cast.ToDateTime(dptNgayHieuLuc.EditValue).AddDays(Cast.ToInt(txtGiaTriThoiHan.EditValue));
                    else if (Cast.ToString(cboLoaiThoiGian.EditValue) == Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Tuan))
                        dtdate = Cast.ToDateTime(dptNgayHieuLuc.EditValue).AddDays(Cast.ToInt(txtGiaTriThoiHan.EditValue) * 7);
                    else if (Cast.ToString(cboLoaiThoiGian.EditValue) == Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Thang))
                        dtdate = Cast.ToDateTime(dptNgayHieuLuc.EditValue).AddMonths(Cast.ToInt(txtGiaTriThoiHan.EditValue));
                    else if (Cast.ToString(cboLoaiThoiGian.EditValue) == Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Nam))
                        dtdate = Cast.ToDateTime(dptNgayHieuLuc.EditValue).AddYears(Cast.ToInt(txtGiaTriThoiHan.EditValue));

                    dptNgayHetHan.EditValue = dtdate.AddDays(-1);
                }
                else
                    dptNgayHetHan.EditValue = null;
            };

            //..
            GridUtil.DataModeChanged += (mod) =>
            {
                chkBanMoi.EditValue = true;

                if (mod == DataModeType.AddNew)
                {
                    btnKichHoat.Enabled = false;
                    btnHuyKichHoat.Enabled = btnKichHoat.Enabled;
                    chkChonTatCa.Enabled = btnKichHoat.Enabled;
                    btnLuuVaInHoaDon.Enabled = true;
                    chkBanMoi.Enabled = true;
                    chkBanBoSung.Enabled = true;
                }
                else
                {
                    RefreshControl();
                    SetOptionBanHang();
                }
            };

            //..
            GvMain.FocusedRowChanged += (sender, e) => RefreshControl();

            //..
            chkChonTatCa.CheckedChanged += (sender, e) =>
            {
                GvMain.PostEditor();

                for (int i = 0; i < GvMain.RowCount; i++)
                {
                    vBanHang dr = GvMain.GetRow(i) as vBanHang;
                    if (dr != null)
                        GvMain.SetRowCellValue(i, "ChonEx", chkChonTatCa.Checked);
                }
            };

            //..
            chkBanMoi.CheckedChanged += (sender, e) => SetOptionBanHang();

            //..
            chkBanBoSung.CheckedChanged += (sender, e) => SetOptionBanHang();
        }

        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvMain, QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }

        private void btnKichHoat_Click(object sender, EventArgs e)
        {
            if (SetConfirm("Bạn thật sự muốn kích hoạt những thông tin bán hàng đã được chọn", "Thông báo") == DialogResult.OK)
            {
                if (chkChonTatCa.Checked)
                {
                    WaitingBox.Show("Hệ thống đang xử lý", "Vui lòng chờ");
                    int iUpdateSuccess = 0, iRowSelect = 0;

                    for (int i = 0; i < GvMain.DataRowCount; i++)
                    {
                        vBanHang drBH = GvMain.GetRow(i) as vBanHang;

                        if (drBH != null && drBH.ChonEx && !(drBH.TrangThai ?? false))
                        {
                            iRowSelect++;
                            State = KichHoat(drBH, true);

                            if (State.Success)
                                iUpdateSuccess++;
                        }
                    }

                    if (iRowSelect == 0)
                        SetFailure("Bạn vui lòng chọn thông tin bán hàng cần kích hoạt");
                    else if (iUpdateSuccess == 0)
                        SetFailure("Kích hoạt thất bại");
                    else if (iUpdateSuccess == iRowSelect)
                        SetSuccess("Kích hoạt thành công");
                    else if (iUpdateSuccess > 0 && iUpdateSuccess != iRowSelect)
                        SetFailure("Kích hoạt thành công một phần danh sách bán hàng được chọn");

                    WaitingBox.Hide(10);
                }
                else
                {
                    vBanHang drBH = GvMain.ExGetCurrentDataRow() as vBanHang;

                    if (drBH != null)
                    {
                        State = KichHoat(drBH, true);

                        if (State.Success)
                            SetSuccess("Kích hoạt thành công");
                        else
                            SetFailure("Kích hoạt thất bại");
                    }
                    else
                        SetFailure("Bạn vui lòng chọn thông tin bán hàng cần kích hoạt");
                }

                Browse();
            }

            chkChonTatCa.EditValue = false;
        }

        private void btnHuyKichHoat_Click(object sender, EventArgs e)
        {
            if (SetConfirm("Bạn thật sự muốn hủy kích hoạt những thông tin bán hàng đã được chọn", "Thông báo") == DialogResult.OK)
            {
                if (chkChonTatCa.Checked)
                {
                    WaitingBox.Show("Hệ thống đang xử lý", "Vui lòng chờ");
                    int iUpdateSuccess = 0, iRowSelect = 0;

                    for (int i = 0; i < GvMain.DataRowCount; i++)
                    {
                        vBanHang drBH = GvMain.GetRow(i) as vBanHang;

                        if (drBH != null && drBH.ChonEx && (drBH.TrangThai ?? false))
                        {
                            iRowSelect++;
                            State = KichHoat(drBH, false);

                            if (State.Success)
                                iUpdateSuccess++;
                        }
                    }

                    if (iRowSelect == 0)
                        SetFailure("Bạn vui lòng chọn thông tin bán hàng cần hủy kích hoạt");
                    else if (iUpdateSuccess == 0)
                        SetFailure("Hủy kích hoạt thất bại");
                    else if (iUpdateSuccess == iRowSelect)
                        SetSuccess("Hủy kích hoạt thành công");
                    else if (iUpdateSuccess > 0 && iUpdateSuccess != iRowSelect)
                        SetFailure("Hủy kích hoạt thành công một phần danh sách bán hàng được chọn");

                    WaitingBox.Hide(10);
                }
                else
                {
                    vBanHang drBH = GvMain.ExGetCurrentDataRow() as vBanHang;

                    if (drBH != null)
                    {
                        State = KichHoat(drBH, false);

                        if (State.Success)
                            SetSuccess("Hủy kích hoạt thành công");
                        else
                            SetFailure("Hủy kích hoạt thất bại");
                    }
                    else
                        SetFailure("Bạn vui lòng chọn thông tin bán hàng cần hủy kích hoạt");
                }

                Browse();
            }

            chkChonTatCa.EditValue = false;
        }

        private void btnLuuVaInHoaDon_Click(object sender, EventArgs e)
        {
            try
            {
                //Xu ly in
                new ThongTinHoaDon().XuLyMayIn(TaoHoaDon);

                //Xu ly luu
                SaveModify(false);
            }
            catch (Exception ex)
            {
                SetFailure(string.Format("Có một số lỗi đã xảy ra trong quá trình xử lý, chi tiết:\n\n{0}", ex.Message), "Thông báo");
            }
        }

        private void TaoHoaDon(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            if (GridUtil.DataMode == DataModeType.AddNew)
            {
                //..
                new ThongTinHoaDon().ChiTietHoaDonBanHang(graphic, cboKhachHang.Text, DataUtils.GetDate(), cboVoucher.Text
                    , Cast.ToInt(txtThanhTien.EditValue), Cast.ToInt(txtGiaTri.EditValue), Cast.ToDateTime(dptNgayHieuLuc.EditValue)
                    , Cast.ToDateTime(dptNgayHetHan.EditValue));
            }
            else
            {
                vBanHang drBH = GvMain.ExGetCurrentDataRow() as vBanHang;
                if (drBH != null)
                {
                    //..
                    new ThongTinHoaDon().ChiTietHoaDonBanHang(graphic, cboKhachHang.Text, drBH.NgayTao ?? DateTime.Now, drBH.TenVoucher
                        , Cast.ToInt(txtThanhTien.EditValue), Cast.ToInt(txtGiaTri.EditValue), Cast.ToDateTime(drBH.NgayHieuLuc)
                        , Cast.ToDateTime(drBH.NgayHetHan));
                }
            }
        }

        public override void Browse()
        {
            base.Browse();

            RefreshControl();
            SetOptionBanHang();
            chkChonTatCa.EditValue = false;
        }

        public override void Save()
        {
            SaveModify(true);
        }

        private void SaveModify(bool ShowNotification)
        {
            GvMain.PostEditor();

            if (!GridUtil.Valid.ValidateOK())
                return;

            if (GridUtil.DataMode == DataModeType.AddNew)
            {
                if (chkBanMoi.Checked)
                    State = DataBusiness.XuLyBanHang((int)Quyen.Add, false, 0, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma
                        , Cast.ToInt(cboKhachHang.EditValue), Cast.ToInt(cboVoucher.EditValue), Cast.ToString(txtMaVach.EditValue)
                        , Cast.ToInt(txtGiaTri.EditValue), Cast.ToInt(txtGiaTri.EditValue), Cast.ToInt(txtThanhTien.EditValue)
                        , Cast.ToDateTime(dptNgayHieuLuc.EditValue), Cast.ToDateTime(dptNgayHetHan.EditValue), false, clsPublicVar.User.Ma
                        , Cast.ToString(txtMaThe.EditValue), Cast.ToString(txtGhiChu.EditValue));
                else if (chkBanBoSung.Checked)
                    State = DataBusiness.ThemBanHangBoSung(Cast.ToInt(cboDanhSachBH.EditValue), Cast.ToInt(txtThanhTien.EditValue)
                        , Cast.ToDateTime(dptNgayHieuLuc.EditValue), Cast.ToDateTime(dptNgayHetHan.EditValue), clsPublicVar.User.Ma
                        , Cast.ToString(txtGhiChu.EditValue), Cast.ToInt(cboVoucher.EditValue), Cast.ToString(txtMaVach.EditValue)
                        , Cast.ToString(txtMaThe.EditValue), Cast.ToInt(txtGiaTri.EditValue));

                if (ShowNotification)
                {
                    if (State.Success)
                        SetSuccess("Thêm thành công");
                    else
                        SetFailure("Thêm thất bại");
                }
            }
            else if (GridUtil.DataMode == DataModeType.Edit)
            {
                vBanHang drBH = GvMain.ExGetCurrentDataRow() as vBanHang;

                if (drBH != null)
                {
                    if (drBH.TrangThai ?? false)
                    {
                        if (SetConfirm("Phiếu bán hàng đã được kích hoạt, nếu bạn vẫn chỉnh sửa thì cần kích hoạt lại.\nBạn vẫn muốn tiếp tục?"
                            , "Thông báo") != DialogResult.OK)
                        {
                            Browse();
                            return;
                        }
                    }

                    State = DataBusiness.XuLyBanHang((int)Quyen.Edit, false, drBH.Ma, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma
                        , Cast.ToInt(cboKhachHang.EditValue), Cast.ToInt(cboVoucher.EditValue), Cast.ToString(txtMaVach.EditValue)
                        , Cast.ToInt(txtGiaTri.EditValue), 0, Cast.ToInt(txtThanhTien.EditValue), Cast.ToDateTime(dptNgayHieuLuc.EditValue)
                        , Cast.ToDateTime(dptNgayHetHan.EditValue), false, clsPublicVar.User.Ma, Cast.ToString(txtMaThe.EditValue)
                        , Cast.ToString(txtGhiChu.EditValue));

                    if (ShowNotification)
                    {
                        if (State.Success)
                            SetSuccess("Sửa thành công");
                        else
                            SetFailure("Sửa thất bại");
                    }
                }
                else
                    SetFailure("Bạn vui lòng chọn thông tin bán hàng cần sửa");
            }
        }

        public override void Delete()
        {
            vBanHang drBH = GvMain.ExGetCurrentDataRow() as vBanHang;

            if (drBH != null)
            {
                tbl_ThanhToan drTT = new Base<tbl_ThanhToan>().First(t => t.MaBanHang == drBH.Ma && !(t.DaXoa ?? false));
                tbl_BanHang drBHDel = new Base<tbl_BanHang>().First(t => t.BanBoSungTu == drBH.Ma && !(t.DaXoa ?? false));

                if (drTT != null || drBHDel != null)
                    SetFailure("Không thể xóa");
                else
                {
                    if (SetConfirm("Bạn chắc chắn muốn xóa thông tin bán hàng?", "Thông báo") == DialogResult.OK)
                    {
                        State = DataBusiness.XuLyBanHang((int)Quyen.Delete, false, drBH.Ma, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma
                            , 0, 0, "", 0, 0, 0, DateTime.Now, DateTime.Now, false, clsPublicVar.User.Ma, Cast.ToString(txtMaThe.EditValue)
                            , Cast.ToString(txtGhiChu.EditValue));

                        if (State.Success)
                            SetSuccess("Xóa thành công");
                        else
                            SetFailure("Xóa thất bại");
                    }
                }
            }
            else
                SetFailure("Bạn vui lòng chọn thông tin bán hàng cần xóa");

            Browse();
        }

        private void RefreshControl()
        {
            vBanHang drBH = GvMain.ExGetCurrentDataRow() as vBanHang;

            if (drBH != null)
            {
                btnKichHoat.Enabled = !(drBH.TrangThai ?? false) && bKichHoat;
                btnHuyKichHoat.Enabled = (drBH.TrangThai ?? false) && bHuyKichHoat;
                btnLuuVaInHoaDon.Enabled = bLuuVaInHoaDon;
                chkChonTatCa.Enabled = btnKichHoat.Enabled || btnHuyKichHoat.Enabled;
            }
            else
            {
                btnKichHoat.Enabled = false;
                btnHuyKichHoat.Enabled = btnKichHoat.Enabled;
                btnLuuVaInHoaDon.Enabled = btnKichHoat.Enabled;
                chkChonTatCa.Enabled = false;
            }
        }

        private void SetOptionBanHang()
        {
            chkBanMoi.Enabled = GridUtil.DataMode == DataModeType.AddNew;
            chkBanBoSung.Enabled = GridUtil.DataMode == DataModeType.AddNew;
            cboDanhSachBH.EditValue = null;
            cboDanhSachBH.Enabled = chkBanBoSung.Checked && GridUtil.DataMode == DataModeType.AddNew;
        }

        private void SetReadOnlyControl(bool ReadOnly)
        {
            //cboVoucher.Properties.ReadOnly = ReadOnly;
            cboKhachHang.Properties.ReadOnly = ReadOnly;
            //txtMaThe.Properties.ReadOnly = ReadOnly;
            //txtMaVach.Properties.ReadOnly = ReadOnly;
        }

        private HandleState KichHoat(vBanHang drBH, bool bKichHoat)
        {
            return DataBusiness.XuLyBanHang((int)Quyen.Edit, true, drBH.Ma, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma
                , Cast.ToInt(cboKhachHang.EditValue), Cast.ToInt(cboVoucher.EditValue), Cast.ToString(txtMaVach.EditValue)
                , Cast.ToInt(txtGiaTri.EditValue), 0, Cast.ToInt(txtThanhTien.EditValue), Cast.ToDateTime(dptNgayHieuLuc.EditValue)
                , Cast.ToDateTime(dptNgayHetHan.EditValue), bKichHoat, clsPublicVar.User.Ma, Cast.ToString(txtMaThe.EditValue)
                , Cast.ToString(txtGhiChu.EditValue));
        }
    }
}
