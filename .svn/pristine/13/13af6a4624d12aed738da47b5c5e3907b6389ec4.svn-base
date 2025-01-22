using Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public partial class sys_User
    {
        public enum GioiTinhType
        {
            Nam,
            Nu
        }

        public string PasswordEx
        {
            get
            {
                if (string.IsNullOrEmpty(Password))
                    return string.Empty;
                return "******";
            }
            set
            {
                Password = Utility.GetSHA512(value);
            }
        }

        public string GioiTinhEx
        {
            get
            {
                if (GioiTinh ?? false)
                    return Enum.GetName(typeof(GioiTinhType), GioiTinhType.Nam);
                return Enum.GetName(typeof(GioiTinhType), GioiTinhType.Nu);
            }
            set
            {
                if (value == Enum.GetName(typeof(GioiTinhType), GioiTinhType.Nam))
                    GioiTinh = true;
                else
                    GioiTinh = false;
            }
        }
    }
}
