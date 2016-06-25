using System;
using System.Collections.Generic;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace devstd.utils
{
    public static class Speech
    {
        public static SpeechSynthesizer SPS;
      public  static bool initialized = false;
         public static void Init()
        {
            try
            {
           
                SPS = new SpeechSynthesizer();
                // Load SPS
                SPS.Rate = 1;
                SPS.Volume = 100;
                initialized = true;
                //if(SettingsManager.GetBool("STARTUPMSG"))
                //    SPS.SpeakAsync("Welcome to Developer Studio 2015 Integrated Development Environment.");

              
            }
            catch
            {
                
            }
        }
        public static void Speak(string text)
        {
            try
            {
                if (text != null && initialized)
                    SPS.SpeakAsync(text);
                else if(initialized)              
                    SPS.SpeakAsync("No text to say");
 
            }
            catch 
            {
               
            }
            finally
            {

            }
        }
        public static void SpeakSync(string text)
        {
            try
            {
                if (text != null && initialized)
                    SPS.Speak(text);
                else if (initialized)
                    SPS.Speak("No text to say");

            }
            catch
            {

            }
            finally
            {

            }
        }
       
    }
}
