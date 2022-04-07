using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

public class HighScoreTable
{
   private DataTable highScoresTable;

   public DataTable HighScores { get { return highScoresTable; } }

   StreamReader readStream;
   StreamWriter writeStream;


   public const int MAX_SCORE_COUNT = 15;
   private string filePath;
   private const string STANDARD_FILE_PATH = "../../../Batty 2.0/Resources/HighScores.txt";

   /// <summary>
   /// Constructor.
   /// </summary>
   public HighScoreTable()
   {
      highScoresTable = new DataTable("High Scores");
      highScoresTable.Columns.Add("Name", typeof(string));
      highScoresTable.Columns.Add("Score", typeof(int));
      filePath = STANDARD_FILE_PATH;
      if (!File.Exists(filePath))
      {
         File.Create(filePath).Close();
      }
      FillTable();
   }
   public HighScoreTable(string file)
   {
      highScoresTable = new DataTable("High Scores");
      highScoresTable.Columns.Add("Name", typeof(string));
      highScoresTable.Columns.Add("Score", typeof(int));
      filePath = file;
      if (!File.Exists(file))
      {
         File.Create(file).Close();
      }
      FillTable();
   }

   /// <summary>
   /// Adds a new high score to the list and sorts the list by scores.
   /// </summary>
   /// <param name="newScore">The high score to add.</param>
   /// <param name="name">The name of the high score holder.</param>
   public void AddHighScore(int newScore, string name)
   {
      if (highScoresTable.Rows.Count >= MAX_SCORE_COUNT)
         highScoresTable.Rows.RemoveAt(MAX_SCORE_COUNT - 1);
      highScoresTable.Rows.Add(name, newScore);
      //highScores.Sort();
      DataView dv = highScoresTable.DefaultView;
      dv.Sort = "Score DESC";
      highScoresTable = dv.ToTable();
      UpdateFile();
      //FillTable();
   }

   /// <summary>
   /// Creates a new high score file and prints the high score table to the
   /// file.
   /// </summary>
   public void UpdateFile()
   {
      try
      {
         File.Create(filePath).Close();
         writeStream = new StreamWriter(filePath);
         for (int i = 0; i < highScoresTable.Rows.Count; i++)
         {
            writeStream.WriteLine(highScoresTable.Rows[i].ItemArray[0] + ":" +
               highScoresTable.Rows[i].ItemArray[1]);   //Format used:  <Name>:<Score>
         }
         writeStream.Close();
      }
      catch (IOException e)
      {
         Console.WriteLine(e);
      }
   }

   /// <summary>
   /// Reads from the high score file, loading the high score table with the
   /// values from the file.
   /// </summary>
   private void FillTable()
   {
      string readLine = "";
      string[] parsedLine;

      try
      {
         readStream = new StreamReader(filePath);
         while (!readStream.EndOfStream)
         {
            readLine = readStream.ReadLine();
            parsedLine = readLine.Split(':');
            highScoresTable.Rows.Add(parsedLine[0], int.Parse(parsedLine[1]));
         }
         readStream.Close();
      }
      catch (IOException e)
      {
         Console.WriteLine(e);
      }
   }

   /// <summary>
   /// Checks if input score is a new high score
   /// </summary>
   /// <param name="score"></param>
   /// <returns></returns>
   public bool IsHighScore(int score)
   {
      if (highScoresTable.Rows.Count == MAX_SCORE_COUNT)
      {
         int lowScore = (int)highScoresTable.Rows[highScoresTable.Rows.Count - 1][1];
         return lowScore < score;
      }
      return true;
   }
}