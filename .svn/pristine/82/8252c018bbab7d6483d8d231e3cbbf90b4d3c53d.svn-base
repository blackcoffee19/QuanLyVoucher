using Base.Common.Items;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_UserBusiness : Base<sys_User>
    {
        public IQueryable<sys_User> GetData()
        {
            return Get(t => !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }

        public bool Password(string TenDangNhap, string Password)
        {
            sys_User u = First(t => t.TenDangNhap == TenDangNhap && t.Password == Password);
            if (u != null)
                return true;
            return false;
        }

    }
}
