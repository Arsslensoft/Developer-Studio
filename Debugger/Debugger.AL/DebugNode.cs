using DevComponents.AdvTree;
using MoreComplexPopup;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Debugger.AL
{
   public  class DebugNode : Node
    {
    
       public TreeNode DebugDataTree;
       public Node DebugParent;
     
      public DebugNode(TreeNode data, Node debugParent)
       {
           DebugParent = debugParent;
           DebugDataTree = data;
  
       }
      public void BindData(int c)
      {
          this.Text = DebugDataTree.Name + "         " + DebugDataTree.Value;
          this.Name = DebugParent.Name + ">" + c.ToString();

          // Add Empty node
          if(this.DebugDataTree.Value != null)
          if (this.DebugDataTree.Value.Length > 0)
          {

              Node nd = new Node();
              nd.Text = this.DebugDataTree.Value;

              nd.Name = "VALUE";
              this.Nodes.Add(nd);
          }
      }
       public void BindData()
       {
           this.Text =  DebugDataTree.Name + "  :   "+ DebugDataTree.Type;
         //  this.Name = DebugParent.Name + ">"+DebugParent.Nodes.Count.ToString();
           this.Name = DebugDataTree.Name;
           // Add Empty node
           if (this.DebugDataTree.Value.Length > 0)
           {

               Node nd = new Node();
               nd.Text = this.DebugDataTree.Value;

               nd.Name = "VALUE";
               this.Nodes.Add(nd);
           }
        
       }
       public void AddNodes(DebugTreeNode nod, ComplexPopup p)
       {

           foreach (TreeNode nd in this.DebugDataTree.GetChildren())
           {
               DebugNode nad = new DebugNode(nd, this);
               nad.BindData(nod.Nodes.Count);
               nod.AddNode(nad);


           }
       }
       public PopupControl.Popup CreatePopup(ref Point newloc)
       {
            ComplexPopup complexPopup = new ComplexPopup();
            newloc = new Point(Control.MousePosition.X+7, Control.MousePosition.Y+3 );

            //Point al = Control.MousePosition;
            //newloc = DebugParent.TreeControl.PointToClient(Control.MousePosition);
           PopupControl.Popup p = new PopupControl.Popup(complexPopup);
           
           p.Resizable = true;
          
           this.AddNodes(complexPopup.debugTreeNode1, complexPopup);
           return p;
       }
       protected override void OnExpandChanging(bool expanded, eTreeAction action)
       {

           if (DebugDataTree.HasChildren && expanded == true)
           {
               // Show Popup
               Point p = new Point(0,0);
              PopupControl.Popup pop = CreatePopup(ref p);
        
              pop.Show(this.TreeControl);
               base.OnExpandChanging(expanded, action);
           }
           else
               base.OnExpandChanging(expanded, eTreeAction.Collapse);

          
       }
    }


   public class DebugNodeNormal : Node
   {

       public TreeNode DebugDataTree;
       public Node DebugParent;

       public DebugNodeNormal(TreeNode data, Node debugParent)
       {
           DebugParent = debugParent;
           DebugDataTree = data;

       }
       public void BindData(int c)
       {
           this.Text = DebugDataTree.Name + "         " + DebugDataTree.Value;
           this.Name = DebugParent.Name + ">" + c.ToString();

           // Add Empty node
           if (this.DebugDataTree.Value != null)
               if (this.DebugDataTree.Value.Length > 0)
               {

                   Node nd = new Node();
                   nd.Text = this.DebugDataTree.Value;

                   nd.Name = "VALUE";
                   this.Nodes.Add(nd);
               }
       }
       public void BindData()
       {
           this.Text = DebugDataTree.Name + "  :   " + DebugDataTree.Type;
           //  this.Name = DebugParent.Name + ">"+DebugParent.Nodes.Count.ToString();
           this.Name = DebugDataTree.Name;
           if (this.DebugDataTree.Value != null)
               if (this.DebugDataTree.Value.Length > 0)
               {

                   Node nd = new Node();
                   nd.Text = this.DebugDataTree.Value;

                   nd.Name = "VALUE";
                   this.Nodes.Add(nd);
               }

       }
       public bool ContainsNode(string name, NodeCollection nds)
       {
           foreach (Node nd in nds)
           {
               if (nd.Name == name)
                   return true;
           }
           return false;
       }
       public void AddNodes(Node nod)
       {

           foreach (TreeNode nd in this.DebugDataTree.GetChildren())
           {
        
               DebugNodeNormal nad = new DebugNodeNormal(nd, this);
               nad.BindData();
           if(!ContainsNode(nad.Name, nod.Nodes))
               nod.Nodes.Add(nad);


           }
       }
       protected override void OnExpandChanging(bool expanded, eTreeAction action)
       {

           if (DebugDataTree.HasChildren && expanded == true)
           {
               AddNodes(this);
               base.OnExpandChanging(expanded, action);
           }
           else
               base.OnExpandChanging(expanded, eTreeAction.Collapse);


       }
   }
}
