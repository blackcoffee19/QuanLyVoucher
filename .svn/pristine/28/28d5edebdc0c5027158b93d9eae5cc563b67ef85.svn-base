using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class sys_RoleBusiness : Base<sys_Role>
    {
        public IQueryable<sys_Role> GetData()
        {
            return Get(t => !(t.DaXoa ?? false)).OrderByDescending(t => t.NgayTao);
        }
    }
}
