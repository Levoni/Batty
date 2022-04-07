using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class BarTest
   {
      const int TEST_DIMENTIONS = 10;
      const int TEST_LOCATION = 100;
      const int TEST_LIVES = 3;
      const int TEST_SPEED = 2;

      [TestMethod]
      public void Bar_Constructed()
      {
         Bar bar1 = new Bar(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Yellow, TEST_LIVES,
            TEST_SPEED);
         Assert.IsNotNull(bar1);
      }

      [TestMethod]
      public void Bar_Handle_Collision_Bomb()
      {
         Bar bar1 = new Bar(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Yellow, TEST_LIVES,
            TEST_SPEED);
         Bomb bomb1 = new Bomb(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Black, TEST_LIVES);
         bar1.HandleCollisions(bomb1.type);
         Assert.IsTrue(bar1.Lives == TEST_SPEED);
      }

      [TestMethod]
      public void Bar_Handle_Collision_Other()
      {
         Bar bar1 = new Bar(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Yellow, TEST_LIVES,
            TEST_SPEED);
         Ball ball1 = new Ball(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Black, 1);
         bar1.HandleCollisions(ball1.type);
         Assert.IsTrue(bar1.Lives == TEST_LIVES);
      }

      [TestMethod]
      public void Bar_Reset_DefaultPosition()
      {
         Bar barTest = new Bar(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Yellow, TEST_LIVES,
            TEST_SPEED);

         barTest.X += TEST_SPEED;
         barTest.Y += TEST_SPEED;

         barTest.Reset();
         Assert.AreEqual(barTest.X, TEST_DIMENTIONS);
         Assert.AreEqual(barTest.Y, TEST_DIMENTIONS);
      }

      [TestMethod]
      public void Bar_DefaultX_TestDimentions()
      {
         Bar barTest = new Bar(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Yellow, TEST_LIVES,
            TEST_SPEED);

         Assert.AreEqual(barTest.DefaultX, TEST_DIMENTIONS);
      }

      [TestMethod]
      public void Bar_Speed_TestSpeed()
      {
         Bar barTest = new Bar(TEST_DIMENTIONS, TEST_DIMENTIONS,
            TEST_LOCATION, TEST_LOCATION, Color.Yellow, TEST_LIVES,
            TEST_SPEED);

         Assert.AreEqual(barTest.Speed, TEST_SPEED);
      }

      [TestMethod]
      public void Bar_SpeedMultiplier_Modified()
      {
         const double SIZE_MODIFIER = 1.5;
         Bar barTest = new Bar(TEST_LOCATION, TEST_LOCATION,
            TEST_DIMENTIONS, TEST_DIMENTIONS, Color.Yellow, TEST_LIVES,
            TEST_SPEED);

         int initWidth = TEST_DIMENTIONS;
         barTest.SizeModifier = SIZE_MODIFIER;

         Assert.AreEqual((double)barTest.Width, initWidth * SIZE_MODIFIER);
      }
   }
}
