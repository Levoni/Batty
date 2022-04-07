using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class SourceCodeControllerTest
   {

      [TestMethod]
      public void GenerateCodeNoCrashTest()
      {
         SourceCodeController.GenerateTheCode();
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void GetCurrentCodeNotEmptyTest()
      {
         SourceCodeController.GenerateTheCode();
         Assert.IsTrue(SourceCodeController.GetCurrentCode() != null && SourceCodeController.GetCurrentCode() != "");
      }

      [TestMethod]
      public void GetNextCodeNotEmptyTest()
      {
         SourceCodeController.GenerateTheCode();
         string s = SourceCodeController.GetNextCode();
         Assert.IsTrue(s != null && s != "");
      }

      [TestMethod]
      public void GetNextCodeNotSame()
      {
         SourceCodeController.GenerateTheCode();
         string s = SourceCodeController.GetNextCode();
         Assert.IsTrue(s != SourceCodeController.GetNextCode());
      }

      [TestMethod]
      public void GetCurrentCodeSame()
      {
         SourceCodeController.GenerateTheCode();
         string s = SourceCodeController.GetCurrentCode();
         Assert.IsTrue(s == SourceCodeController.GetCurrentCode());
      }

      [TestMethod]
      public void GetNextCodeNotSameAsCurrentCode()
      {
         SourceCodeController.GenerateTheCode();
         string s = SourceCodeController.GetCurrentCode();
         Assert.IsTrue(s != SourceCodeController.GetNextCode());
      }
   }


}
