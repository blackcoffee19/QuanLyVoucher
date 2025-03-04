﻿using Base.AppliBase;
using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;

namespace QuanLyVoucher.Category
{
    public partial class frmVoucher : BaseFormOnGrid<tbl_Voucher, tbl_VoucherBusiness, QLVCDataContext>
    {
        private frmVoucherRole m_frmVoucherRole;
        private tbl_ChiNhanhBusiness tbl_ChiNhanhB = new tbl_ChiNhanhBusiness();

        public frmVoucher()
        {
            InitializeComponent();

            ConfigVoucher();
            ConfigVoucherRole();
            btnChiNhanhNganhThemNhanh.Enabled = false;
            cboChiNhanhNganh.Enabled = btnChiNhanhNganhThemNhanh.Enabled;

            //..
            var Nganh = tbl_ChiNhanhB.Get()
                .Join(tbl_ChiNhanhB.DC.GetTable<tbl_Nganh>(), cn => cn.Ma, n => n.MaChiNhanh, (cn, n) => new { cn, n })
                .Where(t => (t.cn.TrangThai ?? false) && !(t.cn.DaXoa ?? false)
                    && (t.n.TrangThai ?? false) && !(t.n.DaXoa ?? false)
                    && t.cn.Ma == clsPublicVar.ChiNhanh.Ma)
                .Select(t => new { Ma = t.n.Ma, Ten = t.cn.Ten + " - " + t.n.Ten })
                .OrderBy(t => t.Ten).ToList();
            cboChiNhanhNganh.Properties.DropDownRows = 20;
            cboChiNhanhNganh.Properties.DataSource = Nganh;
            cboChiNhanhNganh.Properties.ValueMember = "Ma";
            cboChiNhanhNganh.Properties.DisplayMember = "Ten";
        }

        private void ConfigVoucher()
        {
            //..
            FunctionLoadData = "GetData";

            //..define fields what showed on grid
            GvMain.ExAddColumns(new DataFieldColumn("cLoaiVoucher", "Loại voucher", "LoaiVoucher", false, 50),
                                new DataFieldColumn("cTen", "Tên voucher", "Ten", false, 150),
                                new DataFieldColumn("cNhomVoucherID", "Nhóm voucher", "NhomVoucherID", false, 80),
                                new DataFieldColumn("cGiaTri", "Giá trị sử dụng", "GiaTri", false, 50) { _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat },
                                new DataFieldColumn("cLoaiThoiHan", "Loại thời hạn", "LoaiThoiHan", false, 70),
                                new DataFieldColumn("cGiaTriThoiHan", "Giá trị thời hạn", "GiaTriThoiHan", false, 50) { _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat },
                                new DataFieldColumn("cDonGia", "Đơn giá", "DonGia", false, 50) { _formatType = FormatType.Numeric, _formatString = QLVCConst.NumberFormat },
                                new DataFieldColumn("cTrangThai", "Trạng thái", "TrangThai", false, 50));

            //..specify field what use combobox
            List<BasicItem> lLoaiVoucher = new List<BasicItem>(new[]
            {
                new BasicItem("Tiền", Enum.GetName(typeof(LoaiVoucher), LoaiVoucher.Tien)),
                new BasicItem("Gói suất", Enum.GetName(typeof(LoaiVoucher), LoaiVoucher.GoiSuat))
            });
            GvMain.ExSetSearchColumnCombobox("LoaiVoucher", "Text", "Value", lLoaiVoucher.ToList(), 350, 350
                , new LookUpColumnInfo("Text", 100, "Loại voucher"));


            var cboNhomVoucher = GvMain.ExSetSearchColumnCombobox("NhomVoucherID", "Ten", "Ma", new Base<tbl_NhomVoucher>().Get().ToList(), 350, 300
                , new LookUpColumnInfo("Ten", 250, "Tên nhóm"));

            cboNhomVoucher.QueryPopUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;

                cboEdit.Properties.DataSource = new Base<tbl_NhomVoucher>().Get(t => (t.TrangThai ?? false) && !(t.DaXoa ?? false))
                    .OrderByDescending(t => t.Ten).ToList();
            };

