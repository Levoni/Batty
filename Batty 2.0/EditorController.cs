//--------------------------------------------------------
// Authors: David, Matthew, Levon, Alex, and Derek
// Program: Batty
// Purpose: Handle creating a custom level in the editor
//--------------------------------------------------------
using System.Collections.Generic;
using System.Drawing;

public class EditorController
{

   private Block movedBlock;
   private Level editingLevel { set; get; }


   private int rAmount;
   private int cAmount;

   public Color BlockColor { get; set; }

   public int BlockHealth { get; set; }

   /// <summary>
   /// Constructor for the editor manager
   /// </summary>
   /// <param name="rowAmount">Max row count</param>
   /// <param name="colAmount">Max column count</param>
   public EditorController(int rowAmount, int colAmount)
   {
      rAmount = rowAmount;
      cAmount = colAmount;
      movedBlock = null;
      BlockColor = Color.Black;

      BlockHealth = 1;
      int dummyLevelValue = 0;
      editingLevel = new Level(dummyLevelValue, dummyLevelValue, dummyLevelValue, dummyLevelValue, dummyLevelValue);
   }


  /// <summary>
  /// Add a dummy block at given row and column with current color and health
  /// </summary>
  /// <param name="row">Row position</param>
  /// <param name="col">Column position</param>
  /// <returns></returns>
   public bool AddBlock(int row, int col)
   {
      int dummyValue = 0;
      Block tempBLock = new Block(dummyValue, dummyValue, dummyValue, dummyValue, BlockColor, BlockHealth);
      return editingLevel.AddBlock(tempBLock, row, col);
   }


   /// <summary>
   /// Add an enemy to level at given column
   /// </summary>
   /// <param name="col"></param>
   /// <returns></returns>
   public bool AddEnemy(int col)
   {
      int dummyValue = 0;
      Enemy e = new Enemy(dummyValue, dummyValue, dummyValue, dummyValue, System.Drawing.Color.Black, BlockHealth, null, null, null);
      return editingLevel.AddEnemy(e, col);
   }

   /// <summary>
   /// Loads a specific level
   /// </summary>
   /// <param name="level">the level id to load from</param>
   public bool Load(int levelNum)
   {
      if (0 <= levelNum && levelNum <= 10)
      {
         int dummyValue = 0;
         editingLevel = new Level(levelNum, dummyValue, dummyValue, dummyValue, dummyValue);
         editingLevel.LoadFromFile();
         return true;
      }
      return false;
   }

   /// <summary>
   /// Set the editing level to the default level design
   /// </summary>
   public void SetToDefault()
   {
      editingLevel.Default = true;
      editingLevel.LevelNumber = 0;
      editingLevel.LoadFromFile();
   }

   /// <summary>
   /// For testing only
   /// </summary>
   /// <returns></returns>
   public bool IsDefaultTest()
   {
      int dummyValue = 0;
      Level defaultLevel = new Level(0, dummyValue, dummyValue, dummyValue, dummyValue);
      defaultLevel.Default = true;
      defaultLevel.LoadFromFile();

      return defaultLevel.Equals(editingLevel);
   }

   /// <summary>
   /// For testing only
   /// </summary>
   /// <param name="lvlNum"></param>
   /// <returns></returns>
   public bool CompareLevelTest(int lvlNum)
   {
      int dummyValue = 0;
      Level testLevel = new Level(lvlNum, dummyValue, dummyValue, dummyValue, dummyValue);
      testLevel.LevelNumber = 0;
      return testLevel.Equals(editingLevel);
   }

   /// <summary>
   /// Sets the editor level to not the default level
   /// Sets the level to its normal number
   /// </summary>
   private void SetToNormal()
   {
      editingLevel.Default = false;
      editingLevel.LevelNumber = 0;
   }

   /// <summary>
   /// Remove enemy at specified column
   /// Due to how enemies are removed, currently column is converted to: col % (enemy count)
   /// </summary>
   /// <param name="col"></param>
   public void RemoveEnemy(int col)
   {
      List<Enemy> enemies = editingLevel.GetEnemies();
      int dampner = col % enemies.Count;
      editingLevel.RemoveEnemy(enemies[dampner]);
   }

   /// <summary>
   /// Remove block at given row and column position
   /// </summary>
   /// <param name="row"></param>
   /// <param name="col"></param>
   /// <returns></returns>
   public bool RemoveBlock(int row, int col)
   {
         return editingLevel.RemoveBlock(row, col);
   }

   /// <summary>
   /// Save edited level into the first level file
   /// </summary>
   /// <returns>True if level has blocks and saved to file</returns>
   public bool Save()
   {
      SetToNormal();
      bool empty = true;
      foreach (Block b in editingLevel.GetBlocks())
      {
         if (b != null)
         {
            empty = false;
            break;
         }
      }
      if (!empty)
         editingLevel.SaveToFile();
      return !empty;
   }

   /// <summary>
   /// Clears every block and enemy in the level
   /// </summary>
   public void ClearLevel()
   {
      for (int i = 0; i < rAmount; i++)
      {
         for (int j = 0; j < cAmount; j++)
         {
            editingLevel.RemoveBlock(i, j);
         }
      }
      List<Enemy> enemies = editingLevel.GetEnemies();

      for (int i = 0; i < enemies.Count; i++)
      {
         editingLevel.RemoveEnemy(enemies[i]);
      }
   }

   /// <summary>
   /// gets all blocks from a level
   /// </summary>
   /// <returns></returns>
   public Block[,] GetLevelBlocks()
   {
      return editingLevel.GetBlocks();
   }

   /// <summary>
   /// gets all enemies from a level
   /// </summary>
   /// <returns></returns>
   public List<Enemy> GetLevelEnemies()
   {
      return editingLevel.GetEnemies();
   }
}
