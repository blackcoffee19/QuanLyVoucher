using Base.AppliBaseForm.Globals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyVoucher.Transaction
{
    public partial class frmThanhToan
    {
        private usrThanhToanToolTimKiem uSearch = new usrThanhToanToolTimKiem();

        public void ConfigSearchBox()
        {
            uSearch.ReturnSearchText += txt => Browse();
        }

        protected override void BaseForm_Activated(object sender, EventArgs e)
        {
            base.BaseForm_Activated(sender, e);

            //add searchbox
            if (_parent != null && _parent is frmMain
                && Actions.Contains(ButtonAction.Browse) && !ActionsDeny.Contains(ButtonAction.Browse))
                ((frmMain)_parent).btnMainButton.ActiveSearchTool(uSearch);
        }

        protected override void BaseForm_Deactivate(object sender, EventArgs e)
        {
            base.BaseForm_Deactivate(sender, e);

            //remove searchbox
            if (_parent != null && _parent is frmMain)
                ((frmMain)_parent).btnMainButton.ActiveSearchTool(null);
        }
    }
}
