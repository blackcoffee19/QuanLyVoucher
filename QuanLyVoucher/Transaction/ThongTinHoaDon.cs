﻿using Base.Common;
using Common.Globals;
using QuanLyVoucher.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyVoucher.Transaction
{
    public class ThongTinHoaDon
    {
        public void ChiTietHoaDonBanHang(Graphics graphic, string sTenKhachHang, DateTime dtNgayTao, string sTenVoucher, int iThanhTien
            , int iGiaTri, DateTime dtNgayHieuLuc, DateTime dtNgayHetHan)
        {
            //Khai bao bien chung
            Font font = new Font("Times New Roman", 10);
            Font fontBold = new Font("Times New Roman", 10, FontStyle.Bold);
            Font fontNganhBold = new Font("Times New Roman", 12, FontStyle.Bold);
            float FontHeight = font.GetHeight();
            int startX = 0, startY = 0, offset = 0, startXGiaTri = 0, startXThanhTien = 0, DongCachDong = 0
                , DoanCachDoan = 0, WidthBill = 0, WidthTenVoucher = 0, WidthGiaTri = 0, WidthThanhTien = 0;
            QLVCConst.ThongTinInBanHang(ref startX, ref startY, ref startXGiaTri, ref startXThanhTien, ref DongCachDong
                , ref DoanCachDoan, ref WidthBill, ref WidthTenVoucher, ref WidthGiaTri, ref WidthThanhTien);
            StringFormat strFormatCenter = new StringFormat();
            strFormatCenter.Alignment = StringAlignment.Center;
            StringFormat strFormatRight = new StringFormat();
            strFormatRight.Alignment = StringAlignment.Far;

            //Ve logo
            MemoryStream ms = new MemoryStream(clsPublicVar.ChiNhanh.Logo.ToArray(), true);
            ms.Write(clsPublicVar.ChiNhanh.Logo.ToArray(), 0, clsPublicVar.ChiNhanh.Logo.ToArray().Length);
            Image image = Image.FromStream(ms, true);
            Rectangle destRect = new Rectangle(startX + ((WidthBill - image.Width) / 2), startY, image.Width, image.Height);//noi hien thi hinh anh
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            GraphicsUnit units = GraphicsUnit.Pixel;
            graphic.DrawImage(image, destRect, srcRect, units);

            //Thong tin nganh
            offset = offset + destRect.Height + 10;
            Rectangle rectNganhCenter = new Rectangle(startX, startY + offset, WidthBill, fontNganhBold.Height);
            graphic.DrawString(clsPublicVar.Nganh.Ten, fontNganhBold, new SolidBrush(Color.Black), rectNganhCenter, strFormatCenter);
            
            offset = offset + fontNganhBold.Height + DongCachDong;
            Rectangle rectKhachHangCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Khách hàng: {0}", sTenKhachHang), font, new SolidBrush(Color.Black), rectKhachHangCenter, strFormatCenter);
            
            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectNgayTaoCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Ngày tạo: {0:dd/MM/yyyy HH:mm:ss}", dtNgayTao), font, new SolidBrush(Color.Black), rectNgayTaoCenter, strFormatCenter);
            
            //Tieu de
            offset = offset + (int)FontHeight + DoanCachDoan;
            graphic.DrawString("Tên voucher", fontBold, new SolidBrush(Color.Black), startX, startY + offset);
            Rectangle rectTieuDeGiaTri = new Rectangle(startX + startXGiaTri, startY + offset, WidthGiaTri, fontBold.Height);
            graphic.DrawString("Giá trị", fontBold, new SolidBrush(Color.Black), rectTieuDeGiaTri, strFormatRight);
            Rectangle rectTieuDeGia = new Rectangle(startX + startXThanhTien, startY + offset, WidthThanhTien, fontBold.Height);
            graphic.DrawString("Giá", fontBold, new SolidBrush(Color.Black), rectTieuDeGia, strFormatRight);

            //Gach duoi
            offset = offset + (int)FontHeight;
            graphic.DrawString("-------------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            
            //Chi tiet hoa don
            offset = offset + (int)FontHeight;
            SizeF strSizeTenVoucher = new SizeF();//Tinh so dong
            strSizeTenVoucher = graphic.MeasureString(sTenVoucher, font);
            int SoDong = 1;
            float TiLe = strSizeTenVoucher.Width/WidthTenVoucher;

            if (TiLe > Cast.ToInt(TiLe))
                SoDong = Cast.ToInt(TiLe) + 1;
            else
                SoDong = Cast.ToInt(TiLe);

            Rectangle rectTenVoucher = new Rectangle(startX, startY + offset, WidthTenVoucher, ((int)FontHeight * SoDong) + 3);
            graphic.DrawString(sTenVoucher, font, new SolidBrush(Color.Black), rectTenVoucher);
            Rectangle rectGiaTri = new Rectangle(startX + startXGiaTri, startY + offset, WidthGiaTri, font.Height);
            graphic.DrawString(string.Format("{0:n0}", iGiaTri), font, new SolidBrush(Color.Black), rectGiaTri, strFormatRight);
            Rectangle rectThanhTien = new Rectangle(startX + startXThanhTien, startY + offset, WidthThanhTien, font.Height);
            graphic.DrawString(string.Format("{0:n0}", iThanhTien), font, new SolidBrush(Color.Black), rectThanhTien, strFormatRight);
            
            //Gach duoi
            offset = offset + rectTenVoucher.Height;
            graphic.DrawString("-------------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            
            //Tong tien
            offset = offset + (int)FontHeight;
            graphic.DrawString("Tổng tiền", fontBold, new SolidBrush(Color.Black), startX, startY + offset);
            Rectangle rectTongTien = new Rectangle(startX + startXThanhTien, startY + offset, WidthThanhTien, font.Height);
            graphic.DrawString(string.Format("{0:n0}", iThanhTien), fontBold, new SolidBrush(Color.Black), rectTongTien, strFormatRight);
            
            //Thong tin voucher
            offset = offset + (int)FontHeight + DoanCachDoan;
            Rectangle rectThongTinVoucherCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString("Thời gian sử dụng voucher", fontBold, new SolidBrush(Color.Black), rectThongTinVoucherCenter, strFormatCenter);
            
            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectNgayHieuLucCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Từ ngày: {0:dd/MM/yyyy}", dtNgayHieuLuc), font, new SolidBrush(Color.Black), rectNgayHieuLucCenter, strFormatCenter);
            
            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectNgayHetHanCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Đến ngày: {0:dd/MM/yyyy}", dtNgayHetHan), font, new SolidBrush(Color.Black), rectNgayHetHanCenter, strFormatCenter);

            //Thong diep
            offset = offset + (int)FontHeight + DoanCachDoan;
            Rectangle rectNgayInCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Ngày in: {0:dd/MM/yyyy HH:mm:ss}", DataUtils.GetDate()), font, new SolidBrush(Color.Black), rectNgayInCenter, strFormatCenter);
            
            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectNhanVienCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Nhân viên: {0}", clsPublicVar.User.HoTen), font, new SolidBrush(Color.Black), rectNhanVienCenter, strFormatCenter);
            
            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectCamOnCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString("Cảm ơn quý khách và hẹn gặp lại!", font, new SolidBrush(Color.Black), rectCamOnCenter, strFormatCenter);
        }

        public void ChiTietHoaDonThanhToan(Graphics graphic, string SoHoaDon, string sTenKhachHang, DateTime dtNgayTao, string sTenVoucher
            , int iGiaTriTruocSuDung, int iGiaTriSuDung, int iGiaTriSauSuDung, string sMaThe, int iPhuThu)
        {
            //Khai bao bien chung
            Font font = new Font("Times New Roman", 10);
            Font fontBold = new Font("Times New Roman", 10, FontStyle.Bold);
            Font fontNganhBold = new Font("Times New Roman", 12, FontStyle.Bold);
            float FontHeight = font.GetHeight();
            int startX = 0, startY = 0, offset = 0, startXGiaTri = 0, DongCachDong = 0, DoanCachDoan = 0
                , WidthBill = 0, WidthGiaTri = 0;
            QLVCConst.ThongTinInThanhToan(ref startX, ref startY, ref startXGiaTri, ref DongCachDong, ref DoanCachDoan, ref WidthBill
                , ref WidthGiaTri);
            StringFormat strFormatCenter = new StringFormat();
            strFormatCenter.Alignment = StringAlignment.Center;
            StringFormat strFormatRight = new StringFormat();
            strFormatRight.Alignment = StringAlignment.Far;

            //Ve logo
            MemoryStream ms = new MemoryStream(clsPublicVar.ChiNhanh.Logo.ToArray(), true);
            ms.Write(clsPublicVar.ChiNhanh.Logo.ToArray(), 0, clsPublicVar.ChiNhanh.Logo.ToArray().Length);
            Image image = Image.FromStream(ms, true);
            Rectangle destRect = new Rectangle(startX + ((WidthBill - image.Width) / 2), startY, image.Width, image.Height);//noi hien thi hinh anh
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            GraphicsUnit units = GraphicsUnit.Pixel;
            graphic.DrawImage(image, destRect, srcRect, units);

            //Thong tin nganh
            offset = offset + destRect.Height + 10;
            Rectangle rectNganhCenter = new Rectangle(startX, startY + offset, WidthBill, fontNganhBold.Height);
            graphic.DrawString(clsPublicVar.Nganh.Ten, fontNganhBold, new SolidBrush(Color.Black), rectNganhCenter, strFormatCenter);

            offset = offset + fontNganhBold.Height + DongCachDong;
            Rectangle rectMaTheCenter = new Rectangle(startX, startY + offset, WidthBill, fontBold.Height);
            graphic.DrawString(string.Format("Mã thẻ: {0}", sMaThe), fontBold, new SolidBrush(Color.Black), rectMaTheCenter, strFormatCenter);

            offset = offset + fontBold.Height + DongCachDong;
            Rectangle rectKhachHangCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Khách hàng: {0}", sTenKhachHang), font, new SolidBrush(Color.Black), rectKhachHangCenter, strFormatCenter);

            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectNgayTaoCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Ngày thanh toán: {0:dd/MM/yyyy HH:mm:ss}", dtNgayTao), font, new SolidBrush(Color.Black), rectNgayTaoCenter, strFormatCenter);

            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectSoHoaDonCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Số hóa đơn: {0}", SoHoaDon), font, new SolidBrush(Color.Black), rectSoHoaDonCenter, strFormatCenter);

            //Ten voucher
            offset = offset + (int)FontHeight + DoanCachDoan;
            SizeF strSizeTenVoucher = new SizeF();//Tinh so dong
            strSizeTenVoucher = graphic.MeasureString(sTenVoucher, font);
            int SoDong = 1;
            float TiLe = strSizeTenVoucher.Width / WidthBill;

            if (TiLe > Cast.ToInt(TiLe))
                SoDong = Cast.ToInt(TiLe) + 1;
            else
                SoDong = Cast.ToInt(TiLe);

            Rectangle rectTenVoucher = new Rectangle(startX, startY + offset, WidthBill, ((int)FontHeight * SoDong) + 3);
            graphic.DrawString(sTenVoucher, font, new SolidBrush(Color.Black), rectTenVoucher);

            //Gach duoi
            offset = offset + (int)rectTenVoucher.Height;
            graphic.DrawString("-------------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);

            //Chi tiet hoa don
            offset = offset + (int)FontHeight;
            graphic.DrawString("Giá trị trước sử dụng", fontBold, new SolidBrush(Color.Black), startX, startY + offset);
            Rectangle rectGiaTriTruocSuDungRight = new Rectangle(startX + startXGiaTri, startY + offset, WidthGiaTri, font.Height);
            graphic.DrawString(string.Format("{0:n0}", iGiaTriTruocSuDung), font, new SolidBrush(Color.Black), rectGiaTriTruocSuDungRight, strFormatRight);
            offset = offset + (int)FontHeight + DongCachDong;
            graphic.DrawString("Giá trị sử dụng", fontBold, new SolidBrush(Color.Black), startX, startY + offset);
            Rectangle rectGiaTriSuDungRight = new Rectangle(startX + startXGiaTri, startY + offset, WidthGiaTri, font.Height);
            graphic.DrawString(string.Format("{0:n0}", iGiaTriSuDung), font, new SolidBrush(Color.Black), rectGiaTriSuDungRight, strFormatRight);
            offset = offset + (int)FontHeight + DongCachDong;
            graphic.DrawString("Giá trị sau sử dụng", fontBold, new SolidBrush(Color.Black), startX, startY + offset);
            Rectangle rectGiaTriSauSuDungRight = new Rectangle(startX + startXGiaTri, startY + offset, WidthGiaTri, font.Height);
            graphic.DrawString(string.Format("{0:n0}", iGiaTriSauSuDung), font, new SolidBrush(Color.Black), rectGiaTriSauSuDungRight, strFormatRight);

            if (iPhuThu > 0)
            {
                offset = offset + (int)FontHeight + DongCachDong;
                graphic.DrawString("Thanh toán tiền mặt", fontBold, new SolidBrush(Color.Black), startX, startY + offset);
                Rectangle rectPhuThu = new Rectangle(startX + startXGiaTri, startY + offset, WidthGiaTri, font.Height);
                graphic.DrawString(string.Format("{0:n0}", iPhuThu), font, new SolidBrush(Color.Black), rectPhuThu, strFormatRight);
            }

            //Gach duoi
            offset = offset + (int)FontHeight;
            graphic.DrawString("-------------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);

            //Thong diep
            offset = offset + (int)FontHeight + DoanCachDoan;
            Rectangle rectNgayInCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Ngày in: {0:dd/MM/yyyy HH:mm:ss}", DataUtils.GetDate()), font, new SolidBrush(Color.Black), rectNgayInCenter, strFormatCenter);

            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectNhanVienCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString(string.Format("Nhân viên: {0}", clsPublicVar.User.HoTen), font, new SolidBrush(Color.Black), rectNhanVienCenter, strFormatCenter);

            offset = offset + (int)FontHeight + DongCachDong;
            Rectangle rectCamOnCenter = new Rectangle(startX, startY + offset, WidthBill, font.Height);
            graphic.DrawString("Cảm ơn quý khách và hẹn gặp lại!", font, new SolidBrush(Color.Black), rectCamOnCenter, strFormatCenter);
        }

        public void XuLyMayIn(PrintPageEventHandler TaoHoaDon)
        {
            string sTenMayIn = "";
            bool bChonMayInKhiIn = true;
            int iSLHDChoLanIn = 0;
            QLVCConst.ThongTinMayIn(ref sTenMayIn, ref bChonMayInKhiIn, ref iSLHDChoLanIn);

            if (bChonMayInKhiIn)//Cho truong hop chon may in roi in
            {
                PrintDialog PrintDialog = new PrintDialog();
                PrintDocument PrintDocument = new PrintDocument();
                PrintDialog.Document = PrintDocument;

                PrintDocument.PrintPage += new PrintPageEventHandler(TaoHoaDon);
                DialogResult result = PrintDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    for (int i = 0; i < iSLHDChoLanIn; i++)
                        PrintDocument.Print();
                }
            }
            else
            {
                PrintDocument PrintDocument = new PrintDocument();
                PrintDocument.PrinterSettings.PrinterName = sTenMayIn;
                PrintDocument.PrintPage += new PrintPageEventHandler(TaoHoaDon);

                for (int i = 0; i < iSLHDChoLanIn; i++)
                    PrintDocument.Print();
            }
        }
    }
}
