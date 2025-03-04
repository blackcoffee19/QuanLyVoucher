﻿using Base.Common;
using Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DataUtils
    {
        private static QLVCDataContext _DC;
        private static QLVCDataContext DC
        {
            get
            {
                if (_DC == null)
                    _DC = new QLVCDataContext(QLVCConst.ConnectionStrings);
                return _DC;
            }
        }

        public static DateTime GetDate()
        {
            //return Cast.ToDateTime(DC.fnGetDateTimeNow());
            return DC.fnGetDateTimeNow() ?? DateTime.Now;
        }
    }
}
