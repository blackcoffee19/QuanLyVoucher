﻿using Base.Common;
using Base.Common.Items;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Globals
{
    public class QLVCConst
    {
        public static string ConnectionStrings = "";

        public static string ConnectionSPAHHH = "Data Source=172.16.10.39;Initial Catalog=CRM-HHH;User ID=sa;Password=sa@123;MultipleActiveResultSets=true";
        public static ConfigBase cfb = new ConfigBase();

        public static void SetConnectionString(string serverSQL, string UserSQL, string PasswordSQL, string databaseSQL)
        {
            ConnectionStrings = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}"
                , serverSQL, databaseSQL, UserSQL, PasswordSQL);
        }

        public static string PathTemplate
        {
            get
            {
                string path = Application.StartupPath + @"\DataFiles\Templates";

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                return path;
            }
        }

        //Database list
        public static List<BasicItem> ListDatabase = new List<BasicItem>(new[]
        {
            new BasicItem("Quản lý Voucher", "QuanLyVoucher")
        });

        //Config path
        public static string ConfigurePath = string.Format("{0}\\Cfg\\QLVC.Base.xml", Application.StartupPath);

        //Config path in profile
        public static string ConfigurePathInProfile
        {
            get
            {
                string path = string.Format("{0}\\QLVC", Environment.ExpandEnvironmentVariables("%LocalAppData%"));
                string pathFile = string.Format("{0}\\QLVC.Base.xml", path);

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                return pathFile;
            }
        }

        public static string PathReport
        {
            get
            {
                string path = Application.StartupPath + @"\DataFiles\Reports";

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                return path;
            }
        }

        public static string PathTemp
        {
            get
            {
                string path = Application.StartupPath + @"\tmp";

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                return path;
            }
        }

        public static void ThongTinMayIn(ref string _TenMayIn, ref bool _ChonMayInKhiIn, ref int _SLHDChoLanIn)
        {
            string XMLPath = "";

            if (ApplicationDeployment.IsNetworkDeployed)
                XMLPath = ConfigurePathInProfile;
            else
                XMLPath = ConfigurePath;

            if (!LayThongTinMayIn(XMLPath, ref _TenMayIn, ref _ChonMayInKhiIn, ref _SLHDChoLanIn).Success)
            {
                XMLPath = ConfigurePath;
                LayThongTinMayIn(XMLPath, ref _TenMayIn, ref _ChonMayInKhiIn, ref _SLHDChoLanIn);
            }
        }

        private static HandleState LayThongTinMayIn(string XMLPath, ref string _TenMayIn, ref bool _ChonMayInKhiIn
            , ref int _SLHDChoLanIn)
        {
            HandleState handle = cfb.OpenConfig(XMLPath);
            if (handle.Success)
            {
                _TenMayIn = cfb.Get("QLVC.Printer.TenMayIn", "", XMLPath);
                _ChonMayInKhiIn = Cast.ToBoolean(cfb.Get("QLVC.Printer.ChonMayInKhiIn", "", XMLPath));
                _SLHDChoLanIn = Cast.ToInt(cfb.Get("QLVC.Printer.SLHDChoLanIn", "", XMLPath));
            }

            return handle;
        }

        public static void ThongTinInBanHang(ref int startX, ref int startY, ref int startXGiaTri, ref int startXThanhTien
            , ref int DongCachDong, ref int DoanCachDoan, ref int WidthBill, ref int WidthTenVoucher, ref int WidthGiaTri
            , ref int WidthThanhTien)
        {
            string XMLPath = "";

            if (ApplicationDeployment.IsNetworkDeployed)
                XMLPath = ConfigurePathInProfile;
            else
                XMLPath = ConfigurePath;

            if (!LayThongTinInBanHang(XMLPath, ref startX, ref startY, ref startXGiaTri, ref startXThanhTien, ref DongCachDong
                , ref DoanCachDoan, ref WidthBill, ref WidthTenVoucher, ref WidthGiaTri, ref WidthThanhTien).Success)
            {
                XMLPath = ConfigurePath;
                LayThongTinInBanHang(XMLPath, ref startX, ref startY, ref startXGiaTri, ref startXThanhTien, ref DongCachDong
                    , ref DoanCachDoan, ref WidthBill, ref WidthTenVoucher, ref WidthGiaTri, ref WidthThanhTien);
            }
        }

        private static HandleState LayThongTinInBanHang(string XMLPath, ref int startX, ref int startY, ref int startXGiaTri
            , ref int startXThanhTien, ref int DongCachDong, ref int DoanCachDoan, ref int WidthBill, ref int WidthTenVoucher
            , ref int WidthGiaTri, ref int WidthThanhTien)
        {
            HandleState handle = cfb.OpenConfig(XMLPath);
            if (handle.Success)
            {
                startX = Cast.ToInt(cfb.Get("QLVC.InBanHang.startX", "", XMLPath));
                startY = Cast.ToInt(cfb.Get("QLVC.InBanHang.startY", "", XMLPath));
                startXGiaTri = Cast.ToInt(cfb.Get("QLVC.InBanHang.startXGiaTri", "", XMLPath));
                startXThanhTien = Cast.ToInt(cfb.Get("QLVC.InBanHang.startXThanhTien", "", XMLPath));
                DongCachDong = Cast.ToInt(cfb.Get("QLVC.InBanHang.DongCachDong", "", XMLPath));
                DoanCachDoan = Cast.ToInt(cfb.Get("QLVC.InBanHang.DoanCachDoan", "", XMLPath));
                WidthBill = Cast.ToInt(cfb.Get("QLVC.InBanHang.WidthBill", "", XMLPath));
                WidthTenVoucher = Cast.ToInt(cfb.Get("QLVC.InBanHang.WidthTenVoucher", "", XMLPath));
                WidthGiaTri = Cast.ToInt(cfb.Get("QLVC.InBanHang.WidthGiaTri", "", XMLPath));
                WidthThanhTien = Cast.ToInt(cfb.Get("QLVC.InBanHang.WidthThanhTien", "", XMLPath));
            }

            return handle;
        }

        public static void ThongTinInThanhToan(ref int startX, ref int startY, ref int startXGiaTri, ref int DongCachDong
            , ref int DoanCachDoan, ref int WidthBill, ref int WidthGiaTri)
        {
            string XMLPath = "";

            if (ApplicationDeployment.IsNetworkDeployed)
                XMLPath = ConfigurePathInProfile;
            else
                XMLPath = ConfigurePath;

            if (!LayThongTinInThanhToan(XMLPath, ref startX, ref startY, ref startXGiaTri, ref DongCachDong, ref DoanCachDoan
                , ref WidthBill, ref WidthGiaTri).Success)
            {
                XMLPath = ConfigurePath;
                LayThongTinInThanhToan(XMLPath, ref startX, ref startY, ref startXGiaTri, ref DongCachDong, ref DoanCachDoan
                    , ref WidthBill, ref WidthGiaTri);
            }
        }

        private static HandleState LayThongTinInThanhToan(string XMLPath, ref int startX, ref int startY, ref int startXGiaTri
            , ref int DongCachDong, ref int DoanCachDoan, ref int WidthBill, ref int WidthGiaTri)
        {
            HandleState handle = cfb.OpenConfig(XMLPath);
            if (handle.Success)
            {
                startX = Cast.ToInt(cfb.Get("QLVC.InThanhToan.startX", "", XMLPath));
                startY = Cast.ToInt(cfb.Get("QLVC.InThanhToan.startY", "", XMLPath));
                startXGiaTri = Cast.ToInt(cfb.Get("QLVC.InThanhToan.startXGiaTri", "", XMLPath));
                DongCachDong = Cast.ToInt(cfb.Get("QLVC.InThanhToan.DongCachDong", "", XMLPath));
                DoanCachDoan = Cast.ToInt(cfb.Get("QLVC.InThanhToan.DoanCachDoan", "", XMLPath));
                WidthBill = Cast.ToInt(cfb.Get("QLVC.InThanhToan.WidthBill", "", XMLPath));
                WidthGiaTri = Cast.ToInt(cfb.Get("QLVC.InThanhToan.WidthGiaTri", "", XMLPath));
            }

            return handle;
        }

        public const string NumberFormat = "#,##0";
        public const string NumberFormatDecimal = "#,##0.00";
        public const string DateFormat = "dd-MM-yyyy";
        public const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        public const string RequestRecordStatus = "RequestRecordStatus";
        public const string RequestSearchBox = "RequestSearchBox";
        public const string RequestSQL = "RequestSQL";
        public const string RequestNewToolbarButton = "RequestNewToolbarButton";
        public const string RequestOpenForm = "RequestOpenForm";
        public const string StopCloseProgram = "StopCloseProgram";
    }
}
