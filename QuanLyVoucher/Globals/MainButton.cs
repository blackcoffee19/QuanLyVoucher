using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Base.AppliBaseForm.Globals;
using DevExpress.XtraBars;
using Tool.Search;

namespace QuanLyVoucher.Globals
{
    public partial class MainButton : UserControl
    {
        public MainButton()
        {
            InitializeComponent();

            //..
            ActiveSearchTool(null);
        }

        public void EnableButtonAction(Form frmActived, ButtonAction[] actions)
        {
            foreach (var t in actions)
            {
                switch (t)
                {
                    case ButtonAction.Add: btnAdd.Enabled = true; break;
                    case ButtonAction.Delete: btnDelete.Enabled = true; break;
                    case ButtonAction.Edit: btnEdit.Enabled = true; break;
                    case ButtonAction.Save: btnSave.Enabled = true; break;
                    case ButtonAction.Browse: btnBrowse.Enabled = true; break;
                }
            }
        }

        public void ShowButtonAction(ButtonAction action, bool Enabled)
        {
            switch (action)
            {
                case ButtonAction.Add: btnAdd.Enabled = Enabled; break;
                case ButtonAction.Delete: btnDelete.Enabled = Enabled; break;
                case ButtonAction.Edit: btnEdit.Enabled = Enabled; break;
                case ButtonAction.Save: btnSave.Enabled = Enabled; break;
                case ButtonAction.Browse: btnBrowse.Enabled = Enabled; break;
            }
        }

        public void EnableAllButtonActions(bool Enabled)
        {
            btnAdd.Enabled = Enabled;
            btnDelete.Enabled = Enabled;
            btnEdit.Enabled = Enabled;
            btnSave.Enabled = Enabled;
            btnBrowse.Enabled = Enabled;
        }

        public void ActiveSearchTool(SearchPanel co)
        {
            //clear first
            while (cboSearchContainerControl.Controls.Count > 0)
            {
                ((SearchPanel)cboSearchContainerControl.Controls[0]).ReturnSearchText -= MainToolBar_ReturnSearchText;
                ((SearchPanel)cboSearchContainerControl.Controls[0]).PleaseHideMe -= MainToolBar_PleaseHideMe;
                cboSearchContainerControl.Controls.RemoveAt(0);
            }
            MainToolBar_ReturnSearchText("");

            //..
            if (co == null)
                cboSearch.Visibility = BarItemVisibility.Never;
            else
            {
                cboSearch.Visibility = BarItemVisibility.Always;
                cboSearchContainerControl.Controls.Add(co);
                cboSearchContainerControl.Size = co.Size;
                cboSearch.Width = co.iTextWidthMinimum;

                MainToolBar_ReturnSearchText(co.ReturnText);

                co.ReturnSearchText += MainToolBar_ReturnSearchText;
                co.PleaseHideMe += MainToolBar_PleaseHideMe;
            }
        }

        void MainToolBar_ReturnSearchText(string txtFilterSelected)
        {
            cboSearch.EditValue = txtFilterSelected;
        }

        void MainToolBar_PleaseHideMe()
        {
            if (cboSearchContainerControl.OwnerEdit != null)
                cboSearchContainerControl.OwnerEdit.ClosePopup();
        }
    }
}
