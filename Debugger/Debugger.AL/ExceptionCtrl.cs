using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Debugger.AL
{
    public partial class ExceptionCtrl : UserControl
    {
        public ExceptionCtrl()
        {
            InitializeComponent();
        }
        public event GetFileFromTemp OnGetTemp;
        public void Clear()
        {
            try
            {
                AddMSG("", "");
            }
            catch
            {

            }
        }
        private void addMessageToList(string lb, string txt)
        {
            try
            {
                labelX1.Text = lb;
                textBoxX1.Text = txt;
            }
            catch
            {

            }
        }
        private delegate void addMessageDelegate(string lb, string txt);
        void AddMSG(string lb, string txt)
        {
            try
            {

                if (labelX1.InvokeRequired)
                {
                    addMessageDelegate d = new addMessageDelegate(addMessageToList);
                    labelX1.Invoke(d, lb,txt);
                }
                else
                {
                    addMessageToList(lb,txt);
                }

            }
            catch (Exception ex)
            {

            }
        }
        public string GetStackTrace(Thread ext,string formatSymbols, string formatNoSymbols)
        {
            StringBuilder stackTrace = new StringBuilder();
            foreach (StackFrame stackFrame in ext.GetCallstack(100))
            {
                SequencePoint loc = stackFrame.NextStatement;
                stackTrace.Append("   ");
                if (loc != null)
                {
                    stackTrace.AppendFormat(formatSymbols, stackFrame.MethodInfo.FullName, OnGetTemp(loc.Filename), loc.StartLine);
                }
                else
                {
                    stackTrace.AppendFormat(formatNoSymbols, stackFrame.MethodInfo.FullName);
                }
                stackTrace.AppendLine();
            }
            return stackTrace.ToString();
        }
        public string GetStackTrace(Thread ext)
        {
            return GetStackTrace(ext,"at {0} in {1}:line {2}", "at {0}");
        }
        public void FillException(Thread ext)
        {
            try
            {
              List<Value> innerExceptions = new List<Value>();
                for (Value innerException = ext.CurrentException; !innerException.IsNull; innerException = innerException.GetFieldValue("_innerException"))
                {
                    innerExceptions.Add(innerException.GetPermanentReferenceOfHeapValue());
                }
                string stacktrace = string.Empty;
                for (int i = 0; i < innerExceptions.Count; i++)
                {
                    if (i > 0)
                    {
                        stacktrace += " ---> ";
                    }
                    stacktrace += innerExceptions[i].Type.FullName;
                    Value messageValue = innerExceptions[i].GetFieldValue("_message");
                    if (!messageValue.IsNull)
                    {
                        stacktrace += ": " + messageValue.AsString();
                    }
                }
                AddMSG(stacktrace, GetStackTrace(ext));

            }
            catch
            {

            }
        }
    }
}
