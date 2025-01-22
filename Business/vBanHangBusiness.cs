﻿using Base.Common.Items;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class vBanHangBusiness : Base<vBanHang>
    {
        public IQueryable<vBanHang> GetData(int MaChiNhanh, int MaNganh, DateTime TuNgay, DateTime DenNgay, int MaKH, int MaVoucher
            , string MaThe, bool? KichHoat)
        {
            var q = Get(t => t.MaChiNhanh == MaChiNhanh && t.MaNganh == MaNganh
                && t.NgayTao.Value.Date >= TuNgay.Date && t.NgayTao.Value.Date <= DenNgay.Date);

            if (MaKH != 0)
                q = q.Where(t => t.MaKH == MaKH);

            if (MaVoucher != 0)
                q = q.Where(t => t.MaVoucher == MaVoucher);

            if (MaThe != "")
                q = q.Where(t => t.MaThe == MaThe);

            if (KichHoat != null)
                q = q.Where(t => t.TrangThai == KichHoat);

            return q.OrderByDescending(t => t.NgayTao);
        }

        public HandleState XuLyBanHang(int XuLy, bool KichHoat, int MaBanHang, int MaChiNhanh, int MaNganh, int MaKhachHang
            , int MaVoucher, string MaVach, int GiaTri, int SoDu, int ThanhTien, DateTime NgayHieuLuc
            , DateTime NgayHetHan, bool TrangThai, int NguoiXuLy, string MaThe, string GhiChu)
        {
            if (((QLVCDataContext)DC).sp_BanHang(XuLy, KichHoat, MaBanHang, MaChiNhanh, MaNganh, MaKhachHang, MaVoucher, MaVach
                , GiaTri, SoDu, ThanhTien, NgayHieuLuc, NgayHetHan, TrangThai, NguoiXuLy, MaThe, GhiChu) >= 0)
                return new HandleState();
            return new HandleState("Update failed");
        }

        public HandleState ThemBanHangBoSung(int MaBanHangCu, int ThanhTien, DateTime NgayHieuLuc, DateTime NgayHetHan
            , int NguoiXuly, string GhiChu, int MaVoucher, string MaVach, string MaThe, int GiaTriVoucher)
        {
            int? kq = -2;
            ((QLVCDataContext)DC).sp_BanHangBoSung(MaBanHangCu, ThanhTien, NgayHieuLuc, NgayHetHan, NguoiXuly, GhiChu, MaVoucher
                , MaVach, MaThe, GiaTriVoucher, ref kq);

            if (kq >= 0)
                return new HandleState();
            return new HandleState("Update failed");
        }
    }
}