            cboNhomVoucher.CloseUp += (sender, e) =>
            {
                var cboEdit = sender as SearchLookUpEdit;

                cboEdit.Properties.DataSource = new Base<tbl_NhomVoucher>().Get().ToList();
            };

            List<BasicItem> lLoaiThoiHan = new List<BasicItem>(new[]
            {
                new BasicItem("Ngày", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Ngay)),
                new BasicItem("Tuần", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Tuan)),
                new BasicItem("Tháng", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Thang)),
                new BasicItem("Năm", Enum.GetName(typeof(LoaiThoiHan), LoaiThoiHan.Nam))
            });
            GvMain.ExSetSearchColumnCombobox("LoaiThoiHan", "Text", "Value", lLoaiThoiHan.ToList(), 350, 350
                , new LookUpColumnInfo("Text", 100, "Loại thời hạn"));

            //view record of form User
            GvMain.ViewRowNumber = true;

            //..
            panVoucher.Controls.Add(GMain);

            //..just call this function, base will auto detect for field what can be editable in add mode or edit mode
            AutoSetColumnEditor();

            //..set auto values
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiTao", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgayTao", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.AddNew));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NguoiSua", clsPublicVar.User.Ma, dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("NgaySua", dr => DataUtils.GetDate(), dr => GridUtil.DataMode == DataModeType.Edit));
            GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("TrangThai", true, dr => GridUtil.DataMode == DataModeType.AddNew));

            //..set validate
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("LoaiVoucher", "Bạn chưa chọn loại voucher", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("Ten", "Bạn chưa nhập tên voucher", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnKeysExistError("Ten", "Tên voucher đã tồn tại", "", AutoGenExistedOnAddNew("Ten", clsPublicVar.FieldCheckDelete)));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("GiaTri", "Bạn chưa nhập giá trị voucher", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("LoaiThoiHan", "Bạn chưa chọn loại thời gian", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("GiaTriThoiHan", "Bạn chưa nhập giá trị thời hạn", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnBlankError("DonGia", "Bạn chưa nhập đơn giá", ""));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("GiaTri", "Giá trị phải lớn hơn 0", "",
                dr => (dr != null && ((tbl_Voucher)dr).GiaTri > 0) ? true : false));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("GiaTriThoiHan", "Giá trị thời hạn phải lớn hơn 0", "",
                dr => (dr != null && ((tbl_Voucher)dr).GiaTriThoiHan > 0) ? true : false));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("DonGia", "Đơn giá phải lớn hơn 0", "",
                dr => (dr != null && ((tbl_Voucher)dr).DonGia > 0) ? true : false));
            //GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("GiaTri", "Giá trị không được phép chỉnh sửa vì voucher đã được bán", "",
            //    dr => CoTheLuuGiaTriVoucher(((tbl_Voucher)dr))));
            //GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("LoaiThoiHan", "Loại thời hạn không được phép chỉnh sửa vì voucher đã được bán", "",
            //    dr => CoTheLuuLoaiThoiHanVoucher(((tbl_Voucher)dr))));
            //GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("GiaTriThoiHan", "Giá trị thời hạn không được phép chỉnh sửa vì voucher đã được bán", "",
            //    dr => CoTheLuuGiaTriThoiHanVoucher(((tbl_Voucher)dr))));
            GridUtil.GridValid.DataColumnErrors.Add(new DataColumnDynamicRulesError("Ten", "Tên voucher đã tồn tại", "", dr =>
            {
                bool valid = true;

                if (dr != null && GridUtil.DataMode == DataModeType.Edit)
                {
                    tbl_Voucher drV = new Base<tbl_Voucher>().First(t => t.Ten == ((tbl_Voucher)dr).Ten && !(t.DaXoa ?? false)
                        && t.Ma != ((tbl_Voucher)dr).Ma);

                    if (drV != null)
                        valid = false;
                }
                return valid;
            }));

            //..define delete
            AutoGenFunctionDelete();

            //.x. set children
            SetChildren<tbl_BanHang>("Ma", "MaVoucher", false, clsPublicVar.FieldCheckDelete);
            SetChildren<tbl_VoucherRole>("Ma", "MaVoucher", false, clsPublicVar.FieldCheckDelete);

            //just allow edit detail when master in view mode only.
            GridUtil.DataModeChanged += (modType) =>
            {
                m_frmVoucherRole.GMain.Enabled = (modType == DataModeType.View);
            };
        }

        private void ConfigVoucherRole()
        {
            m_frmVoucherRole = new frmVoucherRole() { Name = "frmVoucherRole" };
            m_frmVoucherRole.FunctionLoadData = "GetFollowVoucher";
            m_frmVoucherRole.FuLoadDataParas = () =>
            {
                tbl_Voucher focusedVoucher = GvMain.ExGetCurrentDataRow() as tbl_Voucher;
                if (focusedVoucher != null)
                    return new[] { (object)focusedVoucher.Ma };
                return new[] { (object)int.MinValue };
            };

            //..set auto values
            m_frmVoucherRole.GridUtil.DataFieldAutoValues.Add(new DataFieldAutoValue("MaVoucher", dr => ((tbl_Voucher)this.GvMain.ExGetCurrentDataRow()).Ma));

            //include child form to master form
            m_frmVoucherRole.TopLevel = false;
            m_frmVoucherRole.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            m_frmVoucherRole.ControlBox = false;
            m_frmVoucherRole.Dock = DockStyle.Fill;
            panVoucherRole.Controls.Add(m_frmVoucherRole);

            //config child form behavior
            this.GvMain.FocusedRowChanged += (sender, e) =>
            {
                if (this.GridUtil.DataMode == DataModeType.View)
                {
                    m_frmVoucherRole.Browse();

                    //..
                    this.GridUtil.DataMode = GridUtil.DataMode;
                }
            };

            m_frmVoucherRole.GMain.Enter += (sender, e) =>
            {
                tbl_Voucher currentVoucher = GvMain.ExGetCurrentDataRow() as tbl_Voucher;
                m_frmVoucherRole.m_MaVoucher = (currentVoucher != null ? currentVoucher.Ma : 0);

                this.SetChildFormDeactivate();
                BaseFormFocus = m_frmVoucherRole;
                m_frmVoucherRole.SetChildFormActivated();
            };

            m_frmVoucherRole.GMain.Leave += (sender, e) =>
            {
                m_frmVoucherRole.SetChildFormDeactivate();
                BaseFormFocus = this;
                this.SetChildFormActivated();
            };

            //just allow edit detail when master in view mode only.
            m_frmVoucherRole.GridUtil.DataModeChanged += (modType) =>
            {
                this.GMain.Enabled = (modType == DataModeType.View);
                btnChiNhanhNganhThemNhanh.Enabled = modType == DataModeType.AddNew;
                cboChiNhanhNganh.Enabled = btnChiNhanhNganhThemNhanh.Enabled;

                if (!btnChiNhanhNganhThemNhanh.Enabled)
                {
                    cboChiNhanhNganh.EditValue = null;
                    cboChiNhanhNganh.RefreshEditValue();
                }
            };
        }

        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);

            //..
            m_frmVoucherRole._parent = this._parent;
            m_frmVoucherRole.Show();

            //..
            SupportTool.SetDisplayRowStatus(this, _parent, GvMain, QLVCConst.RequestRecordStatus, clsPublicVar.SumRecord);
        }

        protected override void RaiseEventToChild(ButtonAction action)
        {
            //make sure toolbar mode use for master when grid control focused only
            if (GMain.Focused || !m_frmVoucherRole.GMain.Enabled)
                base.RaiseEventToChild(action);
        }

        public override void Delete()
        {
            UpdateDelete(clsPublicVar.FieldUpdateDelete());
        }

        public override void Browse()
        {
            base.Browse();

            m_frmVoucherRole.Browse();

            //..
            GridUtil.DataMode = GridUtil.DataMode;
        }

        private bool CoTheLuuGiaTriVoucher(tbl_Voucher dr)
        {
            bool kq = true;

            if (dr != null)
            {
                tbl_BanHang drBH = new Base<tbl_BanHang>().First(t => t.MaVoucher == dr.Ma);
                tbl_Voucher drV = new Base<tbl_Voucher>().First(t => t.Ma == dr.Ma);

                if (drBH == null || ( drV != null && dr.GiaTri == drV.GiaTri))
                    kq = true;
                else
                    kq = false;
            }

            return kq;
        }

        private bool CoTheLuuLoaiThoiHanVoucher(tbl_Voucher dr)
        {
            bool kq = true;

            if (dr != null)
            {
                tbl_BanHang drBH = new Base<tbl_BanHang>().First(t => t.MaVoucher == dr.Ma);
                tbl_Voucher drV = new Base<tbl_Voucher>().First(t => t.Ma == dr.Ma);

                if (drBH == null || ( drV != null && dr.LoaiThoiHan == drV.LoaiThoiHan))
                    kq = true;
                else
                    kq = false;
            }

            return kq;
        }

        private bool CoTheLuuGiaTriThoiHanVoucher(tbl_Voucher dr)
        {
            bool kq = true;

            if (dr != null)
            {
                tbl_BanHang drBH = new Base<tbl_BanHang>().First(t => t.MaVoucher == dr.Ma);
                tbl_Voucher drV = new Base<tbl_Voucher>().First(t => t.Ma == dr.Ma);

                if (drBH == null || (drV != null && dr.GiaTriThoiHan == drV.GiaTriThoiHan))
                    kq = true;
                else
                    kq = false;
            }

            return kq;
        }

        private void btnChiNhanhNganhThemNhanh_Click(object sender, EventArgs e)
        {
            if (cboChiNhanhNganh.EditValue != null)
            {
                tbl_VoucherRoleBusiness tbl_VoucherRoleB = new tbl_VoucherRoleBusiness();
                List<tbl_VoucherRole> lNganh = (m_frmVoucherRole.GvMain.GridControl.DataSource as IQueryable<tbl_VoucherRole>).ToList();
                string[] Nganh = Cast.ToString(cboChiNhanhNganh.EditValue).Split(';');

                tbl_Voucher drV = GvMain.ExGetCurrentDataRow() as tbl_Voucher;

                if (drV != null)
                {
                    foreach (string n in Nganh)
                    {
                        tbl_Nganh drN = tbl_VoucherRoleB.DC.GetTable<tbl_Nganh>().FirstOrDefault(t => t.Ma == Cast.ToInt(n.Trim()));

                        if (drN != null && !lNganh.Any(t => t.MaVoucher == drV.Ma && t.MaChiNhanh == drN.MaChiNhanh && t.MaNganh == drN.Ma))
                        {
                            tbl_VoucherRole voucherRole = new tbl_VoucherRole()
                            {
                                MaVoucher = drV.Ma,
                                MaChiNhanh = drN.MaChiNhanh,
                                MaNganh = drN.Ma,
                                TrangThai = true,
                                NguoiTao = clsPublicVar.User.Ma,
                                NgayTao = DataUtils.GetDate()
                            };
                            tbl_VoucherRoleB.Add(voucherRole, true);
                        };
                    }
                }

                m_frmVoucherRole.Browse();
                cboChiNhanhNganh.EditValue = null;
                cboChiNhanhNganh.RefreshEditValue();
            }
        }
    }
}
