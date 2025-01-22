using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class tbl_VoucherBusiness : Base<tbl_Voucher>
    {
        public IQueryable<tbl_Voucher> GetData()
        {
            return Get(t => !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }
    }
}
