using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.Editors;
using ICSharpCode.AvalonEdit;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.AL;

namespace ICSharpCode.CodeCompletion
{
    public partial class QuickClassBrowser : UserControl
    {
        public QuickClassBrowser()
        {
            InitializeComponent();
        }
        public void UpdateSize()
        {
            try
            {
                this.classessbox.Size = new Size((int)(this.Width/2),this.Height);
                this.Update();
            }
            catch
            {
            }
        }
        CodeTextEditor Ceditor;
        public void LoadSource(CodeTextEditor editor)
        {
            try
            {
                Ceditor = editor;
                 SourceCodeParsingResult sr  = null;
                 if (QuickClassBrowser.LatestCompletionSyntaxTree != null)
                     sr = new SourceCodeParsingResult(QuickClassBrowser.LatestCompletionSyntaxTree);
                 else
                     sr = SourceCodeParsingResult.Create(editor.Text);

                if (sr != null)
                    FillBoxes(sr);
            }
            catch
            {

            }
        }
        public static SyntaxTree LatestCompletionSyntaxTree;
        void FillMembers(SourceCodeParsedClass cl)
        {
            try
            {

            //    Ceditor.JumpTo(cl.Location.Line, cl.Location.Column);
                membersbox.Items.Clear();
                lineItem.Clear();
                int ind = 0;
                foreach (SourceCodeParsedMember mem in cl.AllMembers)
                {
                    ComboItem item = new ComboItem();
                    item.Text = mem.Name;
                    item.Tag = mem;
                    item.ForeColor = Color.Black;
                    item.ImageIndex = (int)SourceCodeParsingResult.GetIcon(mem.MemberType, mem.ItemModifiers);
                    membersbox.Items.Add(item);
                    lineItem.Add(mem.Location.Line, ind);
                    ind++;

                }
            }
            catch
            {

            }
              //  membersbox.SelectedIndex = 0;
            
        }
        public void FillBoxes(SourceCodeParsingResult result)
        {
            try
            {
               sel = true;
                classessbox.Items.Clear();
                lineItem.Clear();
               
                    foreach (SourceCodeParsedClass cla in result.Classes)
                    {
                        ComboItem item = new ComboItem();
                        item.Text = cla.Name;
                        item.Tag = cla;
                        item.ForeColor = Color.Black;
                        item.ImageIndex = (int)SourceCodeParsingResult.GetIcon("class", cla.ItemModifiers);
                        classessbox.Items.Add(item);
                        FillMembers(cla);
                      

                    
                    }
                    foreach (SourceCodeParsedClass cla in result.Structs)
                    {
                        ComboItem item = new ComboItem();
                        item.Text = cla.Name;
                        item.Tag = cla;
                        item.ForeColor = Color.Black;
                        item.ImageIndex = (int)SourceCodeParsingResult.GetIcon("struct", cla.ItemModifiers);
                        FillMembers(cla);
                        classessbox.Items.Add(item);
                
                  
                    }
                    foreach (SourceCodeParsedClass cla in result.Interfaces)
                    {
                        ComboItem item = new ComboItem();
                        item.Text = cla.Name;
                        item.Tag = cla;
                        item.ForeColor = Color.Black;
                        item.ImageIndex = (int)SourceCodeParsingResult.GetIcon("interface", cla.ItemModifiers);
                        FillMembers(cla);
                        classessbox.Items.Add(item);
                
                    }
                    foreach (SourceCodeParsedMember cla in result.Enums)
                    {
                        ComboItem item = new ComboItem();
                        item.Text = cla.Name;
                        item.Tag = cla;
                        item.ForeColor = Color.Black;
                        item.ImageIndex = (int)SourceCodeParsingResult.GetIcon("enum", cla.ItemModifiers);

                        classessbox.Items.Add(item);
                    }
                    foreach (SourceCodeParsedMember cla in result.Delegates)
                    {
                        ComboItem item = new ComboItem();
                        item.Text = cla.Name;
                        item.Tag = cla;
                        item.ForeColor = Color.Black;
                        item.ImageIndex = (int)SourceCodeParsingResult.GetIcon("delegate", cla.ItemModifiers);

                        classessbox.Items.Add(item);
                    }
                    classessbox.SelectedIndex = 0;



                
            }
            catch
            {

            }
            sel = false;
        }
        bool IsInside(TextLocation locs,TextLocation loce, int l, int c)
        {
            bool ins = (locs.Line <= l && loce.Line >= l);
            if (ins && l == locs.Line)
                return (locs.Column <= c);
            else  if (ins && l == loce.Line)
                return ( loce.Column >= c);
            else
                return ins;

        }

