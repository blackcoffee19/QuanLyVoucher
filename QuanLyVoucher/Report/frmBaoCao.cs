﻿using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
using Base.Common;
using Base.Common.Items;
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;
using Excel = Microsoft.Office.Interop.Excel;

namespace QuanLyVoucher.Report
{
    public partial class frmBaoCao : BaseForm
    {
        private tbl_VoucherBusiness tbl_VoucherB = new tbl_VoucherBusiness();
        private readonly int m_MenuID = clsPublicVar.MainMenuID;
        private List<vQuyen> ListQuyen = new List<vQuyen>();
        protected object misValue = System.Reflection.Missing.Value;

        public frmBaoCao()
        {
            InitializeComponent();

            //..
            ActionsDeny.AddRange(new[] { ButtonAction.Add, ButtonAction.Edit, ButtonAction.Delete });

            //..
            ListQuyen = clsPublicVar.QuyenCuaUser.Where(t => t.MaMenu == m_MenuID).ToList();

            //..
            List<BasicItem> ListLoaiBaoCao = new List<BasicItem>();
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.LichSuThanhToan))
                ListLoaiBaoCao.Add(new BasicItem("Lịch sử thanh toán", (int)Quyen.LichSuThanhToan));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.LichSuThanhToanTheoGoiSuat))
                ListLoaiBaoCao.Add(new BasicItem("Lịch sử thanh toán theo gói suất", (int)Quyen.LichSuThanhToanTheoGoiSuat));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.LichSuThanhToanTheoTien))
                ListLoaiBaoCao.Add(new BasicItem("Lịch sử thanh toán theo tiền", (int)Quyen.LichSuThanhToanTheoTien));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.DoanhThuTheoNgayDangKy))
                ListLoaiBaoCao.Add(new BasicItem("Doanh thu theo ngày đăng ký", (int)Quyen.DoanhThuTheoNgayDangKy));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.DoanhThuTheoNgayThanhToan))
                ListLoaiBaoCao.Add(new BasicItem("Doanh thu theo ngày thanh toán", (int)Quyen.DoanhThuTheoNgayThanhToan));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.VoucherCuaKhachHang))
                ListLoaiBaoCao.Add(new BasicItem("Voucher của Khách hàng", (int)Quyen.VoucherCuaKhachHang));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.VoucherDangSuDung))
                ListLoaiBaoCao.Add(new BasicItem("Voucher đang sử dụng", (int)Quyen.VoucherDangSuDung));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.VoucherHetSoDu))
                ListLoaiBaoCao.Add(new BasicItem("Voucher hết số dư", (int)Quyen.VoucherHetSoDu));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.VoucherHetHanSuDung))
                ListLoaiBaoCao.Add(new BasicItem("Voucher hết hạn sử dụng", (int)Quyen.VoucherHetHanSuDung));
            if (ListQuyen.Any(t => t.MaQuyen == (int)Quyen.VoucherDaHuy))
                ListLoaiBaoCao.Add(new BasicItem("Voucher đã huỷ", (int)Quyen.VoucherDaHuy));

            bool bXemTatCaVoucherCuaChiNhanh = ListQuyen.Any(t => t.MaQuyen == (int)Quyen.XemTatCaVoucherCuaChiNhanh);

            cboLoaiBaoCao.Build("Text", "Value", ListLoaiBaoCao.ToList(), 450, 300
                , new LookUpColumnInfo("Text", 450, "Loại báo cáo"));

            cboNganh.Build("Ten", "Ma", new Base<tbl_Nganh>().Get(t => t.MaChiNhanh == clsPublicVar.ChiNhanh.Ma
                && (t.TrangThai ?? false) && !(t.DaXoa ?? false)).ToList(), 450, 300
                , new LookUpColumnInfo("Ten", 450, "Tên chi nhánh"));

            cboNhanVien.Build("HoTen", "Ma", new Base<sys_User>().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false)).ToList(), 450, 300
                , new LookUpColumnInfo("HoTen", 450, "Tên nhân viên"));

            cboKhachHang.Build("Ten", "Ma", new Base<tbl_KhachHang>().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false)).ToList(), 450, 300
                , new LookUpColumnInfo("Ten", 450, "Tên khách hàng"));

            cboVoucher.Build("Ten", "Ma", tbl_VoucherB.Get()
                .Join(tbl_VoucherB.DC.GetTable<tbl_VoucherRole>(), v => v.Ma, vr => vr.MaVoucher, (v, vr) => new { v, vr })
                .Where(t => t.vr.MaChiNhanh == clsPublicVar.ChiNhanh.Ma
                    && (bXemTatCaVoucherCuaChiNhanh ? true : t.vr.MaNganh == clsPublicVar.Nganh.Ma)
                    && (t.v.TrangThai ?? false) && !(t.v.DaXoa ?? false)
                    && (t.vr.TrangThai ?? false) && !(t.vr.DaXoa ?? false))
                .GroupBy(t => t.v).Select(t => t.Key).ToList(), 450, 300
                , new LookUpColumnInfo("Ten", 400, "Tên voucher")
                , new LookUpColumnInfo("GiaTri", 50, "Giá trị voucher"));

            //..
            cboLoaiBaoCao.EditValueChanged += (sender, e) =>
            {
                //Reset control
                cboNganh.EditValue = null;
                cboNhanVien.EditValue = null;
                cboKhachHang.EditValue = null;
                txtMaThe.EditValue = null;
                cboVoucher.EditValue = null;

                if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToan
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToanTheoGoiSuat
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToanTheoTien
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.DoanhThuTheoNgayDangKy
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.DoanhThuTheoNgayThanhToan
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherCuaKhachHang
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherDangSuDung
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherHetSoDu
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherHetHanSuDung
                    || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherDaHuy)
                    SetEnableControl(true, true, true, true, true, true);
            };
        }

        public override void Browse()
        {
            cboLoaiBaoCao.EditValue = null;
            DateTime curDate = DataUtils.GetDate();
            dptTuNgay.EditValue = new DateTime(curDate.Year, curDate.Month, 1);
            dptDenNgay.EditValue = curDate;
            cboNganh.EditValue = null;
            cboNhanVien.EditValue = null;
            cboKhachHang.EditValue = null;
            txtMaThe.EditValue = null;
            cboVoucher.EditValue = null;
        }

        private void SetEnableControl(bool TuNgay, bool DenNgay, bool Nganh, bool NhanVien, bool KhachHang, bool Voucher)
        {
            dptTuNgay.Enabled = TuNgay;
            dptDenNgay.Enabled = DenNgay;
            cboNganh.Enabled = Nganh;
            cboNhanVien.Enabled = NhanVien;
            cboKhachHang.Enabled = KhachHang;
            txtMaThe.Enabled = cboKhachHang.Enabled;
            cboVoucher.Enabled = Voucher;
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            if (Cast.ToInt(cboLoaiBaoCao.EditValue) == 0)
            {
                SetFailure("Bạn vui lòng chọn loại báo cáo cần xuất");
                cboLoaiBaoCao.Focus();
            }
            else
            {
                //Kiem tra chon day du thong tin chua
                if (KiemTraDieuKien())
                {
                    if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToan)
                    {
                        #region LichSuThanhToan
                        ExportLichSuThanhToan(LoaiVoucher.All);
                        #endregion LichSuThanhToan
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToanTheoGoiSuat)
                    {
                        #region LichSuThanhToanTheoGoiSuat
                        ExportLichSuThanhToan(LoaiVoucher.GoiSuat);
                        #endregion LichSuThanhToanTheoGoiSuat
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToanTheoTien)
                    {
                        #region LichSuThanhToanTheoTien
                        ExportLichSuThanhToan(LoaiVoucher.Tien);
                        #endregion LichSuThanhToanTheoTien
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.DoanhThuTheoNgayDangKy)
                    {
                        #region DoanhThuTheoNgayDangKy
                        List<sp_DoanhThuTheoNgayDangKyResult> ListResult = new vDoanhThuBusiness()
                            .DoanhThuTheoNgayDangKy(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue)
                            , clsPublicVar.ChiNhanh.Ma, Cast.ToInt(cboNganh.EditValue), Cast.ToInt(cboNhanVien.EditValue)
                            , Cast.ToInt(cboKhachHang.EditValue), Cast.ToString(txtMaThe.EditValue), Cast.ToInt(cboVoucher.EditValue));

                        if (ListResult != null && ListResult.Count > 0)
                        {
                            //..save export file
                            SaveFileDialog cdlg = new SaveFileDialog();
                            cdlg.FileName = "doanh-thu-theo-ngay-dang-ky";
                            cdlg.Filter = "Excel File|*.xlsx";

                            if (cdlg.ShowDialog() == DialogResult.OK)
                            {
                                string sPathTemplate = QLVCConst.PathTemplate + @"\DoanhThuTheoNgayDangKy.xlsx";
                                string filePath = cdlg.FileName;

                                if (System.IO.File.Exists(sPathTemplate))
                                {
                                    //Copy Header Tittle from Example To New Template 
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                                        0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                                        true, false, 0, true, false, false);

                                    //Set sheet 1
                                    Excel.Worksheet excelWorksheet = null;
                                    string sheetName = "Report";

                                    Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                                    excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                                    //Add header
                                    SetValue(excelWorksheet, "F2", string.Format("Doanh thu theo ngày đăng ký từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                                        , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                                    //Add detail
                                    int IndexRow = 5, i = 0, IndexRowStart = 5;
                                    int iRows = ListResult.Count;
                                    int iColumns = 16;
                                    var data = new object[iRows, iColumns];

                                    foreach (sp_DoanhThuTheoNgayDangKyResult item in ListResult)
                                    {
                                        data[i, 0] = i + 1;
                                        data[i, 1] = item.ChiNhanhBan;
                                        data[i, 2] = item.NganhBan;
                                        data[i, 3] = item.ChiNhanhSuDung;
                                        data[i, 4] = item.NganhSuDung;
                                        data[i, 5] = item.TenVoucher;
                                        data[i, 6] = item.MaThe;
                                        data[i, 7] = item.TenKH;
                                        data[i, 8] = item.CMND;
                                        data[i, 9] = item.DienThoai;
                                        data[i, 10] = item.GiaTriVoucher;
                                        data[i, 11] = item.GiaTriThanhToan;
                                        data[i, 12] = item.SoDu;
                                        data[i, 13] = item.NgayHieuLuc;
                                        data[i, 14] = item.NgayHetHan;
                                        data[i, 15] = item.NgayThanhToan;

                                        IndexRow++;
                                        i++;
                                    }

                                    //Set border
                                    SetBorder(excelWorksheet, "A" + IndexRowStart, "P" + (IndexRow - 1));

                                    try
                                    {
                                        var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                                        var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                                        var writeRange = excelWorksheet.Range[startCell, endCell];
                                        writeRange.Value2 = data;

                                        //..save export file      
                                        excelWorkbook.SaveAs(filePath);
                                        excelWorkbook.Close(false, null, null);

                                        SetSuccess("Xuất file thành công");
                                    }
                                    catch { SetFailure("Xuất file thất bại"); }
                                }
                                else
                                    SetFailure("Mẫu không tồn tại");
                            }
                        }
                        else
                            SetFailure("Không có dữ liệu");
                        #endregion DoanhThuTheoNgayDangKy
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.DoanhThuTheoNgayThanhToan)
                    {
                        #region DoanhThuTheoNgayThanhToan
                        List<sp_DoanhThuTheoNgayThanhToanResult> ListResult = new vDoanhThuBusiness()
                            .DoanhThuTheoNgayThanhToan(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue)
                            , clsPublicVar.ChiNhanh.Ma, Cast.ToInt(cboNganh.EditValue), Cast.ToInt(cboNhanVien.EditValue)
                            , Cast.ToInt(cboKhachHang.EditValue), Cast.ToString(txtMaThe.EditValue), Cast.ToInt(cboVoucher.EditValue));

                        if (ListResult != null && ListResult.Count > 0)
                        {
                            //..save export file
                            SaveFileDialog cdlg = new SaveFileDialog();
                            cdlg.FileName = "doanh-thu-theo-ngay-thanh-toan";
                            cdlg.Filter = "Excel File|*.xlsx";

                            if (cdlg.ShowDialog() == DialogResult.OK)
                            {
                                string sPathTemplate = QLVCConst.PathTemplate + @"\DoanhThuTheoNgayThanhToan.xlsx";
                                string filePath = cdlg.FileName;

                                if (System.IO.File.Exists(sPathTemplate))
                                {
                                    //Copy Header Tittle from Example To New Template 
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                                        0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                                        true, false, 0, true, false, false);

                                    //Set sheet 1
                                    Excel.Worksheet excelWorksheet = null;
                                    string sheetName = "Report";

                                    Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                                    excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                                    //Add header
                                    SetValue(excelWorksheet, "F2", string.Format("Doanh thu theo ngày thanh toán từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                                        , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                                    //Add detail
                                    int IndexRow = 5, i = 0, IndexRowStart = 5;
                                    int iRows = ListResult.Count;
                                    int iColumns = 16;
                                    var data = new object[iRows, iColumns];

                                    foreach (sp_DoanhThuTheoNgayThanhToanResult item in ListResult)
                                    {
                                        data[i, 0] = i + 1;
                                        data[i, 1] = item.ChiNhanhBan;
                                        data[i, 2] = item.NganhBan;
                                        data[i, 3] = item.ChiNhanhSuDung;
                                        data[i, 4] = item.NganhSuDung;
                                        data[i, 5] = item.TenVoucher;
                                        data[i, 6] = item.MaThe;
                                        data[i, 7] = item.TenKH;
                                        data[i, 8] = item.CMND;
                                        data[i, 9] = item.DienThoai;
                                        data[i, 10] = item.GiaTriVoucher;
                                        data[i, 11] = item.GiaTriThanhToan;
                                        data[i, 12] = item.SoDu;
                                        data[i, 13] = item.NgayHieuLuc;
                                        data[i, 14] = item.NgayHetHan;
                                        data[i, 15] = item.NgayThanhToan;

                                        IndexRow++;
                                        i++;
                                    }

                                    //Set border
                                    SetBorder(excelWorksheet, "A" + IndexRowStart, "P" + (IndexRow - 1));

                                    try
                                    {
                                        var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                                        var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                                        var writeRange = excelWorksheet.Range[startCell, endCell];
                                        writeRange.Value2 = data;

                                        //..save export file      
                                        excelWorkbook.SaveAs(filePath);
                                        excelWorkbook.Close(false, null, null);

                                        SetSuccess("Xuất file thành công");
                                    }
                                    catch { SetFailure("Xuất file thất bại"); }
                                }
                                else
                                    SetFailure("Mẫu không tồn tại");
                            }
                        }
                        else
                            SetFailure("Không có dữ liệu");
                        #endregion DoanhThuTheoNgayThanhToan
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherCuaKhachHang)
                    {
                        #region VoucherCuaKhachHang
                        List<sp_VoucherCuaKhachHangResult> ListResult = new vDoanhThuBusiness()
                            .VoucherCuaKhachHang(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue)
                            , clsPublicVar.ChiNhanh.Ma, Cast.ToInt(cboNganh.EditValue), Cast.ToInt(cboNhanVien.EditValue)
                            , Cast.ToInt(cboKhachHang.EditValue), Cast.ToString(txtMaThe.EditValue), Cast.ToInt(cboVoucher.EditValue));

                        if (ListResult != null && ListResult.Count > 0)
                        {
                            //..save export file
                            SaveFileDialog cdlg = new SaveFileDialog();
                            cdlg.FileName = "voucher-cua-khach-hang";
                            cdlg.Filter = "Excel File|*.xlsx";

                            if (cdlg.ShowDialog() == DialogResult.OK)
                            {
                                string sPathTemplate = QLVCConst.PathTemplate + @"\VoucherCuaKhachHang.xlsx";
                                string filePath = cdlg.FileName;

                                if (System.IO.File.Exists(sPathTemplate))
                                {
                                    //Copy Header Tittle from Example To New Template 
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                                        0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                                        true, false, 0, true, false, false);

                                    //Set sheet 1
                                    Excel.Worksheet excelWorksheet = null;
                                    string sheetName = "Report";

                                    Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                                    excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                                    //Add header
                                    SetValue(excelWorksheet, "D2", string.Format("Voucher của khách hàng từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                                        , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                                    //Add detail
                                    int IndexRow = 5, i = 0, IndexRowStart = 5;
                                    int iRows = ListResult.Count;
                                    int iColumns = 12;
                                    var data = new object[iRows, iColumns];

                                    foreach (sp_VoucherCuaKhachHangResult item in ListResult)
                                    {
                                        data[i, 0] = i + 1;
                                        data[i, 1] = item.ChiNhanhBan;
                                        data[i, 2] = item.NganhBan;
                                        data[i, 3] = item.TenVoucher;
                                        data[i, 4] = item.MaThe;
                                        data[i, 5] = item.TenKH;
                                        data[i, 6] = item.CMND;
                                        data[i, 7] = item.DienThoai;
                                        data[i, 8] = item.GiaTriVoucher;
                                        data[i, 9] = item.SoDu;
                                        data[i, 10] = item.NgayHieuLuc;
                                        data[i, 11] = item.NgayHetHan;

                                        IndexRow++;
                                        i++;
                                    }

                                    //Set border
                                    SetBorder(excelWorksheet, "A" + IndexRowStart, "L" + (IndexRow - 1));

                                    try
                                    {
                                        var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                                        var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                                        var writeRange = excelWorksheet.Range[startCell, endCell];
                                        writeRange.Value2 = data;

                                        //..save export file      
                                        excelWorkbook.SaveAs(filePath);
                                        excelWorkbook.Close(false, null, null);

                                        SetSuccess("Xuất file thành công");
                                    }
                                    catch { SetFailure("Xuất file thất bại"); }
                                }
                                else
                                    SetFailure("Mẫu không tồn tại");
                            }
                        }
                        else
                            SetFailure("Không có dữ liệu");
                        #endregion VoucherCuaKhachHang
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherDangSuDung)
                    {
                        #region VoucherDangSuDung
                        List<sp_VoucherDangSuDungResult> ListResult = new vDoanhThuBusiness()
                            .VoucherDangSuDung(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue)
                            , clsPublicVar.ChiNhanh.Ma, Cast.ToInt(cboNganh.EditValue), Cast.ToInt(cboNhanVien.EditValue)
                            , Cast.ToInt(cboKhachHang.EditValue), Cast.ToString(txtMaThe.EditValue), Cast.ToInt(cboVoucher.EditValue));

                        if (ListResult != null && ListResult.Count > 0)
                        {
                            //..save export file
                            SaveFileDialog cdlg = new SaveFileDialog();
                            cdlg.FileName = "voucher-dang-su-dung";
                            cdlg.Filter = "Excel File|*.xlsx";

                            if (cdlg.ShowDialog() == DialogResult.OK)
                            {
                                string sPathTemplate = QLVCConst.PathTemplate + @"\VoucherDangSuDung.xlsx";
                                string filePath = cdlg.FileName;

                                if (System.IO.File.Exists(sPathTemplate))
                                {
                                    //Copy Header Tittle from Example To New Template 
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                                        0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                                        true, false, 0, true, false, false);

                                    //Set sheet 1
                                    Excel.Worksheet excelWorksheet = null;
                                    string sheetName = "Report";

                                    Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                                    excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                                    //Add header
                                    SetValue(excelWorksheet, "D2", string.Format("Voucher đang sử dụng từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                                        , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                                    //Add detail
                                    int IndexRow = 5, i = 0, IndexRowStart = 5;
                                    int iRows = ListResult.Count;
                                    int iColumns = 12;
                                    var data = new object[iRows, iColumns];

                                    foreach (sp_VoucherDangSuDungResult item in ListResult)
                                    {
                                        data[i, 0] = i + 1;
                                        data[i, 1] = item.ChiNhanhBan;
                                        data[i, 2] = item.NganhBan;
                                        data[i, 3] = item.TenVoucher;
                                        data[i, 4] = item.MaThe;
                                        data[i, 5] = item.TenKH;
                                        data[i, 6] = item.CMND;
                                        data[i, 7] = item.DienThoai;
                                        data[i, 8] = item.GiaTriVoucher;
                                        data[i, 9] = item.SoDu;
                                        data[i, 10] = item.NgayHieuLuc;
                                        data[i, 11] = item.NgayHetHan;

                                        IndexRow++;
                                        i++;
                                    }

                                    //Set border
                                    SetBorder(excelWorksheet, "A" + IndexRowStart, "L" + (IndexRow - 1));

                                    try
                                    {
                                        var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                                        var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                                        var writeRange = excelWorksheet.Range[startCell, endCell];
                                        writeRange.Value2 = data;

                                        //..save export file      
                                        excelWorkbook.SaveAs(filePath);
                                        excelWorkbook.Close(false, null, null);

                                        SetSuccess("Xuất file thành công");
                                    }
                                    catch { SetFailure("Xuất file thất bại"); }
                                }
                                else
                                    SetFailure("Mẫu không tồn tại");
                            }
                        }
                        else
                            SetFailure("Không có dữ liệu");
                        #endregion VoucherDangSuDung
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherHetSoDu)
                    {
                        #region VoucherHetSoDu
                        List<sp_VoucherHetSoDuResult> ListResult = new vDoanhThuBusiness()
                            .VoucherHetSoDu(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue)
                            , clsPublicVar.ChiNhanh.Ma, Cast.ToInt(cboNganh.EditValue), Cast.ToInt(cboNhanVien.EditValue)
                            , Cast.ToInt(cboKhachHang.EditValue), Cast.ToString(txtMaThe.EditValue), Cast.ToInt(cboVoucher.EditValue));

                        if (ListResult != null && ListResult.Count > 0)
                        {
                            //..save export file
                            SaveFileDialog cdlg = new SaveFileDialog();
                            cdlg.FileName = "voucher-het-so-du";
                            cdlg.Filter = "Excel File|*.xlsx";

                            if (cdlg.ShowDialog() == DialogResult.OK)
                            {
                                string sPathTemplate = QLVCConst.PathTemplate + @"\VoucherHetSoDu.xlsx";
                                string filePath = cdlg.FileName;

                                if (System.IO.File.Exists(sPathTemplate))
                                {
                                    //Copy Header Tittle from Example To New Template 
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                                        0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                                        true, false, 0, true, false, false);

                                    //Set sheet 1
                                    Excel.Worksheet excelWorksheet = null;
                                    string sheetName = "Report";

                                    Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                                    excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                                    //Add header
                                    SetValue(excelWorksheet, "D2", string.Format("Voucher hết số dư từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                                        , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                                    //Add detail
                                    int IndexRow = 5, i = 0, IndexRowStart = 5;
                                    int iRows = ListResult.Count;
                                    int iColumns = 12;
                                    var data = new object[iRows, iColumns];

                                    foreach (sp_VoucherHetSoDuResult item in ListResult)
                                    {
                                        data[i, 0] = i + 1;
                                        data[i, 1] = item.ChiNhanhBan;
                                        data[i, 2] = item.NganhBan;
                                        data[i, 3] = item.TenVoucher;
                                        data[i, 4] = item.MaThe;
                                        data[i, 5] = item.TenKH;
                                        data[i, 6] = item.CMND;
                                        data[i, 7] = item.DienThoai;
                                        data[i, 8] = item.GiaTriVoucher;
                                        data[i, 9] = item.SoDu;
                                        data[i, 10] = item.NgayHieuLuc;
                                        data[i, 11] = item.NgayHetHan;

                                        IndexRow++;
                                        i++;
                                    }

                                    //Set border
                                    SetBorder(excelWorksheet, "A" + IndexRowStart, "L" + (IndexRow - 1));

                                    try
                                    {
                                        var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                                        var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                                        var writeRange = excelWorksheet.Range[startCell, endCell];
                                        writeRange.Value2 = data;

                                        //..save export file      
                                        excelWorkbook.SaveAs(filePath);
                                        excelWorkbook.Close(false, null, null);

                                        SetSuccess("Xuất file thành công");
                                    }
                                    catch { SetFailure("Xuất file thất bại"); }
                                }
                                else
                                    SetFailure("Mẫu không tồn tại");
                            }
                        }
                        else
                            SetFailure("Không có dữ liệu");
                        #endregion VoucherHetSoDu
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherHetHanSuDung)
                    {
                        #region VoucherHetHanSuDung
                        List<sp_VoucherHetHanSuDungResult> ListResult = new vDoanhThuBusiness()
                            .VoucherHetHanSuDung(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue)
                            , clsPublicVar.ChiNhanh.Ma, Cast.ToInt(cboNganh.EditValue), Cast.ToInt(cboNhanVien.EditValue)
                            , Cast.ToInt(cboKhachHang.EditValue), Cast.ToString(txtMaThe.EditValue), Cast.ToInt(cboVoucher.EditValue));

                        if (ListResult != null && ListResult.Count > 0)
                        {
                            //..save export file
                            SaveFileDialog cdlg = new SaveFileDialog();
                            cdlg.FileName = "voucher-het-han-su-dung";
                            cdlg.Filter = "Excel File|*.xlsx";

                            if (cdlg.ShowDialog() == DialogResult.OK)
                            {
                                string sPathTemplate = QLVCConst.PathTemplate + @"\VoucherHetHanSuDung.xlsx";
                                string filePath = cdlg.FileName;

                                if (System.IO.File.Exists(sPathTemplate))
                                {
                                    //Copy Header Tittle from Example To New Template 
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                                        0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                                        true, false, 0, true, false, false);

                                    //Set sheet 1
                                    Excel.Worksheet excelWorksheet = null;
                                    string sheetName = "Report";

                                    Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                                    excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                                    //Add header
                                    SetValue(excelWorksheet, "D2", string.Format("Voucher hết hạn sử dụng từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                                        , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                                    //Add detail
                                    int IndexRow = 5, i = 0, IndexRowStart = 5;
                                    int iRows = ListResult.Count;
                                    int iColumns = 12;
                                    var data = new object[iRows, iColumns];

                                    foreach (sp_VoucherHetHanSuDungResult item in ListResult)
                                    {
                                        data[i, 0] = i + 1;
                                        data[i, 1] = item.ChiNhanhBan;
                                        data[i, 2] = item.NganhBan;
                                        data[i, 3] = item.TenVoucher;
                                        data[i, 4] = item.MaThe;
                                        data[i, 5] = item.TenKH;
                                        data[i, 6] = item.CMND;
                                        data[i, 7] = item.DienThoai;
                                        data[i, 8] = item.GiaTriVoucher;
                                        data[i, 9] = item.SoDu;
                                        data[i, 10] = item.NgayHieuLuc;
                                        data[i, 11] = item.NgayHetHan;

                                        IndexRow++;
                                        i++;
                                    }

                                    //Set border
                                    SetBorder(excelWorksheet, "A" + IndexRowStart, "L" + (IndexRow - 1));

                                    try
                                    {
                                        var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                                        var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                                        var writeRange = excelWorksheet.Range[startCell, endCell];
                                        writeRange.Value2 = data;

                                        //..save export file      
                                        excelWorkbook.SaveAs(filePath);
                                        excelWorkbook.Close(false, null, null);

                                        SetSuccess("Xuất file thành công");
                                    }
                                    catch { SetFailure("Xuất file thất bại"); }
                                }
                                else
                                    SetFailure("Mẫu không tồn tại");
                            }
                        }
                        else
                            SetFailure("Không có dữ liệu");
                        #endregion VoucherHetHanSuDung
                    }
                    else if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherDaHuy)
                    {
                        #region VoucherHetHanSuDung
                        List<sp_BaoCaoVoucer_HuyResult> ListResult = new vDoanhThuBusiness()
                            .VoucherHuy(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue));

                        if (ListResult != null && ListResult.Count > 0)
                        {
                            //..save export file
                            SaveFileDialog cdlg = new SaveFileDialog();
                            cdlg.FileName = "voucher-da-huy";
                            cdlg.Filter = "Excel File|*.xlsx";

                            if (cdlg.ShowDialog() == DialogResult.OK)
                            {
                                string sPathTemplate = QLVCConst.PathTemplate + @"\VoucherDaHuy.xlsx";
                                string filePath = cdlg.FileName;

                                if (System.IO.File.Exists(sPathTemplate))
                                {
                                    //Copy Header Tittle from Example To New Template 
                                    Excel.Application excelApp = new Excel.Application();
                                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                                        0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                                        true, false, 0, true, false, false);

                                    //Set sheet 1
                                    Excel.Worksheet excelWorksheet = null;
                                    string sheetName = "Report";

                                    Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                                    excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                                    //Add header
                                    SetValue(excelWorksheet, "A1", string.Format("Voucher đã huỷ từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                                        , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                                    //Add detail
                                    int IndexRow = 5, i = 0, IndexRowStart = 5;
                                    int iRows = ListResult.Count;
                                    int iColumns = 30;
                                    var data = new object[iRows, iColumns];

                                    foreach (sp_BaoCaoVoucer_HuyResult item in ListResult)
                                    {
                                        data[i, 0] = i + 1;
                                        data[i, 1] = item.TenChiNhanh;
                                        data[i, 2] = item.TenNganh;
                                        data[i, 3] = item.KhachHang;
                                        data[i, 4] = item.TenVoucher;
                                        data[i, 5] = item.MaVach;
                                        data[i, 6] = item.MaThe;
                                        data[i, 7] = item.GiaTri;
                                        data[i, 8] = item.SoDu;
                                        data[i, 9] = item.ThanhTien;
                                        data[i, 10] = item.NgayHieuLuc;
                                        data[i, 11] = item.NgayHetHan;
                                        data[i, 12] = item.NgayKichHoat;
                                        data[i, 13] = item.TrangThai;
                                        data[i, 14] = item.GhiChu;
                                        data[i, 15] = item.NguoiTao;
                                        data[i, 16] = item.NgayTao;
                                        data[i, 17] = item.NguoiSua;
                                        data[i, 18] = item.NgaySua;
                                        data[i, 19] = item.NguoiXoa;
                                        data[i, 20] = item.NgayXoa;


                                        IndexRow++;
                                        i++;
                                    }

                                    //Set border
                                    SetBorder(excelWorksheet, "A" + IndexRowStart, "V" + (IndexRow - 1));

                                    try
                                    {
                                        var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                                        var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                                        var writeRange = excelWorksheet.Range[startCell, endCell];
                                        writeRange.Value2 = data;

                                        //..save export file      
                                        excelWorkbook.SaveAs(filePath);
                                        excelWorkbook.Close(false, null, null);

                                        SetSuccess("Xuất file thành công");
                                    }
                                    catch { SetFailure("Xuất file thất bại"); }
                                }
                                else
                                    SetFailure("Mẫu không tồn tại");
                            }
                        }
                        else
                            SetFailure("Không có dữ liệu");
                        #endregion VoucherHetHanSuDung
                    }
                }
            }
        }

        private void ExportLichSuThanhToan(LoaiVoucher LoaiVoucher)
        {
            List<sp_LichSuThanhToanResult> ListResult = new vDoanhThuBusiness()
                .LichSuThanhToan(Cast.ToDateTime(dptTuNgay.EditValue), Cast.ToDateTime(dptDenNgay.EditValue)
                , clsPublicVar.ChiNhanh.Ma, Cast.ToInt(cboNganh.EditValue), Cast.ToInt(cboNhanVien.EditValue)
                , Cast.ToInt(cboKhachHang.EditValue), Cast.ToString(txtMaThe.EditValue), Cast.ToInt(cboVoucher.EditValue)
                , Enum.GetName(typeof(LoaiVoucher), LoaiVoucher));

            if (ListResult != null && ListResult.Count > 0)
            {
                //..save export file
                SaveFileDialog cdlg = new SaveFileDialog();
                cdlg.FileName = "lich-su-thanh-toan";
                cdlg.Filter = "Excel File|*.xlsx";

                if (cdlg.ShowDialog() == DialogResult.OK)
                {
                    string sPathTemplate = QLVCConst.PathTemplate + @"\LichSuThanhToan.xlsx";
                    string filePath = cdlg.FileName;

                    if (System.IO.File.Exists(sPathTemplate))
                    {
                        //Copy Header Tittle from Example To New Template 
                        Excel.Application excelApp = new Excel.Application();
                        Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sPathTemplate,
                            0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                            true, false, 0, true, false, false);

                        //Set sheet 1
                        Excel.Worksheet excelWorksheet = null;
                        string sheetName = "Report";

                        Excel.Sheets excelSheets = excelWorkbook.Worksheets;
                        excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(sheetName);

                        //Add header
                        SetValue(excelWorksheet, "F2", string.Format("Lịch sử thanh toán của Khách hàng từ ngày {0:dd/MM/yyyy} đến ngày {1:dd/MM/yyyy}"
                            , dptTuNgay.EditValue, dptDenNgay.EditValue), true);

                        //Add detail
                        int IndexRow = 5, i = 0, IndexRowStart = 5;
                        int iRows = ListResult.Count;
                        int iColumns = 20;
                        var data = new object[iRows, iColumns];

                        foreach (sp_LichSuThanhToanResult item in ListResult)
                        {
                            data[i, 0] = i + 1;
                            data[i, 1] = item.ChiNhanhBan;
                            data[i, 2] = item.NganhBan;
                            data[i, 3] = item.ChiNhanhSuDung;
                            data[i, 4] = item.NganhSuDung;
                            data[i, 5] = item.TenVoucher;
                            data[i, 6] = item.MaThe;
                            data[i, 7] = item.TenKH;
                            data[i, 8] = item.CMND;
                            data[i, 9] = item.DienThoai;
                            data[i, 10] = item.GiaTriVoucher;
                            data[i, 11] = item.GiaTriThanhToan;
                            data[i, 12] = item.SoDu;
                            data[i, 13] = item.NgayHieuLuc;
                            data[i, 14] = item.NgayHetHan;
                            data[i, 15] = item.NgayThanhToan;
                            data[i, 16] = item.NhanVienThanhToan;
                            data[i, 17] = item.GhiChuBanHang;
                            data[i, 18] = item.GiaTriThanhToan - item.PhuThu;

                            IndexRow++;
                            i++;
                        }

                        //Set border
                        SetBorder(excelWorksheet, "A" + IndexRowStart, "S" + (IndexRow - 1));

                        try
                        {
                            var startCell = (Excel.Range)excelWorksheet.Cells[IndexRowStart, 1];
                            var endCell = (Excel.Range)excelWorksheet.Cells[iRows + IndexRowStart - 1, iColumns];
                            var writeRange = excelWorksheet.Range[startCell, endCell];
                            writeRange.Value2 = data;

                            //..save export file      
                            excelWorkbook.SaveAs(filePath);
                            excelWorkbook.Close(false, null, null);

                            SetSuccess("Xuất file thành công");
                        }
                        catch { SetFailure("Xuất file thất bại"); }
                    }
                    else
                        SetFailure("Mẫu không tồn tại");
                }
            }
            else
                SetFailure("Không có dữ liệu");
        }

        private bool KiemTraDieuKien()
        {
            if (Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToan
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToanTheoGoiSuat
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.LichSuThanhToanTheoTien
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.DoanhThuTheoNgayDangKy
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.DoanhThuTheoNgayThanhToan
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherCuaKhachHang
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherDangSuDung
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherHetSoDu
                || Cast.ToInt(cboLoaiBaoCao.EditValue) == (int)Quyen.VoucherHetHanSuDung)
            {
                if (dptTuNgay.EditValue == null)
                {
                    SetFailure("Bạn vui lòng chọn ngày cần xem báo cáo");
                    dptTuNgay.Focus();
                    return false;
                }

                if (dptDenNgay.EditValue == null)
                {
                    SetFailure("Bạn vui lòng chọn ngày cần xem báo cáo");
                    dptDenNgay.Focus();
                    return false;
                }
            }

            return true;
        }

        private static void SetBorder(Excel.Worksheet excelWorksheet, string FromCell, string ToCell)
        {
            excelWorksheet.get_Range(FromCell, ToCell).Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
        }

        // Set Value for detail
        protected void SetValue(Excel.Worksheet excelWorksheet, string CellName, object value)
        {
            SetValue(excelWorksheet, CellName, value, false);
        }

        // Set Bold value for Group Value   
        protected void SetValue(Excel.Worksheet excelWorksheet, string CellName, object value, bool bold)
        {
            excelWorksheet.get_Range(CellName, misValue).Formula = value;
            excelWorksheet.get_Range(CellName, misValue).Font.Bold = bold;
        }
    }
}
