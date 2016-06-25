using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace devstd.utils
{
    public class TaskParameters
    {
        public object[] Parameters { get; set; }
    }
   public class DevStdTask
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }
       public byte ProgressValue { get; set; }
       public bool ShowTaskProgress { get; set; }
       public bool ShowTaskProgressInSide { get; set; }
       public Thread Task { get; set; }
       public event EventHandler OnComplete;
       public event AsyncTask Executed;
       public bool Done { get; set; }
     
       public void Execute(params object[] parameter)
       {
           Done = false;
           TaskParameters p = new TaskParameters();
           p.Parameters = parameter;
  

           Task = new Thread(new ParameterizedThreadStart(Exec));
           Task.Priority = ThreadPriority.Lowest;
           Task.Start(p);
          
       }
       public void Abort()
       {
           Task.Abort();
           if (OnComplete != null)
               OnComplete(this, EventArgs.Empty);
           Done = true;
       }
       public void DoWork(TaskParameters p)
       {
           if(Executed != null)
           Executed(p.Parameters);
       }
       void Exec(object p)
       {
           TaskParameters tp = (TaskParameters)p;
          
           DoWork(tp);
           if (OnComplete != null)
               OnComplete(this, EventArgs.Empty);
           Done = true;
       }

    }
}
