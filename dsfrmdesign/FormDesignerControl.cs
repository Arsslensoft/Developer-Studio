using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Windows.Forms.Design;
using System.IO;
using System.ComponentModel.Design;
using System.Reflection;
using System.Drawing.Design;

namespace alfrmdesign
{

    public partial class FormDesignerControl : UserControl
    {
        private Workspace _workspace;
        private ToolboxFiller _toolboxFiller;
        private readonly string MODIFIED_MARKER = " *";
      
        public Type SelectedType
        {
            get { return GetCompType();  }
        }
        public Bar surfaceTabs
        {
            get
            {
                return FormDesignerCalls.GetBar();
            }

        }
        void SelectTab(DockContainerItem tab)
        {
            FormDesignerCalls.Modify(tab, null,false);
        }
        void AddTab(DockContainerItem tab, PanelDockContainer dock)
        {
            FormDesignerCalls.Modify(tab, dock, true);
        }
      
        Type GetCompType()
        {
            Type type = null;
            try{
            string name = toolbox.GetSelectedToolboxItem().ComponentType;
            foreach (Assembly asm in _workspace.References.Assemblies)
            {
                if (asm.GetType(name, false) != null)
                    type = asm.GetType(name, false);
            }
            
            }
                catch{

                }
            return type;
        }
        public void AddRef(Assembly asm)
        {
            try
            {
                if (_workspace != null)
                {
                    if (!_workspace.References.Assemblies.Contains(asm))
                        _workspace.References.AddReference(asm);

                  

                }
            }
            catch
            {

            }
        }
        public FormDesignerControl()
        {
            InitializeComponent();
    
        }
        public string FileName
        {
            get { return _workspace.ActiveDocument.FileName; }
        }
        public string CodeBehindFileName
        {
            get { return _workspace.ActiveDocument.CodeBehindFileName; }
        }
        public void Init(PropertyGrid prop, ToolBoxList tool)
        {
            toolbox = tool;
            propertyGrid = prop;
            FormDesignerCalls.OnTabClose += FormDesignerCalls_OnTabClose;
            LoadWorkspace();
        }
        void AddPanelX(ref  ErrorListTabPage pdc, ref DockContainerItem dci)
        {

            pdc.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            pdc.Location = new System.Drawing.Point(3, 28);
            pdc.Name = "ERRORLIST";
            pdc.Size = new System.Drawing.Size(553, 265);
            pdc.Style.Alignment = System.Drawing.StringAlignment.Center;
            pdc.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            pdc.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            pdc.Style.GradientAngle = 90;
            pdc.TabIndex = 2;


            dci.Control = pdc;
            dci.Name = "TAB";
            pdc.Tag = dci;

        }
        void AddPanel(ref  PanelDockContainer pdc, ref DockContainerItem dci, string file)
        {

            pdc.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            pdc.Location = new System.Drawing.Point(3, 28);
            pdc.Name = "CONTROLID";
            pdc.Size = new System.Drawing.Size(553, 265);
            pdc.Style.Alignment = System.Drawing.StringAlignment.Center;
            pdc.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            pdc.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            pdc.Style.GradientAngle = 90;
            pdc.TabIndex = 2;
            dci.Tag = file;

            dci.Control = pdc;
            dci.Name = "TAB";
            pdc.Tag = dci;

        }
    
        bool ContainsTab(string f)
        {
            return FormDesignerCalls.ContainsTab(f);
        }
        int GetTabIndex(string f)
        {
            return FormDesignerCalls.GetTabIndex(f);
        }


        private void LoadFile(string f)
        {
            if (ContainsTab(f))
            {// tab page for file already existing
               FormDesignerCalls.SelectTab(((DockContainerItem)surfaceTabs.Items[GetTabIndex(f)]));
               
            }
            else
            {
                if (CodeProvider.IsValidFile(f))
                    LoadDocument(f, _workspace);
                else
                    MessageBoxEx.Show("No corresponding .Designer file found for " + f);
            }
        }
        private void LoadWorkspace()
        {
            _workspace = new Workspace();
         FormDesignerCalls.OnTabChanged += delegate
            {
                UpdateWorkspaceActiveDocument();
            };
            _workspace.ActiveDocumentChanged += OnActiveDocumentChanged;
            _workspace.Services.AddService(typeof(IToolboxService), (IToolboxService)toolbox);
            _toolboxFiller = new ToolboxFiller(_workspace.References, toolbox);
            AddErrorsTab();
            _workspace.Load();
        }
        public object Project;
        private void AddErrorsTab()
        {

            DockContainerItem tab = new DockContainerItem();
            ErrorListTabPage errors = new ErrorListTabPage(tab);
            tab.Name = "errorstab";
            tab.Text = "Errors";
            tab.GlobalItem = false;
            AddPanelX(ref errors, ref tab);
           // AddTab(tab, errors);

            _workspace.Services.AddService(typeof(IUIService), (IUIService)errors);
        }
        internal void LoadDocument(string file, Workspace workspace)
        {
            DockContainerItem tab = new DockContainerItem();
            PanelDockContainer tabctrl = new PanelDockContainer();
            AddPanel(ref tabctrl, ref tab, file);
            tab.Text = Path.GetFileNameWithoutExtension(file);
            tab.GlobalItem = false;
            tab.Name = "DESIGNERFORM_"+file; // the key
            tabctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabctrl.Location = new System.Drawing.Point(0, 30);
            tabctrl.Name = "tabctrl" + file;
            tabctrl.DockContainerItem = tab;
            tab.Control = tabctrl;


            // loads and associates the tab page with the document
            Document doc = workspace.CreateDocument(file, tab);
            doc.Services.AddService(typeof(IMenuCommandService), new MenuCommandService(doc.Services));
            doc.Load();
            doc.Services.AddService(typeof(UndoEngine), new UndoRedoEngine(doc.Services));
            if (doc.LoadSuccessful)
            {
                doc.Modified += OnDocumentModified;
                workspace.ActiveDocument = doc;
                ((Control)doc.DesignSurface.View).Dock = DockStyle.Fill;
                ((Control)doc.DesignSurface.View).BackColor = Color.FromArgb(255, 239, 239, 242);
                ((Control)doc.DesignSurface.View).ForeColor = Color.Black;
                Control c = FormDesignerCalls.ExchangeControls((Control)doc.DesignSurface.View,tab,tabctrl,Project);
                tab.Tag = c;
                tabctrl.Controls.Add(c);
                surfaceTabs.SuspendLayout();
              
                AddTab(tab, tabctrl);
                surfaceTabs.ResumeLayout();
                surfaceTabs.SelectedDockContainerItem = tab;
            }
            else
            {
                MessageBoxEx.Show("Unable to load!");
                tab.Dispose();
                workspace.CloseDocument(doc);
            }
        }

