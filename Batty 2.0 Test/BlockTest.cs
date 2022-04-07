using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class BlockTest
   {
      private const int TEST_HEALTH = 1;
      [TestMethod]
      public void Block_Constructor()
      {
         Block block = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);

         Assert.IsNotNull(block);
      }

      [TestMethod]
      public void Block_LoseAllHealth()
      {
         Block block = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);

         --block.health;
         Assert.AreEqual(0, block.health);
      }

      [TestMethod]
      public void Block_PointValueMultiplier()
      {
         Block block = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);

         Assert.AreEqual(Block.POINT_MULTIPLIER * TEST_HEALTH, block.pointValue);
      }

      [TestMethod]
      public void Block_Equals_ObjectNull()
      {
         Block block = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);
         Assert.IsTrue(!block.Equals(null));
      }

      [TestMethod]
      public void Block_Equals_ObjectNotBlock()
      {
         Enemy enemy = new Enemy(1, 1, 1, 1,
            System.Drawing.Color.AliceBlue, TEST_HEALTH, null, null, null);
         Block block = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);

         Assert.IsTrue(!block.Equals(enemy));
      }

      [TestMethod]
      public void Block_Equals_BlocksNotEqual()
      {
         Block block1 = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);
         Block block2 = new Block(2, 2, 2, 2, System.Drawing.Color.AliceBlue, TEST_HEALTH);

         Assert.IsTrue(!block1.Equals(block2));
      }

      [TestMethod]
      public void Block_Equals_BlocksEqual()
      {
         Block block1 = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);
         Block block2 = new Block(1, 1, 1, 1, System.Drawing.Color.AliceBlue, TEST_HEALTH);

         Assert.IsTrue(block1.Equals(block2));
      }
   }
}
