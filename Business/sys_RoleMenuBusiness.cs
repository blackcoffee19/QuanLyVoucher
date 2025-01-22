using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_RoleMenuBusiness : Base<sys_RoleMenu>
    {
        public IQueryable<sys_RoleMenu> GetFollowRole(int RoleID)
        {
            return Get(t => t.MaRole == RoleID  && !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }

        public List<sp_DanhSachRoleMenuResult> GetFollowRole4PhanQuyen(int MaRole)
        {
            return ((QLVCDataContext)DC).sp_DanhSachRoleMenu(MaRole).ToList();
        }
    }
}
