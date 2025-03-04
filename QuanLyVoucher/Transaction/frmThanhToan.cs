﻿using Base.AppliBase;
using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
using Base.Common.Items;
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;

namespace QuanLyVoucher.Transaction
{
    public partial class frmThanhToan : BaseFormOnGrid<vThanhToan, vThanhToanBusiness, QLVCDataContext>
    {
        private readonly int m_MenuID = clsPublicVar.MainMenuID;
        private bool bInHoaDon = false;

        public frmThanhToan()
        {
            InitializeComponent();

            //..
            bInHoaDon = clsPublicVar.QuyenCuaUser.Any(t => t.MaMenu == m_MenuID && t.MaQuyen == (int)Quyen.InLaiHoaDon);
            layoutInHoaDon.Visibility = bInHoaDon ? LayoutVisibility.Always : LayoutVisibility.Never;
            emptySpaceItem1.Visibility = layoutInHoaDon.Visibility;

            //..
            ConfigSearchBox();

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
                (object)uSearch.GetValue("txtMaThe")
            };

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cCode", "Số hóa đơn", "Code", false, 50) { _isReadOnly = true }, 
                                new DataFieldColumn("cMaKH", "Khách hàng", "MaKH", false, 70) { _isReadOnly = true },
                                new DataFieldColumn("cMaThe", "Mã thẻ", "MaThe", false, 30) { _isReadOnly = true },
                                new DataFieldColumn("cMaVoucher", "Voucher", "MaVoucher", false, 110) { _isReadOnly = true },
                                new DataFieldColumn("cGiaTriThanhToan", "Giá trị thanh toán", "GiaTriThanhToan", false, 40) { _isReadOnly = true, _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat },
                                new DataFieldColumn("cPhuThu", "Phụ thu", "PhuThu", false, 40) { _isReadOnly = true, _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat },
                                new DataFieldColumn("cSoDu", "Số dư", "SoDu", false, 40) { _isReadOnly = true, _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat },
                                new DataFieldColumn("cGhiChu", "Ghi chú", "GhiChu", false, 110),
                                new DataFieldColumn("cNguoiTao", "Người thanh toán", "NguoiTao", false, 50) { _isReadOnly = true },
                                new DataFieldColumn("cNgayTao", "Ngày thanh toán", "NgayTao", false, 50) { _isReadOnly = true, _formatType = FormatType.DateTime, _formatString = QLVCConst.DateTimeFormat });

            //..specify field what use combobox
            GvMain.ExSetSearchColumnCombobox("MaKH", "Ten", "Ma", new Base<tbl_KhachHang>().Get().ToList(), 450, 300
                , new LookUpColumnInfo("Ten", 200, "Tên khách hàng")
                , new LookUpColumnInfo("DienThoai", 125, "Số điện thoại")
                , new LookUpColumnInfo("CMND", 125, "CMND")
                //, new LookUpColumnInfo("DiaChi", 200, "Địa chỉ")
                );

            GvMain.ExSetSearchColumnCombobox("MaVoucher", "Ten", "Ma", new Base<tbl_Voucher>().Get().ToList(), 350, 300
                , new LookUpColumnInfo("Ten", 250, "Tên voucher")
                , new LookUpColumnInfo("GiaTri", 100, "Giá trị voucher"));

            GvMain.ExSetSearchColumnCombobox("NguoiTao", "HoTen", "Ma", new Base<sys_User>().Get().ToList(), 350, 300
                , new LookUpColumnInfo("HoTen", 250, "Tên nhân viên"));

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..
            panContent.Controls.Add(GMain);

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

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
            frmTaoThanhToan frm = new frmTaoThanhToan(m_MenuID);
            frm.ShowDialog();
            Browse();
        }

        public override void Save()
        {
            GvMain.PostEditor();

            if (GridUtil.DataMode == DataModeType.Edit)
            {
                for (int i = 0; i < GvMain.DataRowCount; i++)
                {
                    vThanhToan drTT = GvMain.GetRow(i) as vThanhToan;

                    if(drTT != null)
                        State = DataBusiness.XuLySuaThanhToan(drTT.Ma, drTT.GhiChu, clsPublicVar.User.Ma);
                }

                if (State.Success)
                    SetSuccess("Sửa thành công");
                else
                    SetFailure("Sửa thất bại");
            }

            Browse();
        }

        public override void Delete()
        {
            vThanhToan drTT = GvMain.ExGetCurrentDataRow() as vThanhToan;

            if (drTT != null)
            {
                if (SetConfirm("Bạn chắc chắn muốn xóa thanh toán?", "Thông báo") == DialogResult.OK)
                {
                    State = DataBusiness.XuLyXoaThanhToan(drTT.Ma, drTT.MaBanHang, drTT.GiaTriThanhToan, clsPublicVar.User.Ma);

                    if (State.Success)
                        SetSuccess("Xóa thành công");
                    else
                        SetFailure("Xóa thất bại");
                }
            }
            else
                SetFailure("Bạn vui lòng chọn thanh toán cần xóa");

            Browse();
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            //In hoa don
            vThanhToan drTT = GvMain.ExGetCurrentDataRow() as vThanhToan;
            if (drTT != null)
                new ThongTinHoaDon().XuLyMayIn(TaoHoaDon);
            else
                SetFailure("Bạn vui lòng chọn phiếu thanh toán muốn in lại");
        }

        private void TaoHoaDon(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            //..
            vThanhToan drTT = GvMain.ExGetCurrentDataRow() as vThanhToan;
            if (drTT != null)
                new ThongTinHoaDon().ChiTietHoaDonThanhToan(graphic, drTT.Code, drTT.TenKH, (drTT.NgayTao ?? DateTime.Now), drTT.TenVoucher
                    , drTT.GiaTriThanhToan + (drTT.SoDu ?? 0), drTT.GiaTriThanhToan, (drTT.SoDu ?? 0), drTT.MaThe, drTT.PhuThu ?? 0);
        }
    }
}
