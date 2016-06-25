using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using ICSharpCode.AvalonEdit;
using System.Windows.Forms.Integration;
using DevComponents.DotNetBar;
using ICSharpCode.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Windows.Threading;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.AL;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Folding;
using System.IO;
using alproj;
using DevComponents.AdvTree;
using System.Reflection;
using ALCodeDomProvider;
using System.Threading;
using alfrmdesign;
using DevComponents.DotNetBar.Controls;
using MoreComplexPopup;
using Debugger.AL;
using devstd.host;
using CompilersLibraryAPI;
using ICSharpCode.NRefactory.AL.TypeSystem;
using devstd.Forms;
using System.Diagnostics;
using devstd.utils;
using ICSharpCode.AvalonEdit.Search;
using ICSharpCode.NRefactory;

namespace devstd
{
    public partial class MainForm : MetroForm
    {
        FormDesignerControl formDesignerControl1;
        public MainForm()
        {
            InitializeComponent();
          DesignerEvent.ShowCodeEvent += new DSEventBinderDelegate(DesignerEvent_ShowCodeEvent);
            // TODO ADD RES DESIGN
            //formDesignerControl1.Init( this.propertyGrid,this.toolbox);
          formDesignerControl1 = new FormDesignerControl();
           InitDesigner();

            Framework.LoadTargetFrameworks();
      parsetimer.Enabled = true;
      exceptionCtrl1.OnGetTemp += callStack1_OnGetTemp;
            callStack1.OnGetTemp += callStack1_OnGetTemp; 
            callStack1.OnStackSelected += callStack1_OnStackSelected;
            
     
        }
        void props_OpenInfo(object sender, EventArgs e)
        {
            try
            {
                ALProject proj = (ALProject)sender;
                if (proj != null)
                {
                    string file = null;
                    foreach(KeyValuePair<string,SourceFile> sf in proj.SourceFiles)
                        if (sf.Value.SourceType == SourceFileType.AsmInfo)
                        {
                            file = sf.Value.SourcePath;
                            break;
                        }

                    if (file != null)
                    {
                        DockContainerItem tab = FindTabFileName(file);
                        if (tab != null)
                            bar5.SelectedDockContainerItem = tab;

                        else
                            AddNewTextEditor(Path.GetFileName(file), file, proj);
                    }
                
                }

            }
            catch
            {
            }
        }
        void callStack1_OnStackSelected(object sender, EventArgs e)
        {
            try{
                if (debugproj != null)
                {
                    if (sender is Debugger.StackFrame)
                    {
                        Debugger.StackFrame stf = (Debugger.StackFrame)sender;
                        string file = debugproj.TempService.GetFileFromTemp(stf.NextStatement.Filename);
                        DockContainerItem tab = FindTabFileName(file);
                        if (tab != null)
                        {
                            bar5.SelectedDockContainerItem = tab;
                            ActiveEditor.JumpTo(stf.NextStatement.StartLine, stf.NextStatement.StartColumn);
                        }
                        else
                        {
                            AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                            ActiveEditor.JumpTo(stf.NextStatement.StartLine, stf.NextStatement.StartColumn);
                        }
                    }
                    else if(sender is Breakpoint)
                    {
                        Breakpoint stf = (Breakpoint)sender;
                        string file = debugproj.TempService.GetFileFromTemp(stf.FileName);
                        DockContainerItem tab = FindTabFileName(file);
                        if (tab != null)
                        {
                            bar5.SelectedDockContainerItem = tab;
                            ActiveEditor.JumpTo(stf.Line, 0);
                        }
                        else
                        {
                            AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                            ActiveEditor.JumpTo(stf.Line, 0);
                        }
                    }
                }

            }
            catch{
            }
        }
        string callStack1_OnGetTemp(string file)
        {
            return debugproj.TempService.GetFileFromTemp(file);
        }
        private void ressourceExplorerControl1_OnRessourceModified(object sender, EventArgs e)
        {

            try
            {
                 advTree1.Nodes.Remove(advTree1.Nodes[0]);
                 if (!projectworker.IsBusy)
                 {
                     projact = ProjectAction.JustSave;
                     progbar.Visible = true;
                     projectworker.RunWorkerAsync();
                 }
                 LoadSolutionControl();
                 if (CSol != null && IsProjectProperties)
                 {
                     if (parg == null)
                         parg = new ParserArgument();

                     parg.Command = ParserCommand.ParseRessource;
                   //  parg.FileName = ActiveEditor.FileName;
                     parg.Project = (ALProject)sender;
             
                 progbar.Visible = true;
                 parserworker.RunWorkerAsync();
                 }
            }
            catch
            {

            }

        }

        string recordinghost;
        CVP.CVPRecorder cvpw;
        bool cvprecording = false;

        #region Editor
        public TabHost ActiveHost
        {
            get{
                DockContainerItem tab = bar5.SelectedDockContainerItem;
                if (tab != startpage)
                     return (TabHost)tab.Tag;
                
                return null;
            }
        }
        public bool IsFormDesigner
        {
            get
            {
                if (ActiveHost != null)
                    return (ActiveHost.ControlType == EditorType.FormDesigner);
                else return false;

            }
        }
        public bool IsCodeEditorProject
        {
            get
            {
                if (ActiveHost != null)
                    return (ActiveHost.ControlType == EditorType.CodeEditorProject );
                else return false;

            }
        }
        public bool IsCodeEditor
        {
            get
            {
                if (ActiveHost != null)
                    return (ActiveHost.ControlType == EditorType.CodeEditor);
                else return false;

            }
        }
        public bool IsProjectProperties
        {
            get
            {
                if (ActiveHost != null)
                    return (ActiveHost.ControlType == EditorType.PropertyControl);
                else return false;

            }
        }

        public ProjectProperties ActiveProperties
        {
            get
            {

                try
                {

                    if (IsProjectProperties)
                        return ActiveHost.ProjectPropertiesEditor;
                    
                }
                catch
                {

                }

                return null;
            }
        }

        private ICSharpCode.CodeCompletion.CSharpCompletion completion = new ICSharpCode.CodeCompletion.CSharpCompletion(new ScriptProvider());
        CodeTextEditor current;
        DispatcherTimer foldingUpdateTimer;
        /// <summary>Returns the currently displayed editor, or null if none are open</summary>
        private CodeTextEditor ActiveEditor
        {
            get
            {
                try
                {

                    if (IsCodeEditor || IsCodeEditorProject)
                    {
                        current = ActiveHost.TextEditor;
                        return ActiveHost.TextEditor;
                    }
                }
                catch
                {

                }
    
                return null;
            }
        }
       private bool IsModified(CodeTextEditor editor)
        {
            // TextEditorControl doesn't seem to contain its own 'modified' flag, so 
            // instead we'll treat the "*" on the filename as the modified flag.
            DockContainerItem doc = (DockContainerItem)(((PanelDockContainer)editor.Tag).Tag);
            return doc.Text.EndsWith("*");
        }
        private void SetModifiedFlag(CodeTextEditor editor, bool flag)
        {
            try
            {
                if (IsModified(editor) != flag)
                {
                    DockContainerItem doc = (DockContainerItem)(((PanelDockContainer)editor.Tag).Tag);
                    if (IsModified(editor))
                        doc.Text = doc.Text.Substring(0, doc.Text.Length - 1);
                    else
                        doc.Text += "*";
                }
            }
            catch
            {

            }
        }
  
