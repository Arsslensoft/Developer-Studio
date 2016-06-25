using DevComponents.DotNetBar;
using ICSharpCode.AvalonEdit;
using ICSharpCode.CodeCompletion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms.Integration;


namespace devstd.host
{
    public class TabHost : System.Windows.Forms.UserControl
    {
        public EditorType ControlType { get; set; }
        public alproj.ALProject Project { get; set; }
        public string FileName { get; set; }
        public DockContainerItem Tab { get; set; }
        public PanelDockContainer ControlTab { get; set; }
        // Editors
        public ICSharpCode.CodeCompletion.CodeTextEditor TextEditor { get; set; }
        public alproj.ProjectProperties ProjectPropertiesEditor { get; set; }


        // Methods
        public void AddCodeEditor(CodeTextEditor editor, DockContainerItem tab, PanelDockContainer ctrl)
        {
            ElementHost h = new ElementHost();
            h.Dock = System.Windows.Forms.DockStyle.Fill;
            h.Child = editor;
            this.Controls.Add(h);
            this.FileName = editor.FileName;
            ControlType = EditorType.CodeEditor;
            ControlTab = ctrl;
            Tab = tab;
            TextEditor = editor;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
        }
        public void AddCodeEditorProject(CodeTextEditor editor, DockContainerItem tab, PanelDockContainer ctrl, alproj.ALProject project)
        {
            ElementHost h = new ElementHost();
            h.Dock = System.Windows.Forms.DockStyle.Fill;
            h.Child = editor;
            this.Controls.Add(h);
            this.FileName = editor.FileName;
            ControlType = EditorType.CodeEditorProject;
            Project = project;
            ControlTab = ctrl;
            Tab = tab;
            TextEditor = editor;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
        }
        public void AddFormDesigner(System.Windows.Forms.Control designer, DockContainerItem tab, PanelDockContainer ctrl, alfrmdesign.FormDesignerControl control, alproj.ALProject proj)
        {
            designer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(designer);
            this.FileName = Path.GetDirectoryName(proj.FileName) + @"\forms\" + Path.GetFileNameWithoutExtension(control.CodeBehindFileName) + ".al";
            Project = proj;
            ControlType = EditorType.FormDesigner;
            ControlTab = ctrl;
            Tab = tab;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
        }
        public void AddProperty(alproj.ProjectProperties ctrl, DockContainerItem tab, PanelDockContainer contrl, alproj.ALProject proj)
        {
            this.Controls.Add(ctrl);
            this.FileName = proj.FileName;
            ControlType = EditorType.PropertyControl;
            ControlTab = contrl;
            Tab = tab;
            ProjectPropertiesEditor = ctrl;
            Project = proj;
            ctrl.Init(proj);
            ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
     
        }

    }
}
