using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class tbl_ChiNhanhBusiness : Base<tbl_ChiNhanh>
    {
        public IQueryable<tbl_ChiNhanh> GetData()
        {
            return Get(t => !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }
    }
}
