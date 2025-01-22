using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_QuyenBusiness : Base<sys_Quyen>
    {
        public IQueryable<sys_Quyen> GetData()
        {
            return Get(t => !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }
    }
}
