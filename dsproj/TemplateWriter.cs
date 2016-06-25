using System;
using System.Collections.Generic;
using System.Text;

namespace alproj
{
  public static class Templates
    {
      public const string ConsoleCode = "include System;\r\n program {$namespace.$} \r\n { \r\n class ALEntry : ALCRT \r\n {  \r\n static sub Main(string[] args) \r\n { \r\n \r\n } \r\n } \r\n }";
      public const string HelloWorldCode = "include System;\r\n include ArsslenLang;\r\n  program HelloWorld \r\n { \r\n class ALEntry : ALCRT \r\n {  \r\n static sub Main(string[] args) \r\n { \r\n Console.WriteLine(\"Hello World Application!\"); \r\n } \r\n } \r\n }";
      public static string AssemblyInfoCode = alproj.Properties.Resources.AssemblyInfoCode;
      public static string EPCode = alproj.Properties.Resources.EntryPointCode;
      public static string EventCode = alproj.Properties.Resources.EventCode;
      public static string DesignerCode = alproj.Properties.Resources.DesignerCode;
      public static string FormCode = alproj.Properties.Resources.FormCode;

      public static string ClassCode = alproj.Properties.Resources.ClassCode;
      public static string ControlCode = alproj.Properties.Resources.ControlCode;
      public static string UserControlCode = alproj.Properties.Resources.UserControlCode;
      public static string ManifestCode = alproj.Properties.Resources.dsmanifest;
      public static string ConfigCode = alproj.Properties.Resources.dsconfig;
    }   
}
