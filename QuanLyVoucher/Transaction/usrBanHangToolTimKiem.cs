using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool.Search;
using Base.Common.Items;
using DevExpress.XtraEditors.Controls;
using Service;
using Base.Common;
using Business;
using DevExpress.XtraEditors;
using QuanLyVoucher.Globals;

namespace QuanLyVoucher.Transaction
{
    public partial class usrBanHangToolTimKiem : SearchPanel
    {
        private tbl_VoucherBusiness tbl_VoucherB = new tbl_VoucherBusiness();

        public usrBanHangToolTimKiem()
        {
            InitializeComponent();

            DateTime currDate = DataUtils.GetDate();

            SC.Add(new SearchControl(dptTuNgay
                , () => Cast.ToDateTime(string.Format("{0:yyyy/MM/dd 00:00:00}", currDate.AddMonths(-1).AddDays(1)))
                , () => null
                , dFromDate => string.Format("Từ ngày: {0:dd/MM/yyyy};", dFromDate)));

            SC.Add(new SearchControl(dptDenNgay
                , () => Cast.ToDateTime(string.Format("{0:yyyy/MM/dd 23:59:59}", currDate))
                , () => null
                , dToDate => string.Format(" Đến ngày: {0:dd/MM/yyyy};", dToDate)));

            SC.Add(new SearchControl(cboKhachHang
                , () => null
                , () => null
                , cKhachHang => string.Format(" Khách hàng: {0};", cKhachHang)));

            SC.Add(new SearchControl(cboVoucher
                , () => null
                , () => null
                , cVoucher => string.Format(" Voucher: {0};", cVoucher)));

            SC.Add(new SearchControl(txtMaThe
                , () => null
                , () => null
                , tMaThe => string.Format(" Mã thẻ: {0};", tMaThe)));

            SC.Add(new SearchControl(chkKichHoat
                , () => null
                , () => null
                , cKichHoat => string.Format(" Kích hoạt: {0};", cKichHoat)));

            //..
            cboVoucher.Build("Ten", "Ma", tbl_VoucherB.Get().ToList(), 350, 300
                , new LookUpColumnInfo("Ten", 250, "Tên voucher")
                , new LookUpColumnInfo("GiaTri", 100, "Giá trị voucher"));

            cboVoucher.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;
                tbl_VoucherB = new tbl_VoucherBusiness();

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
                , new LookUpColumnInfo("CMND", 50, "CMND")
                , new LookUpColumnInfo("DiaChi", 200, "Địa chỉ"));

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

            //default value
            Reset();
            Accept();

            //event
            List<string> IgnoreListClear = new List<string>();
            IgnoreListClear.Add("dptTuNgay");
            IgnoreListClear.Add("dptDenNgay");

            AppyNormalEvent(btnCancel, btnOK, btnReset, btnClear, IgnoreListClear);
            EnterCallButtonOK();
            FocusFirstControl();

            //..
            chkKichHoat.EditValue = null;
        }

        void EnterCallButtonOK()
        {
            dptTuNgay.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            dptDenNgay.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            cboKhachHang.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            cboVoucher.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            txtMaThe.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            chkKichHoat.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };
        }

        //focus first control when showpopup, click button
        void FocusFirstControl()
        {
            //default foscus first control when show popup
            this.Enter += (sender, e) => dptTuNgay.Select();

            //button
            btnCancel.Click += (sender, e) => dptTuNgay.Select();
            btnClear.Click += (sender, e) => dptTuNgay.Select();
            btnOK.Click += (sender, e) => dptTuNgay.Select();
            btnReset.Click += (sender, e) => dptTuNgay.Select();
        }
    }
}
