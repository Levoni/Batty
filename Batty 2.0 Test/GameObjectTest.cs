using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class GameObjectTest
   {
      [TestMethod]
      public void GameObject_Constructed()
      {
         GameObject oOne = new GameObject(10, 10, 100, 100, Color.Black);
         Assert.IsNotNull(oOne);
      }
   }
}
