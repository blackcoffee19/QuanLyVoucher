using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;
using Business;
using QuanLyVoucher.Globals;
using DevExpress.XtraBars;
using System.Reflection;
using Business.Globals;
using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
using QuanLyVoucher.Admin;
using Tool;

namespace QuanLyVoucher
{
    public partial class frmMain
    {
        protected List<sys_Menu> menu = new List<sys_Menu>();
        protected List<vQuyen> menuPermission = clsPublicVar.QuyenCuaUser;

        private void LoadMenu()
        {
            int iCountMenuAlways = 0;
            sys_MenuBusiness menuB = new sys_MenuBusiness();
            menu = menuB.GetMenu(clsPublicVar.User.Ma, clsPublicVar.ChiNhanh.Ma, clsPublicVar.Nganh.Ma).ToList();
            List<sys_Menu> mGroup = menu.Where(t => t.MenuCha == null && (t.HienMenu ?? false)).OrderBy(t => t.SoThuTu).ToList();

            //Set menu File
            barManager.BeginUpdate();

            mGroup.ForEach(mg =>
            {
                BarSubItem subMenu = new BarSubItem(barManager, mg.Ten);
                subMenu.Glyph = Properties.Resources.ResourceManager.GetObject(mg.Icon) as Image;
                subMenu.PaintStyle = BarItemPaintStyle.CaptionGlyph;

                List<sys_Menu> mChildren = menu.Where(t => t.MenuCha == mg.Ma && (t.HienMenu ?? false)).OrderBy(t => t.SoThuTu).ToList();

                if (mg.Code != "file")
                {
                    mChildren.ForEach(mc =>
                    {
                        BarButtonItem button = new BarButtonItem(barManager, mc.Ten);
                        button.Glyph = Properties.Resources.ResourceManager.GetObject(mc.Icon) as Image;
                        button.Links.Add(button);

                        if (!string.IsNullOrEmpty(mc.DuongDan))
                            RegisterFormChild(button, mc.Ma, mc.Code, Assembly.GetExecutingAssembly(), mc.DuongDan, mc.ThamSo ?? "");

                        //check auto popup form                    
                        vQuyen gp = menuPermission.FirstOrDefault(t => t.MaMenu == mc.Ma && t.MaQuyen == (short)Quyen.OpenStartup);
                        if (gp != null)
                            LinkClick(button);

                        if (mc.LuonLuonHien ?? false)
                        {
                            iCountMenuAlways++;
                            BarLargeButtonItem buttonAlways = new BarLargeButtonItem(barManager, mc.Ten);
                            //buttonAlways.LargeGlyph = Properties.Resources.ResourceManager.GetObject(mc.Icon) as Image;
                            buttonAlways.Glyph = Properties.Resources.ResourceManager.GetObject(mc.Icon) as Image;

                            if (!string.IsNullOrEmpty(mc.DuongDan))
                                RegisterFormChild(buttonAlways, mc.Ma, mc.Code, Assembly.GetExecutingAssembly(), mc.DuongDan, mc.ThamSo ?? "");

                            bar3.AddItem(buttonAlways);
                        }

                        subMenu.AddItem(button);
                    });
                }
                else
                {
                    //..
                    BarButtonItem buttonChangePassword = new BarButtonItem(barManager, "Đổi mật khẩu");
                    buttonChangePassword.Glyph = QuanLyVoucher.Properties.Resources.key2;
                    buttonChangePassword.Id = (int)FileMenu.ChangePassword;

                    //..
                    BarButtonItem buttonConfigurePrinter = new BarButtonItem(barManager, "Cấu hình máy in");
                    buttonConfigurePrinter.Glyph = QuanLyVoucher.Properties.Resources.Print;
                    buttonConfigurePrinter.Id = (int)FileMenu.ConfigurePrinter;

                    //..
                    BarButtonItem buttonLogout = new BarButtonItem(barManager, "Đăng xuất");
                    buttonLogout.Glyph = QuanLyVoucher.Properties.Resources.User_logout;
                    buttonLogout.Id = (int)FileMenu.Logout;

                    //..
                    BarButtonItem buttonExit = new BarButtonItem(barManager, "Thoát");
                    buttonExit.Glyph = QuanLyVoucher.Properties.Resources.Crystal_Project_Exit;
                    buttonExit.Id = (int)FileMenu.Exit;

                    subMenu.AddItems(new BarItem[] { buttonChangePassword, buttonConfigurePrinter, buttonLogout, buttonExit });
                }

                bar1.AddItem(subMenu);
            });

            if (iCountMenuAlways == 0)
                bar3.Visible = false;

            barManager.ItemClick += new ItemClickEventHandler(barManager_ItemClick);

            barManager.EndUpdate();
        }

        private static void LinkClick(BarButtonItem button)
        {
            BarButtonItemLink link = button.Links.Cast<BarButtonItemLink>().FirstOrDefault(t => t.Item == button);
            MethodInfo mi = button.GetType().GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi != null && link != null)
                mi.Invoke(button, new object[] { link });
        }

        private void barManager_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarSubItem subMenu = e.Item as BarSubItem;
            if (subMenu != null) return;

            if (e.Item.Id == (int)FileMenu.ChangePassword)
                new frmChangePassword().ShowDialog();
            else if (e.Item.Id == (int)FileMenu.ConfigurePrinter)
                new frmConfigurePrinter().ShowDialog();
            else if (e.Item.Id == (int)FileMenu.Logout)
            {
                CloseAllChildren();

                bSignOut = true;
                this.Close();
            }
            else if (e.Item.Id == (int)FileMenu.Exit)
            {
                this.Close();
            }
        }

        public override Form CreateChild(Type frmType, string args)
        {
            sys_Menu mn = menu.FirstOrDefault(t => t.DuongDan == frmType.FullName);

            if (mn != null)
            {
                clsPublicVar.MainMenuID = mn.Ma;
                Form frm = base.CreateChild(frmType, args);

                //Append auto authorization
                if (frm is BaseForm)
                {
                    BaseForm baseForm = frm as BaseForm;
                    SetActionForForm.SetAction(baseForm, clsPublicVar.MainMenuID, clsPublicVar.QuyenCuaUser);
                }

                return frm;
            }

            return base.CreateChild(frmType, args);
        }
    }
}
