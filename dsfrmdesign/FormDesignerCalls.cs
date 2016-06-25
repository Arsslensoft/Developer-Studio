using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace alfrmdesign
{
    public delegate int GetFormTabIndex(string file);
    public delegate Bar GetTabs();
    public delegate void BarSelectTab(DockContainerItem tab, PanelDockContainer dock, bool add);
    public delegate void BarEditTab(DockContainerItem tab, string text );
    public delegate Control ExchangeControlHandler(Control c, DockContainerItem tab, PanelDockContainer pdc, object proj);
   public class FormDesignerCalls
    {
       // Events To Hook
       public static event EventHandler OnTabChanged;
       public static event EventHandler OnTabClose;
       // Events to be hooked
       public static event GetTabs OnBarNeeded;
       public static event BarSelectTab OnBarSelectNeeded;
       public static event GetFormTabIndex OnFormIndexNeeded;
       public static event BarEditTab OnTabChangeText;
       public static event ExchangeControlHandler OnExchangeControl;

       public static void TabChanged()
       {
           OnTabChanged(null, EventArgs.Empty);
       }
       public static void TabClose(DockContainerItem tab)
       {
           OnTabClose(tab, EventArgs.Empty);
       }
       public static bool ContainsTab(string file)
       {
           return (OnFormIndexNeeded(file) != -1);
       }
       public static int GetTabIndex(string file)
       {
           return OnFormIndexNeeded(file);
       }
       public static Bar GetBar()
       {
           return OnBarNeeded();
       }
       public static void Modify(DockContainerItem tab, PanelDockContainer dock, bool add)
       {
           OnBarSelectNeeded(tab,dock, add);
       }
       public static void SelectTab(DockContainerItem tab)
       {
           OnBarSelectNeeded(tab, null, false);
       }
       public static void SetTabText(DockContainerItem tab, string text)
       {
           OnTabChangeText(tab, text);
       }
       public static Control ExchangeControls(Control c, DockContainerItem tab,PanelDockContainer pdc, object proj)
       {
         return  OnExchangeControl(c,tab,pdc,proj);
       }
    }
}
