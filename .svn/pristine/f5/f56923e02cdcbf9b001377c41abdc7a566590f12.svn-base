using Base.Connection.LINQ;
using Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Base<T> : ContextBase<T> where T : class, new()
    {
        public Base()
            : base()
        {
            ConfigDataContext<QLVCDataContext>(QLVCConst.ConnectionStrings);
        }
    }

    public class BaseCRMHHH<T> : ContextBase<T> where T : class, new()
    {
        public BaseCRMHHH()
            : base()
        {
            ConfigDataContext<QLVCDataContext>(QLVCConst.ConnectionSPAHHH);
        }
    }
}
