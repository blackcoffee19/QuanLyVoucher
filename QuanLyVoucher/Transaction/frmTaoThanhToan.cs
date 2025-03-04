﻿using Base.AppliBase;
using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
using Base.Common;
using Base.DevExpressEx.Utility;
using Business;
using Business.Globals;
using Common.Globals;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
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
    public partial class frmTaoThanhToan : BaseFormOnGrid<vTaoThanhToan, vTaoThanhToanBusiness, QLVCDataContext>
    {
        private int m_MenuID = 0, m_SoDu = 0;
        private ValidateUtis Valid = new ValidateUtis();
        private string m_sCode = "";
        private DateTime m_NgayTao = DateTime.Now;

        public frmTaoThanhToan()
        {
            InitializeComponent();
        }

        public frmTaoThanhToan(int MenuID)
        {
            InitializeComponent();

            //Set focus
            txtMaVach.Focus();

            //..
            ActionsDeny.AddRange(new[] { ButtonAction.Add, ButtonAction.Delete });

            //..
            m_MenuID = MenuID;

            //..
            List<vQuyen> ListQuyen = clsPublicVar.QuyenCuaUser.Where(t => t.MaMenu == m_MenuID).ToList();
            bool bThanhToanVoucherHetHan = ListQuyen.Any(t => t.MaQuyen == (int)Quyen.ThanhToanVoucherHetHan);
            layoutVoucherHetHan.Visibility = bThanhToanVoucherHetHan ? LayoutVisibility.Always : LayoutVisibility.Never;
            layoutChonTatCa.Visibility = layoutVoucherHetHan.Visibility;

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cMaKH", "Khách hàng", "MaKH", false, 120) { _isReadOnly = true },
                                new DataFieldColumn("cMaVoucher", "Voucher", "MaVoucher", false, 150) { _isReadOnly = true },
                                new DataFieldColumn("cNgayHetHan", "Ngày hết hạn", "NgayHetHan", false, 50) { _isReadOnly = true, _formatType = FormatType.DateTime, _formatString = QLVCConst.DateFormat },
                                new DataFieldColumn("cSoDu", "Số dư", "SoDu", false, 50) { _isReadOnly = true, _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat });

            if (bThanhToanVoucherHetHan)
                GvMain.ExAddColumns(new DataFieldColumn("cChonEx", "Chọn", "ChonEx", false, 10));

            //..specify field what use combobox
            GvMain.ExSetSearchColumnCombobox("MaKH", "Ten", "Ma", new Base<tbl_KhachHang>().Get().ToList(), 450, 300
                , new LookUpColumnInfo("Ten", 200, "Tên khách hàng")
                , new LookUpColumnInfo("CMND", 50, "CMND")
                , new LookUpColumnInfo("DiaChi", 200, "Địa chỉ"));

            GvMain.ExSetSearchColumnCombobox("MaVoucher", "Ten", "Ma", new Base<tbl_Voucher>().Get().ToList(), 350, 300
                , new LookUpColumnInfo("Ten", 250, "Tên voucher")
                , new LookUpColumnInfo("GiaTri", 100, "Giá trị voucher"));

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..
            panContent.Controls.Add(GMain);

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..define delete
            AutoGenFunctionDelete();            

            //..
            txtMaVach.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    LoadDataGrid();

                    if (GvMain.DataRowCount == 0)
                    {
                        tbl_BanHang drBH = new Base<tbl_BanHang>().First(t => t.MaVach == Cast.ToString(txtMaVach.EditValue)
                            && !(t.DaXoa ?? false));
                        DateTime currDate = DataUtils.GetDate();

                        if (drBH != null)
                        {
                            if (!(drBH.TrangThai ?? false))
                                SetFailure("Voucher chưa được kích hoạt");
                            else if (drBH.NgayHieuLuc.Value.Date > currDate.Date)
                                SetFailure("Voucher chưa đến ngày sử dụng");
                            else if (drBH.NgayHetHan.Value.Date < currDate.Date)
                                SetFailure("Voucher đã hết hạn sử dụng");
                            else if (drBH.SoDu <= 0)
                                SetFailure("Voucher đã hết số dư");
                            else
                                SetFailure("Voucher không hợp lệ, vui lòng liên hệ admin để được hỗ trợ");
                        }
                        else
                            SetFailure("Voucher không tồn tại");
                        
                        txtMaVach.Focus();
                        AcceptButton = null;
                    }
                    else
                    {
                        txtGiaTriSuDung.Focus();
                        AcceptButton = btnTaoThanhToan;
                    }
                }
            };

            //..
            txtMaVach.EditValueChanged += (sender, e) =>
            {
                if (AcceptButton != null)
                    AcceptButton = null;
            };

            //..
            chkTTVoucherHetHan.EditValueChanged += (sender, e) => LoadDataGrid();

            //..
            chkChonTatCa.CheckedChanged += (sender, e) =>
            {
                GvMain.PostEditor();

                for (int i = 0; i < GvMain.RowCount; i++)
                {
                    vTaoThanhToan dr = GvMain.GetRow(i) as vTaoThanhToan;
                    if (dr != null)
                        GvMain.SetRowCellValue(i, "ChonEx", chkChonTatCa.Checked);
                }
            };
        }

        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvMain, QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }

        private void btnTaoThanhToan_Click(object sender, EventArgs e)
        {
            if (!chkTTVoucherHetHan.Checked)
            {
                Valid.AddValidKeysExist(txtMaVach, "Bạn vui lòng nhập mã vạch", t => Cast.ToString(txtMaVach.EditValue) == "", "");
                Valid.AddValidKeysExist(txtGiaTriSuDung, "Bạn vui lòng nhập giá trị sử dụng", t => Cast.ToInt(txtGiaTriSuDung.EditValue) <= 0, "");
                Valid.AddValidKeysExist(txtGiaTriSuDung, "Giá trị sử dụng vượt quá số dư", t =>
                {
                    vTaoThanhToan dr = GvMain.ExGetCurrentDataRow() as vTaoThanhToan;
                    if (dr != null && Cast.ToInt(txtGiaTriSuDung.EditValue) > dr.SoDu)
                    {
                        txtGiaTriSuDung.Focus();
                        return true;
                    }
                    return false;
                }, "");

                if (!Valid.ValidateOK())
                    return;
            }

            vTaoThanhToan drTTT = GvMain.ExGetCurrentDataRow() as vTaoThanhToan;

            if (drTTT != null)
            {
                if (SetConfirm("Bạn chắc chắn muốn tạo thanh toán?", "Thông báo") == DialogResult.OK)
                {
                    //Luu du lieu
                    m_NgayTao = DataUtils.GetDate();

                    if (chkTTVoucherHetHan.Checked)
                    {
                        bool bDaTaoThanhToan = false;

                        for (int i = 0; i < GvMain.DataRowCount; i++)
                        {
                            vTaoThanhToan dr = GvMain.GetRow(i) as vTaoThanhToan;
                            
                            if (dr != null && dr.ChonEx)
                            {
                                m_NgayTao = DataUtils.GetDate();
                                m_sCode = string.Format("{0:ddMMyyHHmmssfff}", m_NgayTao);
                                bDaTaoThanhToan = true;
                                DataBusiness.XuLyTaoThanhToan(m_sCode, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma, dr.MaBanHang
                                    , (dr.SoDu ?? 0), 0, 0, txtGhiChu.Text, clsPublicVar.User.Ma, m_NgayTao);
                            }
                        }

                        if (!bDaTaoThanhToan)
                        {
                            m_sCode = string.Format("{0:ddMMyyHHmmssfff}", m_NgayTao);
                            DataBusiness.XuLyTaoThanhToan(m_sCode, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma, drTTT.MaBanHang
                                , (drTTT.SoDu ?? 0), 0, 0, txtGhiChu.Text, clsPublicVar.User.Ma, m_NgayTao);
                        }
                    }
                    else
                    {
                        int GiaTriSuDung = Cast.ToInt(txtGiaTriSuDung.EditValue);
                        int PhuThu = Cast.ToInt(txtPhuThu.EditValue);
                        m_sCode = string.Format("{0:ddMMyyHHmmssfff}", m_NgayTao);

                        vTaoThanhToan drXuLy = new Base<vTaoThanhToan>().First(t => t.MaBanHang == drTTT.MaBanHang);

                        if (drXuLy != null)
                        {
                            m_SoDu = drXuLy.SoDu ?? 0;
                            DataBusiness.XuLyTaoThanhToan(m_sCode, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma, drXuLy.MaBanHang
                                , GiaTriSuDung, m_SoDu - GiaTriSuDung, PhuThu, txtGhiChu.Text, clsPublicVar.User.Ma, m_NgayTao);

                            //In hoa don
                            new ThongTinHoaDon().XuLyMayIn(TaoHoaDon);
                        }
                    }
                }
                else
                {
                    txtMaVach.Focus();
                    AcceptButton = null;
                    return;
                }
            }
            else
            {
                SetFailure("Bạn vui lòng chọn phiếu bán hàng phù hợp trước khi tạo thanh toán");
                txtMaVach.Focus();
                AcceptButton = null;
                return;
            }

            //Done
            this.Close();
        }

        private void TaoHoaDon(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            //..
            vTaoThanhToan drTTT = GvMain.ExGetCurrentDataRow() as vTaoThanhToan;
            if (drTTT != null)
                new ThongTinHoaDon().ChiTietHoaDonThanhToan(graphic, m_sCode, drTTT.TenKH, m_NgayTao, drTTT.TenVoucher, m_SoDu
                    , Cast.ToInt(txtGiaTriSuDung.EditValue), m_SoDu - Cast.ToInt(txtGiaTriSuDung.EditValue), drTTT.MaThe
                    , Cast.ToInt(txtPhuThu.EditValue));
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadDataGrid()
        {
            GMain.DataSource = null;

            if (chkTTVoucherHetHan.Checked)
                GMain.DataSource = new vTaoThanhToanBusiness().GetDataVoucherHetHan(clsPublicVar.ChiNhanh.Ma);
            else
                GMain.DataSource = new vTaoThanhToanBusiness().GetData(clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma
                    , Cast.ToString(txtMaVach.EditValue));

            //..
            txtMaVach.Focus();

            LoadHinh();
        }

        private void LoadHinh()
        {
            try
            {
                vTaoThanhToan drTTT = GvMain.ExGetCurrentDataRow() as vTaoThanhToan;
                tbl_KhachHang dr = new tbl_KhachHangBusiness().First(t => t.Ma == drTTT.MaKH);
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

        public override void Browse()
        {
            GMain.DataSource = null;
            GridUtil.DataMode = DataModeType.Edit;
        }
    }
}
