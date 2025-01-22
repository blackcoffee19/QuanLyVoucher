using Base.AppliBaseForm;
using Base.AppliBaseForm.Globals;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class SetActionForForm
    {
        public enum Quyen
        {
            Add = 1,
            Edit = 2,
            Delete = 3
        }

        public static void SetAction(BaseForm baseForm, int MenuCha, List<vQuyen> ListTatCaQuyen)
        {
            sys_Menu mn = new Base<sys_Menu>().First(t => t.DuongDan == baseForm.GetType().ToString()
                && t.MenuCon == MenuCha);

            if (mn == null)
                mn = new Base<sys_Menu>().First(t => t.DuongDan == baseForm.GetType().ToString());

            if (mn != null)
            {
                List<vQuyen> ListQuyen = ListTatCaQuyen.Where(t => t.MaMenu == mn.Ma)
                .GroupBy(t => t.MaQuyen).Select(t => t.FirstOrDefault()).ToList();

                foreach (vQuyen p in ListQuyen)
                {
                    switch (p.MaQuyen)
                    {
                        case (int)Quyen.Add:
                            {
                                if (!baseForm.Actions.Exists(t => t == ButtonAction.Save))
                                    baseForm.ActionsDeny.Remove(ButtonAction.Add);
                            }
                            break;
                        case (int)Quyen.Edit:
                            {
                                if (!baseForm.Actions.Exists(t => t == ButtonAction.Save))
                                    baseForm.ActionsDeny.Remove(ButtonAction.Edit);
                            }
                            break;
                        case (int)Quyen.Delete: baseForm.ActionsDeny.Remove(ButtonAction.Delete);
                            break;
                    }
                }
            }
        }
    }
}
