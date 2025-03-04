﻿using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class vDoanhThuBusiness : Base<vDoanhThu>
    {
        public List<sp_DoanhThuTheoNgayDangKyResult> DoanhThuTheoNgayDangKy(DateTime dtTuNgay, DateTime dtDenNgay, int MaChiNhanh
            , int MaNganh, int MaNhanVien, int MaKH, string MaThe, int MaVoucher)
        {
            return ((QLVCDataContext)DC).sp_DoanhThuTheoNgayDangKy(dtTuNgay, dtDenNgay, MaChiNhanh, MaNganh, MaNhanVien, MaKH, MaThe
                , MaVoucher).ToList();
        }

        public List<sp_DoanhThuTheoNgayThanhToanResult> DoanhThuTheoNgayThanhToan(DateTime dtTuNgay, DateTime dtDenNgay, int MaChiNhanh
            , int MaNganh, int MaNhanVien, int MaKH, string MaThe, int MaVoucher)
        {
            return ((QLVCDataContext)DC).sp_DoanhThuTheoNgayThanhToan(dtTuNgay, dtDenNgay, MaChiNhanh, MaNganh, MaNhanVien, MaKH, MaThe
                , MaVoucher).ToList();
        }

        public List<sp_LichSuThanhToanResult> LichSuThanhToan(DateTime dtTuNgay, DateTime dtDenNgay, int MaChiNhanh
            , int MaNganh, int MaNhanVien, int MaKH, string MaThe, int MaVoucher, string LoaiVoucher)
        {
            return ((QLVCDataContext)DC).sp_LichSuThanhToan(dtTuNgay, dtDenNgay, MaChiNhanh, MaNganh, MaNhanVien, MaKH, MaThe
                , MaVoucher, LoaiVoucher).ToList();
        }

        public List<sp_VoucherCuaKhachHangResult> VoucherCuaKhachHang(DateTime dtTuNgay, DateTime dtDenNgay, int MaChiNhanh
            , int MaNganh, int MaNhanVien, int MaKH, string MaThe, int MaVoucher)
        {
            return ((QLVCDataContext)DC).sp_VoucherCuaKhachHang(dtTuNgay, dtDenNgay, MaChiNhanh, MaNganh, MaNhanVien, MaKH, MaThe
                , MaVoucher).ToList();
        }

        public List<sp_VoucherDangSuDungResult> VoucherDangSuDung(DateTime dtTuNgay, DateTime dtDenNgay, int MaChiNhanh
            , int MaNganh, int MaNhanVien, int MaKH, string MaThe, int MaVoucher)
        {
            return ((QLVCDataContext)DC).sp_VoucherDangSuDung(dtTuNgay, dtDenNgay, MaChiNhanh, MaNganh, MaNhanVien, MaKH, MaThe
                , MaVoucher).ToList();
        }

        public List<sp_VoucherHetSoDuResult> VoucherHetSoDu(DateTime dtTuNgay, DateTime dtDenNgay, int MaChiNhanh
            , int MaNganh, int MaNhanVien, int MaKH, string MaThe, int MaVoucher)
        {
            return ((QLVCDataContext)DC).sp_VoucherHetSoDu(dtTuNgay, dtDenNgay, MaChiNhanh, MaNganh, MaNhanVien, MaKH, MaThe
                , MaVoucher).ToList();
        }

        public List<sp_VoucherHetHanSuDungResult> VoucherHetHanSuDung(DateTime dtTuNgay, DateTime dtDenNgay, int MaChiNhanh
            , int MaNganh, int MaNhanVien, int MaKH, string MaThe, int MaVoucher)
        {
            return ((QLVCDataContext)DC).sp_VoucherHetHanSuDung(dtTuNgay, dtDenNgay, MaChiNhanh, MaNganh, MaNhanVien, MaKH, MaThe
                , MaVoucher).ToList();
        }

        public List<sp_BaoCaoVoucer_HuyResult> VoucherHuy(DateTime dtTuNgay, DateTime dtDenNgay)
        {
            return ((QLVCDataContext)DC).sp_BaoCaoVoucer_Huy(dtTuNgay, dtDenNgay).ToList();
        }
    }
}
