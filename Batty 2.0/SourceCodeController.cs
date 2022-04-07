using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

public static class SourceCodeController
{
   private const string PATH = "../../../Batty 2.0/";
   private static List<string> theCodes;
   private static int currentCode;

   /// <summary>
   /// Get the code contained in the next class
   /// </summary>
   /// <returns>The Code
   /// (of the next class)</returns>
   public static string GetNextCode()
   {
      if (currentCode < theCodes.Count)
         return theCodes[++currentCode];
      return "";
   }

   /// <summary>
   /// Get the code from the current class
   /// </summary>
   /// <returns>The Code
   /// (of the current class)</returns>
   public static string GetCurrentCode()
   {
      if (currentCode < theCodes.Count)
         return theCodes[currentCode];
      return "";
   }

   /// <summary>
   /// Gets the code from all the classes
   /// </summary>
   public static void GenerateTheCode()
   {
      theCodes = new List<string>();
      string theCode = "";

      DirectoryInfo d = new DirectoryInfo(PATH);
      FileInfo[] Files = d.GetFiles("*.cs");
      foreach (FileInfo file in Files)
      {
         if (!file.Name.Contains("BattyForm"))
         {
            StreamReader codeReader = new StreamReader(file.OpenRead());
            theCode += file.Name + "\r\n";
            theCode += codeReader.ReadToEnd();
            codeReader.Close();
            theCodes.Add(theCode);
            theCode = "";
         }
      }
   }
}
