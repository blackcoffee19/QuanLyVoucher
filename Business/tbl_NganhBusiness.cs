using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class tbl_NganhBusiness : Base<tbl_Nganh>
    {
        public IQueryable<tbl_Nganh> GetFollowChiNhanh(int MaChiNhanh)
        {
            return Get(t => t.MaChiNhanh == MaChiNhanh && !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }
    }
}
