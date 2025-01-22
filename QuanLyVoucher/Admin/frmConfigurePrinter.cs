using Base.AppliBaseForm;
using Base.Common;
using Base.Common.Items;
using Base.DevExpressEx.Utility;
using Common;
using Common.Globals;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyVoucher.Admin
{
    public partial class frmConfigurePrinter : BaseForm
    {
        private ValidateUtis Valid = new ValidateUtis();
        private ConfigBase cfb = new ConfigBase();

        public frmConfigurePrinter()
        {
            InitializeComponent();

            //..
            List<BasicItem> ListPrinter = new List<BasicItem>();
            foreach (string printname in PrinterSettings.InstalledPrinters)
                ListPrinter.Add(new BasicItem(printname, printname));
            cboPrinter.Build("Text", "Value", ListPrinter.ToList(), 200, 300
                , new LookUpColumnInfo("Text", 100, "Tên máy in"));

            //..
            string sTenMayIn = "";
            bool bChonMayInKhiIn = true;
            int iSLHDChoLanIn = 0;
            QLVCConst.ThongTinMayIn(ref sTenMayIn, ref bChonMayInKhiIn, ref iSLHDChoLanIn);
            cboPrinter.EditValue = sTenMayIn;
            chkChonMayInKhiIn.Checked = bChonMayInKhiIn;
            txtSLInHoaDon.EditValue = iSLHDChoLanIn;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Valid.AddValidKeysExist(cboPrinter, "Bạn vui lòng chọn máy in mặc định hoặc check chọn máy in khi in"
                , o => Cast.ToString(cboPrinter.EditValue) == "" && !chkChonMayInKhiIn.Checked ? true : false, "");
            Valid.AddValidKeysExist(txtSLInHoaDon, "Số lượng in hóa đơn/lần phải lớn hơn 0"
                , o => Cast.ToInt(txtSLInHoaDon.EditValue) <= 0, "");

            if (!Valid.ValidateOK())
                return;
            else
            {
                HandleState handleState = new HandleState("Error");

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    handleState = XuLyLuu(QLVCConst.ConfigurePathInProfile);
                    XuLyLuu(QLVCConst.ConfigurePath);
                }
                else
                    handleState = XuLyLuu(QLVCConst.ConfigurePath);

                if (handleState.Success)
                {
                    SetSuccess("Cấu hình máy in thành công", "Thông báo");
                    this.Close();
                }
                else
                    SetFailure(State.Exception.Message, "Không thể thay đổi cấu hình");
            }
        }

        private HandleState XuLyLuu(string XMLPath)
        {
            if (cfb.OpenConfig(XMLPath).Success)
            {
                cfb.Set("QLVC.Printer.TenMayIn", "", Cast.ToString(cboPrinter.EditValue), XMLPath);
                cfb.Set("QLVC.Printer.ChonMayInKhiIn", "", Cast.ToString(chkChonMayInKhiIn.Checked), XMLPath);
                cfb.Set("QLVC.Printer.SLHDChoLanIn", "", Cast.ToString(txtSLInHoaDon.EditValue), XMLPath);
                HandleState handleState = cfb.SaveConfig(XMLPath);
                return handleState;
            }
            return new HandleState("Error");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