        private void OnDocumentModified(object sender, EventArgs args)
        {
            if (!surfaceTabs.SelectedDockContainerItem.Text.EndsWith(MODIFIED_MARKER))
              FormDesignerCalls.SetTabText( surfaceTabs.SelectedDockContainerItem, surfaceTabs.SelectedDockContainerItem.Text += MODIFIED_MARKER);
        }

        private void OnActiveDocumentChanged(object sender, ActiveDocumentChangedEventArgs args)
        {
            if (args.NewDocument != null)
                propertyGrid.Update(args.NewDocument.Services);
        }

        private void CloseDocument(Document doc)
        {
            doc.Modified -= OnDocumentModified;
            _workspace.CloseDocument(doc);
            propertyGrid.Clear();
        }
        private void UpdateWorkspaceActiveDocument()
        {
            if (!(surfaceTabs.SelectedDockContainerItem.Text == "Errors"))
                _workspace.ActiveDocument = _workspace.GetDocument(surfaceTabs.SelectedDockContainerItem);
            else if( surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
                _workspace.ActiveDocument = null;
        }

        public void Open(string file)
        {
            LoadFile(file);
        }
        public void Save()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null && _workspace.ActiveDocument.IsModified)
                {
                    _workspace.ActiveDocument.Save();

                    surfaceTabs.SelectedDockContainerItem.Text = Path.GetFileNameWithoutExtension(_workspace.ActiveDocument.FileName);
                }
            }
        }
        private void FormDesignerCalls_OnTabClose(object sender, EventArgs e)
        {
            DockContainerItem d = (DockContainerItem)sender;
            if (!(d.Text == "Errors") && d.Text.StartsWith("DESIGNERFORM_"))
                    if (_workspace.ActiveDocument != null)
                        CloseDocument(_workspace.ActiveDocument);
        
        }
        public void UpdateReferences(List<Assembly> refs)
        {
            foreach (Assembly refer in refs)
            {
                try
                {
                    if(!_workspace.References.Assemblies.Contains(refer))
                       _workspace.References.AddReference(refer);
                }
                catch
                {

                }
            }
        }
       
        public void Undo()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null)
                {
                    UndoRedoEngine undoEngine = _workspace.ActiveDocument.DesignSurface.GetService(typeof(UndoEngine)) as UndoRedoEngine;
                    if (undoEngine != null)
                        undoEngine.Undo();
                }
            }
        }
        public void Redo()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null)
                {
                    UndoRedoEngine undoEngine = _workspace.ActiveDocument.DesignSurface.GetService(typeof(UndoEngine)) as UndoRedoEngine;
                    if (undoEngine != null)
                        undoEngine.Redo();
                }
            }
        }
        public void Delete()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null)
                {
                    IMenuCommandService menuCommands = _workspace.ActiveDocument.DesignSurface.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
                    if (menuCommands != null)
                        menuCommands.FindCommand(StandardCommands.Delete).Invoke();
                }
            }
        }
        public void Copy()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null)
                {
                    IMenuCommandService menuCommands = _workspace.ActiveDocument.DesignSurface.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
                    if (menuCommands != null)
                        menuCommands.FindCommand(StandardCommands.Copy).Invoke();
                }
            }
        }
        public void Cut()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null)
                {
                    IMenuCommandService menuCommands = _workspace.ActiveDocument.DesignSurface.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
                    if (menuCommands != null)
                        menuCommands.FindCommand(StandardCommands.Cut).Invoke();
                }
            }
        }
        public void Paste()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null)
                {
                    IMenuCommandService menuCommands = _workspace.ActiveDocument.DesignSurface.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
                    if (menuCommands != null)
                        menuCommands.FindCommand(StandardCommands.Paste).Invoke();
                }
            }
        }
        public void SelectAll()
        {
            if (surfaceTabs.SelectedDockContainerItem.Name.StartsWith("DESIGNERFORM_"))
            {
                if (_workspace.ActiveDocument != null)
                {
                    IMenuCommandService menuCommands = _workspace.ActiveDocument.DesignSurface.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
                    if (menuCommands != null)
                        menuCommands.FindCommand(StandardCommands.SelectAll).Invoke();
                }
            }
        }
        public void New(string file,string classname,string nsname)
        {

                TemplateManager.WriteCode(TemplateManager.AvailableTemplates[0], file, CodeProvider.GetCodeBehindFileName(file),
                               nsname, classname);
                this.Open(file);
         

             
        }


    
    }
}
