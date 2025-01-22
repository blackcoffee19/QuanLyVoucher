using Base.DevExpressEx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace Tool.Search
{
    public class SearchControl
    {
        BindingControlsUtil_DevEx BCU = new BindingControlsUtil_DevEx();

        public Control Co { get; set; }
        public object Value { get; set; }

        protected Func<object> FuDefaultValue;
        protected Func<object> FuEmptyValue;
        protected Func<object, string> FuReturnText;
        public Action FuSpecialUndo;
        public Func<object> FuSpecialAccept;

        public SearchControl(Control co
            , Expression<Func<object>> fuDefaultValue, Expression<Func<object>> fuEmptyValue, Func<object, string> fuReturnText)
        {
            Co = co;
            FuDefaultValue = fuDefaultValue.Compile();
            FuEmptyValue = fuEmptyValue.Compile();
            FuReturnText = fuReturnText;
        }

        public void SetDefault()
        {
            BCU.SetValue(Co, FuDefaultValue.Invoke());
        }

        public void SetEmpty()
        {
            BCU.SetValue(Co, FuEmptyValue.Invoke());
        }

        public void AcceptSearch()
        {
            if (FuSpecialAccept == null)
                Value = BCU.GetValue(Co);
            else
                Value = FuSpecialAccept.Invoke();
        }

        public void UndoSearch()
        {
            if (FuSpecialUndo == null)
                BCU.SetValue(Co, Value);
            else
                FuSpecialUndo.Invoke();
        }

        public string GetReturnText()
        {
            if (Value != FuEmptyValue.Invoke())
                return FuReturnText.Invoke(Value);
            return "";
        }
    }
}
