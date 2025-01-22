using Base.AppliBaseForm;
using Base.DevExpressEx.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool
{
    public class SupportTool
    {
        public static void SetDisplayRowStatus(BaseForm frmChild, BaseForm frmParent, GridViewEx GvMain, string RequestRecordStatus, string text)
        {
            GvMain.GridControl.Enter += (sender, e) =>
                frmChild.Request(frmChild, frmParent, RequestRecordStatus, string.Format("{0}: {1}", text, GvMain.RowCount));

            GvMain.RowCountChanged += (sender, e) =>
            {
                if (GvMain.IsFocusedView)
                    frmChild.Request(frmChild, frmParent, RequestRecordStatus, string.Format("{0}: {1}", text, GvMain.RowCount));
            };

            frmChild.Activated += (sender, e) =>
            {
                if (GvMain.GridControl.Enabled)
                    frmChild.Request(frmChild, frmParent, RequestRecordStatus, string.Format("{0}: {1}", text, GvMain.RowCount));
            };

            frmChild.Leave += (sender, e) =>
                frmChild.Request(frmChild, frmParent, RequestRecordStatus, "");
        }
    }
}
