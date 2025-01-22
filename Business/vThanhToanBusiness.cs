using Base.Common.Items;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class vThanhToanBusiness : Base<vThanhToan>
    {
        public IQueryable<vThanhToan> GetData(int MaChiNhanh, int MaNganh, DateTime TuNgay, DateTime DenNgay, int MaKH, int MaVoucher
            , string MaThe)
        {
            var q = Get(t => t.MaChiNhanh == MaChiNhanh && t.MaNganh == MaNganh
                && t.NgayTao.Value.Date >= TuNgay.Date && t.NgayTao.Value.Date <= DenNgay.Date);

            if (MaKH != 0)
                q = q.Where(t => t.MaKH == MaKH);

            if (MaVoucher != 0)
                q = q.Where(t => t.MaVoucher == MaVoucher);

            if (MaThe != "")
                q = q.Where(t => t.MaThe == MaThe);

            return q.OrderByDescending(t => t.NgayTao);
        }

        public HandleState XuLySuaThanhToan(int MaThanhToan, string GhiChu, int NguoiXuLy)
        {
            if (((QLVCDataContext)DC).sp_ThanhToan_Sua(MaThanhToan, GhiChu, NguoiXuLy) >= 0)
                return new HandleState();
            return new HandleState("Update failed");
        }

        public HandleState XuLyXoaThanhToan(int MaThanhToan, int MaBanHang, int GiaTriVuaXoa, int NguoiXuLy)
        {
            int? kq = -2;
            ((QLVCDataContext)DC).sp_ThanhToan_Xoa(MaThanhToan, MaBanHang, GiaTriVuaXoa, NguoiXuLy, ref kq);

            if (kq >= 0)
                return new HandleState();
            return new HandleState("Update failed");
        }
    }
}
