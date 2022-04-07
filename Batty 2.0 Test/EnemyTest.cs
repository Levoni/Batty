using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class EnemyTest
   {
      private const int X_POS = 0;
      private const int Y_POS = 1;
      private const int ENEMY_WIDTH = 1;
      private const int ENEMY_HEIGHT = 1;
      private readonly Color ENEMY_COLOR = Color.Green;
      private const int ENEMY_HEALTH = 1;

      private const int SCREEN_HEIGHT = 10;
      private const int SCREEN_WIDTH = 10;

      [TestMethod]
      public void Enemy_Constructor()
      {
         Enemy enemy = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);

         Assert.IsNotNull(enemy);
      }

      //-----------------------------------------------------------------------
      // Note: To run, comment out the following lines in Enemy.Update()
      //       int randomDirectionChange = random.Next(10);
      //
      //       if (randomDirectionChange == 1)
      //          direction = -direction;
      //-----------------------------------------------------------------------
      [TestMethod]
      public void Enemy_Update_MoveRightInBounds()
      {
         GameManager gameManager = new GameManager(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT, 0, 0, Color.AliceBlue, Color.AliceBlue);

         Enemy enemy = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         Assert.IsTrue(enemy.X == X_POS);

         
         enemy.Update();
         Assert.IsTrue(enemy.X == X_POS + Enemy.SPEED);
      }

      //-----------------------------------------------------------------------
      // Note: To run, comment out the following lines in Enemy.Update()
      //       int randomDirectionChange = random.Next(10);
      //
      //       if (randomDirectionChange == 1)
      //          direction = -direction;
      //-----------------------------------------------------------------------
      [TestMethod]
      public void Enemy_Update_MoveRightOutOfBounds()
      {
         GameManager gameManager = new GameManager(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT, 0, 0, Color.AliceBlue, Color.AliceBlue);

         Enemy enemy = new Enemy(SCREEN_WIDTH, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         Assert.IsTrue(enemy.X == SCREEN_WIDTH);

         enemy.Update();
         Assert.IsTrue(enemy.X == SCREEN_WIDTH - ENEMY_WIDTH);
      }

      //-----------------------------------------------------------------------
      // Note: To run, comment out the following lines in Enemy.Update()
      //       int randomDirectionChange = random.Next(10);
      //
      //       if (randomDirectionChange == 1)
      //          direction = -direction;
      //-----------------------------------------------------------------------
      [TestMethod]
      public void Enemy_Update_MoveLeftInBounds()
      {
         GameManager gameManager = new GameManager(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT, 0, 0, Color.AliceBlue, Color.AliceBlue);

         Enemy enemy = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         Assert.IsTrue(enemy.X == SCREEN_WIDTH - 1 - Enemy.SPEED);

         enemy.Update();
         Assert.IsTrue(enemy.X == 0);
      }

      //-----------------------------------------------------------------------
      // Note: To run, comment out the following lines in Enemy.Update()
      //       int randomDirectionChange = random.Next(10);
      //
      //       if (randomDirectionChange == 1)
      //          direction = -direction;
      //-----------------------------------------------------------------------
      [TestMethod]
      public void Enemy_Update_MoveLeftOutOfBounds()
      {
         GameManager gameManager = new GameManager(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT, 0, 0, Color.AliceBlue, Color.AliceBlue);

         Enemy enemy = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();

         enemy.Update();
         enemy.Update();
         enemy.Update();
         enemy.Update();
         Assert.IsTrue(enemy.X == 0);

         enemy.Update();
         Assert.IsTrue(enemy.X == Enemy.SPEED);
      }

      [TestMethod]
      public void Enemy_Equals_ObjectNull()
      {
         Enemy enemy = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);
         Assert.IsTrue(!enemy.Equals(null));
      }

      [TestMethod]
      public void Enemy_Equals_ObjectNotEnemy()
      {
         Enemy enemy = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);
         Block block = new Block(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT, ENEMY_COLOR, ENEMY_HEALTH);

         Assert.IsTrue(!enemy.Equals(block));
      }

      [TestMethod]
      public void Enemy_Equals_EnemiesNotEqual()
      {
         Enemy enemy1 = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);
         Enemy enemy2 = new Enemy(Y_POS, X_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);

         Assert.IsTrue(!enemy1.Equals(enemy2));
      }

      [TestMethod]
      public void Enemy_Equals_EnemiesEqual()
      {
         Enemy enemy1 = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);
         Enemy enemy2 = new Enemy(X_POS, Y_POS, ENEMY_WIDTH, ENEMY_HEIGHT,
            ENEMY_COLOR, ENEMY_HEALTH, null, null, null);

         Assert.IsTrue(enemy1.Equals(enemy2));
      }
   }
}
