using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace Batty_2._0_Test
{
   [TestClass]
   public class LevelTest
   {
      private const int LEVEL_NUM = -1;
      private const int LEVEL_NUM_FILE_IO = -2;
      private const int LEVEL_ROW_HEIGHT = 8;
      private const int LEVEL_COLUMN_WIDTH = 10;
      private const int LEVEL_OFFSET_X = 20;
      private const int LEVEL_OFFSET_Y = 20;

      private const int X_POS_1 = 0;
      private const int X_POS_2 = 1;
      private const int X_POS_3 = 2;
      private const int X_POS_4 = 3;
      private const int X_POS_5 = 4;
      private const int X_POS_6 = 5;
      private const int X_POS_7 = 6;
      private const int X_POS_8 = 7;
      private const int X_POS_9 = 8;
      private const int X_POS_10 = 9;
      private const int Y_POS_1 = 0;
      private const int Y_POS_2 = 1;
      private const int Y_POS_3 = 2;
      private const int Y_POS_4 = 3;
      private const int Y_POS_5 = 4;
      private const int Y_POS_6 = 5;
      private const int Y_POS_7 = 6;
      private const int Y_POS_8 = 7;
      private const int Y_POS_9 = 8;
      private const int Y_POS_10 = 9;

      private const int BLOCK_HEALTH = 1;

      private const int ENEMY_X_POS_1 = 20;
      private const int ENEMY_X_POS_2 = 30;
      private const int ENEMY_X_POS_3 = 40;
      private const int ENEMY_X_POS_4 = 50;
      private const int ENEMY_X_POS_5 = 60;
      private const int ENEMY_X_POS_6 = 70;
      private const int ENEMY_X_POS_7 = 80;
      private const int ENEMY_X_POS_8 = 90;
      private const int ENEMY_X_POS_9 = 100;
      private const int ENEMY_X_POS_10 = 110;
      private const int ENEMY_Y_POS_1 = 0;
      private const int ENEMY_Y_POS_2 = 0;
      private const int ENEMY_Y_POS_3 = 0;
      private const int ENEMY_Y_POS_4 = 0;
      private const int ENEMY_Y_POS_5 = 0;
      private const int ENEMY_Y_POS_6 = 0;
      private const int ENEMY_Y_POS_7 = 0;
      private const int ENEMY_Y_POS_8 = 0;
      private const int ENEMY_Y_POS_9 = 0;
      private const int ENEMY_Y_POS_10 = 0;

      private readonly Color COLOR_1 = Color.Brown;
      private readonly Color COLOR_2 = Color.Blue;
      private readonly Color COLOR_3 = Color.Red;
      private readonly Color COLOR_4 = Color.Green;
      private readonly Color COLOR_5 = Color.Yellow;

      private const int ROW_ADD_SUCCESS = 0;
      private const int COLUMN_ADD_SUCCESS = 0;

      private const int ROW_ADD_FAIL = GameManager.NUM_ROWS;          //Index Level.ROWS, Row Level.ROWS + 1
      private const int COLUMN_ADD_FAIL = GameManager.NUM_COLS;    //Index Level.COLUMNS, Column Level.COLUMNS + 1

      private const int MAX_ENEMY_COUNT = 10;

      [TestMethod]
      public void Level_Constructor()
      {
         Level l = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT, LEVEL_COLUMN_WIDTH,
            LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Assert.IsNotNull(l);
      }

      [TestMethod]
      public void Level_AddBlock_Success()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Block block = new Block(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH);

         bool success = level.AddBlock(block, ROW_ADD_SUCCESS,
            COLUMN_ADD_SUCCESS);
         Assert.IsTrue(success);
      }

      [TestMethod]
      public void Level_AddBlock_Fail()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Block block = new Block(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH);

         bool fail = level.AddBlock(block, ROW_ADD_FAIL, COLUMN_ADD_FAIL);
         Assert.IsFalse(fail);
      }

      [TestMethod]
      public void Level_GetBlock()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Block block = new Block(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH);

         level.AddBlock(block, ROW_ADD_SUCCESS, COLUMN_ADD_SUCCESS);
         Assert.AreEqual(block, level.GetBlock(ROW_ADD_SUCCESS,
            COLUMN_ADD_SUCCESS));
      }

      [TestMethod]
      public void Level_RemoveBlock_RowColumnSuccess()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Block block = new Block(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH);

         level.AddBlock(block, ROW_ADD_SUCCESS, COLUMN_ADD_SUCCESS);
         level.RemoveBlock(ROW_ADD_SUCCESS, COLUMN_ADD_SUCCESS);

         Assert.IsNull(level.GetBlock(ROW_ADD_SUCCESS, COLUMN_ADD_SUCCESS));
      }

      [TestMethod]
      public void Level_RemoveBlock_RowColumnFail()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Assert.IsFalse(level.RemoveBlock(ROW_ADD_FAIL, COLUMN_ADD_FAIL));
      }

      [TestMethod]
      public void Level_RemoveBlock_Block()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Block block = new Block(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH);

         level.AddBlock(block, ROW_ADD_SUCCESS, COLUMN_ADD_SUCCESS);

         level.RemoveBlock(block);
         Assert.IsNull(level.GetBlock(ROW_ADD_SUCCESS, COLUMN_ADD_SUCCESS));
      }

      [TestMethod]
      public void Level_RemoveBlock_Fail()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Block block = new Block(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH);

         Assert.IsFalse(level.RemoveBlock(block));
      }

      [TestMethod]
      public void Level_AddEnemy_Success()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Enemy enemy = new Enemy(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH, null, null, null);

         Assert.IsTrue(level.AddEnemy(enemy, 0));
      }

      [TestMethod]
      public void Level_AddEnemy_Fail()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Enemy enemy = new Enemy(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH, null, null, null);

         for (int i = 0; i < MAX_ENEMY_COUNT; i++)
            level.AddEnemy(enemy, 0);

         Assert.IsFalse(level.AddEnemy(enemy, 0));
      }

      [TestMethod]
      public void Level_RemoveEnemy_Success()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Enemy enemy = new Enemy(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH, null, null, null);

         level.AddEnemy(enemy, 0);
         level.RemoveEnemy(enemy);

         Assert.IsFalse(level.GetEnemies().Contains(enemy));
      }

      [TestMethod]
      public void Level_RemoveEnemy_False()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Enemy enemy1 = new Enemy(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH, null, null, null);
         Enemy enemy2 = new Enemy(Y_POS_2, X_POS_1, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_2, BLOCK_HEALTH, null, null, null);

         level.AddEnemy(enemy1, 0);
         List<Enemy> enemyList = level.GetEnemies();

         Assert.IsFalse(level.RemoveEnemy(enemy2));
      }

      [TestMethod]
      public void Level_UpdateEnemies()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y);

         Enemy enemy = new Enemy(X_POS_1, Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH, null, null, null);

         level.AddEnemy(enemy, 0);
         level.UpdateEnemies();

         Assert.IsTrue(true);
      }

      [TestMethod]
      public void Level_FileIO()
      {
         Level level1 = new Level(LEVEL_NUM_FILE_IO, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Level level2;

         Block block1 = new Block(10 * X_POS_1 + LEVEL_OFFSET_X, 6 * Y_POS_10 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_1, BLOCK_HEALTH);
         Block block2 = new Block(10 * X_POS_2 + LEVEL_OFFSET_X, 6 * Y_POS_9 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_2, BLOCK_HEALTH);
         Block block3 = new Block(10 * X_POS_3 + LEVEL_OFFSET_X, 6 * Y_POS_8 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_3, BLOCK_HEALTH);
         Block block4 = new Block(10 * X_POS_4 + LEVEL_OFFSET_X, 6 * Y_POS_7 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_4, BLOCK_HEALTH);
         Block block5 = new Block(10 * X_POS_5 + LEVEL_OFFSET_X, 6 * Y_POS_6 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_5, BLOCK_HEALTH);
         Block block6 = new Block(10 * X_POS_6 + LEVEL_OFFSET_X, 6 * Y_POS_5 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_1, BLOCK_HEALTH);
         Block block7 = new Block(10 * X_POS_7 + LEVEL_OFFSET_X, 6 * Y_POS_4 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_2, BLOCK_HEALTH);
         Block block8 = new Block(10 * X_POS_8 + LEVEL_OFFSET_X, 6 * Y_POS_3 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_3, BLOCK_HEALTH);
         Block block9 = new Block(10 * X_POS_9 + LEVEL_OFFSET_X, 6 * Y_POS_2 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_4, BLOCK_HEALTH);
         Block block10 = new Block(10 * X_POS_10 + LEVEL_OFFSET_X, 6 * Y_POS_1 + 6 + LEVEL_OFFSET_Y, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT * 4 / 5, COLOR_5, BLOCK_HEALTH);

         /*Image img = Image.FromFile("../../../Batty 2.0/Resources/Block(Grayscale).png");
         block1.image = img;
         block2.image = img;
         block3.image = img;
         block4.image = img;
         block5.image = img;
         block6.image = img;
         block7.image = img;
         block8.image = img;
         block9.image = img;
         block10.image = img;*/

         Enemy enemy1 = new Enemy(ENEMY_X_POS_1, ENEMY_Y_POS_10, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH, null, null, null);
         Enemy enemy2 = new Enemy(ENEMY_X_POS_2, ENEMY_Y_POS_9, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_2, BLOCK_HEALTH, null, null, null);
         Enemy enemy3 = new Enemy(ENEMY_X_POS_3, ENEMY_Y_POS_8, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_3, BLOCK_HEALTH, null, null, null);
         Enemy enemy4 = new Enemy(ENEMY_X_POS_4, ENEMY_Y_POS_7, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_4, BLOCK_HEALTH, null, null, null);
         Enemy enemy5 = new Enemy(ENEMY_X_POS_5, ENEMY_Y_POS_6, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_5, BLOCK_HEALTH, null, null, null);
         Enemy enemy6 = new Enemy(ENEMY_X_POS_6, ENEMY_Y_POS_5, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_1, BLOCK_HEALTH, null, null, null);
         Enemy enemy7 = new Enemy(ENEMY_X_POS_7, ENEMY_Y_POS_4, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_2, BLOCK_HEALTH, null, null, null);
         Enemy enemy8 = new Enemy(ENEMY_X_POS_8, ENEMY_Y_POS_3, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_3, BLOCK_HEALTH, null, null, null);
         Enemy enemy9 = new Enemy(ENEMY_X_POS_9, ENEMY_Y_POS_2, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_4, BLOCK_HEALTH, null, null, null);
         Enemy enemy10 = new Enemy(ENEMY_X_POS_10, ENEMY_Y_POS_1, LEVEL_COLUMN_WIDTH,
            LEVEL_ROW_HEIGHT, COLOR_5, BLOCK_HEALTH, null, null, null);

         level1.AddBlock(block1, Y_POS_10, X_POS_1);
         level1.AddBlock(block2, Y_POS_9, X_POS_2);
         level1.AddBlock(block3, Y_POS_8, X_POS_3);
         level1.AddBlock(block4, Y_POS_7, X_POS_4);
         level1.AddBlock(block5, Y_POS_6, X_POS_5);
         level1.AddBlock(block6, Y_POS_5, X_POS_6);
         level1.AddBlock(block7, Y_POS_4, X_POS_7);
         level1.AddBlock(block8, Y_POS_3, X_POS_8);
         level1.AddBlock(block9, Y_POS_2, X_POS_9);
         level1.AddBlock(block10, Y_POS_1, X_POS_10);

         level1.AddEnemy(enemy1, 0);
         level1.AddEnemy(enemy2, 1);
         level1.AddEnemy(enemy3, 2);
         level1.AddEnemy(enemy4, 3);
         level1.AddEnemy(enemy5, 4);
         level1.AddEnemy(enemy6, 5);
         level1.AddEnemy(enemy7, 6);
         level1.AddEnemy(enemy8, 7);
         level1.AddEnemy(enemy9, 8);
         level1.AddEnemy(enemy10, 9);

         level1.SaveToFile();

         level2 = new Level(LEVEL_NUM_FILE_IO, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);

         Assert.AreEqual(level1, level2);

      }

      [TestMethod]
      public void Level_Equals_ObjectNull()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Assert.IsTrue(!level.Equals(null));
      }

      [TestMethod]
      public void Level_Equals_ObjectNotLevel()
      {
         Level level = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Block block = new Block(1, 1, 1, 1, Color.AliceBlue, 1);

         Assert.IsTrue(!level.Equals(block));
      }

      [TestMethod]
      public void Level_Equals_LevelsNotEqual_GeneralVariables()
      {
         Level level1 = new Level(LEVEL_NUM_FILE_IO, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Level level2 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);

         Assert.IsTrue(!level1.Equals(level2));

         level1 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         level2 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);

         level1.AddEnemy(new Enemy(1, 1, 1, 1, Color.AliceBlue, 1, null, null, null), 0);

         Assert.IsTrue(!level1.Equals(level2));


      }

      [TestMethod]
      public void Level_Equals_LevelsNotEqual_EnemyList_DifferentEnemies()
      {
         Level level1 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Level level2 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);

         level1.AddEnemy(new Enemy(1, 1, 1, 1, Color.AliceBlue, 1, null, null, null), 0);
         level2.AddEnemy(new Enemy(1, 1, 1, 1, Color.AliceBlue, 1, null, null, null), 1);

         Assert.IsTrue(!level1.Equals(level2));
      }

      [TestMethod]
      public void Level_Equals_LevelsNotEqual_EnemyList_DifferentCount()
      {
         Level level1 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Level level2 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);

         level1.AddEnemy(new Enemy(1, 1, 1, 1, Color.AliceBlue, 1, null, null, null), 0);
         
         Assert.IsTrue(!level1.Equals(level2));
      }

      [TestMethod]
      public void Level_Equals_LevelsNotEqual_BlockList()
      {
         Level level1 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
           LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Level level2 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);

         level1.AddBlock(new Block(1, 1, 1, 1, Color.AliceBlue, 1), 1, 1);

         Assert.IsTrue(!level1.Equals(level2));
      }

      [TestMethod]
      public void Level_Equals_LevelsEqual()
      {
         Level level1 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Level level2 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);

         Assert.IsTrue(level1.Equals(level2));
      }

      [TestMethod]
      public void Level_GetFilePath_DefaultLevel()
      {
         Level level1 = new Level(LEVEL_NUM, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         level1.Default = true;
         level1.LoadFromFile();
         Assert.IsNotNull(level1.GetBlock(1, 0));

      }

      [TestMethod]
      public void Level_LoadFromFile_loadFileWithJunkBlock()
      {
         Level level1 = new Level(50, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         System.IO.File.Delete("../../../Batty 2.0/Resources/Levels/" + "75" + ".txt");
         level1.LoadFromFile();
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void Level_Constructor_createFile()
      {
         System.IO.File.Delete("../../../Batty 2.0/Resources/Levels/" + "75" + ".txt");
         Level level1 = new Level(75, LEVEL_ROW_HEIGHT,
            LEVEL_COLUMN_WIDTH, LEVEL_OFFSET_X, LEVEL_OFFSET_Y * 2);
         Assert.IsTrue(true);
      }
   }
}
