using Base.Common;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_User_CRM_SPA_HHHBusiness : BaseCRMHHH<sys_User_CRM_HHH>
    {
        private static int UserIDadd = 100000;

        //public List<sys_User_CRM_HHH> GetData()
        //{
        //    return Get(t => (t.Status ?? true) && !(t.Deleted ?? false)).ToList().Select(t => { t.ID += UserIDadd; return t; }).ToList();
        //}

        //public sys_User GetUserAsSys_User(string token)
        //{
        //    sys_User_CRM_HHH ur = Get(t => (t.Status ?? true) && 
        //                                !(t.Deleted ?? false) && 
        //                                t.Token != null  && 
        //                                t.Token != "" && 
        //                                t.Token == token).FirstOrDefault();
        //    if(ur != null)
        //    {
        //        return new sys_User()
        //        {
        //            TenDangNhap = ur.Username,
        //            CRMUser = true,
        //            Password = ur.Password,
        //            Ma = ur.ID + UserIDadd,
        //            HoTen = ur.FullName,
        //            GioiTinh = ur.Gender ?? false,
        //            NgayTao = ur.CreatedDate,
        //            Email = ur.Email,
        //            TrangThai = true,
        //        };
        //    }
        //    return null;
        //}

        //public List<sys_User> GetDataAsSys_User()
        //{
        //    return GetData().Select(t => new sys_User()
        //    {
        //        TenDangNhap = t.Username,
        //        CRMUser = true,
        //        Password = t.Password,
        //        Ma = t.ID,
        //        HoTen = t.FullName,
        //        GioiTinh = t.Gender ?? false,
        //        NgayTao = t.CreatedDate,
        //        Email = t.Email,
        //        TrangThai = true,
        //    }).ToList() ;
        //}

        //public bool Password(string TenDangNhap, string token)
        //{
        //   return true;
        //}
    }
}
