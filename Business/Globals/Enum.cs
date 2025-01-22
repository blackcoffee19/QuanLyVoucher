﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Globals
{
    public enum Quyen
    {
        Add = 1,
        Edit = 2,
        Delete = 3,
        OpenStartup = 4,
        ResetPassword = 5,
        ChenHinhAnh = 6,
        KichHoat = 7,
        HuyKichHoat = 8,
        LuuVaInHoaDon = 9,
        InLaiHoaDon = 10,
        LichSuThanhToan = 11,
        XemTatCaVoucherCuaChiNhanh = 12,
        VoucherCuaKhachHang = 13,
        VoucherDangSuDung = 14,
        VoucherHetSoDu = 15,
        VoucherHetHanSuDung = 16,
        DoanhThuTheoNgayDangKy = 17,
        DoanhThuTheoNgayThanhToan = 18,
        LichSuThanhToanTheoGoiSuat = 19,
        LichSuThanhToanTheoTien = 20,
        ThanhToanVoucherHetHan = 21,
        VoucherDaHuy = 22
    }

    public enum LoaiThanhToan
    {
        BinhThuong,
        VoucherHetHan
    }

    public enum Role
    {
        Admin = 1
    }

    public enum LoaiVoucher
    {
        All,
        Tien,
        GoiSuat
    }

    public enum FileMenu
    {
        ChangePassword = 1,
        ConfigurePrinter = 2,
        Logout = 3,
        Exit = 4
    }

    public enum GioiTinh
    {
        Nam,
        Nu
    }

    public enum LoaiThoiHan
    {
        Ngay,
        Tuan,
        Thang,
        Nam
    }
}