        public void UpdatePosition()
        {
            try
            {
             sel = true;
                SourceCodeParsedMember me = null;
                SourceCodeParsedClass cl=null;
                byte found = 0;
                // get current location
                int l = Ceditor.TextArea.Caret.Line;
                int c = Ceditor.TextArea.Caret.Column;
                // get current class
                foreach (ComboItem b in classessbox.Items)
                {
                    if (b.Tag is SourceCodeParsedClass)
                    {
                        cl = (SourceCodeParsedClass)b.Tag;
                        if (IsInside(cl.Location, cl.EndLocation, l, c))
                        {
                            found = 1;
                            classessbox.SelectedItem = b;
                            break;
                        }
                        else classessbox.Text = "";
                    }
                    else if (b.Tag is SourceCodeParsedMember)
                    {
                        me = (SourceCodeParsedMember)b.Tag;
                        if (IsInside(me.Location, me.EndLocation, l, c))
                        {
                            found = 2;
                            classessbox.SelectedItem = b;
                            break;
                        }
                        else classessbox.Text = "";
                       
                    }
                }



                if (found == 1)
                {

                   // class
                    bool f = false;
                   membersbox.Items.Clear();
                        lineItem.Clear();
                        int ind = 0;
                        foreach (SourceCodeParsedMember mem in cl.AllMembers)
                        {
                            ComboItem item = new ComboItem();
                            item.Text = mem.Name;
                            item.Tag = mem;
                            item.ForeColor = Color.Black;
                            item.ImageIndex = (int)SourceCodeParsingResult.GetIcon(mem.MemberType, mem.ItemModifiers);
                            membersbox.Items.Add(item);
                            lineItem.Add(mem.Location.Line, ind);
                            ind++;

                        }
                        membersbox.SelectedIndex = 0;
                        // find methods
                        foreach (SourceCodeParsedMember mem in cl.AllMembers)
                        {
                            if (mem.MemberType == "method")
                            {
                                if (IsInside(mem.Location,mem.EndLocation,l,c))
                                {
                                    f = true;
                                    me = mem;
                                   
                                    break;
                                }
                          
                            }
                            else
                            {
                                if (mem.Location.Line == l)
                                {
                                    f = true;
                                    me = mem;
                                    break;
                                }
                            }
                        }
                        if (f)
                        {
                            membersbox.SelectedIndex = lineItem[me.Location.Line];
                        }
                        else membersbox.Text = "";

                    
                 
                    
                }
                else if (found == 2)
                {
                    // method
                    membersbox.SelectedIndex = lineItem[me.Location.Line];
                }
            }
            catch
            {

            }
        sel = false;
        }
        Dictionary<int, int> lineItem = new Dictionary<int, int>();
        bool sel = false;
        private void classessbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sel)
            {
                classessbox.ForeColor = Color.Black;
                membersbox.ForeColor = Color.Black;
                try
                {

                    ComboItem b = (ComboItem)classessbox.SelectedItem;
                    classpic.Image = classessbox.Images.Images[b.ImageIndex];
                    if (b.Tag is SourceCodeParsedMember)
                    {
                        SourceCodeParsedMember cl = (SourceCodeParsedMember)b.Tag;
                        Ceditor.JumpTo(cl.Location.Line, cl.Location.Column);
                    }
                    else
                    {
                        SourceCodeParsedClass cl = (SourceCodeParsedClass)b.Tag;
                        Ceditor.JumpTo(cl.Location.Line, cl.Location.Column);
                    }
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    ComboItem b = (ComboItem)classessbox.SelectedItem;
                    classpic.Image = classessbox.Images.Images[b.ImageIndex];
                }
                catch
                {

                }
            }
        }
        private void membersbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sel)
            {
                classessbox.ForeColor = Color.Black;
                membersbox.ForeColor = Color.Black;
                try
                {

                    ComboItem b = (ComboItem)membersbox.SelectedItem;
                    mempic.Image = membersbox.Images.Images[b.ImageIndex];
                    if (b.Tag is SourceCodeParsedClass)
                    {
                        SourceCodeParsedClass cl = (SourceCodeParsedClass)b.Tag;
                        Ceditor.JumpTo(cl.Location.Line, cl.Location.Column);
                    }
                    else if (b.Tag is SourceCodeParsedMember)
                    {
                        SourceCodeParsedMember cl = (SourceCodeParsedMember)b.Tag;
                        Ceditor.JumpTo(cl.Location.Line, cl.Location.Column);
                    }
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    ComboItem b = (ComboItem)membersbox.SelectedItem;
                    mempic.Image = membersbox.Images.Images[b.ImageIndex];
                }
                catch
                {

                }
            }
        }

        private void QuickClassBrowser_Resize(object sender, EventArgs e)
        {
            try
            {

                classessbox.Width = (int)(this.Size.Width / 2) - 40;
            }
            catch
            {

            }
        }
    }
}
