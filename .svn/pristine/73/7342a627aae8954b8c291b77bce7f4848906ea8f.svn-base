using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Base.AppliBaseForm;
using QuanLyVoucher.Globals;
using Base.AppliBase.Items;
using Base.AppliBase;
using Base.AppliBaseForm.Globals;
using Business;
using Service;
using Common.Globals;
using Base.Common;
using DevExpress.XtraBars;
using DevExpress.XtraTabbedMdi;
using Tool.Search;
using System.IO;
using DevExpress.Utils;
using System.Net.NetworkInformation;
using System.Deployment.Application;
using Base.DevExpressEx.Utility;

namespace QuanLyVoucher
{
    public partial class frmMain : BaseMDIForm
    {
        public bool bSignOut = false;
        private bool StopCloseProgram = false;
        private Timer _TimerCheckVersion = new Timer();

        public frmMain()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            panMain.Dock = DockStyle.Fill;
            panMain.BringToFront();

            //..
            LoadListClass();

            //.. grand key actions
            KeyBrowse = new KeyAction(Keys.F1);
            KeyAdd = new KeyAction(Keys.F2);
            KeyEdit = new KeyAction(Keys.F3);
            KeySave = new KeyAction(Keys.F4);
            KeyDelete = new KeyAction(Keys.F5);

            //.. grand button click actions
            btnMainButton.btnAdd.ItemClick += AddEvent;
            btnMainButton.btnEdit.ItemClick += EditEvent;
            btnMainButton.btnSave.ItemClick += SaveEvent;
            btnMainButton.btnDelete.ItemClick += DeleteEvent;
            btnMainButton.btnBrowse.ItemClick += BrowseEvent;

            //..
            ConfigTabbedMdi(xtraTabbedMdiManager);
            VisibleUserControl(false);
            btnMainButton.EnableAllButtonActions(false);
            xtraTabbedMdiManager.PageRemoved += new MdiTabPageEventHandler(_xtm_PageRemoved);

            //..
            LoadMenu();
            LoadInfo();

            this.MdiChildActivate += (sender, e) => panMain.Visible = (this.ActiveMdiChild == null);

            //Check version of user
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                _TimerCheckVersion.Interval = 3600000;//One hour
                _TimerCheckVersion.Enabled = true;
                _TimerCheckVersion.Tick += (sender, e) =>
                {
                    ApplicationDeployment updateCheck = ApplicationDeployment.CurrentDeployment;
                    UpdateCheckInfo info = updateCheck.CheckForDetailedUpdate();

                    if (info.UpdateAvailable)
                    {
                        _TimerCheckVersion.Enabled = false;

                        if (SetConfirm("Hệ thống đã có bản cập nhật mới bạn cần phải cập nhật phần mềm", "Thông báo") == DialogResult.OK)
                        {
                            WaitingBox.Show("Hệ thống đang cập nhật và sẽ khởi động lại ứng dụng", "Vui lòng chờ...");
                            updateCheck.Update();
                            WaitingBox.Hide(10);

                            //Restart
                            Application.Restart();
                        }
                        else
                        {
                            _TimerCheckVersion.Interval = 600000;//Ten minute
                            _TimerCheckVersion.Enabled = true;
                        }
                    }
                };
            }

