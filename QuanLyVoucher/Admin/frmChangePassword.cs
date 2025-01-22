using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Base.AppliBaseForm;
using Business;
using Base.DevExpressEx.Utility;
using QuanLyVoucher.Globals;

namespace QuanLyVoucher.Admin
{
    public partial class frmChangePassword : BaseForm
    {
        private sys_UserBusiness sys_UserB = new sys_UserBusiness();
        private ValidateUtis Valid = new ValidateUtis();

        public frmChangePassword()
        {
            InitializeComponent();

            //..
            Valid.AddValidNotBlank(txtOldPassword, "Bạn chưa nhập mật khẩu cũ");
            Valid.AddValidNotBlank(txtNewPassword, "Bạn chưa nhập mật khẩu mới");
            Valid.AddValidNotBlank(txtConfirmPassword, "Bạn chưa nhập mật khẩu xác nhận");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Valid.ValidateOK())
                return;
            else
            {
                Valid.AddValidKeysExist(txtOldPassword, "Mật khẩu cũ không đúng"
                  , o => !sys_UserB.Password(clsPublicVar.User.TenDangNhap, Base.Common.Utility.GetSHA512(txtOldPassword.Text)), "");
                Valid.AddValidKeysExist(txtConfirmPassword, "Mật khẩu mới và xác nhận mật khẩu không đúng"
                    , o => txtNewPassword.Text != txtConfirmPassword.Text, "");
                Valid.AddValidKeysExist(txtNewPassword, "Mật khẩu mới và cũ giống nhau. Xin vui lòng nhập mật khẩu khác"
                    , o => txtNewPassword.Text == txtOldPassword.Text, "");

                if (!Valid.ValidateOK())
                    return;

                clsPublicVar.User.Password = Base.Common.Utility.GetSHA512(txtNewPassword.Text);
                State = sys_UserB.Update(clsPublicVar.User, t => t.Ma == clsPublicVar.User.Ma, new string[] { "Password" }, false, true);

                if (State.Success)
                {
                    SetSuccess("Cập nhật thành công", "Thông báo");
                    this.Close();
                }
                else
                    SetFailure(State.Exception.Message, "Không thể thay đổi mật khẩu");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
