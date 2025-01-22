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

namespace QuanLyVoucher.Category
{
    public partial class usrKhachHangToolTimKiem : SearchPanel
    {
        private tbl_VoucherBusiness tbl_VoucherB = new tbl_VoucherBusiness();

        public usrKhachHangToolTimKiem()
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

            SC.Add(new SearchControl(txtHoTen
                , () => null
                , () => null
                , tHoTen => string.Format(" Họ tên: {0};", tHoTen)));

            SC.Add(new SearchControl(txtSoDienThoai
                , () => null
                , () => null
                , tSoDienThoai => string.Format(" Số điện thoại: {0};", tSoDienThoai)));

            SC.Add(new SearchControl(txtCMND
                , () => null
                , () => null
                , tCMND => string.Format(" CMND: {0};", tCMND)));

            SC.Add(new SearchControl(txtDiaChi
                , () => null
                , () => null
                , tDiaChi => string.Format(" Địa chỉ: {0};", tDiaChi)));

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

            txtHoTen.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            txtCMND.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            txtSoDienThoai.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btnOK.PerformClick();
            };

            txtDiaChi.KeyPress += (sender, e) =>
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