            //..
            this.FormClosing += (sender, e) =>
            {
                if (StopCloseProgram)
                {
                    e.Cancel = true;
                    StopCloseProgram = false;
                    return;
                }

                //Delete all file in folder tmp
                string Path = QLVCConst.PathTemp;
                if (System.IO.Directory.Exists(Path))
                {
                    string[] filePaths = Directory.GetFiles(Path);
                    try
                    {
                        foreach (string filePath in filePaths)
                            File.Delete(filePath);
                    }
                    catch { }
                }
            };
        }

        private void LoadInfo()
        {
            try
            {
                //Add temp ChiefAccountant
                lblUserInfo.Caption = string.Format("{0} | {1:dd/MM/yyyy} | Chi nhánh: {2} - Ngành: {3}"
                    , clsPublicVar.User.TenDangNhap, DataUtils.GetDate(), clsPublicVar.ChiNhanh.Ten, clsPublicVar.Nganh.Ten);
            }
            catch { }
        }

        public override void SetStatus(DataModeType e)
        {
            switch (e)
            {
                case DataModeType.View: ChangeStatus("Chế độ xem.", Properties.Resources.imac_on); break;
                case DataModeType.AddNew: ChangeStatus("Chế độ thêm.", Properties.Resources.Add_button); break;
                case DataModeType.Edit: ChangeStatus("Chế độ chỉnh sửa.", Properties.Resources.Edit); break;
                case DataModeType.Unknown: ChangeStatus("Sẳn sàng.", Properties.Resources.imac_off); break;
            }
        }

        public void ChangeStatus(string Text, Bitmap ico)
        {
            lblStatus.Caption = Text;
            if (ico != null)
                lblStatus.Glyph = ico;
            else
                lblStatus.Glyph = Properties.Resources.imac_off;
        }

        protected override object ReceivedRequest(BaseForm frmRequest, string requestType, object requestValue)
        {
            switch (requestType)
            {
                case QLVCConst.RequestRecordStatus:
                    lbRecord.Caption = Cast.ToString(requestValue);
                    return lbRecord.Caption;
                case QLVCConst.RequestSearchBox:
                    btnMainButton.ActiveSearchTool(requestValue as SearchPanel);
                    return requestValue;
                case QLVCConst.RequestSQL:
                    return SQLReport.ResourceManager.GetString(Cast.ToString(requestValue));
                //case QLVCConst.RequestNewToolbarButton:
                //    return btnMainButton.RegistrButton(frmRequest, requestValue as ActionItem);
                //case QLVCConst.RequestOpenForm:
                //    BarButtonItem item = bar1.Items.Cast<NavBarItemEx>().FirstOrDefault(btn => (((sysMenu)btn.Tag).ID == Cast.ToSafeString(requestValue)));
                //    if (item != null)
                //    {
                //        item.LinkClick();
                //        return item.Tag as sysMenu;
                //    }
                //    return null;
                case QLVCConst.StopCloseProgram:
                    StopCloseProgram = Cast.ToBoolean(requestValue);
                    return requestValue;
            }

            return null;
        }

        public override void ShowButtonAction(ButtonAction Action, bool IsEnable)
        {
            VisibleUserControl(true);
            btnMainButton.ShowButtonAction(Action, IsEnable);
        }

        public override void EnableAllButtonActions(bool Enable)
        {
            btnMainButton.EnableAllButtonActions(Enable);

            lbRecord.Caption = string.Format("{0}: {1}", clsPublicVar.SumRecord, 0);
        }

        public override void EnableButtonAction(Form frmChild, ButtonAction[] actions)
        {
            VisibleUserControl(true);
            btnMainButton.EnableButtonAction(frmChild, actions);
        }

        private void _xtm_PageRemoved(object sender, MdiTabPageEventArgs e)
        {
            if (xtraTabbedMdiManager.Pages.Count < 1)
            {
                VisibleUserControl(false);
                btnMainButton.EnableAllButtonActions(false);
            }
        }

        private void VisibleUserControl(bool IsVisible)
        {
            btnMainButton.Visible = IsVisible;
        }

        private void barManager_HighlightedLinkChanged(object sender, HighlightedLinkChangedEventArgs e)
        {
            if (e.Link == null)
            {
                barManager.GetToolTipController().HideHint();
                return;
            }

            if (e.Link.IsLinkInMenu || e.Link.Bar.BarName == bar1.BarName)
                return;

            ToolTipControllerShowEventArgs te = new ToolTipControllerShowEventArgs();
            te.ToolTipLocation = ToolTipLocation.BottomCenter;
            te.SuperTip = new SuperToolTip();
            te.SuperTip.Items.Add(e.Link.Caption);
            Point linkPoint = new Point(e.Link.Bounds.Right, e.Link.Bounds.Bottom);
            barManager.GetToolTipController().ShowHint(te, e.Link.LinkPointToScreen(linkPoint));
        }
    }
}
