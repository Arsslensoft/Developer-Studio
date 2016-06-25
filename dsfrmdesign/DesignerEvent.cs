using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace alfrmdesign
{
    public delegate bool GoToLineDelegate(int line);
    public delegate bool DSEventBinderDelegate(IComponent component, EventDescriptor e, string methodName);
    public delegate bool UpdateRessourceDelegate(string member, object data);
   public class DesignerEvent
    {
       public static event DSEventBinderDelegate ShowCodeEvent;
       public static event GoToLineDelegate OnGoToLine;
       public static event UpdateRessourceDelegate OnRessourceAdded;

       internal static bool ShowCode(IComponent component, EventDescriptor e, string methodName)
       {
           if (ShowCodeEvent != null)
              return ShowCodeEvent(component, e, methodName);
           else
               return false;
       }
       internal static bool JumpTo(int line)
       {
           if (OnGoToLine != null)
               return OnGoToLine(line);
           else
               return false;
       }
       internal static bool UpdateRessource(string member, object data)
       {
           if (OnRessourceAdded != null)
               return OnRessourceAdded( member,  data);
           else
               return false;
       }
    }
}
