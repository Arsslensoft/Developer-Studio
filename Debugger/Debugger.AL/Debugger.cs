using DevComponents.AdvTree;
using MoreComplexPopup;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Debugger.AL
{
    public class Breakpoint
    {
        public string FileName {get;set;}
        public int Line {get;set;}
        public bool Enabled {get;set;}
       
    }
    public class ArsslenLanguageDebugger
    {
        public List<Breakpoint> BP = new List<Breakpoint>();
        public Process ExecutionProcess
        {
            get { return WindowsDebugger.CurrentProcess; }
        }
        internal NDebugger deb;
        internal List<TreeNode> Nodes;
        public Options DebuggerOptions { get; set; }
        public event EventHandler<DebuggerPausedEventArgs> OnBreakPointHit;
        public event EventHandler<DebuggerPausedEventArgs> OnPaused;
        public string Executable { get; set; }
        public ArsslenLanguageDebugger()
        {
            deb = new NDebugger();
            DebuggerOptions = new Options();
        }

        public List<Thread> Threads
        {
            get
            {
                List<Thread> t = new List<Thread>();
                foreach (Thread thr in ExecutionProcess.Threads)
                    t.Add(thr);

                return t;
            }
        }
  

        public void SetBreakPointState(string file, int line, bool state)
        {
            int i = 0;
            foreach (Breakpoint p in BP)
            {

                if (p.Line == line && p.FileName == file)
                    break;
                i++;
            }
            if (i < BP.Count)
                BP[i].Enabled = state;
        }
        public void RemoveBreakpoint(string file, int line)
        {
            int i = 0;
            foreach (Breakpoint p in BP)
            {

                if (p.Line == line && p.FileName == file)
                    break;
                i++;
            }
            if (i < BP.Count)
                BP.Remove(BP[i]);
        }
        public void AddBreakPoint(string file, int line)
        {
            int i = 0;
            foreach (Breakpoint p in BP)
            {

                if (p.Line == line && p.FileName == file)
                    return;
                i++;
            }
            BP.Add(new Breakpoint { FileName = file, Enabled = true, Line = line });
        }

        public void DisableAllBreakPoints()
        {
            for (int i = 0; i < BP.Count; i++)
                BP[i].Enabled = false;

         
        }
        public void EnableAllBreakPoints()
        {
            for (int i = 0; i < BP.Count; i++)
                BP[i].Enabled = true;


        }
        public void DeleteAllBreakPoints()
        {
            BP.Clear();
        }

        public void SetOptionsToDefault()
        {
            DebuggerOptions.SuppressJITOptimization = true;
            DebuggerOptions.SuppressNGENOptimization = true;
            DebuggerOptions.EnableJustMyCode = true;
            DebuggerOptions.StepOverFieldAccessProperties = true;
            DebuggerOptions.StepOverDebuggerAttributes = true;
            DebuggerOptions.StepOverAllProperties = false;
            DebuggerOptions.PauseOnHandledExceptions = false;
            DebuggerOptions.EnableEditAndContinue = false;
        }

        public void Start(string executable)
        {
            Start(executable, "", Path.GetDirectoryName(executable), false);
        }
        public void Start(string executable, string args)
        {
            Start(executable, args, Path.GetDirectoryName(executable), false);
        }
        public void Start(string executable, string args,  string workingdir)
        {
            Start(executable, "", workingdir, false);
        }
        public void Start(string executable, string args, string workingdir, bool breakmain)
        {
            NDebugger deb = new NDebugger();
           
            // Set Option
            deb.Options = DebuggerOptions;
            List<ISymbolSource> symbolSources = new List<ISymbolSource>();
            symbolSources.Add(new PdbSymbolSource());
            deb.SymbolSources = symbolSources;

            // TODO:Add Status Invoker
            foreach(Breakpoint p in BP)
              deb.AddBreakpoint(p.FileName, p.Line, 0, p.Enabled);
            Executable = executable;
            Process proc = deb.Start(executable, workingdir,args, breakmain);
            proc.Paused += p_Paused;   
            WindowsDebugger.CurrentProcess = proc;
        }
        public void StartWithoutDebug(string exe)
        {
            deb = new NDebugger();

            // Set Option
            deb.Options = DebuggerOptions;
            System.Diagnostics.ProcessStartInfo ps = new System.Diagnostics.ProcessStartInfo();
            ps.FileName = exe;
            foreach (Breakpoint p in BP)
                deb.AddBreakpoint(p.FileName, p.Line, 0, p.Enabled);
           deb.StartWithoutDebugging(ps);
        }
        List<string> UsedDebugs = new List<string>();
        void TryClean()
        {
            try
            {
                bool deleted = false;
                try
                {
                    deleted = !File.Exists(Path.ChangeExtension(Executable, ".pdb"));
                    File.Delete(Path.ChangeExtension(Executable,".pdb"));
                    deleted = true;
                }
                catch
                {

                }
           
                   foreach(string fu in UsedDebugs)
                   {
                       try
                       {
                    File.Delete(fu);
                }
                catch
                {

                }
        }


                if(!deleted)
                {
                    string name = Path.ChangeExtension(Executable,".pdb");
                    string nn = Path.GetDirectoryName(name) + @"\"+DateTime.Now.ToFileTimeUtc().ToString()+".pdb";
                    File.Move(name, nn);
                    UsedDebugs.Add(nn);
               }

            }
            catch
            {

            }
        }
        public void Stop()
        {
            try
            {
                if (deb == null)
                    return;
                ExecutionProcess.Terminate();
              
            }
            catch
            {

            }
        }

        public void Continue()
        {
            try
            {
                if (deb == null)
                    return;
                ExecutionProcess.AsyncContinue();
            }
            catch
            {

            }
        }

        public void StepInto()
        {
            if (WindowsDebugger.CurrentStackFrame != null)
            {
                WindowsDebugger.CurrentStackFrame.AsyncStepInto();
            }
        }

        public void StepOver()
        {
            if (WindowsDebugger.CurrentStackFrame != null)
            {
                WindowsDebugger.CurrentStackFrame.AsyncStepOver();
            }
        }

        public void StepOut()
        {
            if (WindowsDebugger.CurrentStackFrame != null)
            {
                WindowsDebugger.CurrentStackFrame.AsyncStepOut();
            }
        }
        public  void Break()
        {
            if (ExecutionProcess != null && ExecutionProcess.IsRunning)
            {
               ExecutionProcess.Break();
            }
        }
        void p_Paused(object sender, DebuggerPausedEventArgs e)
        {
            try
            {
                if (e.BreakpointsHit.Count > 0)
                {

                    StackFrame current = e.Thread.GetCallstack(1)[0];
                    WindowsDebugger.CurrentThread = e.Thread;
                    WindowsDebugger.EvalThread = e.Thread;
                    WindowsDebugger.CurrentStackFrame = current;

                    Nodes = ValueNode.GetLocalVariables().ToList();
                    OnBreakPointHit(sender, e);
                }
                else OnPaused(sender, e);
            }
            catch
            {

            }
        }

        public List<StackFrame> GetCallStack()
        {

            return WindowsDebugger.CurrentThread.Callstack.ToList();
        }
        public List<Thread> GetThreads()
        {
            return WindowsDebugger.CurrentProcess.Threads.ToList();
        }

        public TreeNode GetVariableNode(string name)
        {
            foreach (TreeNode nod in Nodes)
            {
                if (nod.Name == name)
                    return nod;
            }
            return null;
        }
        public void ShowVariable(string varname, Point pos)
        {
            ComplexPopup complexPopup = new ComplexPopup();
            PopupControl.Popup p = new PopupControl.Popup(complexPopup);
            p.Resizable = true;
            TreeNode n = GetVariableNode(varname);
            if(n != null)
            {
            Node grandpa = new Node();
            DebugNode and = new DebugNode(n, grandpa);
                and.BindData();
                complexPopup.debugTreeNode1.Nodes.Add(and);
      
            p.Show(pos.X, pos.Y);
             }
     
        }
        public Node ShowVariables()
        {
            ComplexPopup complexPopup = new ComplexPopup();
            PopupControl.Popup p = new PopupControl.Popup(complexPopup);
            p.Resizable = true;
            Node grandpa = new Node();
            grandpa.Text = "Local Variables";
            grandpa.Name = "LVARS";
            foreach (TreeNode n in Nodes)
            {

                DebugNodeNormal and = new DebugNodeNormal(n, grandpa);
                and.BindData();
                grandpa.Nodes.Add(and);
            }
            return grandpa;
        }

    }
}
