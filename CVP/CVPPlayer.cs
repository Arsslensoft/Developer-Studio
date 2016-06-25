using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CVP
{
    public delegate void PlayerEventHandler(CVPINS opcode,byte[] data,int line, int column);
    public delegate void AsyncPlayer();

   public class CVPPlayer
    {
       public event PlayerEventHandler OnCVPInstructionArrived;
       public event EventHandler OnCVPComplete;
       public CVPReader Reader { get; set; }
       public bool IsPlaying { get; set; }
       public void Load(string file)
       {
           Reader = new CVPReader(file);
           IsPlaying = false;   
       }

       public void PlaySync()
       {
           IsPlaying = true;

           while (!Reader.EndOfFile() &&  IsPlaying)
           {
               CVPInstruction ins = Reader.ReadCurrentInstruction();
               if (ins.Instruction == CVPINS.WAIT)
                   Thread.Sleep(BitConverter.ToInt32(ins.Data,0));
               else if(OnCVPInstructionArrived != null)
                 OnCVPInstructionArrived(ins.Instruction, ins.Data,ins.Line,ins.Column);
           }
           Reader.Finalize();
           IsPlaying = false;
           if (OnCVPComplete != null)
               OnCVPComplete(null, null);

       }
       public void Play()
       {
           AsyncPlayer play = new AsyncPlayer(PlaySync);
           play.BeginInvoke(null, null);
       }

       public void Stop()
       {
           IsPlaying = false;
       }

    }
}
