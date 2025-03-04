﻿using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class tbl_VoucherRoleBusiness : Base<tbl_VoucherRole>
    {
        public IQueryable<tbl_VoucherRole> GetFollowVoucher(int MaVoucher)
        {
            return Get(t => t.MaVoucher == MaVoucher && !(t.DaXoa ?? false))
                .OrderBy(t => t.MaChiNhanh).ThenBy(t => t.MaNganh);
        }
    }
}
