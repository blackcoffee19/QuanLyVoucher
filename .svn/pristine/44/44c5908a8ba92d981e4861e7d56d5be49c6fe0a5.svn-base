using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_MenuBusiness : Base<sys_Menu>
    {
        public IQueryable<sys_Menu> GetData()
        {
            return Get(t => !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }

        public List<sys_Menu> GetMenu(int MaUser, int MaChiNhanh, int MaNganh)
        {
            var menu = Get(m => DC.GetTable<sys_RoleMenu>().Any(rm => rm.MaMenu == m.Ma && (rm.ChoPhepTruyCap ?? false) && (rm.TrangThai ?? false) && !(rm.DaXoa ?? false)
                                    && DC.GetTable<sys_Role>().Any(r => rm.MaRole == r.Ma && (r.TrangThai ?? false) && !(r.DaXoa ?? false)
                                        && DC.GetTable<sys_UserRole>().Any(ur => r.Ma == ur.MaRole && (ur.TrangThai ?? false) && ur.MaUser == MaUser
                                            && ur.MaChiNhanh == MaChiNhanh && ur.MaNganh == MaNganh && !(ur.DaXoa ?? false))))
                                && (m.HienMenu ?? false) && (m.TrangThai ?? false) && !(m.DaXoa ?? false)).ToList();

            //var assigment = Get(m => DC.GetTable<sysAuthorizationDetail>().Any(aud => aud.MenuID == m.ID && !(aud.Inactive ?? false)// && (aud.HeadAllow ?? false)
            //                            && DC.GetTable<sysAuthorization>().Any(au => aud.AuthorizationID == au.ID && !(au.Inactive ?? false) && au.AssignTo == userID)
            //                            && DC.GetTable<sysPermission>().Any(p => aud.PermissionID == p.ID && (p.RequireAccessingForm ?? false)))
            //                        && (m.VisibleMenu ?? false))
            //                .Join(DC.GetTable<sysMenu>(), m => m.ParentID, mParent => mParent.ID, (m, mParent) => new { m, mParent }).ToList();

            //var menuParent = assigment.Select(t => t.mParent).Distinct().ToList();
            //var menuAssign = assigment.Select(t => t.m).Distinct().ToList();

            //menu = menu.Union(menuParent).Union(menuAssign).ToList();

            return menu;
        }
    }
}
