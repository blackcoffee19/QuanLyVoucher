using Base.Common.Items;
using Business.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class vTaoThanhToanBusiness : Base<vTaoThanhToan>
    {
        public IQueryable<vTaoThanhToan> GetData(int MaChiNhanh, int MaNganh, string MaVach)
        {
            if((MaVach ?? "") == "")
                return null;
            return Get(t => t.MaChiNhanh == MaChiNhanh && t.MaNganh == MaNganh && t.MaVach == MaVach
                && t.LoaiThanhToan == Enum.GetName(typeof(LoaiThanhToan), LoaiThanhToan.BinhThuong))
                .OrderByDescending(t => t.NgayTao);
        }

        public IQueryable<vTaoThanhToan> GetDataVoucherHetHan(int MaChiNhanh)
        {
            return Get(t => t.MaChiNhanh == MaChiNhanh
                && t.LoaiThanhToan == Enum.GetName(typeof(LoaiThanhToan), LoaiThanhToan.VoucherHetHan))
                .OrderBy(t => t.NgayHetHan);
        }

        public HandleState XuLyTaoThanhToan(string Code, int MaChiNhanh, int MaNganh, int MaBanHang, int GiaTriThanhToan, int SoDu
            , int PhuThu, string GhiChu, int NguoiXuLy, DateTime NgayTao)
        {
            int? kq = -2;
            ((QLVCDataContext)DC).sp_ThanhToan_Them(Code, MaChiNhanh, MaNganh, MaBanHang, GiaTriThanhToan, SoDu, PhuThu, GhiChu, NguoiXuLy
                , NgayTao, ref kq);
            
            if (kq >= 0)
                return new HandleState();
            return new HandleState("Update failed");
        }
    }
}
