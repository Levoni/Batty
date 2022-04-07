using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class BombTest
   {
      const int TEST_DIMENTIONS = 10;
      const int TEST_LOCATION = 100;
      const int TEST_LIVES = 3;
      const int TEST_SPEED = 2; 

      [TestMethod]
      public void Bomb_Constructed()
      {
         Bomb bomb1 = new Bomb(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Black, TEST_LIVES);
         Assert.IsNotNull(bomb1);
      }

      [TestMethod]
      public void Bomb_Update()
      {
         Bomb bomb1 = new Bomb(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Black, TEST_SPEED);
         bomb1.Update();
         Assert.IsTrue(bomb1.Y == TEST_DIMENTIONS + TEST_SPEED);
      }
   }
}
