using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class tbl_KhachHangBusiness : Base<tbl_KhachHang>
    {
        public IQueryable<tbl_KhachHang> GetData(DateTime TuNgay, DateTime DenNgay, string HoTen, string SoDienThoai
            , string CMND, string DiaChi)
        {
            var q = Get(t => t.NgayTao.Value.Date >= TuNgay.Date && t.NgayTao.Value.Date <= DenNgay.Date && !(t.DaXoa ?? false));

            if (HoTen != "")
                q = q.Where(t => t.Ten.Contains(HoTen));

            if (SoDienThoai != "")
                q = q.Where(t => t.DienThoai.Contains(SoDienThoai));

            if (CMND != "")
                q = q.Where(t => t.CMND.Contains(CMND));

            if (DiaChi != "")
                q = q.Where(t => t.DiaChi.Contains(DiaChi));

            return q.OrderByDescending(t => t.NgayTao);
        }
    }
}
