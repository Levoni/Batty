using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   /// <summary>
   /// Summary description for EditorControllerTest
   /// </summary>
   [TestClass]
   public class EditorControllerTest
   {
      private EditorController EC;

      public EditorControllerTest()
      {
         //
         // TODO: Add constructor logic here
         //
      }

      private TestContext testContextInstance;

      /// <summary>
      ///Gets or sets the test context which provides
      ///information about and functionality for the current test run.
      ///</summary>
      public TestContext TestContext
      {
         get
         {
            return testContextInstance;
         }
         set
         {
            testContextInstance = value;
         }
      }

      #region Additional test attributes
      //
      // You can use the following additional attributes as you write your tests:
      //
      // Use ClassInitialize to run code before running the first test in the class
      // [ClassInitialize()]
      // public static void MyClassInitialize(TestContext testContext) { }
      //
      // Use ClassCleanup to run code after all tests in a class have run
      // [ClassCleanup()]
      // public static void MyClassCleanup() { }
      //
      // Use TestInitialize to run code before running each test 
      // [TestInitialize()]
      // public void MyTestInitialize() { }
      //
      // Use TestCleanup to run code after each test has run
      // [TestCleanup()]
      // public void MyTestCleanup() { }
      //
      #endregion

      [TestInitialize()]
      public void MyTestInitialize()
      {
         EC = new EditorController(GameManager.NUM_ROWS, GameManager.NUM_COLS);
      }



      [TestMethod]
      public void EditorController_Constructor()
      {
         EC = new EditorController(GameManager.NUM_ROWS, GameManager.NUM_COLS);
         Assert.IsNotNull(EC);
      }

      // to do: need load to be fixed to work
      [TestMethod]
      public void LoadLevel_validInput_true()
      {
         Assert.IsTrue(EC.Load(0));
      }

      [TestMethod]
      public void LoadLevel_lowInvalidInput_false()
      {
         Assert.IsFalse(EC.Load(-1));
      }

      [TestMethod]
      public void LoadLevel_HighInvalidInput_False()
      {
         Assert.IsFalse(EC.Load(15));
      }

      // to do: need load to be fixed to work
      [TestMethod]
      public void AddBlock_ValidPosition_True()
      {
         EC.Load(0);
         Assert.IsTrue(EC.AddBlock(2, 2));
      }

      [TestMethod]
      public void AddBlock_InvalidPosition_False()
      {
         Assert.IsFalse(EC.AddBlock(15, 12));
      }

      // to do: need load to be fixed to work, and there is block in this position
      [TestMethod]
      public void RemoveBlock_ValidOccupiedPosition_True()
      {
         EC.Load(1);
         Assert.IsTrue(EC.RemoveBlock(0, 0));
      }

      [TestMethod]
      public void RemoveBlock_ValidUnoccupiedPosition_true()
      {
         EC.Load(0);
         Assert.IsFalse(EC.RemoveBlock(120, 20));
      }

      [TestMethod]
      public void RemoveBlock_InvalidPosition_false()
      {
         Assert.IsFalse(EC.RemoveBlock(2000, 2000));
      }
      
      // to do: need load to be fixed to work
      [TestMethod]
      public void Save_ValidLevel_DoesNotCrash()
      {
         EC.Load(0);
         Assert.IsTrue(EC.Save());
      }

      [TestMethod]
      public void Save_InvalidLevel_DoesNotCrash()
      {
         EC.Load(0);
         EC.ClearLevel();
         Assert.IsFalse(EC.Save());
      }

      [TestMethod]
      public void ClearLevel_ClearingLevel_LevelWillNotSave()
      {
         EC.Load(0);
         EC.ClearLevel();
         Assert.IsFalse(EC.Save());
      }

      [TestMethod]
      public void Editor_SetBlockColor_EqualTestColor()
      {
         System.Drawing.Color testColor = System.Drawing.Color.Yellow;
         EC.BlockColor = testColor;
         Assert.AreEqual(EC.BlockColor, testColor);
      }

      [TestMethod]
      public void Editor_CheckBlockColor_EqualBlack()
      {
         System.Drawing.Color testColor = System.Drawing.Color.Black;
         Assert.AreEqual(EC.BlockColor, testColor);
      }

      [TestMethod]
      public void Editor_SetBlockHealth_EqualTestHealth()
      {
         int health = 2;
         EC.BlockHealth = health;
         Assert.AreEqual(EC.BlockHealth, health);
      }

      [TestMethod]
      public void Editor_CheckBlockHealth_EqualInitHealth()
      {
         int health = 1;
         Assert.AreEqual(EC.BlockHealth, health);
      }

      [TestMethod]
      public void Editor_AddValidEnemy_True()
      {
         int validCol = 0;
         Assert.IsTrue(EC.AddEnemy(validCol));
      }

      [TestMethod]
      public void Editor_AddInvalidEnemyCol_False()
      {
         int invalidCol = GameManager.NUM_COLS + 1;
         Assert.IsFalse(EC.AddEnemy(invalidCol));
      }

      [TestMethod]
      public void Editor_SetDefault_True()
      {
         EC.Load(1);
         Assert.IsFalse(EC.IsDefaultTest());
         EC.SetToDefault();
         Assert.IsTrue(EC.IsDefaultTest());
      }

      [TestMethod]
      public void Editor_SetNormal_True()
      {
         int testNum = 1;
         EC.Load(testNum);
         EC.Save();
         Assert.IsTrue(EC.CompareLevelTest(testNum));
         EC.SetToDefault();
         EC.Save();
      }

      [TestMethod]
      public void Editor_RemoveEnemy_Decreased()
      {
         EC.SetToDefault();
         int initEnemyCount = EC.GetLevelEnemies().Count;
         EC.RemoveEnemy(0);
         int postEnemyCount = EC.GetLevelEnemies().Count;
         Assert.IsTrue(initEnemyCount > postEnemyCount);

      }

      [TestMethod]
      public void Editor_CheckBlockCount_Positive()
      {
         EC.SetToDefault();
         int blockCount = EC.GetLevelBlocks().Length;
         Assert.IsTrue(blockCount > 0);
      }

      [TestMethod]
      public void Editor_clearLevel_EnemyZero()
      {
         EC.SetToDefault();
         EC.ClearLevel();
         int enemyCount = EC.GetLevelEnemies().Count;
         Assert.IsTrue(enemyCount == 0);
      }


   }
}
