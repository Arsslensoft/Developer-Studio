using Debugger;
using Debugger.MetaData;
using DevComponents.AdvTree;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.AL.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;
using MoreComplexPopup;
using PopupControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debugger.AL
{
   
    public class WindowsDebugger
    {
        public static Thread EvalThread;
        public static Thread CurrentThread;
        public static Debugger.StackFrame CurrentStackFrame;
        public static Debugger.Process CurrentProcess;
       internal static List<string> Names = new List<string>();
    }
    
   public class DebugTreeNode : AdvTree
    {
      //public Popup Callercomplex;
      //public Point CallerPosition;
      //public Node DebugNode;

      // public Popup complex;
      // public ComplexPopup complexPopup;
       // public void AddNodeX(TreeNode node, Node snd)
       // {
       //     if (!WindowsDebugger.Names.Contains(snd.Name + "DEFAULTTREENODEBACK"))
       //     {
       //         WindowsDebugger.Names.Add(snd.Name + "DEFAULTTREENODEBACK");
       //         Node nad = new Node();
       //         nad.Text = "Back";
       //         nad.Name = snd.Name + "DEFAULTTREENODEBACK";
       //         snd.Nodes.Add(nad);
       //         nad.NodeClick += nad_NodeClick;
       //         snd.Nodes.Add(nad);
       //     }
       //     if (node != null)
       //         if (node.HasChildren)
       //         {
       //             foreach (TreeNode nd in node.GetChildren())
       //             {
       //                 Node nad = new Node();
       //                 nad.Text = nd.Name + " " + nd.Type;
       //                 nad.Tag = nd;
       //                 nad.Name = snd.Name + ">" + nad.Text; ;
       //                 if (!WindowsDebugger.Names.Contains(nad.Name))
       //                 {
       //                     WindowsDebugger.Names.Add(nad.Name);
       //                     snd.Nodes.Add(nad);
       //                 }


       //             }
       //         }
 
       //     this.Nodes.Add(snd);
       // }
       // void nad_NodeClick(object sender, EventArgs e)
       // {
       //     Callercomplex.Hide();
       // }
       //public void AddNode(TreeNode node, ref Node snd)
       //{
       //    if(node != null)
       //    if(node.HasChildren)
       //    {
       //        foreach (TreeNode nd in node.GetChildren())
       //        {
       //            Node nad = new Node();
       //            nad.Text = nd.Name + " " + nd.Type;
       //            nad.Tag = nd;
       //            nad.Name = snd.Name + ">" + nad.Text; ;
       //            if (!WindowsDebugger.Names.Contains(nad.Name))
       //            {
       //               WindowsDebugger. Names.Add(nad.Name);
       //                snd.Nodes.Add(nad);
       //            }


       //        }
       //    }
       //}
       // TODO TREE LISTENER (NO BACK BUTTON)
       protected override void OnBeforeNodeSelect(AdvTreeNodeCancelEventArgs args)
       {
           Node n = args.Node;
           //if (!n.Name.Contains("DEFAULTTREENODEBACK"))
           //{
           //    Debugger.AddIn.TreeModel.TreeNode tn = (Debugger.AddIn.TreeModel.TreeNode)n.Tag;

           //    complex = new Popup(complexPopup = new ComplexPopup());
           //    complex.Resizable = true;
           //    complexPopup.debugTreeNode1.Callercomplex = complex;
           //    complexPopup.debugTreeNode1.AddNodeX(tn, n);
           //    complexPopup.Height = (complexPopup.Height) * complexPopup.debugTreeNode1.Nodes.Count;
           //    complex.Height = (complexPopup.Height ) * complexPopup.debugTreeNode1.Nodes.Count; ;

           //    int x = n.Bounds.X + n.Bounds.Width;
           //    int y = n.Bounds.Y +n.Bounds.Height;
           //    Point point = new Point(x, y);
           //    Point absPoint = complexPopup.debugTreeNode1.PointToScreen(point);
           //    complex.Show(absPoint);
           //}
           base.OnBeforeNodeSelect(args);
       }
       public bool ContainsNode(string name)
       {
           foreach (Node nd in this.Nodes)
           {
               if (nd.Name == name)
                   return true;
           }
           return false;
       }
       public void AddNode(DebugNode nd)
       {
           if (!ContainsNode(nd.Name))
               this.Nodes.Add(nd);
       }
    }
}
