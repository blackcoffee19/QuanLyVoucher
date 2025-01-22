using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_RoleQuyenBusiness : Base<sys_RoleQuyen>
    {
        public IQueryable<sys_RoleQuyen> GetFollowRoleMenu(int RoleID, int MenuID)
        {
            return Get(t => t.MaRole == RoleID && t.MaMenu == MenuID && !(t.DaXoa ?? false))
                .OrderByDescending(t => t.NgayTao);
        }

        public List<sp_DanhSachRoleQuyenResult> GetFollowRoleMenu4PhanQuyen(int MaRole)
        {
            return ((QLVCDataContext)DC).sp_DanhSachRoleQuyen(MaRole).ToList();
        }
    }
}
