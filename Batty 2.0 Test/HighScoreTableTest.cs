using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Batty_2._0_Test
{
   [TestClass]
   public class HighScoreTableTest
   {
      [TestMethod]
      public void HighScoreTable_ConstructorTest()
      {
         HighScoreTable table = new HighScoreTable();
         Assert.IsNotNull(table);
      }

      [TestMethod]
      public void HighScoreTable_AddScorePastMax()
      {
         HighScoreTable table = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTest.txt");
         int i;
         for (i = 0; i < HighScoreTable.MAX_SCORE_COUNT + 1; i++)
            table.AddHighScore(i, "CAR" + i);
         Assert.AreEqual(HighScoreTable.MAX_SCORE_COUNT, table.HighScores.Rows.Count);
      }

      [TestMethod]
      public void HighScoreTable_AddScoreRemovesLowest()
      {
         HighScoreTable table = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTest.txt");
         int i;
         for (i = 0; i < HighScoreTable.MAX_SCORE_COUNT + 1; i++)
            table.AddHighScore(i, "MAB" + i);
         Assert.AreNotEqual(0, table.HighScores.Rows[HighScoreTable.MAX_SCORE_COUNT - 1][1]);
      }

      [TestMethod]
      public void HighScoreTable_TestIO()
      {
         HighScoreTable table = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTest.txt");
         table.AddHighScore(5, "DN" + 3);
         table.AddHighScore(4, "BO" + 2);
         table.AddHighScore(56, "LT" + 9);
         table.AddHighScore(58, "DM" + 7);
         table.AddHighScore(56789, "" + 365);
         table.AddHighScore(5526, "" + 356);
         table.AddHighScore(5837, "" + 783);
         table.AddHighScore(42696, "" + 334);
         table.AddHighScore(5, "SA" + 3);
         table.AddHighScore(5248, "GML");
         table.UpdateFile();
         HighScoreTable newTable = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTest.txt");
         for (int i = 0; i < table.HighScores.Rows.Count; i++)
         {
            Assert.AreEqual(table.HighScores.Rows[i][1], newTable.HighScores.Rows[i][1]);
            Assert.AreEqual(table.HighScores.Rows[i][0], newTable.HighScores.Rows[i][0]);
         }
      }

      [TestMethod]
      public void HighScoreTable_IsHighScore_True()
      {
         HighScoreTable table = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTest.txt");
         int highScore = 10000000;

         Assert.IsTrue(table.IsHighScore(highScore));
         
      }

      [TestMethod]
      public void HighScoreTable_IsHighScore_False()
      {
         HighScoreTable table = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTest.txt");
         int lowScore = 0;

         Assert.IsFalse(table.IsHighScore(lowScore));
      }

      [TestMethod]
      public void HighScoreTable_Constructor_CreateFile()
      {
         System.IO.File.Delete("../../../Batty 2.0/Resources/HighScoresTestTemp.txt");
         HighScoreTable table = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTestTemp.txt");
         Assert.IsTrue(System.IO.File.Exists("../../../Batty 2.0/Resources/HighScoresTestTemp.txt"));
      }

      [TestMethod]
      public void HighScoreTable_FillTable_ExeptionFromJunkFile()
      {
         HighScoreTable table = new HighScoreTable("../../../Batty 2.0/Resources/HighScoresTest2.txt");
         Assert.IsTrue(table.IsHighScore(50000));
      }
   }
}
