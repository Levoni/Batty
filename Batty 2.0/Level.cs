using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System;
public class Level
{
   private Block[,] blockList;
   //private List<Enemy> enemies;
   private Enemy[] enemies;
   private int enemyCount;

   private int levelNum;

   public int LevelNumber { set { levelNum = value; } }

   public bool Default { private get; set; }

   private StreamReader readStream;
   private StreamWriter writeStream;

   private int rowHeight, colWidth, offsetX, offsetY;

   /// <summary>
   /// Level Constructor
   /// </summary>
   /// <param name="levelNum"></param>
   /// <param name="rowHeight"></param>
   /// <param name="colWidth"></param>
   /// <param name="offsetX"></param>
   /// <param name="offsetY"></param>
   public Level(int levelNum, int rowHeight, int colWidth, int offsetX, int offsetY)
   {
      blockList = new Block[GameManager.NUM_COLS, GameManager.NUM_ROWS];

      for (int i = 0; i < GameManager.NUM_COLS; i++)
         for (int j = 0; j < GameManager.NUM_ROWS; j++)
            blockList[i, j] = null;

      enemies = new Enemy[GameManager.NUM_COLS];
      enemyCount = 0;

      this.levelNum = levelNum;
      this.rowHeight = rowHeight * 4 / 5;
      this.colWidth = colWidth;
      this.offsetX = offsetX;
      this.offsetY = offsetY / 2;
      Default = false;
      if (!File.Exists(GetFilePath()))
      {
         File.Create(GetFilePath()).Close();
      }
      LoadFromFile();
   }

   /// <summary>
   /// Adds a block to the level.
   /// </summary>
   /// <param name="block">The block to be added.</param>
   public bool AddBlock(Block block, int row, int col)
   {
      if (row < GameManager.NUM_ROWS - 1 && col < GameManager.NUM_COLS)
      {
         blockList[col, row] = block;
         return true;
      }
      return false;
   }

   /// <summary>
   /// Removes a block by index
   /// </summary>
   /// <param name="row"></param>
   /// <param name="col"></param>
   /// <returns></returns>
   public bool RemoveBlock(int row, int col)
   {
      if (row < GameManager.NUM_ROWS && col < GameManager.NUM_COLS)
      {
         if (blockList[col, row] == null)
            return false;
         blockList[col, row] = null;
         return true;
      }
      return false;
   }

   /// <summary>
   /// Remove specified Block
   /// </summary>
   /// <param name="block"></param>
   /// <returns></returns>
   public bool RemoveBlock(Block block)
   {
      for (int i = 0; i < GameManager.NUM_ROWS; i++)
      {
         for (int j = 0; j < GameManager.NUM_COLS; j++)
         {
            if (blockList[j, i] != null && blockList[j, i].Equals(block))
            {
               blockList[j, i].health--;
               if (blockList[j, i].health <= 0)
               {
                  blockList[j, i] = null;
                  return true;
               }
            }
         }
      }
      return false;
   }

   /// <summary>
   /// Gets blovck at index
   /// </summary>
   /// <param name="row"></param>
   /// <param name="col"></param>
   /// <returns></returns>
   public Block GetBlock(int row, int col)
   {
      return blockList[col, row];
   }

   /// <summary>
   /// Gets the list of enemies
   /// </summary>
   /// <returns></returns>
   public List<Enemy> GetEnemies()
   {
      List<Enemy> enemyList = new List<Enemy>();
      foreach (Enemy e in enemies)
      {
         if (e != null)
            enemyList.Add(e);
      }
      return enemyList;
   }

   /// <summary>
   /// Adds an enemy id list is not full
   /// </summary>
   /// <param name="enemy"></param>
   /// <returns></returns>
   public bool AddEnemy(Enemy enemy, int col)
   {
      if (enemyCount < GameManager.NUM_COLS && col < GameManager.NUM_COLS)
      {
         enemies[col] = enemy;
         enemyCount++;
         return true;
      }
      return false;
   }

   /// <summary>
   /// Removes a specified enemy
   /// Returns true if succeded
   /// </summary>
   /// <param name="enemy"></param>
   public bool RemoveEnemy(Enemy enemy)
   {
      for(int i = 0; i < enemies.Length; i++)
      {
         if (enemies[i] != null && enemies[i].Equals(enemy))
         {
            enemies[i] = null;
            return true;
         }
      }
      return false;
   }

   /// <summary>
   /// gets the 2d block array
   /// </summary>
   /// <returns></returns>
   public Block[,] GetBlocks()
   {
      return blockList;
   }

   /// <summary>
   /// Calls update for all enemies
   /// </summary>
   public void UpdateEnemies()
   {
      foreach (Enemy e in enemies)
      {
         if (e != null)
            e.Update();
      }    
   }

