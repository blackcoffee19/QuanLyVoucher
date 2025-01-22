using Base.Common;
using Base.Connection.OLEDB;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVoucher.Globals
{
    public class clsPublicVar
    {
        public static ConnectionBase Conn;
        public static sys_User User = null;
        public static tbl_ChiNhanh ChiNhanh = null;
        public static tbl_Nganh Nganh = null;
        public static List<vQuyen> QuyenCuaUser = new List<vQuyen>();
        public static int MainMenuID = 0;
        public static string SumRecord = "Tổng số dòng";
        public static string currentVersion = "Version test";

        public static void ClearGlobalVariances()
        {
            User = null;
            ChiNhanh = null;
            Nganh = null;
            QuyenCuaUser = null;
            MainMenuID = 0;
        }

        public static string[] FieldUpdateDelete()
        {
            if (User != null && Conn != null)
                return new[] { "DaXoa", Cast.ToString(true), "NguoiXoa", Cast.ToString(clsPublicVar.User.Ma)
                    , "NgayXoa", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DataUtils.GetDate()) };
            return new[] { "" };
        }

        public static string[] FieldCheckDelete = new[] { "DaXoa", Cast.ToString(false) };//tim nhung record co DaXoa = false or null
    }
}
