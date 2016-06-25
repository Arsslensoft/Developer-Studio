using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace devstd.utils
{
    public delegate void AsyncTask(params object[] parameters);
   public static class TaskManager
    {
       public static List<DevStdTask> Tasks = new List<DevStdTask>(); 
      
       public static void ExecuteTask(DevStdTask task,params object[] param)
       {
           if (FindTaskByName(task.Name) == null)
           {
               task.Id = Tasks.Count;
               if (task.ShowTaskProgress)
               {
                   task.Execute(param);
                       TaskForm frm = new TaskForm(task);
                       frm.Show();
                   
                   task.OnComplete += task_OnComplete;
               }
               else if (task.ShowTaskProgressInSide)
               {

               }
               Tasks.Add(task);
           }


       }
       static void task_OnComplete(object sender,EventArgs e)
          {
              try
              {
                  Tasks.Remove((DevStdTask)sender);

              }
              catch
              {

              }
          }
       public static DevStdTask FindTaskById(int id)
       {
           foreach (DevStdTask task in Tasks)
               if (task.Id == id)
                   return task;

           return null;
       }
       public static DevStdTask FindTaskByName(string id)
       {
           foreach (DevStdTask task in Tasks)
               if (task.Name == id)
                   return task;

           return null;
       }
       public static void StopTaskByName(string name)
       {
          DevStdTask t = FindTaskByName(name);
          if (t != null)
              t.Abort();

       }
       public static void StopTaskById(int id)
       {
           DevStdTask t = FindTaskById(id);
           if (t != null)
               t.Abort();
       }

    }
}
