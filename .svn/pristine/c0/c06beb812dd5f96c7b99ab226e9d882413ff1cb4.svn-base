using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class tbl_NhomVoucherBusiness : Base<tbl_NhomVoucher>
    {
        public IQueryable<tbl_NhomVoucher> GetData()
        {
            return Get(t => !(t.DaXoa ?? false)).OrderBy(t => t.Ten);
        }
    }
}
