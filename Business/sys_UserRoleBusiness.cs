using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_UserRoleBusiness : Base<sys_UserRole>
    {
        public IQueryable<sys_UserRole> GetFollowUser(int UserID)
        {
            return Get(t => t.MaUser == UserID && !(t.DaXoa ?? false))
                .OrderBy(t => t.MaChiNhanh).ThenBy(t => t.MaNganh);
        }
    }
}
