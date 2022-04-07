using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Batty_2._0_Test
{
   /// <summary>
   /// Summary description for GameManagerTest
   /// </summary>
   [TestClass]
   public class GameManagerTest
   {

      private GameManager gm;

      public GameManagerTest()
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
            //MyTestInitialize();
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

      [TestInitialize]
      public void MyTestInitialize()
      {
         gm = new GameManager(3, 0, 500, 500, 25, 25,
            System.Drawing.Color.Bisque, System.Drawing.Color.DarkSlateGray);
      }

      [TestCleanup]
      public void MyTestCleanup()
      {
         //gm = null;
      }

      [TestMethod]
      public void GM_ConstructorTest()
      {
         Assert.IsNotNull(gm);
      }

      [TestMethod]
      public void GM_LoadAllLevels()
      {
         for (int i = 0; i < GameManager.MAX_LEVEL - 1; ++i)
         {
            gm.SkipLevel();
         }
         Assert.AreEqual(GameManager.GameState.InProgress, gm.gameState);
      }

      [TestMethod]
      public void GM_WinGameTest()
      {
         for (int i = 0; i < GameManager.MAX_LEVEL + 1; ++i)
         {
            gm.SkipLevel();
         }
         Assert.AreEqual(GameManager.GameState.Won, gm.gameState);
      }

      [TestMethod]
      public void GM_GetObjectsTest_NotNull()
      {
         List<GameObject> objects = gm.GetObjects();
         foreach (GameObject o in objects)
         {
            Assert.IsTrue(o != null);
         }
      }

      [TestMethod]
      public void GM_EndLevel_True()
      {
         Assert.IsTrue(gm.TestEndLevel());
      }

      [TestMethod]
      public void GM_EndLevel_False()
      {
         Assert.IsFalse(gm.TestNotEndLevel());
      }

      [TestMethod]
      public void GM_Lives_EndGame()
      {
         gm.ModifyLives(0);
         gm.Update();
         Assert.AreEqual(gm.gameState, GameManager.GameState.Lost);
      }

      [TestMethod]
      public void GM_BatKilled_Removed()
      {
         int initLives = gm.GetEnemyCount();
         gm.SetUpEnemyBallCollision(); // Added a bat
         gm.GameStart();
         gm.Update();
         Assert.IsTrue(initLives >= gm.GetEnemyCount());
      }

      [TestMethod]
      public void GM_GetValues()
      {
         Assert.IsTrue(gm.Score > -1);
         Assert.IsTrue(gm.Lives > -1);
         Assert.IsTrue(gm.BallSpeed > -1);
         gm.SkipLevel();
         Assert.IsTrue(gm.Level > -1);
      }

      [TestMethod]
      public void GM_SkipAllLvels_GameWon()
      {
         while(gm.gameState == GameManager.GameState.InProgress)
         {
            gm.SkipLevel();
         }

         Assert.AreEqual(gm.gameState, GameManager.GameState.Won);
      }

      [TestMethod]
      public void GM_SetLivesCheat_Max()
      {
         int maxLives = 99;
         gm.ModifyLives(99);
         Assert.AreEqual(gm.Lives, maxLives);
      }

      [TestMethod]
      public void GM_BlockCollision_Destroyed()
      {
         gm.SkipLevel();
         int initDead = gm.GetDeadBlocksCount();
         gm.SetUpBallBlockCollision();
         gm.GameStart();
         gm.Update();
         Assert.IsTrue(initDead < gm.GetDeadBlocksCount());
      }

      [TestMethod]
      public void GM_BlockCollisionBottom_GameLost()
      {
         gm.SetUpBallBottomCollision();
         gm.GameStart();
         gm.Update();
         Assert.AreEqual(gm.gameState, GameManager.GameState.Lost);
      }

      [TestMethod]
      public void GM_TogglePause()
      {
         bool pausedStatus = gm.Paused;
         gm.TogglePause();
         Assert.AreNotEqual(pausedStatus, gm.Paused);
      }

      [TestMethod]
      public void GM_ToggleSound()
      {
         bool soundStatus = gm.Muted;
         gm.ToggleSound();
         Assert.AreNotEqual(soundStatus, gm.Muted);
      }



      [TestMethod]
      public void GM_TestModify()
      {
         List<GameObject> objects = gm.GetObjects();
         Image heart = Image.FromFile("../../../Batty 2.0/Resources/Heart.png");
         gm.ModifyBallShape(heart);
         gm.BallColor = Color.MintCream;
         //gm.ModifyBallColor(Color.MintCream);
         gm.ModifyBallSpeed(10000);
         //gm.ModifyBarColor(Color.Peru);
         gm.BarColor = Color.Peru;
         gm.ModifyBarLength(100000);
         foreach (GameObject o in objects)
         {
            if (o.GetType() == typeof(Ball))
            {
               Assert.AreEqual(heart, o.image);
               Assert.AreEqual(Color.MintCream, o.color);
               Assert.AreEqual(10000, ((Ball)o).Speed);
            }
            else if (o.GetType() == typeof(Bar))
            {
               Assert.AreEqual(Color.Peru, o.color);
               Assert.AreEqual(100000, o.Width);
            }
         }
      }

      [TestMethod]
      public void GM_NewModify_Increased()
      {
         int initBarSize, initBallSpeed;
         initBarSize = initBallSpeed = 0;

         List<GameObject> objects = gm.GetObjects();
         foreach (GameObject o in objects)
         {
            if (o is Ball)
               initBallSpeed = (o as Ball).Speed;
            else if (o is Bar)
               initBarSize = (int)(o as Bar).Width;
         }
         int extensionModifier = 50;
         gm.ChangeBarSize(extensionModifier);
         gm.ChangeBallSpeed(extensionModifier);

         objects = gm.GetObjects();
         foreach (GameObject o in objects)
         {
            if (o is Ball)
               Assert.IsTrue((o as Ball).Speed > initBallSpeed);
            else if (o is Bar)
               Assert.IsTrue((o as Bar).Width > initBarSize);
         }
      }

      [TestMethod]
      public void GM_NewModify_Decreased()
      {
         int initBarSize, initBallSpeed;
         initBarSize = initBallSpeed = 0;

         List<GameObject> objects = gm.GetObjects();
         foreach (GameObject o in objects)
         {
            if (o is Ball)
               initBallSpeed = (o as Ball).Speed;
            else if (o is Bar)
               initBarSize = (int)(o as Bar).Width;
         }
         int extensionModifier = 1;
         gm.ChangeBarSize(extensionModifier);
         gm.ChangeBallSpeed(extensionModifier);

         objects = gm.GetObjects();
         foreach (GameObject o in objects)
         {
            if (o is Ball)
               Assert.IsTrue((o as Ball).Speed < initBallSpeed);
            else if (o is Bar)
               Assert.IsTrue((o as Bar).Width < initBarSize);
         }
      }

      [TestMethod]
      public void GM_ReloadLevel_EqualInitLevel()
      {
         int initLevel = gm.Level;
         gm.ReloadLevel();
         Assert.AreEqual(gm.Level, initLevel);
      }

      [TestMethod]
      public void GM_RestartGame_Default()
      {
         int initScore = gm.Score;
         int initLevel = gm.Level;

         gm.SkipLevel();
         gm.SkipLevel();
         gm.gameState = GameManager.GameState.Won;

         gm.Restart();

         Assert.AreEqual(gm.Score, initScore);
         Assert.AreEqual(gm.Level, initLevel);
         Assert.AreEqual(gm.gameState, GameManager.GameState.InProgress);
      }

      [TestMethod]
      public void GM_AddPoints_10()
      {
         int gmScore = gm.Score;
         gm.AddPoints(10);
         Assert.AreEqual(gmScore + 10, gm.Score);
      }
      
      [TestMethod]
      public void GM_DecrimentTime_timeAbove0()
      {
         int gmInitailTime = gm.DecrimenntTime(0);
         gm.DecrimenntTime(1000);
         Assert.AreEqual(gmInitailTime - 1, gm.DecrimenntTime(0));
      }

      [TestMethod]
      public void GM_DecrimentTime_timeBelow0()
      {
         gm.DecrimenntTime(190000);
         Assert.AreEqual(gm.DecrimenntTime(0),0);
      }

      [TestMethod]
      public void BombBallCollision()
      {
         int bombs = gm.SetUpBombBallCollision();
         gm.GameStart();
         gm.Update();
         Assert.AreEqual(gm.SetUpBombBallCollision(), bombs);
      }

      [TestMethod]
      public void BombBarCollision()
      {
         gm.SetUpBombBarCollision();
         gm.GameStart();
         gm.Update();
         Assert.AreEqual(gm.gameState, GameManager.GameState.Lost);
      }

      [TestMethod]
      public void EnemyDropBomb()
      {
         int bombs = gm.SetUpBombDrop();
         gm.GameStart();
         gm.Update();
         Assert.AreNotEqual(gm.SetUpBombDrop(), bombs);
      }

      [TestMethod]
      public void BallBarCollision()
      {
         float dir = gm.SetUpBarBallCollision();
         gm.GameStart();
         gm.Update();
         Assert.AreNotEqual(gm.SetUpBarBallCollision(), dir);
      }
   }
}
