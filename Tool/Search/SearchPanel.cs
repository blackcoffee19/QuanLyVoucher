﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Base.DevExpressEx.Utility;
using DevExpress.XtraEditors.Controls;

namespace Tool.Search
{
    public partial class SearchPanel : UserControl
    {
        public event Action<string> ReturnSearchText;
        public event Action PleaseHideMe;

        public virtual string ReturnText
        {
            get
            {
                StringBuilder str = new StringBuilder();
                SC.ForEach(t => str.Append(t.GetReturnText()));
                return str.ToString();
            }
        }
        public int iTextWidthMinimum = 200;
        protected List<SearchControl> SC = new List<SearchControl>();

        private BindingControlsUtil_DevEx BCU = new BindingControlsUtil_DevEx();

        public SearchPanel()
        {
            ReturnSearchText += t => { };
            PleaseHideMe += () => { };
        }

        public void OnReturnSearchText()
        {
            ReturnSearchText(ReturnText);
        }

        public void OnPleaseHideMe() { PleaseHideMe(); }

        protected virtual void Accept()
        {
            SC.ForEach(t => t.AcceptSearch());
        }

        protected virtual void Undo()
        {
            SC.ForEach(t => t.UndoSearch());
        }

        protected virtual void Reset()
        {
            SC.ForEach(t =>
            {
                if (t.Co is CheckEdit && ((CheckEdit)t.Co).Properties.CheckStyle != CheckStyles.Radio)
                    ((CheckEdit)t.Co).EditValue = null;
                else
                    t.SetDefault();
            });
        }

        protected virtual void Clear()
        {
            SC.ForEach(t =>
            {
                if (t.Co is CheckEdit && ((CheckEdit)t.Co).Properties.CheckStyle != CheckStyles.Radio)
                    ((CheckEdit)t.Co).EditValue = null;
                else
                    t.SetEmpty();
            });
        }

        protected virtual void Clear(List<string> IgnoreListClear)
        {
            SC.ForEach(t =>
            {
                if (!IgnoreListClear.Contains(t.Co.Name.ToString()))
                {
                    if (t.Co is CheckEdit && ((CheckEdit)t.Co).Properties.CheckStyle != CheckStyles.Radio)
                        ((CheckEdit)t.Co).EditValue = null;
                    else
                        t.SetEmpty();
                }
            });
        }

        public object GetValue(string controlName)
        {
            return SC.First(t => t.Co.Name == controlName).Value;
        }

        public virtual void SetValue(string controlName, object value)
        {
            BCU.SetValue(SC.First(t => t.Co.Name == controlName).Co, value);
            Accept();
        }

        public void AppyNormalEvent(SimpleButton btnCancel, SimpleButton btnOK, SimpleButton btnReset, SimpleButton btnClear)
        {
            btnCancel.Click += (sender, e) =>
            {
                Undo();
                OnPleaseHideMe();
            };

            btnOK.Click += (sender, e) =>
            {
                Accept();
                OnReturnSearchText();
                OnPleaseHideMe();
            };

            btnReset.Click += (sender, e) => Reset();
            btnClear.Click += (sender, e) => Clear();
        }

        public void AppyNormalEvent(SimpleButton btnCancel, SimpleButton btnOK, SimpleButton btnReset, SimpleButton btnClear
            , List<string> IgnoreListClear)
        {
            btnCancel.Click += (sender, e) =>
            {
                Undo();
                OnPleaseHideMe();
            };

            btnOK.Click += (sender, e) =>
            {
                try
                {
                    Accept();
                    OnReturnSearchText();
                    OnPleaseHideMe();
                }
                catch
                {
                    MessageBox.Show("Lỗi không lấy được dữ liệu");

                    Reset();
                }
            };

            btnReset.Click += (sender, e) => Reset();
            btnClear.Click += (sender, e) => Clear(IgnoreListClear);
        }
    }
}