        int coedit = 0;
        EditorAdapter ad;
        CodeResolverManager man;
        FoldingManager foldingManager;
        object foldingStrategy;
        void UpdateFoldings()
        {
            try
            {
                if (foldingStrategy is BraceFoldingStrategy)
                {
                    ((BraceFoldingStrategy)foldingStrategy).UpdateFoldings(foldingManager, ActiveEditor.Document);
                }
            }
            catch
            {

            }
     
        }
        private bool DoSave(CodeTextEditor editor)
        {

            if (!string.IsNullOrEmpty(editor.FileName))
            {                try
                {
                    if (CSol != null && IsCodeEditorProject && SelectedProject != null)
                        editor.BreakpointManager.Save(Path.GetDirectoryName(SelectedProject.FileName) + @"\data", Path.GetFileName(editor.FileName));
                        
                    editor.Save(editor.FileName);
                    SetModifiedFlag(editor, false);
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
            return false;
        }
        void LoadProjectBreakPoints(ALProject proj)
        {
            try
            {
                foreach (string bfi in Directory.GetFiles(Path.GetDirectoryName(proj.FileName) + @"\data", "*.breaks"))
                {
                    if (proj.SourceFiles.ContainsKey(Path.GetFileNameWithoutExtension(bfi)))
                    {
                        SourceFile sg = proj.SourceFiles[Path.GetFileNameWithoutExtension(bfi)];
                        foreach (string line in File.ReadAllLines(bfi))
                        {
                            string[] x = line.Split('=');
                            if (x.Length == 2)
                            {
                                BreakPointState st = (BreakPointState)byte.Parse(x[1]);
                             string file = proj.TempService.GetTempFileNameWithoutIndex(sg.SourcePath);
                             if (st == BreakPointState.Normal)
                                 aldebug.AddBreakPoint(proj.CodeDomProvider.TempDirectory + @"\" + file, int.Parse(x[0]));
                             
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        private CodeTextEditor AddNewTextEditor(string title, string file,ALProject pproj)
        {
            man = new CodeResolverManager();
            CodeTextEditor editor = new CodeTextEditor();
            editor.ClassBrowser = new QuickClassBrowser();
            editor.BreakpointManager.FileName = file;
             editor.Completion = completion;
            editor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editor.IsReadOnly = false;
            editor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(editor.Options);
            foldingStrategy = new BraceFoldingStrategy();
            PanelDockContainer pdc = new PanelDockContainer();
            editor.FontSize = 12;
            editor.OpenFile(file);
            man.Project = completion.projectContent;
            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("AL");
          editor.ColorUpdater = new UpdateCodeColor(editor.SyntaxHighlighting.MainRuleSet);
          // Breakpoints events  
            editor.BreakpointManager.OnBreakpointAdded +=
              new BreakpointEventHandler((e,bfilename) =>
              {
                  try
                  {
                      aldebug.AddBreakPoint( SelectedProject.CodeDomProvider.TempDirectory + @"\" + pproj.TempService.GetTempFileNameWithoutIndex(bfilename), e.Line);
                      SetModifiedFlag(editor, true);
                  }
                  catch
                  {
                  }
              });
            editor.BreakpointManager.OnBreakpointModified +=
     new BreakpointEventHandler((e,bfilename) =>
     {
         try
         {
             SetModifiedFlag(editor, true);
                        if (e.State == ICSharpCode.AvalonEdit.BreakPointState.Normal)
                 aldebug.SetBreakPointState(SelectedProject.CodeDomProvider.TempDirectory + @"\" + pproj.TempService.GetTempFileNameWithoutIndex(bfilename), e.Line, true);
             else if (e.State == ICSharpCode.AvalonEdit.BreakPointState.Disabled)
                 aldebug.SetBreakPointState(SelectedProject.CodeDomProvider.TempDirectory + @"\" + pproj.TempService.GetTempFileNameWithoutIndex(bfilename), e.Line, false);

         }
         catch
         {

         }
     });
            editor.BreakpointManager.OnBreakpointRemoved +=
     new BreakpointEventHandler((e,bfilename) =>
     {
         try
         {
             SetModifiedFlag(editor, true);
             aldebug.RemoveBreakpoint(SelectedProject.CodeDomProvider.TempDirectory + @"\" + pproj.TempService.GetTempFileNameWithoutIndex(bfilename), e.Line);
         }
         catch
         {

         }

     });
            int lastln = 0;
            editor.TextArea.Caret.PositionChanged +=
                  new EventHandler((sender, e) =>
                  {
                      labelItem1.Text = "    Ln " + editor.TextArea.Caret.Line.ToString() + "   ";
                      labelItem11.Text = "   Col " + editor.TextArea.Caret.Column.ToString() + "   ";
                  
                      if(lastln != editor.TextArea.Caret.Line)
                          editor.ClassBrowser.UpdatePosition();
                      lastln = editor.TextArea.Caret.Line;
                  });
            editor.MouseHover +=
                    new System.Windows.Input.MouseEventHandler((sender, e) =>
                    {
                        var pos = editor.TextArea.TextView.GetPositionFloor(e.GetPosition(editor.TextArea.TextView) + editor.TextArea.TextView.ScrollOffset);
                        if (pos != null)
                        {

                            var LogicalPosition = pos.Value.Location;
                            if (!IsDebugging)
                            {
                                man.Project = completion.projectContent;
                                man.CodeDocumentationToolTip(LogicalPosition);
                                if (man.Doc != null)
                                {

                                    ToolTipPopup d = new ToolTipPopup(man.Doc);
                                    PopupControl.Popup p = new PopupControl.Popup(d);
                                    p.Resizable = true;
                                    System.Drawing.Point pxa = Control.MousePosition;
                                    p.Show(pxa.X, pxa.Y);
                                }
                            }
                            else if (aldebug.ExecutionProcess.IsPaused)
                            {
                                string vari = man.DebugVariableToolTip(editor, LogicalPosition, file);
                                System.Drawing.Point pxa = Control.MousePosition;
                                if (vari != null)
                                    aldebug.ShowVariable(vari, pxa);
                            }
                        }
                    });
            editor.TextChanged += new EventHandler((sender, e) =>
            {


                SetModifiedFlag(editor, true);
                try
                {
                    // TODO CVP
             if (cvprecording && editor.FileName == recordinghost)
                 cvpw.AddCode(editor.Text, editor.TextArea.Caret.Line, editor.TextArea.Caret.Column);

                }
                catch
                {

                }

            });
       
            pdc.Enter += new EventHandler((sender, e) =>
            {
                current = editor;
         
            });
            pdc.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            pdc.Location = new System.Drawing.Point(3, 28);
            pdc.Name = "CONTROLID" + bar5.Items.Count.ToString();
            pdc.Size = new System.Drawing.Size(553, 265);
            pdc.Style.Alignment = System.Drawing.StringAlignment.Center;
            pdc.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            pdc.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            pdc.Style.GradientAngle = 90;
            pdc.TabIndex = 2;
            editor.Name = "EDIT" + coedit.ToString();
            coedit++;
            editor.Tag = pdc;

            editor.ClassBrowser.Dock = DockStyle.Top;
            TabHost ho = new TabHost();
            SearchInputHandler hand = new SearchInputHandler(editor.TextArea);
            hand.Attach();

            pdc.Controls.Add(ho);
            pdc.Controls.Add(editor.ClassBrowser);


            DockContainerItem dci = new DockContainerItem();
            ho.AddCodeEditorProject(editor, dci, pdc, pproj);
            dci.Control = pdc;
            dci.Name = "TAB" + bar5.Items.Count.ToString();
            dci.Text = title;
            pdc.Tag = dci;
            dci.Tag = ho;
            bar5.Controls.Add(pdc);
            bar5.Items.Add(dci);
            bar5.SelectedDockContainerItem = dci;
            current = editor;
            ad = new EditorAdapter(current);
            ad.OnLineJump += ad_OnLineJump;
            
            try
            {
                if (foldingUpdateTimer == null)
                {
                    foldingUpdateTimer = new DispatcherTimer();
                    foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
                    foldingUpdateTimer.Tick += delegate { UpdateFoldings(); };
                    foldingUpdateTimer.Start();
                }
                if (foldingStrategy != null)
                {
                    if (foldingManager == null)
                        foldingManager = FoldingManager.Install(editor.TextArea);
                    UpdateFoldings();
                }
                else
                {
                    if (foldingManager != null)
                    {
                        FoldingManager.Uninstall(foldingManager);
                        foldingManager = null;
                    }
                }
            }
            catch
            {

            }
            if (CSol != null)
                editor.BreakpointManager.LoadBreaks(Path.GetDirectoryName(SelectedProject.FileName) + @"\data", Path.GetFileName(editor.FileName));
            return editor;

        }
        private CodeTextEditor AddNewTextEditorFile(string title, string file)
        {
            CodeTextEditor editor = new CodeTextEditor();
            editor.IsReadOnly = false;
            editor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            PanelDockContainer pdc = new PanelDockContainer();
            editor.FontSize = 12;
            editor.OpenFile(file);
            //editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("AL");

            SearchInputHandler hand = new SearchInputHandler(editor.TextArea);
            hand.Attach();

            editor.TextArea.Caret.PositionChanged +=
                  new EventHandler((sender, e) =>
                  {
                      labelItem1.Text = "    Ln " + editor.TextArea.Caret.Line.ToString() + "   ";
                      labelItem11.Text = "   Col " + editor.TextArea.Caret.Column.ToString() + "   ";
                    //  clasb.UpdatePosition();
                  });
        
            editor.TextChanged += new EventHandler((sender, e) =>
            {


                SetModifiedFlag(editor, true);
                try
                {
                    // TODO CVP
                        if (cvprecording && editor.FileName == recordinghost)
                            cvpw.AddCode(editor.Text, editor.TextArea.Caret.Line, editor.TextArea.Caret.Column);
                }
                catch
                {

                }

            });

            pdc.Enter += new EventHandler((sender, e) =>
            {
                current = editor;

            });
            pdc.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            pdc.Location = new System.Drawing.Point(3, 28);
            pdc.Name = "CONTROLID" + bar5.Items.Count.ToString();
            pdc.Size = new System.Drawing.Size(553, 265);
            pdc.Style.Alignment = System.Drawing.StringAlignment.Center;
            pdc.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            pdc.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            pdc.Style.GradientAngle = 90;
            pdc.TabIndex = 2;
            editor.Name = "EDIT" + coedit.ToString();
            coedit++;
            editor.Tag = pdc;

            TabHost ho = new TabHost();
            pdc.Controls.Add(ho);


            DockContainerItem dci = new DockContainerItem();
            dci.Tag = ho;
            dci.Control = pdc;
            ho.AddCodeEditor(editor, dci, pdc);
            dci.Name = "TAB" + bar5.Items.Count.ToString();
            dci.Text = title;
            pdc.Tag = dci;
            bar5.Controls.Add(pdc);
            bar5.Items.Add(dci);
            bar5.SelectedDockContainerItem = dci;
            current = editor;
            ad = new EditorAdapter(current);
            ad.OnLineJump += ad_OnLineJump;
            dci.Tag = ho;
            return editor;

        }
        void ad_OnLineJump(bool ish, int line, System.Windows.Media.Color c)
        {
            if (!ish)
            {
                HighlightDebugLineBackgroundRenderer h = new HighlightDebugLineBackgroundRenderer(current);
                h.Line = line;
                h.color = c;
                current.TextArea.TextView.BackgroundRenderers.Add(h);
                current.JumpTo(line, 0);
            }
        }
        void FindAndReplace()
        {
            try
            {
                FindReplaceControl.ShowForReplace(current);
                ElementHost h = new ElementHost();
                h.Dock = DockStyle.Fill;
                h.Child = FindReplaceControl.theDialog;
                FindAndReplaceFrm f = new FindAndReplaceFrm();
                f.Controls.Add(h);
                f.Show();
            }
            catch
            {

            }
        }
        private void OpenFiles(string[] fns)
        {

            // Open file(s)
            foreach (string fn in fns)
            {


                try
                {
                  
                    if (File.Exists(fn))
                    {
                        if (fn.EndsWith(".cvp"))
                        {
                            // TODO:CVP
                            //CVPReader rdr = new CVPReader(fn);
                            //CVPlay frm = new CVPlay(rdr);
                            //frm.ShowDialog();

                        }
                        else if (true)
                        {
                            // TODO:ALLOWED TO OPEN
                         

                           
                            if (fn.EndsWith(".al"))
                            {
                                var editor = AddNewTextEditorFile(Path.GetFileName(fn), fn);
                                SetModifiedFlag(editor, false);
                                UpdateFoldings();
                            }

                            else if (fn.EndsWith(".dsol"))
                            {
                                Thread.Sleep(100);
                                LoadSolution(fn);
                            }
                            else if (fn.EndsWith(".alproj"))
                            {
                                // Create Solution And Load
                                string solu = Path.ChangeExtension(fn, ".dsol");
                                if (File.Exists(solu))
                                    LoadSolution(solu);
                                //Create Solution

                            }
                            else
                            {
                                var editor = AddNewTextEditorFile(Path.GetFileName(fn), fn);
                                SetModifiedFlag(editor, false);
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(ex.Message, ex.GetType().Name);

                    return;
                }

            }
        }

        DockContainerItem FindTabByText(string text)
        {
            foreach (BaseItem it in bar5.Items)
            {
                if (it.Text == text)
                    return (DockContainerItem)it;
                else if (it.Text.Replace("*", "").Replace(" ", "") == text.Replace(" ", ""))
                    return (DockContainerItem)it;
            }
            return null;
        }
        DockContainerItem FindTabFileName(string filename)
        {
            foreach (DockContainerItem it in bar5.Items)
            {
                if (it.Tag != null)
                {
                    TabHost h = (TabHost)it.Tag;
                    if (h.FileName == filename)
                        return it;
                }

            }
            return null;
        }
     
#endregion 

        #region Project Solution Management
        internal DSolution CSol;
        internal   string SolVersion = "6.0";
         internal  string solfile;
         public ProjectProperties OpenProjectProperties(ALProject proj)
         {
             try
             {
                 ProjectProperties props = new ProjectProperties();
                 props.OpenInfo += props_OpenInfo;
                 PanelDockContainer pdc = new PanelDockContainer();
                 pdc.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
                 pdc.Location = new System.Drawing.Point(3, 28);
                 pdc.Name = "CONTROLID" + bar5.Items.Count.ToString();
                 pdc.Size = new System.Drawing.Size(553, 265);
                 pdc.Style.Alignment = System.Drawing.StringAlignment.Center;
                 pdc.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
                 pdc.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
                 pdc.Style.GradientAngle = 90;
                 pdc.TabIndex = 2;
                 TabHost ho = new TabHost();
                 pdc.Controls.Add(ho);

                 DockContainerItem dci = new DockContainerItem();
                 dci.Control = pdc;
                 ho.AddProperty(props, dci, pdc, proj);
                 dci.Name = "TAB" + bar5.Items.Count.ToString();
                 pdc.Tag = dci;
                 dci.Text = proj.Name;
                 bar5.Controls.Add(pdc);
                 bar5.Items.Add(dci);
                 bar5.SelectedDockContainerItem = dci;
                 dci.Tag = ho;
                 props.ressourceExplorerControl1.OnRessourceModified += ressourceExplorerControl1_OnRessourceModified;
                 return props;
             }
             catch
             {

             }
             return null;
         }
         Node GetProjectNode(ALProject proj)
         {
             foreach (Node nd in advTree1.Nodes[0].Nodes)
             {
                 if (nd.Text == proj.Name)
                     return nd;
             }
             return null;
         }
        void ForceCloseAllProjectTabs()
      {
          try
          {
              foreach (DSProjectItem it in CSol.Projects)
              {
                  foreach (KeyValuePair<string, SourceFile> d in it.Project.SourceFiles)
                  {
                      if (this.FindTabFileName(d.Value.SourcePath) != null)
                      {
                          DockContainerItem tab = FindTabFileName(d.Value.SourcePath);
                          bar5.Controls.Remove((PanelDockContainer)tab.Control);
                          bar5.Items.Remove(tab);
                      }
                  }
                  if (this.FindTabFileName(it.Project.FileName) != null)
                  {
                      DockContainerItem tab = FindTabFileName(it.Project.FileName);
                      bar5.Controls.Remove((PanelDockContainer)tab.Control);
                      bar5.Items.Remove(tab);
                  }
              }
          }
          catch
          {

          }

      }
        void UnloadSolution()
        {
            try
            {

                if (CSol != null)
                {
                    ForceCloseAllProjectTabs();
                    this.Text = "Arsslensoft Developer Studio 2015";
                    if(advTree1.Nodes.Count > 0)
                       advTree1.Nodes.Remove(advTree1.Nodes[0]);
                    if (!projectworker.IsBusy)
                    {
                        projact = ProjectAction.OpenNew;
                        progbar.Visible = true;
                        projectworker.RunWorkerAsync();
                    }
                    //CSol = null;
                    File.WriteAllText(Application.StartupPath + @"\Session_Watch.dat", "");
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        internal void UnloadSolutionNew()
        {
            try
            {

                if (CSol != null)
                {
                    ForceCloseAllProjectTabs();
                    this.Text = "Arsslensoft Developer Studio 2015";
                    if (advTree1.Nodes.Count > 0)
                        advTree1.Nodes.Remove(advTree1.Nodes[0]);
                    if (!projectworker.IsBusy)
                    {
                        projact = ProjectAction.Open;
                        progbar.Visible = true;
                        projectworker.RunWorkerAsync();
                    }
                    //CSol = null;
                    File.WriteAllText(Application.StartupPath + @"\Session_Watch.dat", "");
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        internal void OpenSolution()
        {
            if (!projectworker.IsBusy)
            {
                projact = ProjectAction.Open;
                progbar.Visible = true;
                projectworker.RunWorkerAsync();
            }
        }
        internal void LoadSolution(string file)
        {
            try
            {
                aldebug = new ArsslenLanguageDebugger();
                if (File.Exists(file) && file.EndsWith(".dsol"))
                {
                    if (CSol == null)
                    {
                        CSol = new DSolution(file, SolVersion);
                       
                        File.WriteAllText(Application.StartupPath + @"\Session_Watch.dat", file);
                        if (!projectworker.IsBusy)
                        {
                            projact = ProjectAction.Open;
                            progbar.Visible = true;
                            projectworker.RunWorkerAsync();
                        }

                    }
                    else
                    {
                        UnloadSolution();
                        solfile = file;
                        File.WriteAllText(Application.StartupPath + @"\Session_Watch.dat", file);
            
                  

                    }
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        internal void LoadSolutionControl()
        {
            try
            {
                if (advTree1.Nodes.Count > 0)
                    advTree1.Nodes.Clear();
                this.Text = Path.GetFileNameWithoutExtension(CSol.FileName) + " - Arsslensoft Developer Studio 2015";
                // Add Projects
                CSol.SortByBuildOrder();
                Node soln = new Node();
                soln.Image = devstd.Properties.Resources.dsol;
                soln.Text = CSol.Name;
                soln.Name = "SOL" + CSol.Name;
                soln.Expanded = true;
                soln.Tag = CSol;
                foreach (DSProjectItem proj in CSol.Projects)
                    LoadProjectControl(proj.Project, soln, proj.Startup);

                
                advTree1.Nodes.Add(soln);
                solexplr.Selected = true;
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        internal void LoadProjectControl(ALProject proj, Node solutionnode, bool start)
        {
            Node projnode = new Node();
            projnode.Image = devstd.Properties.Resources.alproj;
            projnode.Text = proj.Name;
            projnode.Name = "PROJ" + proj.Name;
            projnode.Expanded = true;
            projnode.Tag = proj;
            if (start)
                projnode.Style = elementStyle5;
    Node src = new Node();
            src.Image = devstd.Properties.Resources.folder_26;
            src.Text = "src";
            src.Name = "FOLDERSRC";
            src.Tag = proj;
            Node res = new Node();
            res.Image = devstd.Properties.Resources.folder_26;
            res.Text = "res";
            res.Name = "FOLDERRES";
            res.Tag = proj;
            Node frm = new Node();
            frm.Image = devstd.Properties.Resources.folder_26;
            frm.Text = "forms";
            frm.Name = "FOLDERFRM";
            frm.Tag = proj;
            Node eventfrm = new Node();
            eventfrm.Image = devstd.Properties.Resources.folder_26;
            eventfrm.Text = "formevent";
            eventfrm.Name = "FOLDEREVENTFRM";
            eventfrm.Tag = proj;
            Node asminfo = new Node();
            asminfo.Image = devstd.Properties.Resources.folder_26;
            asminfo.Text = "asminfo";
            asminfo.Name = "FOLDERASMINFO";
            asminfo.Tag = proj;
            Node refer = new Node();
            refer.Image = devstd.Properties.Resources.folder_26;
            refer.Text = "references";
            refer.Name = "FOLDERREFS";
            refer.Tag = proj;

            projnode.Nodes.Add(refer);
            projnode.Nodes.Add(asminfo);
            projnode.Nodes.Add(res);
            projnode.Nodes.Add(src);
            projnode.Nodes.Add(frm);
            projnode.Nodes.Add(eventfrm);

            foreach (KeyValuePair<string, alproj.SourceFile> p in proj.SourceFiles)
            {
                try
                {
                    if (p.Value.SourceType == SourceFileType.Source)
                    {
                        Node nd = new Node();
                        nd.Name = p.Value.Name;
                        nd.Text = p.Value.Name;
                        nd.Tag = p.Value;
                        nd.Image = devstd.Properties.Resources.AL;
                        src.Nodes.Add(nd);
                        //intel.UpdateOrAdd(p.Value.SourcePath);

                    }
                    else if (p.Value.SourceType == SourceFileType.Form)
                    {
                        Node nd = new Node();
                        nd.Name = p.Value.Name;
                        nd.Text = p.Value.Name;
                        nd.Tag = p.Value;
                        nd.Image = devstd.Properties.Resources.formd;
                        frm.Nodes.Add(nd);
                    }
                    else if (p.Value.SourceType == SourceFileType.Event)
                    {
                        Node nd = new Node();
                        nd.Name = p.Value.Name;
                        nd.Text = p.Value.Name;
                        nd.Tag = p.Value;
                        nd.Image = devstd.Properties.Resources.AL;
                        eventfrm.Nodes.Add(nd);
                    }
                    else if (p.Value.SourceType == SourceFileType.AsmInfo)
                    {
                        Node nd = new Node();
                        nd.Name = p.Value.Name;
                        nd.Text = p.Value.Name;
                        nd.Tag = p.Value;
                        nd.Image = devstd.Properties.Resources.AL;
                        asminfo.Nodes.Add(nd);
                    }
                }
                catch (Exception ex)
                {
                   //ELog.LogEx(ex);
                }
            }
            foreach (KeyValuePair<string, ReferencedAssembly> p in proj.Asm)
            {
                try
                {
                    Node nd = new Node();
                    nd.Name = p.Value.Name;
                    nd.Text = p.Value.Name;
                    nd.Tag = p.Value;

                    //intel.UpdateAID(p.Value.SourcePath);
                    if (File.Exists(p.Value.SourcePath))
                    {
                        Assembly a = Assembly.LoadFile(p.Value.SourcePath);
                        if (proj.Properties.Target == "winexe")
                            formDesignerControl1.AddRef(a);
                        nd.Tooltip = "Runtime Version : " + a.ImageRuntimeVersion;
                        nd.Image = devstd.Properties.Resources.library;
                    }
                    else
                        nd.Image = devstd.Properties.Resources.Unpinned;

                    refer.Nodes.Add(nd);
                }
                catch
                {
                    Node nd = new Node();
                    nd.Name = p.Value.Name;
                    nd.Text = p.Value.Name;
                    nd.Tag = p.Value;
                    nd.Image = devstd.Properties.Resources.library;
                    refer.Nodes.Add(nd);
                }

            }
            foreach (KeyValuePair<string, alproj.RessourceItem> p in proj.Ressources)
            {
                try
                {
                    Node nd = new Node();
                    nd.Name = p.Value.Name;
                    nd.Text = p.Value.Name;
                    nd.Tag = p.Value;
                    if (p.Value.Type == RessourceType.Icon || p.Value.Type == RessourceType.Image)
                        nd.Image = devstd.Properties.Resources.image;
                    else if (p.Value.Type == RessourceType.Integer)
                        nd.Image = devstd.Properties.Resources.integer;
                    else if (p.Value.Type == RessourceType.Boolean)
                        nd.Image = devstd.Properties.Resources.tick;
                    else if (p.Value.Type == RessourceType.Real)
                        nd.Image = devstd.Properties.Resources._float;
                    else if (p.Value.Type == RessourceType.String)
                        nd.Image = devstd.Properties.Resources.text;
                    else
                        nd.Image = devstd.Properties.Resources.data;

                    res.Nodes.Add(nd);
                }
                catch
                {

                }
            }

            solutionnode.Nodes.Add(projnode);
            solexplr.Focus();
        }
        void AddProjectProperties(ALProject proj)
        {
            try
            {
                if (FindTabFileName(proj.FileName) != null)
                {
                    bar5.SelectedDockContainerItem = FindTabFileName(proj.FileName);
                    TabHost tb = (TabHost)bar5.SelectedDockContainerItem.Tag;
                    tb.ProjectPropertiesEditor.SelectNormal();
                }
                else
                    OpenProjectProperties(proj).SelectNormal();
                
              
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void AddRessourceExplorer(ALProject proj)
        {
            try
            {

                if (FindTabFileName(proj.FileName) != null)
                {
                    bar5.SelectedDockContainerItem = FindTabFileName(proj.FileName);
                    TabHost tb = (TabHost)bar5.SelectedDockContainerItem.Tag;
                    tb.ProjectPropertiesEditor.SelectRessource();
                }
                else
                    OpenProjectProperties(proj).SelectRessource();
                
              
            }
            catch(Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void AddReference(ALProject proj, Node projnode)
        {
            try
            {
                ReferenceAdder refadd = new ReferenceAdder();
                refadd.ShowDialog();
                foreach (Assembly asm in refadd.ToAdd)
                {
                    ReferencedAssembly ra = new ReferencedAssembly();
                    Node nd = new Node();
                    nd.Name = Path.GetFileNameWithoutExtension(asm.ManifestModule.Name);
                    nd.Text = Path.GetFileNameWithoutExtension(asm.ManifestModule.Name);
                    nd.Tag = new ReferencedAssembly(Path.GetFileNameWithoutExtension(asm.ManifestModule.Name), asm.Location, refadd.CopyAsm);

                    nd.Tooltip = "Runtime Version : " + asm.ImageRuntimeVersion;
                    if (!proj.Asm.ContainsKey(Path.GetFileNameWithoutExtension(asm.ManifestModule.Name)))
                    {
                        nd.Image = devstd.Properties.Resources.folder_26;
                        proj.Asm.Add(Path.GetFileNameWithoutExtension(asm.ManifestModule.Name), new ReferencedAssembly(Path.GetFileNameWithoutExtension(asm.ManifestModule.Name), asm.Location, refadd.checkBoxX1.Checked));
                    }
                    else
                        nd.Image = devstd.Properties.Resources.Unpinned;

                    projnode.Nodes[0].Nodes.Add(nd);
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void LoadForm( ALProject proj, string designerfile)
        {
            try
            {
                if (FindTabFileName(designerfile) == null)
                {
                    string name = Path.GetFileNameWithoutExtension(designerfile).Split('.')[0];
                    string classfile = Path.GetDirectoryName(proj.FileName) + @"\src\" + name + ".al";

                    File.WriteAllText(Path.GetDirectoryName(proj.FileName) + @"\temp\" + name + ".cs", proj.alang.InterpretAlToCs(File.ReadAllText(classfile)));
                    File.WriteAllText(Path.GetDirectoryName(proj.FileName) + @"\temp\" + name + ".Designer.cs", proj.alang.InterpretAlToCs(File.ReadAllText(designerfile)));
                    formDesignerControl1.Project = proj;
                    formDesignerControl1.Open(Path.GetDirectoryName(proj.FileName) + @"\temp\" + name + ".cs");
                }
                else
                    bar5.SelectedDockContainerItem = FindTabFileName(designerfile);
                //MessageBoxEx.Show("Some controls may cause Developer Studio 2015 crash. " + Environment.NewLine + "If you encounter a crash you have to insert the control manually (by code)." + Environment.NewLine + "You can add control code to InitializeControlSafely", "Form Designer Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {

            }
        }
        void AddComponent(ALProject proj)
        {
            try
            {
                if (proj != null)
                {
                    AddFrm frm = new AddFrm();
                    frm.ShowDialog();
                    if (frm.IsAddControl)
                    {
                        if (frm.ElementType == "Class")
                            proj.AddClass(frm.ClassName);

                        else if (frm.ElementType == "Form")
                            proj.AddForm(frm.ClassName);

                        else if (frm.ElementType == "Control")
                            proj.AddControl(frm.ClassName);
                        else
                            proj.AddUserControl(frm.ClassName);
                    }
                    advTree1.Nodes.Clear();
                    LoadSolutionControl();
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        private void advTree1_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            try
            {
                Node nd = e.Node;
                if (!nd.Name.StartsWith("FOLDER") && !nd.Name.StartsWith("PROJ") && !nd.Name.StartsWith("SOL"))
                {
                    Node pa = nd.Parent;

                    if (pa.Name == "FOLDERSRC" || pa.Name == "FOLDEREVENTFRM" || pa.Name == "FOLDERASMINFO")
                    {
                      
                        SourceFile file = (SourceFile)nd.Tag;
                        if (FindTabByText(file.Name) != null)
                            bar5.SelectedDockContainerItem = FindTabByText(file.Name);
                        else
                        {
                             ALProject proj = (ALProject)pa.Tag;
                             AddNewTextEditor(file.Name, file.SourcePath, proj);
                        }

                    }
                    else if (pa.Name == "FOLDERFRM")
                    {
                        ALProject proj = (ALProject)pa.Tag;
                        SourceFile file = (SourceFile)nd.Tag;
                        LoadForm(proj,file.SourcePath);
                    }
                    else if (pa.Name == "FOLDERRES")
                    {
                        ALProject proj = (ALProject)pa.Tag;
                        
                        AddRessourceExplorer(proj);

                        ActiveProperties.OpenRessource(((RessourceItem)nd.Tag).Type);

                    }
                }
                else
                {

                    if (nd.Name == "FOLDERRES")
                    {
                        ALProject sproj = (ALProject)nd.Tag;
                        AddRessourceExplorer(sproj);
                    }
                    else if (nd.Name == "FOLDERREFS")
                    {
                        ALProject sproj = (ALProject)nd.Tag;
                        AddReference(sproj, nd.Parent);
                    }
                    else if (nd.Name == "FOLDERSRC" || nd.Name == "FOLDEREVENTFRM" || nd.Name == "FOLDERFRM")
                    {
                        ALProject sproj = (ALProject)nd.Tag;
                        AddComponent(sproj);
                    }
                    else if (nd.Name.StartsWith("PROJ"))
                    {
                          ALProject sproj = (ALProject)nd.Tag;
                          AddProjectProperties(sproj);
                    }

                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        bool DesignerEvent_ShowCodeEvent(IComponent component, EventDescriptor e, string methodName)
        {
            try
            {
                if (IsFormDesigner)
                {
                    DockContainerItem tab = FindTabFileName(SelectedProject.CodeDomProvider.ProjectDirectory + @"\formevent\"+ Path.GetFileNameWithoutExtension( formDesignerControl1.FileName) + ".Events.al");
                    formDesignerControl1.Save();
                    if (SelectedProject != null)
                        File.WriteAllText(Path.GetDirectoryName(SelectedProject.FileName) + @"\forms\" + Path.GetFileNameWithoutExtension(formDesignerControl1.CodeBehindFileName) + ".al", SelectedProject.alang.InterpretCsToAl(File.ReadAllText(formDesignerControl1.CodeBehindFileName)));
                    
                    MethodInfo[] mInfos = e.EventType.GetMethods(BindingFlags.Public);

                    MemberInfo[] mi = e.EventType.GetMember("Invoke");
                    MethodBase methodBase = ((MethodInfo)mi[0]);
                    string parameters = "";
                    foreach (ParameterInfo pi in methodBase.GetParameters())
                        parameters += ", " + pi.ToString();
                    if (parameters.Length > 0)
                        parameters = parameters.Remove(0, 1);
                    string subheader = "sub " + methodName + "(" + parameters + ")";
                    if (tab != null)
                    {
                        bar5.SelectedDockContainerItem = tab;
                        Thread.Sleep(20);
                        // Check if the sub was defined
                        if (ActiveEditor.Text.IndexOf(subheader) != -1)
                        {
                            // Goto
                            ActiveEditor.TextArea.Caret.Offset = ActiveEditor.Text.IndexOf(subheader);
                        }
                        else
                        {
                            // Add Event Sub
                            int off = ActiveEditor.Text.IndexOf("public sub InitializeEvents");
                            if (off > 0)
                            {
                                ActiveEditor.Text = ActiveEditor.Text.Insert(off, Environment.NewLine + "sub " + methodName + "(" + parameters + ")" + Environment.NewLine + "{" + Environment.NewLine + Environment.NewLine + "}" + Environment.NewLine);
                                ActiveEditor.TextArea.Caret.Offset = ActiveEditor.Text.IndexOf(subheader);
                            }
                            else Log.ShowAlert("InitializeEvents not found please respect the spacing order \npublic sub InitializeEvents", "Event Creator");
                        }
                    }
                    else
                    {
                        // open
                        AddNewTextEditor(Path.GetFileNameWithoutExtension(formDesignerControl1.FileName) + ".Events.al", SelectedProject.CodeDomProvider.ProjectDirectory + @"\formevent\" + Path.GetFileNameWithoutExtension(formDesignerControl1.FileName) + ".Events.al", SelectedProject);
                        Thread.Sleep(20);
                            // Check if the sub was defined
                        if (ActiveEditor.Text.IndexOf(subheader) != -1)
                        {
                            // Goto
                            ActiveEditor.TextArea.Caret.Offset = ActiveEditor.Text.IndexOf(subheader);
                        }
                        else
                        {
                            // Add Event Sub
                            int off = ActiveEditor.Text.IndexOf("public sub InitializeEvents");
                            // INSERT
                            if (off > 0)
                            {
                                ActiveEditor.Text = ActiveEditor.Text.Insert(off, Environment.NewLine + "sub " + methodName + "(" + parameters + ")" + Environment.NewLine + "{" + Environment.NewLine + Environment.NewLine + "}" + Environment.NewLine);
                                ActiveEditor.TextArea.Caret.Offset = ActiveEditor.Text.IndexOf(subheader);
                            }
                            else Log.ShowAlert("InitializeEvents not found please respect the spacing order \npublic sub InitializeEvents", "Event Creator");
                        }
                    }
                }
            }
            catch
            {

            }
            return false;
        }
#endregion

        #region Compiler Errors Management
        delegate void AppendProgressDelegate(ProgressBarItem txt, int value);
        void UpdateProgress(ProgressBarItem txt, int value)
        {
            try
            {
                AppendProgressDelegate d = new AppendProgressDelegate(UP);
                logbox.BeginInvoke(d, new object[] { txt, value });


            }
            catch
            {

            }
        }
        void UP(ProgressBarItem txt, int  t)
        {
            try
            {
                if (t == 100 || t == 0)
                    progbar.Visible = false;
                else
                    progbar.Visible = true;

                progbar.Value = t;
            }
            catch
            {

            }
        }

        delegate void AppendTextDelegate(TextBoxX txt, string text);
        void AppendLog(TextBoxX txt, string t)
        {
            try
            {
                AppendTextDelegate d = new AppendTextDelegate(AL);
                logbox.BeginInvoke(d, new object[] { txt, t });


            }
            catch
            {

            }
        }
        void AL(TextBoxX txt, string t)
        {
            try
            {
                txt.Text += Environment.NewLine + t;
                slb.Text = t;
            }
            catch
            {

            }
        }
        private void addMessageToList(CompilersLibraryAPI.CompileMessage message)
        {
            string msg = message.Message;
            if (CSol != null)
            {
                msg = message.Code.Replace("CS", "AL") + ": " + message.Message;

                ListViewItem item = messagesListView.Items.Add(new ListViewItem(msg));
                message.FileName = CSol.GetProjectByName(message.Project).TempService.GetFileFromTemp(message.FileName);
                item.SubItems.Add(Path.GetFileName(message.FileName));
                item.SubItems.Add(message.LineNumber.ToString());
                item.SubItems.Add(message.CharNumber.ToString());
                item.SubItems.Add(message.Project);
                item.Tag = message;

                switch (message.Type)
                {
                    case CompilersLibraryAPI.CompileMessage.MessageTypes.Error:
                        item.ForeColor = System.Drawing.Color.Red;
                        item.ImageKey = "error";
                        item.Selected = true;
                        messagesListView_ItemActivate(this, new EventArgs());
                        break;
                    case CompilersLibraryAPI.CompileMessage.MessageTypes.Note:
                    case CompilersLibraryAPI.CompileMessage.MessageTypes.Info:
                        item.ImageKey = "info";
                        break;
                    case CompilersLibraryAPI.CompileMessage.MessageTypes.Warning:
                        item.ImageKey = "warning";
                        break;
                }
            }
        }
        private delegate void addMessageDelegate(CompilersLibraryAPI.CompileMessage message);
        void AddMessage(CompilersLibraryAPI.CompileMessage e)
        {
            try
            {

                if (messagesListView.InvokeRequired)
                {
                    addMessageDelegate d = new addMessageDelegate(addMessageToList);
                    messagesListView.Invoke(d, e);
                }
                else
                {
                    addMessageToList(e);
                }

            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
   
        private void messagesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            
                if (messagesListView.SelectedItems.Count >  0 && CSol != null)
                {
                 
                   
                    ListViewItem item = messagesListView.SelectedItems[0];
                    CompilersLibraryAPI.CompileMessage m = (CompilersLibraryAPI.CompileMessage)item.Tag;
                    ALProject proj = CSol.GetProjectByName(m.Project);
                    
                    if (ActiveEditor != null)
                    {
                        if (m.FileName != null)
                        {
                            if (File.Exists(m.FileName))
                            {
                                if (FindTabByText(Path.GetFileName(m.FileName)) != null)
                                {
                                    bar5.SelectedDockContainerItem = FindTabByText(Path.GetFileName(m.FileName));
                                    ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                                 

                                }
                            }
                            else
                            {
                                ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                            }
                        }
                        else
                        {
                            ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                        }
                    }
                    else
                    {
                        if (m.FileName != null)
                        {
                            if (File.Exists(m.FileName))
                            {
                                AddNewTextEditor(Path.GetFileName(m.FileName), m.FileName, proj);
                                ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                            }
                            else
                            {
                                ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                            }
                        }
                        else
                        {
                            ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                        }
                    }



                }


            }
            catch
            {

            }
        }
        private void messagesListView_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (messagesListView.SelectedItems.Count > 0 && CSol != null)
                {

                    ListViewItem item = messagesListView.SelectedItems[0];
                    CompilersLibraryAPI.CompileMessage m = (CompilersLibraryAPI.CompileMessage)item.Tag;
                    ALProject proj = CSol.GetProjectByName(m.Project);
                    if (ActiveEditor != null)
                    {
                        if (m.FileName != null)
                        {
                            if (File.Exists(m.FileName))
                            {
                                if (FindTabByText(Path.GetFileName(m.FileName)) != null)
                                {
                                    bar5.SelectedDockContainerItem = FindTabByText(Path.GetFileName(m.FileName));
                                    ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);

                                }
                            }
                            else
                            {
                                ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                            }
                        }
                        else
                        {
                            ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                        }
                    }
                    else
                    {
                        if (m.FileName != null)
                        {
                            if (File.Exists(m.FileName))
                            {
                                AddNewTextEditor(Path.GetFileName(m.FileName), m.FileName, proj);
                                ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                            }
                            else
                            {
                                ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                            }
                        }
                        else
                        {
                            ActiveEditor.JumpTo(m.LineNumber, m.CharNumber);
                        }
                    }



                }

            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
   


        #endregion

        #region Parser Management
        bool checkedf = false;
        private void parsetimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Class Browser Parser
                if (ActiveEditor != null)
                {
                    ActiveEditor.ClassBrowser.LoadSource(ActiveEditor);
                    if(QuickClassBrowser.LatestCompletionSyntaxTree != null)
                       ActiveEditor.ColorUpdater.UpdateCurrentFile(QuickClassBrowser.LatestCompletionSyntaxTree);
                }
            }
            catch(Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        #endregion

        #region Debugger & Output
        ArsslenLanguageDebugger aldebug = new ArsslenLanguageDebugger();
        ALProject SelectedProject
        {
            get
            {
                if (ActiveHost != null)
                    return ActiveHost.Project;

                return null;
            }
        }
       
        bool IsDebugging = false;
        public void Build()
        {
            try
            {
                if (CSol != null)
                {
                    // Build
                    if (!buildworker.IsBusy)
                    {
                        isbuild = true;
                        progbar.Visible = true;
                        buildworker.RunWorkerAsync();
                    }
                }

       
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void Debug()
        {
            try
            {

                if (CSol != null)
                {
                    ALProject startproj = CSol.GetStartupProject();
                    debugproj = startproj;

               //  ReBuild();
                 
                    aldebug.DeleteAllBreakPoints();
                    LoadProjectBreakPoints(startproj);
                    aldebug.OnBreakPointHit += aldebug_OnBreakPointHit;
                    aldebug.OnPaused += aldebug_OnPaused;
             
                    // TODO:Add Debug Options int settings
                    if (startproj.Properties.Target == "winexe" || startproj.Properties.Target == "exe")
                    {
                        aldebug.SetOptionsToDefault();
                  
                        //foreach (ICSharpCode.AvalonEdit.BreakPoint d in ActiveEditor.BreakpointManager.BreakPoints)
                        //{
                        //    // Add TEMP PATH for file

                        //  //  aldebug.SetBreakPointState(ActiveEditor.FileName, d.Line, d.);
                        //}
                        aldebug.Start(startproj.CodeDomProvider.OutputDirectory + @"\" + startproj.Properties.Output);
                        aldebug.ExecutionProcess.LogMessage += ExecutionProcess_LogMessage;
                        aldebug.ExecutionProcess.ModuleLoaded += ExecutionProcess_ModuleLoaded;
                        aldebug.ExecutionProcess.ModuleUnloaded += ExecutionProcess_ModuleUnLoaded;
                        aldebug.ExecutionProcess.Resumed += ExecutionProcess_Resumed;
                        aldebug.ExecutionProcess.Exited += ExecutionProcess_Exited;
                        breakControl1.Clear();
                      breakControl1.OnGetTemp += callStack1_OnGetTemp;
                      breakControl1.OnStackSelected += callStack1_OnStackSelected;
                        foreach (Breakpoint bp in aldebug.BP)
                            breakControl1.AddBreak(bp);

                        IsDebugging = true;
                    }
                    else
                        MessageBoxEx.Show("Developer Studio supports only executable for debugging", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");
                DebugControls(IsDebugging);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void Run()
        {
            try
            {

                if (CSol != null)
                {
                    ALProject startproj = CSol.GetStartupProject();
                    // Build
                    if (!buildworker.IsBusy)
                    {
                        isbuild = true;
                        progbar.Visible = true;
                        buildworker.RunWorkerAsync();

                    }
                    // TODO:Add Debug Options int settings
                    if (startproj.Properties.Target == "winexe" || startproj.Properties.Target == "exe")
                    {
                        aldebug.StartWithoutDebug(startproj.CodeDomProvider.OutputDirectory + @"\" + startproj.Properties.Output);

                        IsDebugging = false;
                    }
                    else
                        MessageBoxEx.Show("Developer Studio supports only executable for debugging", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");
  
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void ReBuild()
        {
            try
            {
                if (CSol != null)
                {
                    // Build

                    if (!buildworker.IsBusy)
                    {
                        isbuild = false;
                        progbar.Visible = true;
                        buildworker.RunWorkerAsync();
                    }
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");
             
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void Clean()
        {
            try
            {
                if (CSol != null)
                    // Clean
                    CSol.Clean();
                else
                    Log.ShowInfo("No solution found", "Project Manager");

            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
      //TEST:JUST
        public void BuildProject()
        {
            try
            {
                if (CSol != null)
                {
                    // Build
                    if (SelectedProject != null)
                        SelectedProject.Build();
                    else
                    {
                        ALProject startproj = CSol.GetStartupProject();
                        startproj.Build();
                    }
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");
                CheckErrorMessages();

            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void DebugProject()
        {
            try
            {
                if (CSol != null)
                {
              
                        // Build
                    if (SelectedProject != null)
                    {

                        aldebug.DeleteAllBreakPoints();
                        LoadProjectBreakPoints(SelectedProject);
                        // TODO:Add Debug Options int settings
                        if (SelectedProject.Properties.Target == "winexe" || SelectedProject.Properties.Target == "exe")
                        {
                            aldebug.Start(SelectedProject.CodeDomProvider.OutputDirectory + @"\" + SelectedProject.Properties.Output);
                            aldebug.ExecutionProcess.LogMessage += ExecutionProcess_LogMessage;
                            aldebug.ExecutionProcess.ModuleLoaded += ExecutionProcess_ModuleLoaded;
                            aldebug.ExecutionProcess.ModuleUnloaded += ExecutionProcess_ModuleUnLoaded;
                            aldebug.ExecutionProcess.Resumed += ExecutionProcess_Resumed;
                            aldebug.ExecutionProcess.Exited += ExecutionProcess_Exited;
                            IsDebugging = true;
                        }
                        else
                            MessageBoxEx.Show("Developer Studio supports only executable for debugging", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {



                        ALProject startproj = CSol.GetStartupProject();
                        // TODO:Add Debug Options int settings
                        if (startproj.Properties.Target == "winexe")
                            aldebug.Start(startproj.CodeDomProvider.OutputDirectory + @"\" + startproj.Properties.Output);
                        else
                            MessageBoxEx.Show("Developer Studio supports only executable for debugging", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");
                DebugControls(IsDebugging);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        //TEST:JUST
        public void ReBuildProject()
        {
            try
            {
                if (CSol != null)
                {
                    if (SelectedProject != null)
                        SelectedProject.Build();
                    else
                    {
                        ALProject startproj = CSol.GetStartupProject();
                        startproj.Build();
                    }
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");

                CheckErrorMessages();

            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void RunProject()
        {
            try
            {

                if (CSol != null && SelectedProject != null)
                {
               
                    // TODO:Add Debug Options int settings
                    if (SelectedProject.Properties.Target == "winexe" || SelectedProject.Properties.Target == "exe")
                    {
                        aldebug.StartWithoutDebug(SelectedProject.CodeDomProvider.OutputDirectory + @"\" + SelectedProject.Properties.Output);

                        IsDebugging = false;
                    }
                    else
                        MessageBoxEx.Show("Developer Studio supports only executable for debugging", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    Log.ShowInfo("No project selected \nPlease at least open a file from the desired project", "Project Manager");

          
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void CleanProject()
        {
            try
            {
                if (CSol != null)
                {
                    if (SelectedProject != null)
                        SelectedProject.Clean();
                    else
                    {
                        ALProject startproj = CSol.GetStartupProject();
                        startproj.Clean();
                    }
                }
                else
                    Log.ShowInfo("No project selected \nPlease at least open a file from the desired project", "Project Manager");


            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        public void BuildProject(ALProject alp)
        {
            try
            {
                if (CSol != null)
                {
                    // Build
                    if (alp != null)
                        alp.Build();
                    else
                        Log.ShowInfo("No project found", "Project Manager");
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");
                CheckErrorMessages();

            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        public void DebugProject(ALProject start)
        {
            try
            {
                if (CSol != null)
                {

                    // Build
                    if (start != null)
                    {

                        aldebug.DeleteAllBreakPoints();
                        LoadProjectBreakPoints(start);
                        // TODO:Add Debug Options int settings
                        if (start.Properties.Target == "winexe" || start.Properties.Target == "exe")
                        {
                            aldebug.Start(start.CodeDomProvider.OutputDirectory + @"\" + start.Properties.Output);
                            aldebug.ExecutionProcess.LogMessage += ExecutionProcess_LogMessage;
                            aldebug.ExecutionProcess.ModuleLoaded += ExecutionProcess_ModuleLoaded;
                            aldebug.ExecutionProcess.ModuleUnloaded += ExecutionProcess_ModuleUnLoaded;
                            aldebug.ExecutionProcess.Resumed += ExecutionProcess_Resumed;
                            aldebug.ExecutionProcess.Exited += ExecutionProcess_Exited;
                            IsDebugging = true;
                        }
                        else
                            MessageBoxEx.Show("Developer Studio supports only executable for debugging", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        Log.ShowInfo("No project found", "Project Manager");
                }
                else
                    Log.ShowInfo("No solution found", "Project Manager");
                DebugControls(IsDebugging);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }


        public void CheckErrorMessages()
        {
            try
            {
                messagesListView.Items.Clear();
                CSol.GetErrors();
                bool build = true;
                if (CSol.Errors.Count > 0)
                {
                 
                    foreach (CompileMessage ms in CSol.Errors)
                    {

                        CompilersLibraryAPI.CompileMessage m = new CompilersLibraryAPI.CompileMessage(ms.LineNumber, ms.CharNumber, ms.Message, (CompilersLibraryAPI.CompileMessage.MessageTypes)ms.Type, ms.FileName,ms.Compile,ms.Code,ms.Project);
                        if (m.Type == CompilersLibraryAPI.CompileMessage.MessageTypes.Error)
                            build = false;
                        AddMessage(m);

                    }
                }
     
                if(build)
                    AppendLog(logbox, "Successfully Built");
                else
                AppendLog(logbox, "Build error");
                  
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        int CurrentBreakLine = 0;
        bool paused = false;
        ALProject debugproj;
        // events
        void aldebug_OnBreakPointHit(object sender, Debugger.DebuggerPausedEventArgs e)
        {
            try
            {
               
                CurrentBreakLine = e.BreakpointsHit[0].Line;

                string file = debugproj.TempService.GetFileFromTemp(e.BreakpointsHit[0].FileName);
                if (FindTabFileName(file) != null)
                {
                    // check if it's the tab
                    DockContainerItem tab = FindTabFileName(file);
                    this.bar5.SelectedDockContainerItem = tab;
                    debugtree.Nodes.Clear();
                    debugtree.Nodes.Add(aldebug.ShowVariables());
                    threadCtrl1.FillThreadCtrl(aldebug.Threads);
                    callStack1.FillStackCtrl(e.Thread);

                    if (ActiveEditor.FileName == file)
                    {
                        bar5.SelectedDockContainerItem = tab;
                        current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                        current.JumpTo(CurrentBreakLine, 1);
                    }
                    else
                    {
                        AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                        current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                        current.JumpTo(CurrentBreakLine, 1);
                    }

                }
                else
                {
                    AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                    current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                    current.JumpTo(CurrentBreakLine, 1);
                }
       // TODO:Paused Controls
                AppendLog(logbox, "Breakpoint at  line : "+CurrentBreakLine.ToString());
                PauseControls();
            }
            catch(Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
      Debugger.AL.Breakpoint TempBP;
        void aldebug_OnPaused(object sender, Debugger.DebuggerPausedEventArgs e)
        {
            try
            {
                paused = true;
                    debugtree.Nodes.Clear();


                    if (e.ExceptionsThrown.Count > 0)
                    {
                        threadCtrl1.FillThreadCtrl(aldebug.Threads);
                        callStack1.FillStackCtrl(e.ExceptionsThrown[0]);
                        exceptionCtrl1.FillException(e.ExceptionsThrown[0]);

                        CurrentBreakLine = e.ExceptionsThrown[0].GetCallstack(1)[0].NextStatement.StartLine;
                        string file = debugproj.TempService.GetFileFromTemp(e.ExceptionsThrown[0].GetCallstack(1)[0].NextStatement.Filename);
                        if (FindTabFileName(file) != null)
                        {
                            // check if it's the tab
                            DockContainerItem tab = FindTabFileName(file);
                            bar5.SelectedDockContainerItem = tab;
                            if (ActiveEditor.FileName == file)
                            {
                                bar5.SelectedDockContainerItem = tab;
                                current.BreakpointManager.AddMarker2(CurrentBreakLine);
                                TempBP = new Breakpoint();
                                TempBP.FileName = file;
                                TempBP.Line = CurrentBreakLine;
                                current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                                current.JumpTo(CurrentBreakLine, 1);
                            }
                            else
                            {
                                AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                                current.BreakpointManager.AddMarker2(CurrentBreakLine);
                                TempBP = new Breakpoint();
                                TempBP.FileName = file;
                                TempBP.Line = CurrentBreakLine;
                                current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                                current.JumpTo(CurrentBreakLine, 1);
                            }
                        }
                        else
                        {
                            AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                            current.BreakpointManager.AddMarker2(CurrentBreakLine);
                            TempBP = new Breakpoint();
                            TempBP.FileName = file;
                            TempBP.Line = CurrentBreakLine;
                            current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                            current.JumpTo(CurrentBreakLine, 1);
                        }
                    }
                    else if(e.Thread != null)
                    {
                        threadCtrl1.FillThreadCtrl(aldebug.Threads);
                        callStack1.FillStackCtrl(e.Thread);
                        exceptionCtrl1.FillException(e.Thread);

                        CurrentBreakLine = e.Thread.GetCallstack(1)[0].NextStatement.StartLine;
                        string file = debugproj.TempService.GetFileFromTemp(e.Thread.GetCallstack(1)[0].NextStatement.Filename);
                        if (FindTabFileName(file) != null)
                        {
                            // check if it's the tab
                            DockContainerItem tab = FindTabFileName(file);
                            bar5.SelectedDockContainerItem = tab;
                            if (ActiveEditor.FileName == file)
                            {
                                bar5.SelectedDockContainerItem = tab;
                                current.BreakpointManager.AddMarker2(CurrentBreakLine);
                                TempBP = new Breakpoint();
                                TempBP.FileName = file;
                                TempBP.Line = CurrentBreakLine;
                                current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                                current.JumpTo(CurrentBreakLine, 1);
                            }
                            else
                            {
                                AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                                current.BreakpointManager.AddMarker2(CurrentBreakLine);
                                TempBP = new Breakpoint();
                                TempBP.FileName = file;
                                TempBP.Line = CurrentBreakLine;
                                current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                                current.JumpTo(CurrentBreakLine, 1);
                            }
                        }
                        else
                        {
                            AddNewTextEditor(Path.GetFileName(file), file, debugproj);
                            current.BreakpointManager.AddMarker2(CurrentBreakLine);
                            TempBP = new Breakpoint();
                            TempBP.FileName = file;
                            TempBP.Line = CurrentBreakLine;
                            current.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Hit);
                            current.JumpTo(CurrentBreakLine, 1);
                        }
                    }
                    AppendLog(logbox, "Paused at  line : " + CurrentBreakLine.ToString());
                
                PauseControls();
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void ExecutionProcess_LogMessage(object sender, Debugger.MessageEventArgs e)
        {
            try
            {
                AppendLog(logbox, "PROCESS:" + e.Category + ":" + e.Message);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void ExecutionProcess_ModuleLoaded(object sender, Debugger.ModuleEventArgs e)
        {
            try
            {
                AppendLog(logbox, "Module Loaded:" +e.Module.Name);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void ExecutionProcess_ModuleUnLoaded(object sender, Debugger.ModuleEventArgs e)
        {
            try
            {
                AppendLog(logbox, "Module UnLoaded:" + e.Module.Name);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void ExecutionProcess_Resumed(object sender, Debugger.DebuggerEventArgs e)
        {
            try
            {
         
                    ActiveEditor.BreakpointManager.SetBreakPointState(CurrentBreakLine, ICSharpCode.AvalonEdit.BreakPointState.Normal);
                AppendLog(logbox, "Debugging Resumed");
                paused = false;
                DebugControls(true);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void ExecutionProcess_Exited(object sender, Debugger.DebuggerEventArgs e)
        {
            try
            {
                AppendLog(logbox, "Process Exited : Debug Complete");
                DebugControls(false);
                IsDebugging = false;
               
               // aldebug.TryClean();
           
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        // Debug Controls 
        void PauseControls()
        {
            try
            {
                continuemn.Enabled = true;
                pausemn.Enabled = false;
                pausebt.Text = "Continue";
                pausebt.Image = devstd.Properties.Resources.play_26;
                this.Refresh();
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        void DebugControls(bool debug)
        {
            try
            {
                bar4.Visible = !debug;
                bar9.Visible = !debug;
                //bar7.Visible = debug;
                threadtab.Visible = debug;
                locvartab.Visible = debug;
                errlisttab.Visible = !debug;
                callstacktab.Visible = debug;
                 exceptab.Visible = debug;
                // menu
              stepintomn .Enabled = debug;
              stepovermn.Enabled = debug;
              stepoutmn.Enabled = debug;
              stepoutbt.Enabled = debug;
              stepovrbt.Enabled = debug;
              stepinbt.Enabled = debug;

              stopmn.Enabled = debug;
              pausemn.Enabled = debug;
              buildmn.Visible = !debug;
              continuemn.Enabled = !debug;

              pausebt.Enabled = debug;
              stopbt.Enabled = debug;
              restartbt.Enabled = debug;

              pausebt.Text = "Pause";
              pausebt.Image = devstd.Properties.Resources.pause_26;
              startbtn.Enabled = !debug;
               
              this.Refresh();
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        #endregion

        #region Tab Event
        private void bar5_DockTabClosed(object sender, DockTabClosingEventArgs e)
        {
            try
            {
                if (FindTabByText(e.DockContainerItem.Text) != null)
                {
                    e.RemoveDockTab = false;
                    bar5.Controls.Remove((PanelDockContainer)e.DockContainerItem.Control);
                    bar5.Items.Remove(e.DockContainerItem);
                }
            }
            catch
            {

            }
        }
        private void bar5_DockTabChange(object sender, DockTabChangeEventArgs e)
        {
            try
            {
                DockContainerItem t = (DockContainerItem)e.NewTab;
                if (t != startpage)
                {

                    if (IsFormDesigner)
                        FormDesignerCalls.TabChanged();
                    TabHost tb = (TabHost)t.Tag;
                    if (tb.Project != null)
                    {
                        completion.projectContent = tb.Project.NativeProject;

                        if (tb.Project != null)
                        {
                            buildprojmn.Text = "Build " + tb.Project.Name;
                            rebuildprojmn.Text = "Rebuild " + tb.Project.Name;
                            cleanprojmn.Text = "Clean " + tb.Project.Name;
                        }
                        else
                        {
                            buildprojmn.Text = "Build Project";
                            rebuildprojmn.Text = "Rebuild Project";
                            cleanprojmn.Text = "Clean Project";
                        }



                        if (CSol != null && tb.ControlType == EditorType.CodeEditorProject)
                        {
                            if (!parserworker.IsBusy)
                            {

                                if (parg == null)
                                    parg = new ParserArgument();

                                if (tb.TextEditor != null)
                                {
                                    parg.Editor = tb.TextEditor;
                                    parg.Text = tb.TextEditor.Text;

                                }
                                else
                                {
                                    parg.Editor = null;
                                    parg.Text = "";
                                }
                                parg.Command = ParserCommand.ParseFile;
                                parg.FileName = ActiveEditor.FileName;
                                parg.Project = ActiveHost.Project;

                                progbar.Visible = true;
                                parserworker.RunWorkerAsync();

                            }
                        }
                        else if (CSol != null && tb.ControlType == EditorType.FormDesigner)
                        {
                            if (!parserworker.IsBusy)
                            {

                                if (parg == null)
                                    parg = new ParserArgument();

                                parg.Command = ParserCommand.ParseFile;
                                parg.FileName = ActiveHost.FileName;
                                parg.Project = ActiveHost.Project;

                                progbar.Visible = true;
                                parserworker.RunWorkerAsync();

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
            finally
            {

            }
        }
        private void bar5_DockTabClosing(object sender, DockTabClosingEventArgs e)
        {
            try
            {
                if (e.DockContainerItem != startpage)
                {
                    if (bar5.Items.Count == 1)
                    {
                        e.Cancel = true;
                        MessageBoxEx.Show("You can't close this tab", "Tab Close", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DockContainerItem t = e.DockContainerItem;
                        if (!t.Name.StartsWith("DESIGN"))
                        {

                            if (IsModified(ActiveEditor))
                            {
                                var r = MessageBoxEx.Show(string.Format("Save changes to {0}?", ActiveEditor.FileName ?? "new file"),
                                    "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                if (r == DialogResult.Cancel)
                                    e.Cancel = true;
                                else if (r == DialogResult.Yes)
                                {
                                    if (!DoSave(ActiveEditor))
                                        e.Cancel = true;

                                }
                            }
                        }
                        else
                        {
                            if (t.Text.EndsWith("*"))
                            {
                                var r = MessageBoxEx.Show(string.Format("Save changes to {0}?", formDesignerControl1.FileName ?? "new file"),
                                    "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                if (r == DialogResult.Cancel)
                                    e.Cancel = true;
                                else if (r == DialogResult.Yes)
                                {
                                    formDesignerControl1.Save();
                                    if (SelectedProject != null)
                                    {
                                        File.WriteAllText(Path.GetDirectoryName(SelectedProject.FileName) + @"\forms\" + Path.GetFileNameWithoutExtension(formDesignerControl1.CodeBehindFileName) + ".al", SelectedProject.alang.InterpretCsToAl(File.ReadAllText(formDesignerControl1.CodeBehindFileName)));
                                    }
                                }
                            }
                        }
                    }

                }
                else if (bar5.SelectedDockContainerItem == startpage)
                {
                    e.Cancel = true;
                    MessageBoxEx.Show("You can't close this tab", "Tab Close", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
             
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
            finally
            {

            }
        }
        #endregion

        #region Form Designer Control Event
        void InitDesigner()
        {
            formDesignerControl1.Init(propertyGrid, toolbox);
            FormDesignerCalls.OnTabChangeText += UpdateTabText;
            FormDesignerCalls.OnFormIndexNeeded += FormDesignerCalls_OnFormIndexNeeded;
            FormDesignerCalls.OnBarSelectNeeded += SelectOrAdd;
            FormDesignerCalls.OnBarNeeded += FormDesignerCalls_OnBarNeeded;
            FormDesignerCalls.OnExchangeControl += FormDesignerCalls_OnExchangeControl;
        }
        Bar FormDesignerCalls_OnBarNeeded()
        {
            return bar5;
        }
        int FormDesignerCalls_OnFormIndexNeeded(string name)
        {
            int i = 0;
            foreach (BaseItem it in bar5.Items)
            {
                if (it.Tag is string)
                {
                    if (it.Tag.ToString() == name && IsFormDesigner)
                        return i;
                  
                }
                i++;
            }
            return -1;
        }
        Control FormDesignerCalls_OnExchangeControl(Control c, DockContainerItem tab, PanelDockContainer pdc, object proj)
        {
            TabHost tb = new TabHost();
            tb.AddFormDesigner(c,tab,pdc,formDesignerControl1,(ALProject)proj);
            return tb;
        }
        delegate void SetTabText(DockContainerItem txt, string value);
        void UpdateTabText(DockContainerItem txt, string value)
        {
            try
            {
                SetTabText d = new SetTabText(STT);
                logbox.BeginInvoke(d, new object[] { txt, value });


            }
            catch
            {

            }
        }
        void STT(DockContainerItem txt, string t)
        {
            try
            {
                txt.Text = t;
            }
            catch
            {

            }
        }

        void SelectOrAdd(DockContainerItem txt, PanelDockContainer dock, bool add)
        {
            try
            {
                if (this.bar5.InvokeRequired)
                {
                    BarSelectTab d = new BarSelectTab(SelectOrAdd);
                    this.bar5.Invoke(d, new object[3] { txt, dock, add });
                }
                else
                {
                    if (add)
                    {
                        bar5.Controls.Add(dock);
                        bar5.Items.Add(txt);
                      
                    }
                    bar5.SelectedDockContainerItem = txt;
                }
            }
            catch
            {

            }
        }
        #endregion
      
        #region File Menu
        private void newprojmn_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectN frm = new ProjectN(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        private void newsrcmn_Click(object sender, EventArgs e)
        {
            try
            {
                SourceN frm = new SourceN();
                frm.ShowDialog();
                if (frm.FileWrit != null)
                    OpenFiles(new string[1] { frm.FileWrit });
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        private void openprojmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    OpenFiles(openFileDialog1.FileNames);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        private void openfilemn_Click(object sender, EventArgs e)
        {
            try
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    OpenFiles(openFileDialog.FileNames);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        private void buttonItem18_Click(object sender, EventArgs e)
        {
            try
            {
                if (CSol != null)
                {
                    if (!projectworker.IsBusy)
                    {
                        progbar.Visible = true;
                        projact = ProjectAction.JustSave;
                        projectworker.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        private void savemn_Click(object sender, EventArgs e)
        {
            try
            {

                if (IsFormDesigner)
                {
                    formDesignerControl1.Save();
                    if (SelectedProject != null)
                        File.WriteAllText(Path.GetDirectoryName(SelectedProject.FileName) + @"\forms\" + Path.GetFileNameWithoutExtension(formDesignerControl1.CodeBehindFileName) + ".al",  SelectedProject.alang.InterpretCsToAl(File.ReadAllText(formDesignerControl1.CodeBehindFileName)));
                    
                }
                else if (IsProjectProperties)
                    ActiveProperties.Save();

                else if (ActiveEditor != null)
                    DoSave(ActiveEditor);
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        private void printmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                {
                    ActiveEditor.PrintDirectDocument();
                }
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }
        private void exitmn_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

        private void printprevmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.PrintPreview();
            }
            catch (Exception ex)
            {
                ELog.LogEx(ex);
            }
        }

#endregion

        #region Build Menu
        private void buildsolmn_Click(object sender, EventArgs e)
        {
            Build();
        }

        private void rebuildsolmn_Click(object sender, EventArgs e)
        {
            ReBuild();
        }

        private void cleansolmn_Click(object sender, EventArgs e)
        {
            Clean();
        }
        private void buildprojmn_Click(object sender, EventArgs e)
        {
            BuildProject();
        }

        private void rebuildprojmn_Click(object sender, EventArgs e)
        {
            ReBuildProject();
        }

        private void cleanprojmn_Click(object sender, EventArgs e)
        {
            CleanProject();
        }
        private void cancelbuildmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (buildworker.IsBusy)
                {
                    buildworker.CancelAsync();
                    slb.Text = "Build Cancelled";
                    cancelbuildmn.Enabled = false;
                }
                else
                    Log.ShowInfo("No build task", "Cancel Build");
            }
            catch
            {

            }
        }
        #endregion

        #region Edit Menu
        private void undomn_Click(object sender, EventArgs e)
        {
            try
            {

                if (ActiveEditor != null)
                {
                    ActiveEditor.Undo();

                    
                }
                else if (IsFormDesigner)
                {
                    formDesignerControl1.Undo();
                }
            }
            catch
            {

            }
        }

        private void redomn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.Redo();
                else if (IsFormDesigner)
                {
                    formDesignerControl1.Redo();
                }
            }
            catch
            {

            }
        }

        private void cutmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.Cut();
                else if (IsFormDesigner)
                    formDesignerControl1.Cut();
                
  
            }
            catch
            {

            }
        }

        private void copymn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.Copy              ();
                else if (IsFormDesigner)
                    formDesignerControl1.Copy();
               
            }
            catch
            {

            }
        }

        private void pastemn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.Paste();
                else if (IsFormDesigner)
                    formDesignerControl1.Paste();
               
            }
            catch
            {

            }
        }

        private void deletemn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.Delete();
                else if (IsFormDesigner)
                {
                    formDesignerControl1.Delete();
                }
            }
            catch
            {

            }
        }

        private void selectallmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.SelectAll();
                else if (IsFormDesigner)
                    formDesignerControl1.SelectAll();
               
            }
            catch
            {

            }
        }

        private void endmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.JumpTo(ActiveEditor.Document.LineCount - 1,0);
            }
            catch
            {

            }
        }

        private void findmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    FindAndReplace();
             
            }
            catch
            {

            }
        }

        private void togbookmn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveEditor != null)
                    ActiveEditor.BreakpointManager.AddMarker(ActiveEditor.TextArea.Caret.Line);
             
            }
            catch
            {

            }
            
        }

#endregion

        #region View Menu
        private void solmn_Click(object sender, EventArgs e)
        {
            try
            {
                solexplr.Selected = true;
            }
            catch
            {

            }
        }

        private void propmn_Click(object sender, EventArgs e)
        {
            try
            {
              proptab.Selected = true;
            }
            catch
            {

            }
        }

    
        private void errormn_Click(object sender, EventArgs e)
        {
            try
            {
             errlisttab.Selected = true;
            }
            catch
            {

            }
        }

        private void outputmn_Click(object sender, EventArgs e)
        {
            try
            {
               outputab.Selected = true;
            }
            catch
            {

            }
        }

        private void locvarmn_Click(object sender, EventArgs e)
        {
            try
            {
               locvartab.Selected = true;
            }
            catch
            {

            }
        }

        private void thrmn_Click(object sender, EventArgs e)
        {
            try
            {
              threadtab.Selected = true;
            }
            catch
            {

            }
        }

        private void stackmn_Click(object sender, EventArgs e)
        {
            try
            {
              callstacktab.Selected = true;
            }
            catch
            {

            }
        }

        private void exceptionsmn_Click(object sender, EventArgs e)
        {
            try
            {
              exceptab.Selected = true;
            }
            catch
            {

            }
        }

        private void bpointsmn_Click(object sender, EventArgs e)
        {
            try
            {
                breakstab.Visible = true;
                breakstab.Selected = true;
                
            }
            catch
            {

            }
        }

        private void findtabmn_Click(object sender, EventArgs e)
        {
            try
            {
                fresultstab.Selected = true;
                
            }
            catch
            {

            }
        }

#endregion

        #region Debug Menu
  private void startdebugmn_Click(object sender, EventArgs e)
  {
      Debug();
  }

  private void runmn_Click(object sender, EventArgs e)
  {
      Run();
  }

  private void continuemn_Click(object sender, EventArgs e)
  {
      try
      {
          callStack1.Clear();
          threadCtrl1.Clear();
          exceptionCtrl1.Clear();
          debugtree.Nodes.Clear();
          if (TempBP != null)
          {
              if (FindTabFileName(TempBP.FileName) != null)
              {
                  bar5.SelectedDockContainerItem = FindTabFileName(TempBP.FileName);
                  ActiveEditor.BreakpointManager.RemoveMarker2(TempBP.Line);
              }
              
          }
          aldebug.Continue();
      }
      catch
      {

      }
  }
  private void stopmn_Click(object sender, EventArgs e)
  {
      try
      {
          aldebug.Stop();
      }
      catch
      {

      }

  }
  private void pausemn_Click(object sender, EventArgs e)
  {
      try
      {
          aldebug.Break();
      }
      catch
      {

      }

  }
  private void stepovermn_Click(object sender, EventArgs e)
  {
      try
      {
          aldebug.StepOver();
      }
      catch
      {

      }
  }
  private void stepintomn_Click(object sender, EventArgs e)
  {
      try
      {
          aldebug.StepInto();
      }
      catch
      {

      }
  }

  private void stepoutmn_Click(object sender, EventArgs e)
  {
      try
      {
          aldebug.StepOut();
      }
      catch
      {

      }
  }
  private void disablebreaksmn_Click(object sender, EventArgs e)
  {
      try
      {
          if (disablebreaksmn.Checked)
          {
              disablebreaksmn.Checked = false;
              aldebug.EnableAllBreakPoints();
          }
          else
          {
              disablebreaksmn.Checked = true;
              aldebug.DisableAllBreakPoints();
          }
      }
      catch
      {

      }
  }

  private void delbreaksmn_Click(object sender, EventArgs e)
  {
      try
      {
          aldebug.DeleteAllBreakPoints();
      }
      catch
      {

      }
  }
  #endregion

  private void pausebt_Click(object sender, EventArgs e)
  {
      try
      {
          if (pausebt.Text == "Continue" && IsDebugging)
          {
              callStack1.Clear();
              threadCtrl1.Clear();
              exceptionCtrl1.Clear();
              debugtree.Nodes.Clear();
              if (TempBP != null)
              {
                  if (FindTabFileName(TempBP.FileName) != null)
                  {
                      bar5.SelectedDockContainerItem = FindTabFileName(TempBP.FileName);
                      ActiveEditor.BreakpointManager.RemoveMarker2(TempBP.Line);
                  }

              }
              aldebug.Continue();
          }
          else
              aldebug.Break();
      }
      catch
      {

      }
  }

  private void stopbt_Click(object sender, EventArgs e)
  {
      stopmn_Click(this, e);
  }

  private void restartbt_Click(object sender, EventArgs e)
  {
      try
      {
          if (IsDebugging)
          {
              aldebug.Stop();
            //  aldebug.TryClean();
              Debug();
          }

      }
      catch
      {

      }
  }
  private void commn_Click(object sender, EventArgs e)
  {
      try
      {
          if (ActiveEditor != null)
          {
              // have a selection
              if (ActiveEditor.SelectionLength > 0)
              {
                  // selection end line
                  DocumentLine dl = ActiveEditor.Document.GetLineByOffset(ActiveEditor.SelectionStart + ActiveEditor.SelectionLength);
                  TextLocation stloc = new TextLocation(ActiveEditor.TextArea.Caret.Line, 0);
                  int soff = ActiveEditor.Document.GetOffset(stloc);
                  if (dl.LineNumber != ActiveEditor.TextArea.Caret.Line)
                  {
                      if (ActiveEditor.Text.Substring(soff, 2) != "/*")
                      {
                          // add multiline selection
                          ActiveEditor.JumpTo(ActiveEditor.TextArea.Caret.Line, 0);

                       
                          ActiveEditor.Document.Insert(soff, "/* ");
                          ActiveEditor.Document.Insert(dl.EndOffset, "*/ ");
                      }
                      else
                      {
                       
                          ActiveEditor.Document.Remove(soff, 2);
                          // remove multiline selection
                        int endp = dl.Offset + ActiveEditor.Document.GetText(dl).IndexOf("*/");
                          ActiveEditor.Document.Remove(endp, 2);
                      }
                  }
                  else
                  {
                      
                      if (ActiveEditor.Text.Substring(dl.Offset, 2) != "//")
                      {
                          // add line selection
                          ActiveEditor.Document.Insert(dl.Offset, "// ");

                      }
                      else
                      {
                          // remove  line selection
                          ActiveEditor.Document.Remove(dl.Offset, 2);
                      }
                  }
              }
           
          }
      }
      catch
      {

      }
  }

  private void tabmn_Click(object sender, EventArgs e)
  {
      try
      {
          if (ActiveEditor != null)
          {

              ActiveEditor.JumpTo(ActiveEditor.TextArea.Caret.Line, 0);
              ActiveEditor.Document.Insert(ActiveEditor.TextArea.Caret.Offset, "      ");
          }
      }
      catch
      {

      }
  }

  #region Worker
  bool isbuild = false;
  ProjectAction projact;
 private void buildworker_DoWork(object sender, DoWorkEventArgs e)
  {
      try
      {
         
          BackgroundWorker bgw = (BackgroundWorker)sender;
          if (isbuild)
              CSol.Build(bgw);
          else
              CSol.ReBuild(bgw);
      }
      catch(Exception ex)
      {
          ELog.LogEx(ex);
      }
  }
 private void buildworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
 {
     try
     {
         cancelbuildmn.Enabled = true;
         slb.Text = e.UserState.ToString();
     }
     catch
     {

     }
 }
 private void buildworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
 {
     try
     {
         slb.Text = "Ready";
         progbar.Visible = false;
         
         CheckErrorMessages();
     }
     catch
     {

     }
 }
 ParserArgument parg;
private void parserworker_DoWork(object sender, DoWorkEventArgs e)
 {
     try
     {
     
         if (parg.Command == ParserCommand.ParseFile)
             parg.Project.ParserService.UpdateSourceFile(parg.FileName);
         else if (parg.Command == ParserCommand.ParseRessource)
             parg.Project.ParserService.UpdateRessources();

         if (parg.Editor != null)
         {
             CodeTextEditor edit = (CodeTextEditor)parg.Editor;
             SyntaxTree st = new ALParser().Parse(parg.Text, parg.FileName);
             edit.ColorUpdater.UpdateCurrentFile(st);
             edit.ColorUpdater.UpdateProject(parg.Project.ParserService.NativeProject);
         }
     }
     catch (Exception ex)
     {
         ELog.LogEx(ex);

     }

 }
private void parserworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
{
    try
    {
        slb.Text = "Ready";
        progbar.Visible = false;
        if (SelectedProject != null && IsCodeEditorProject)
            completion.projectContent = SelectedProject.NativeProject;


        //CheckErrorMessages();
    }
    catch
    {

    }
}
private void projectworker_DoWork(object sender, DoWorkEventArgs e)
{
    try
    {
        BackgroundWorker bgw = (BackgroundWorker)sender;
        if (projact == ProjectAction.Open)
            CSol.Open(bgw);    
        else  CSol.Save(bgw);
    }
    catch (Exception ex)
    {
        ELog.LogEx(ex);

    }
}
private void projectworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
{
    try
    {
        slb.Text = "Ready";
        progbar.Visible = false;

        if (projact == ProjectAction.Open)
                LoadSolutionControl();
        else if (projact == ProjectAction.SaveClose)
            CSol = null;
        else if (projact == ProjectAction.ReLoad)
        {
            projact = ProjectAction.Open;
            progbar.Visible = true;
            projectworker.RunWorkerAsync();
        }
        else if (projact == ProjectAction.OpenNew)
        {
            CSol = new DSolution(solfile, SolVersion);
            projact = ProjectAction.Open;
            progbar.Visible = true;
            projectworker.RunWorkerAsync();
        }
     
    }
    catch
    {

    }
}
#endregion

#region Project Menu
private void addfrmmn_Click(object sender, EventArgs e)
{
    try
    {
        if(SelectedProject != null)
          AddComponent(SelectedProject);
        else
         Log.ShowInfo("No project selected \nPlease at least open a file from the desired project", "Add Component");
             
    }
    catch (Exception ex)
    {
        ELog.LogEx(ex);
    }
}

private void addrefmn_Click(object sender, EventArgs e)
{
    try
    {
        if (SelectedProject != null)
        {
            Node nd = GetProjectNode(SelectedProject);
            if(nd != null)
               AddReference(SelectedProject, nd);

        }
       else
         Log.ShowInfo("No project selected \nPlease at least open a file from the desired project", "Add Reference");
    }
    catch (Exception ex)
    {
        ELog.LogEx(ex);
    }
}

private void excludefromprojmn_Click(object sender, EventArgs e)
{
    try
    {
        
        if(advTree1.SelectedNodes.Count > 0)
        if (advTree1.SelectedNode.Tag is SourceFile)
        {
            ALProject pro = (ALProject)advTree1.SelectedNode.Parent.Tag;
            SourceFile sf = (SourceFile)advTree1.SelectedNode.Tag;
            advTree1.SelectedNode.Parent.Nodes.Remove(advTree1.SelectedNode);
            pro.RemoveSource(sf.Name);

        }
        else
            Log.ShowInfo("Only source files are supported", "Exclude from project");
    }
    catch(Exception ex)
    {
        ELog.LogEx(ex);
    }
}

private void removeprojmn_Click(object sender, EventArgs e)
{
    try
    {

        if (advTree1.SelectedNodes.Count > 0)
            if (advTree1.SelectedNode.Tag is SourceFile)
            {
                ALProject pro = (ALProject)advTree1.SelectedNode.Parent.Tag;
                SourceFile sf = (SourceFile)advTree1.SelectedNode.Tag;
                pro.RemoveSource(sf.Name);
                advTree1.SelectedNode.Parent.Nodes.Remove(advTree1.SelectedNode);
                File.Delete(sf.SourcePath);

            }
            else
                Log.ShowInfo("Only source files are supported", "Remove from project");
    }
    catch (Exception ex)
    {
        ELog.LogEx(ex);
    }
}

private void addexistingitem_Click(object sender, EventArgs e)
{
    try
    {
        if (SelectedProject != null)
        {
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = openFileDialog2.FileName;
                if (file.EndsWith(".Designer.al"))
                {
                    // Form
                    if(!SelectedProject.SourceFiles.ContainsKey(Path.GetFileName(file)))
                    {
                        // add
                        SourceFile sf = new SourceFile(Path.GetFileName(file), file, Path.GetFileName(file), SourceFileType.Form);
                        SelectedProject.SourceFiles.Add(sf.Name, sf);
                    }
                }
                else
                {
                    // Si
                    if (!SelectedProject.SourceFiles.ContainsKey(Path.GetFileName(file)))
                    {
                        // add
                        SourceFile sf = new SourceFile(Path.GetFileName(file), file, Path.GetFileName(file), SourceFileType.Source);
                        SelectedProject.SourceFiles.Add(sf.Name, sf);
                    }
                }
            }
        }
        else
            Log.ShowInfo("No project selected \nPlease at least open a file from the desired project", "Add Component");
    }
    catch
    {

    }
}

private void setasstartmn_Click(object sender, EventArgs e)
{
    try
    {
        if (SelectedProject != null && CSol != null)
        {
            CSol.SetStartupProject(SelectedProject.Name);
            advTree1.Nodes.Remove(advTree1.Nodes[0]);
            LoadSolutionControl();
        }
        else
            Log.ShowInfo("No project selected \nPlease at least open a file from the desired project", "Startup project");
        
    }
    catch
    {

    }
}

private void propertiesmn_Click(object sender, EventArgs e)
{
    try
    {
        if (SelectedProject != null)
            OpenProjectProperties(SelectedProject);
        else
            Log.ShowInfo("No project selected \nPlease at least open a file from the desired project", "Project Properties");
    }
    catch
    {

    }
}


#endregion

#region Tools Menu
private void recordcvpmn_Click(object sender, EventArgs e)
{
    try
    {
        if (ActiveEditor != null)
        {
            if (cvprecording)
            {

                cvpw.Stop();
                cvprecording = false;
                recordinghost = ActiveEditor.Name;
                recordcvpmn.Text = "Record CVP";
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "*.cvp|*.cvp";
                sfd.Title = "Code Visual Presentation";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    cvpw = new CVP.CVPRecorder();
                    cvpw.Create(sfd.FileName);
                    cvpw.Record();
                    recordinghost = ActiveEditor.FileName;
                    cvprecording = true;
                     recordcvpmn.Text = "Stop Recording CVP";
                }
            }
        }
    }
    catch
    {

    }
}

private void playcvpmn_Click(object sender, EventArgs e)
{
    try
    {
        OpenFileDialog sfd = new OpenFileDialog();
        sfd.Filter = "*.cvp|*.cvp";
        sfd.Title = "Code Visual Presentation";
        if (sfd.ShowDialog() == DialogResult.OK && !cvprecording)
        {

            CVPPlay frm = new CVPPlay();
            frm.Play(sfd.FileName);
            frm.ShowDialog();
        }
    }
    catch
    {

    }
}

private void addttsmn_Click(object sender, EventArgs e)
{
    try
    {
        if (cvprecording)
        {
            AddTTSFrm frm = new AddTTSFrm();
            frm.ShowDialog();
            if (frm.Said && frm.textBoxX1.Text.Length > 0)
            {
                if (frm.checkBoxX1.Checked)
                    cvpw.AddSaySync(frm.textBoxX1.Text);
                else
                    cvpw.AddSayAsync(frm.textBoxX1.Text);
            }
        }
    }
    catch
    {

    }
}

private void altocsmn_Click(object sender, EventArgs e)
{
    try
    {

        if (IsCodeEditorProject && SelectedProject != null)
        {
            if (ActiveEditor.SelectedText.Length > 0 && ActiveEditor.FileName.EndsWith(".al"))
            {
                int offset = ActiveEditor.SelectionStart;
                string code = SelectedProject.alang.InterpretAlToCs(ActiveEditor.SelectedText.Replace("\n", "\n ").Replace("\r", " \r"));
                ActiveEditor.Text = ActiveEditor.Text.Remove(offset, ActiveEditor.SelectionLength);
                ActiveEditor.Text = ActiveEditor.Text.Insert(offset, code);
            }
            else
                Log.ShowInfo("To successfully convert the code you need to :\n* Select an AL or C# code\n* The Code must be full (contains the program/(class-enum..../members...) tree)", "Code Converter ");
             
        }
    }
    catch (Exception ex)
    {
        ELog.LogEx(ex);
    }
}

private void cstoalmn_Click(object sender, EventArgs e)
{
    try
    {

        if (IsCodeEditorProject && SelectedProject != null)
        {
            if (ActiveEditor.SelectedText.Length > 0 && ActiveEditor.FileName.EndsWith(".al"))
            {
                int offset = ActiveEditor.SelectionStart;
                string code = SelectedProject.alang.InterpretCsToAl(ActiveEditor.SelectedText.Replace("\n", "\n ").Replace("\r", " \r"));
                ActiveEditor.Text = ActiveEditor.Text.Remove(offset, ActiveEditor.SelectionLength);
                ActiveEditor.Text = ActiveEditor.Text.Insert(offset, code);
            }
            else
                Log.ShowInfo("To successfully convert the code you need to :\n* Select an AL or C# code\n* The Code must be full (contains the program/(class-enum..../members...) tree)", "Code Converter ");
             

        }
    }
    catch (Exception ex)
    {
        ELog.LogEx(ex);
    }
}

#endregion

private void mysqlmanagermn_Click(object sender, EventArgs e)
{
    try
    {
        
        Process.Start("http://www.wampserver.com/");
    }
    catch
    {

    }
}

private void MainForm_Load(object sender, EventArgs e)
{
    try
    {

    }
    catch
    {

    }
}

private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
{
    try
    {

    }
    catch
    {

    }
}

private void MainForm_Shown(object sender, EventArgs e)
{
    Programs.CallClosespl();
}
protected override void WndProc(ref Message message)
{
    if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
    {
        ShowWindow();
    }
    base.WndProc(ref message);
}
bool minimizedToTray = false;
public void ShowWindow()
{
    try
    {
        if (minimizedToTray)
        {

            this.Show();
            this.WindowState = FormWindowState.Maximized;
            minimizedToTray = false;
        }
        else
        {
            WinApi.ShowToFront(this.Handle);
        }

    }
    catch
    {

    }
    try
    {
        if (File.Exists(Application.StartupPath + @"\ARGS.t"))
        {
            OpenFiles(File.ReadAllLines(Application.StartupPath + @"\ARGS.t"));
            File.Delete(Application.StartupPath + @"\ARGS.t");
        }
    }
    catch
    {
    }
}

#region Context Menu
Node ctxnode = null;
ALProject TryExtractProject(Node nod)
{

        if (!(ctxnode.Tag is DSolution))
        {
            if (ctxnode.Tag is ALProject)
                return (ALProject)ctxnode.Tag;
            else if (ctxnode.Parent.Tag is ALProject)
                return (ALProject)ctxnode.Parent.Tag;
            else if (ctxnode.Parent.Parent.Tag is ALProject)
                return (ALProject)ctxnode.Parent.Parent.Tag;
        }

    
    return null;
}
private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
{
    try
            {

                Node nd = advTree1.SelectedNode;
                ctxnode = nd;
                remrefcm.Visible = false;
                remrescm.Visible = false;
                addrefcm.Visible = false;
                addrescm.Visible = false;
                addcompcm.Visible = false;
                remcompcm.Visible = false;

                addnewprojcm.Visible = false;
                addexprojcm.Visible = false;
                debugprojcm.Visible = false;
                propscm.Visible = false;
                setasstartcm.Visible = false;
                buildprojcm.Visible = false;
                rebuildprojcm.Visible = false;
                if (nd.Name == "FOLDERRES")
                    addrescm.Visible = true;
                else if (nd.Name == "FOLDERREFS")             
                    addrefcm.Visible = true;
                else if (nd.Name == "FOLDERSRC" || nd.Name == "FOLDEREVENTFRM" || nd.Name == "FOLDERFRM")
                    addcompcm.Visible = true;
                else if (nd.Parent.Name == "FOLDERRES")
                    remrescm.Visible = true;
                else if (nd.Parent.Name == "FOLDERREFS")      
                    remrefcm.Visible = true;
                else if (nd.Parent.Name == "FOLDERSRC" || nd.Parent.Name == "FOLDEREVENTFRM" || nd.Parent.Name == "FOLDERFRM")
                    remcompcm.Visible = true;
                else if (nd.Tag is ALProject)
                {
                    debugprojcm.Visible = true;
                    propscm.Visible = true;
                    setasstartcm.Visible = true;
                    buildprojcm.Visible = true;
                    rebuildprojcm.Visible = true;
                }
                else if (nd.Tag is DSolution)
                {
                    addnewprojcm.Visible = true;
                    addexprojcm.Visible = true;
                }

            }
            catch
            {

            }
}

private void addrefcm_Click(object sender, EventArgs e)
{
    try
    {
        if (ctxnode != null)
        {
            ALProject proj = TryExtractProject(ctxnode);
            if (proj != null)
                AddReference(proj, GetProjectNode(proj));
            else
                Log.ShowInfo("You should select an element from the desired project", "Add Reference");
        }
        else
            Log.ShowInfo("You should select an element from the desired project","Add Reference");
    }
    catch
    {

    }
}
private void addrescm_Click(object sender, EventArgs e)
{
    if (ctxnode != null)
    {
        ALProject proj = TryExtractProject(ctxnode);
        if (proj != null)
           AddRessourceExplorer(proj);
        else
            Log.ShowInfo("You should select an element from the desired project", "Add Reference");
    }
    else
        Log.ShowInfo("You should select an element from the desired project", "Add Reference");
}
private void addcompcm_Click(object sender, EventArgs e)
{
    if (ctxnode != null)
    {
        ALProject proj = TryExtractProject(ctxnode);
        if (proj != null)
           AddComponent(proj);
        else
            Log.ShowInfo("You should select an element from the desired project", "Add Reference");
    }
    else
        Log.ShowInfo("You should select an element from the desired project", "Add Reference");
}

private void remrefcm_Click(object sender, EventArgs e)
{
    try
    {

        if (ctxnode != null && (ctxnode.Parent.Tag is ALProject))
        {
            ALProject proj = (ALProject)ctxnode.Parent.Tag;
            ReferencedAssembly asm = (ReferencedAssembly)ctxnode.Tag;
            proj.RemoveAsm(asm.Name);

            ctxnode.Remove();
        }
        LoadSolutionControl();
    }
    catch
    {

    }
}

private void remcompcm_Click(object sender, EventArgs e)
{
    try
    {

        if (ctxnode != null && (ctxnode.Parent.Tag is ALProject))
        {
            ALProject proj = (ALProject)ctxnode.Parent.Tag;
            SourceFile asm = (SourceFile)ctxnode.Tag;
            proj.RemoveSource(asm.Name);

            ctxnode.Remove();
        }
        LoadSolutionControl();
    }
    catch
    {

    }
}

private void remrescm_Click(object sender, EventArgs e)
{
    try
    {

        if (ctxnode != null && (ctxnode.Parent.Tag is ALProject))
        {
            ALProject proj = (ALProject)ctxnode.Parent.Tag;
            RessourceItem asm = (RessourceItem)ctxnode.Tag;
            proj.RemoveRes(asm.Name);

            ctxnode.Remove();
        }
        LoadSolutionControl();
    }
    catch
    {

    }
}

private void addnewprojcm_Click(object sender, EventArgs e)
{
    try
    {
        if(ctxnode != null)
            if (ctxnode.Tag is DSolution)
            {
                ProjectN pro = new ProjectN(this);
                pro.AddProject = true;
                pro.ShowDialog();
            }
    }
    catch
    {

    }
}
private void addexprojcm_Click(object sender, EventArgs e)
{
    try
    {
        if (ctxnode != null)
            if (ctxnode.Tag is DSolution)
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialog1.FileName.EndsWith(".alproj"))
                    {
                        ALProject alp = new ALProject(openFileDialog1.FileName);
                        CSol.AddEditProject(alp, (byte)CSol.Projects.Count, true);
                        LoadSolutionControl();
                    }
                }
            }
    }
    catch
    {

    }
}

private void buildprojcm_Click(object sender, EventArgs e)
{
    try
    {
        if (ctxnode != null)
        {
            if (ctxnode.Tag is ALProject)
                BuildProject((ALProject)ctxnode.Tag);
        }
    }
    catch
    {

    }
}

private void rebuildprojcm_Click(object sender, EventArgs e)
{
    try
    {
        if (ctxnode != null)
        {
            if (ctxnode.Tag is ALProject)
                BuildProject((ALProject)ctxnode.Tag);
        }
    }
    catch
    {

    }
}

private void setasstartcm_Click(object sender, EventArgs e)
{
    try
    {
        if (ctxnode != null)
        {
            if (ctxnode.Tag is ALProject)
            {
               
                CSol.SetStartupProject(((ALProject)ctxnode.Tag).Name);
                LoadSolutionControl();
            }
        }
    }
    catch
    {

    }
}

private void debugprojcm_Click(object sender, EventArgs e)
{
    try
    {
        if (ctxnode != null)
        {
            if (ctxnode.Tag is ALProject)
                DebugProject((ALProject)ctxnode.Tag);
            
        }
    }
    catch
    {

    }
}

private void propscm_Click(object sender, EventArgs e)
{
    try
    {
        if (ctxnode != null)
        {
            if (ctxnode.Tag is ALProject)
           OpenProjectProperties((ALProject)ctxnode.Tag);
        }
    }
    catch
    {

    }
}
#endregion

private void colortimer_Tick(object sender, EventArgs e)
{
    try
    {
        if (IsCodeEditorProject && SelectedProject != null)
            ActiveEditor.ColorUpdater.UpdateColor(ActiveEditor);
        
    }
    catch
    {

    }
}

    }
}