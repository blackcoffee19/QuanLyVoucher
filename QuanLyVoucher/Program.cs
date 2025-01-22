using Base.Common;
using Microsoft.Win32;
using QuanLyVoucher.Globals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVoucher
{
    static class Program
    {
        public static bool m_IsLoginAs = false;
        public static string m_Username = "";
        public static frmMain fMain;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string[] myList = null;
            try
            {
                string[] activationData = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
                foreach (string arg in activationData)
                {
                    myList = activationData[0].Split(new char[] { ',' },
                      StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch
            {

            }

            bool bTryAgain = true;

            while (bTryAgain)
            {
                bTryAgain = false;
                var dialog = new frmLogin(myList);
                //var dialog = new frmLogin(new string[] { "", "823083F955FE556EB61344E906AA954064E4C501FBF7B8067CA73035A5D4FAB8C10F51A2004F9B7BB68C254A8EBE0A43392F5695C71269BF50927B21C7FB0873" });
                if (m_IsLoginAs)
                {
                    dialog = new frmLogin(true, m_Username);
                    m_IsLoginAs = false;
                }

                using (dialog)
                {
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    var font = new Font(Base.AppliBaseForm.Globals.Const.DefaultFont, Cast.ToInt(Base.AppliBaseForm.Globals.Const.DefaultFontSize));
                    DevExpress.Utils.AppearanceObject.DefaultFont = font;
                    DevExpress.Utils.AppearanceDefault.Empty.Font = font;

                    DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BonusSkins).Assembly);

                    fMain = new frmMain();
                    fMain.Show();

                    Application.Run(fMain);

                    //..
                    bTryAgain = fMain.bSignOut;
                }
            }

            try
            {
                clsPublicVar.Conn.Close();
            }
            catch { }
        }
    }
}