   /// <summary>
   /// Reads the level data stored in the following way.
   /// |color:health|
   /// Enemies are the first line
   /// Blocks are all following lines
   /// </summary>
   public void LoadFromFile()
   {
      enemies = new Enemy[GameManager.NUM_COLS];
      enemyCount = 0;
      for (int i = 0; i < GameManager.NUM_COLS; i++)
         for (int j = 0; j < GameManager.NUM_ROWS; j++)
            blockList[i, j] = null;

      readStream = new StreamReader(GetFilePath());

      string[] splitLine;
      string[] splitBlock;
      string line = readStream.ReadLine();
      if (line == null)
      {
         readStream.Close();
      }
      else
      {
         Bitmap imageSource = new Bitmap("../../../Batty 2.0/Resources/Enemies/bat-48x64.png");
         Rectangle location1 = new Rectangle(104, 149, 31, 21);
         Rectangle location2 = new Rectangle(56, 148, 31, 21);
         Rectangle location3 = new Rectangle(8, 149, 31, 21);
         Image frame1 = imageSource.Clone(location1, imageSource.PixelFormat);
         Image frame2 = imageSource.Clone(location2, imageSource.PixelFormat);
         Image frame3 = imageSource.Clone(location3, imageSource.PixelFormat);
         splitLine = line.Split('|');
         for (int n = 0; n < GameManager.NUM_COLS; n++)
         {
            splitBlock = splitLine[n].Split(',');
            int xpos = colWidth * n + offsetX;
            int ypos = offsetY - rowHeight;
            string[] splitColor = splitBlock[0].Split(':');
            try
            {
               Enemy e = new Enemy(xpos, ypos, (int)GameManager.screenWidth / 25, (int)GameManager.screenHeight / 21,
                  Color.FromArgb(int.Parse(splitColor[0]), int.Parse(splitColor[1]),
                  int.Parse(splitColor[2]), int.Parse(splitColor[3])),
                  int.Parse(splitBlock[1]), frame1, frame2, frame3);
               AddEnemy(e, n);
               //enemies.Add(e);
            }
            catch (Exception)
            { }
         }
         for (int i = 0; i < GameManager.NUM_ROWS; i++)
         {
            line = readStream.ReadLine();
            if (line == null)
               readStream.Close();
            else
            {
               splitLine = line.Split('|');
               for (int j = 0; j < GameManager.NUM_COLS; j++)
               {
                  if (splitLine[j].Trim() != ",")
                  {
                     splitBlock = splitLine[j].Split(',');
                     int xpos = colWidth * j + offsetX;
                     int ypos = rowHeight * i + offsetY + rowHeight;
                     string[] splitColor = splitBlock[0].Split(':');
                     try
                     {
                        blockList[j, i] = new Block(xpos, ypos,
                           colWidth, rowHeight,
                           Color.FromArgb(int.Parse(splitColor[0]), int.Parse(splitColor[1]),
                           int.Parse(splitColor[2]), int.Parse(splitColor[3])), int.Parse(splitBlock[1]));
                       // blockList[j, i].image = Image.FromFile("../../../Batty 2.0/Resources/Block(Grayscale).png");
                     }
                     catch (Exception)
                     {
                        blockList[j, i] = null;
                     }
                  }
               }
            }
         }
      }
      readStream.Close();
   }

   /// <summary>
   /// Populates the level with blocks and enemies from file
   /// </summary>
   public void SaveToFile()
   {
      File.Create(GetFilePath()).Close();
      writeStream = new StreamWriter(GetFilePath());
      string line = "";
      //List<int> cols = new List<int>();
      //foreach (Enemy e in enemies)
      //{
      //   cols.Add((int)((e.X - offsetX) / colWidth));
      //}
      //cols.Sort();
      //int offset = 0;
      for (int n = 0; n < enemies.Length; n++)
      {
         //line = "";
         string color = "               ";
         string health = " ";
         if (enemies[n] != null)
         {
            color = string.Format("{0:000}", (enemies[n].color.A)) + ':' +
            string.Format("{0:000}", (enemies[n].color.R)) + ':' +
            string.Format("{0:000}", (enemies[n].color.G)) + ':' +
            string.Format("{0:000}", (enemies[n].color.B));
            health = enemies[n].Health.ToString();
         }
         line += color + "," + health + "|";
      }
      writeStream.WriteLine(line);


      for (int i = 0; i < GameManager.NUM_ROWS - 1; i++)
      {
         line = "";
         for (int j = 0; j < GameManager.NUM_COLS; j++)
         {
            string color = "               ";
            string health = " ";
            if (blockList[j, i] != null)
            {
               color = string.Format("{0:000}", (blockList[j, i].color.A)) + ':' +
               string.Format("{0:000}", (blockList[j, i].color.R)) + ':' +
               string.Format("{0:000}", (blockList[j, i].color.G)) + ':' +
               string.Format("{0:000}", (blockList[j, i].color.B));
               health = blockList[j, i].health.ToString(); // no health >9
            }
            line += color + "," + health + "|";
         }

         writeStream.WriteLine(line);
      }
      //writeStream.WriteLine(line);
      writeStream.Close();
   }

   private string GetFilePath()
   {
      if(Default)
         return "../../../Batty 2.0/Resources/Levels/" + "Default" + ".txt";
      else
         return "../../../Batty 2.0/Resources/Levels/" + levelNum + ".txt";
   }

   public override bool Equals(object obj)
   {
      if (obj == null || obj.GetType() != this.GetType())
         return false;

      Level level = (Level)obj;

      if (levelNum != level.levelNum || rowHeight != level.rowHeight || colWidth != level.colWidth || offsetX != level.offsetX || offsetY != level.offsetY)
         return false;

      for (int i = 0; i < GameManager.NUM_COLS; i++)
         for (int j = 0; j < GameManager.NUM_ROWS; j++)
            if ((blockList[i, j] != null && !blockList[i, j].Equals(level.blockList[i, j])) 
               || (blockList[i, j] == null && (blockList[i, j] != level.blockList[i, j])))
               return false;

      if (enemyCount != level.enemyCount)
         return false;

      for (int i = 0; i < GameManager.NUM_COLS; i++)
         if ((enemies[i] == null && level.enemies[i] != null) ||
            (enemies[i] != null && level.enemies[i] == null) ||
            (enemies[i] != null && level.enemies[i] != null && !enemies[i].Equals(level.enemies[i])))
            return false;

      return true;
   }
}
