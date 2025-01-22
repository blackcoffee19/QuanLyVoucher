using Base.Common;
using Base.Common.Items;
using Common;
using Common.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyVoucher
{
    public class Remember : ConfigBase
    {
        public HandleState SetRemember(string _userID, string _passWord, string _chiNhanh, string _nganh
            , string _serverSQL, string _userSQL, string _passwordSQL, string _databaseSQL)
        {
            try
            {
                HandleState handleState = new HandleState("Error");

                //Copy XML file from QLVC app to profile in clickonce application
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    string sourceFile = QLVCConst.ConfigurePath;
                    string destFile = QLVCConst.ConfigurePathInProfile;
                    int iCountNodeSource = GetCountNode("QLVC", sourceFile), iCountNodeDest = GetCountNode("QLVC", destFile);

                    //Copy neu chua co file XML
                    if ((File.Exists(sourceFile) && !File.Exists(destFile))
                        || iCountNodeSource != iCountNodeDest)
                        File.Copy(sourceFile, destFile, true);

                    //Xu ly luu file trong profile user
                    handleState = SetInformation(QLVCConst.ConfigurePathInProfile, _userID, _passWord, _chiNhanh, _nganh
                        , _serverSQL, _userSQL, _passwordSQL, _databaseSQL);
                }

                //Xu ly luu file trong folder setup
                handleState = SetInformation(QLVCConst.ConfigurePath, _userID, _passWord, _chiNhanh, _nganh, _serverSQL, _userSQL
                    , _passwordSQL, _databaseSQL);

                return handleState;
            }
            catch (Exception ex) { return new HandleState(ex); }
        }

        public HandleState GetRemember(ref string XMLPath, ref string _userID, ref string _passWord, ref string _chiNhanh
            , ref string _nganh, ref string _serverSQL, ref string _userSQL, ref string _passwordSQL, ref string _databaseSQL)
        {
            try
            {
                _serverSQL = Get("QLVC.ConnectionSQL.Server", "", XMLPath);
                _userSQL = Get("QLVC.ConnectionSQL.User", "", XMLPath);
                _passwordSQL = GetDecrypt("QLVC.ConnectionSQL.Password", "", XMLPath);
                _databaseSQL = Get("QLVC.ConnectionSQL.Database", "", XMLPath);

                GetInformation(XMLPath, ref _userID, ref _passWord, ref _chiNhanh, ref _nganh);

                if (string.IsNullOrWhiteSpace(_userID) && ApplicationDeployment.IsNetworkDeployed)
                {
                    XMLPath = QLVCConst.ConfigurePathInProfile;
                    if (OpenConfig(XMLPath).Success)
                        GetInformation(XMLPath, ref _userID, ref _passWord, ref _chiNhanh, ref _nganh);
                }

                return new HandleState();
            }
            catch (Exception ex) { return new HandleState(ex); }
        }

        private void GetInformation(string XMLPath, ref string _userID, ref string _passWord, ref string _chiNhanh, ref string _nganh)
        {
            _userID = Get("QLVC.Login.user", "", XMLPath);
            _passWord = GetDecrypt("QLVC.Login.password", "", XMLPath);
            _chiNhanh = Get("QLVC.Login.chinhanh", "", XMLPath);
            _nganh = Get("QLVC.Login.nganh", "", XMLPath);
        }

        private HandleState SetInformation(string XMLPath, string _userID, string _passWord, string _chiNhanh, string _nganh, string _serverSQL, string _userSQL, string _passwordSQL, string _databaseSQL)
        {
            if (OpenConfig(XMLPath).Success)
            {
                Set("QLVC.Login.user", "", _userID, XMLPath);
                Set("QLVC.Login.password", "", CryptographyUtility.Encrypt(_passWord), XMLPath);
                Set("QLVC.Login.chinhanh", "", _chiNhanh, XMLPath);
                Set("QLVC.Login.nganh", "", _nganh, XMLPath);
                Set("QLVC.ConnectionSQL.Server", "", _serverSQL, XMLPath);
                Set("QLVC.ConnectionSQL.User", "", _userSQL, XMLPath);
                Set("QLVC.ConnectionSQL.Password", "", CryptographyUtility.Encrypt(_passwordSQL), XMLPath);
                Set("QLVC.ConnectionSQL.Database", "", _databaseSQL, XMLPath);

                if (!string.IsNullOrEmpty(_userID))
                    Set("QLVC.Login.date", "", CryptographyUtility.Encrypt(DataUtils.GetDate().ToString("yyyyMMdd")), XMLPath);
                else
                    Set("QLVC.Login.date", "", "", XMLPath);

                HandleState handleState = SaveConfig(XMLPath);
                return handleState;
            }
            return new HandleState("Error");
        }
    }
}
