//--------------------------------------------------------
// Authors: David, Matthew, Levon, Alex, and Derek
// Program: Batty
// Purpose: To handle the playing of a game, along with 
//          enforcing the game rules during the game
//--------------------------------------------------------
using Batty_2._0;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Timers;
using System.Threading;

public class GameManager
{
   public const int MAX_LEVEL = 10; // Edit for testing

   public const int NUM_ROWS = 12, NUM_COLS = 10;

   private const double BAR_SIZE_MULTIPLIER = .35;
   private const double BAR_SIZE_MULTIPLIER_BASE = .05;

   private bool isPaused, muted, waitingForStart;

   public enum GameState { InProgress, Won, Lost, LevelEnd }

   public GameState gameState { get; set; }

   private int livesInitial, score, currentLevel;

   private List<Bomb> bombs;

   private Ball ball;

   private Bar bar;

   private Level level;

   private Color barColor, ballColor;

   public static float screenWidth, screenHeight;

   private float offsetWidth, offsetHeight;

   bool ballCollided;
   
   SoundPlayer soundPlayer;

   private float LevelTime;

   private CollisionManager colManager;

   public int Lives { get { return bar.Lives; } }

   public int Level { get { return currentLevel; } }

   public int Score { get { return score; } }

   public int BallSpeed { get { return ball.Speed; } }

   public bool Paused { get { return isPaused; } }

   public bool Muted { get { return muted; } }


   // Get is for testing only
   public Color BallColor { set { ball.color = value; } get { return ball.color; } }

   // Get is for testing only
   public Color BarColor { set { bar.color = value; } get { return bar.color; } }

   private List<GameObject> objects;

   /// <summary>
   /// Creates a new GameManger, initializes game data, and loads the first level
   /// </summary>
   /// <param name="lives">The number of lives to give the player</param>
   /// <param name="ball_shape">The selected ball shape</param>
   /// <param name="screenW">The screen width of the game</param>
   /// <param name="screenH">The screen height of the game</param>
   /// <param name="offsetW">The offset to place items from the left side of the screen</param>
   /// <param name="offsetH">The offset to place items from the top of the screen</param>
   /// <param name="barColor">Sets the bar color</param>
   /// <param name="ballColor">Sets the ball color</param>
   public GameManager(int lives, int ball_shape, float screenW, float screenH,
      float offsetW, float offsetH, Color barColor, Color ballColor)
   {
      livesInitial = lives;
      isPaused = false;
      score = 0;
      ballCollided = false;
      soundPlayer = new SoundPlayer(@"../../../Batty 2.0/Resources/Sounds/Bounce.wav");
      screenWidth = screenW;
      screenHeight = screenH;
      offsetHeight = offsetH;
      offsetWidth = offsetW;
      this.barColor = barColor;
      this.ballColor = ballColor;
      Restart();
      colManager = new CollisionManager();
   }

   /// <summary>
   /// Resets the game to the initial game state
   /// </summary>
   public void Restart()
   {
      score = 0;
      gameState = GameState.InProgress;
      currentLevel = -1;
      LoadNextLevel();
      bar.Lives = livesInitial;
      isPaused = false;
   }

   /// <summary>
   /// Restarts the current level (without refreshing lives)
   /// </summary>
   public void ReloadLevel()
   {
      --currentLevel;
      LoadNextLevel();
   }

   /// <summary>
   /// Loads the next level and checks the game won condition
   /// </summary>
   private void LoadNextLevel()
   {
      int currLives = livesInitial;
      if (bar != null)
         currLives = bar.Lives;
      ++currentLevel;
      if (currentLevel <= MAX_LEVEL)
      {
         waitingForStart = true;

         bombs = new List<Bomb>();
         int barW = (int)screenWidth / 5;
         int barH = (int)screenHeight / 20;
         int barSpeed = (int)screenWidth / 75 + 1;
         bar = new Bar((int)(screenWidth - barW) / 2, (int)(screenHeight - barH * 2), barW,
            barH, barColor, currLives, barSpeed);
         int ballH = barH * 3 / 4;
         int ballW = ballH;
         int ballSpeed = (int)(ballH * .25);
         ball = new Ball((int)(screenWidth - ballW) / 2, (int)(screenHeight - ballH - barH * 2), ballW,
            ballH, ballColor, ballSpeed);
         ball.InitSpeed = barSpeed;
         UpdateBallMaxMin();

         bar.image = Image.FromFile("../../../Batty 2.0/Resources/Bar.png");
         ball.image = Image.FromFile("../../../Batty 2.0/Resources/MessyBallV2.png");

         level = new Level(currentLevel, (int)((screenHeight - (2 * offsetHeight)) / NUM_ROWS),
              (int)((screenWidth - (2 * offsetWidth)) / NUM_COLS), (int)offsetWidth, (int)offsetHeight);
         level.LoadFromFile();
         gameState = GameState.InProgress;
         LevelTime = 180 + 10 * currentLevel;
         FillObjects();
      }
      else
         gameState = GameState.Won;
   }

   /// <summary>
   /// Fills the objects list, which is the list which gets drawn
   /// </summary>
   private void FillObjects()
   {
      objects = new List<GameObject>();
      foreach (GameObject o in level.GetEnemies())
      {
         if (o != null)
            objects.Add(o);
      }

      objects.Add(ball);
      objects.Add(bar);
      foreach (Block b in level.GetBlocks())
      {
         if (b != null)
            objects.Add(b);
      }

   }

   /// <summary>
   /// Gets the list of objects to be drawn
   /// </summary>
   /// <returns>The list of objects to draw</returns>
   public List<GameObject> GetObjects()
   {
      return objects;
   }

   /// <summary>
   /// Changes the ball image
   /// </summary>
   /// <param name="image">The new ball image</param>
   public void ModifyBallShape(Image image)
   {
      ball.image = image;
   }

   /// <summary>
   /// Updates the ball speed
   /// </summary>
   /// <param name="newSpeed">The new speed of the ball</param>
   [System.Obsolete("ModifyBallSpeed is deprecated. Use ChangeBallSpeed method")]
   public void ModifyBallSpeed(int newSpeed)
   {
      ball.Speed = newSpeed;
   }

   /// <summary>
   /// Updates the bar color
   /// </summary>
   /// <param name="c">The new bar color</param>
   [System.Obsolete("ModifyBarColor is deprecated. Use BarColor property")]
   public void ModifyBarColor(Color c)
   {
      bar.color = c;
   }

   /// <summary>
   /// Updates the bar length
   /// </summary>
   /// <param name="len">The new bar length</param>
   [System.Obsolete("ModifyBarLength is deprecated. Use ChangeBarSize method")]
   public void ModifyBarLength(int len)
   {
      bar.Width = len;
   }

   /// <summary>
   /// Updates the number of lives
   /// </summary>
   /// <param name="lives">The new number of lives</param>
   public void ModifyLives(int lives)
   {
      bar.Lives = lives;
   }

   /// <summary>
   /// Updates the ball color
   /// </summary>
   /// <param name="c">The new ball color</param>
   [System.Obsolete("ModifyBallColor is deprecated. Use BallColor property")]
   public void ModifyBallColor(Color c)
   {
      ball.color = c;
   }

   /// <summary>
   /// Loads the next level
   /// </summary>
   public void SkipLevel()
   {
      LoadNextLevel();
   }

   /// <summary>
   /// Toggles the paused state of the game
   /// </summary>
   public void TogglePause()
   {
      isPaused = !isPaused;
   }

   /// <summary>
   /// Toggles the sound of the game
   /// </summary>
   public void ToggleSound()
   {
      muted = !muted;
   }

   /// <summary>
   /// Starts the round
   /// </summary>
   public void GameStart()
   {
      if (waitingForStart)
      {
         waitingForStart = false;
         ball.xDirection = 1;
         ball.yDirection = -4;
         ball.Released = true;
         isPaused = false;
      }
   }

   /// <summary>
   /// Checks if the level has been beat yet
   /// </summary>
   /// <param name="blocks">The list of blocks currently in the level</param>
   /// <returns>True if the round has no remaining blocks, and false otherwise</returns>
   private bool CheckEndLevel(Block[,] blocks)
   {
      bool endLevel = true;
      foreach (Block b in blocks)
      {
         if (b != null)
         {
            endLevel = false;
            return endLevel;
         }
      }
      return endLevel;
   }

   /// <summary>
   /// Updates the bar size by a percentage of its length
   /// </summary>
   /// <param name="modifier"></param>
   public void ChangeBarSize(int modifier)
   {
      bar.SizeModifier = (modifier * BAR_SIZE_MULTIPLIER) + BAR_SIZE_MULTIPLIER_BASE;
      UpdateBallMaxMin();
   }

   /// <summary>
   /// Updates the maximum and minimum of the ball (so collisions work)
   /// </summary>
   private void UpdateBallMaxMin()
   {
      if (waitingForStart)
      {
         ball.X = bar.X + bar.Width / 2 - ball.Width / 2;
         ball.MaxInitPos = (int)((int)screenWidth - (int)(bar.Width / 2) - (int)(ball.Width / 2));
         ball.MinInitPos = (int)((int)(bar.Width / 2) - (int)(ball.Width / 2));
      }
      ball.DefaultX = (int)(bar.DefaultX + (int)(bar.Width / 2) - (int)(ball.Width / 2));
   }

   /// <summary>
   /// Updates the ball speed by a percentage of the ball speed
   /// </summary>
   /// <param name="modifier"></param>
   public void ChangeBallSpeed(int modifier)
   {
      ball.SpeedModifier = (modifier * BAR_SIZE_MULTIPLIER) + BAR_SIZE_MULTIPLIER_BASE;
   }

   /// <summary>
   /// Decrements the timer by a change in time
   /// </summary>
   /// <param name="DeltaTime">The time between the change and now in miliseconds</param>
   /// <returns>the current time left on level</returns>
   public int DecrimenntTime(float DeltaTime)
   {
      LevelTime = LevelTime - (DeltaTime / 1000);
      if (LevelTime <= 0)
      {
         for (int i = bar.Lives; i > 0; i--)
         {
            LoseLife();
         }
         return 0;
      }
      else
      {
         return ((int)LevelTime + 1);
      }
   }

   /// <summary>
   /// Adds a number of points to the current score
   /// </summary>
   /// <param name="points"></param>
   public void AddPoints(int points)
   {
      score += points;
   }

   /// <summary>
   /// Updates all of the game objects, and controls the gameplay
   /// </summary>
   public void Update()
   {
      if (gameState == GameState.InProgress)
      {
         if (!isPaused)
         {
            bar.Update();
            ball.Update();

            if (!waitingForStart)
            {
               //Code to update goes here
               //----------------------------------------------------------------------------------
               level.UpdateEnemies();

               for (int i = 0; i < level.GetEnemies().Count; i++)
               {
                  Enemy e = level.GetEnemies()[i];
                  if (colManager.CheckCircleRectCollision(ball, e))
                  {
                     ballCollided = true;
                     e.Health -= 1;
                     if (e.Health <= 0)
                     {
                        i -= 1;
                        objects.Remove(e);
                        level.RemoveEnemy(e);
                     }
                  }
                  if (e.DroppedBomb)
                  {
                     Bomb b = new Bomb((int)e.X, (int)e.Y, (int)ball.Width, (int)ball.Height, Color.DarkRed, 3);
                     objects.Add(b);
                     bombs.Add(b);
                     e.DroppedBomb = false;
                     b.image = Image.FromFile("../../../Batty 2.0/Resources/Bomb.png");
                  }
               }
               List<Bomb> tempBombs = new List<Bomb>(bombs);
               foreach (Bomb b in tempBombs)
               {
                  b.Update();
                  if (colManager.CheckCircleCircleCollision(ball, b))
                  {
                     objects.Remove(b);
                     bombs.Remove(b);
                  }
                  if (colManager.CheckCircleRectCollision(b, bar))
                  {
                     foreach (Bomb bomb in tempBombs)
                        objects.Remove(bomb);
                     LoseLife();
                  }
               }

               if (colManager.CheckCircleRectCollision(ball, bar))
               {
                  //ballCollided = true;
                  ball.AddCollisionDirection(colManager.FindCircleCollisionSide(ball, bar));
                  float xBall = ball.X + ball.Width / 2;
                  float xBar = bar.X + bar.Width / 2;
                  if (ball.Y + ball.Height / 2 <= bar.Y + bar.Y / 2)
                  {
                     float ratio = (xBall - xBar) / (bar.Width / 2);
                     float angle = ratio * 75;//90-75 = 15 min angle
                     ball.SetBallAngle(90 + angle);
                     ball.HandleCollisions(GameObject.GameObjectType.BAR);
                  }
                  Thread thread = new Thread(new ThreadStart(AudioManager.PlayHit));
                  thread.Start();
               }
               else if (ball.atBottom) //ball hit bottom
               {
                  LoseLife();
               }
               else
               {
                  bool hit = false;
                  foreach (Block b in level.GetBlocks())
                  {
                     if (b != null)
                     {
                        if (colManager.CheckCircleRectCollision(ball, b))
                        {
                           ballCollided = true;
                           ball.AddCollisionDirection(colManager.FindCircleCollisionSide(ball, b));
                           b.HandleCollisions(GameObject.GameObjectType.BALL);
                           hit = true;
                           if (level.RemoveBlock(b))
                           {
                              score += b.pointValue;
                              objects.Remove(b);
                              if (CheckEndLevel(level.GetBlocks()))
                                 gameState = GameState.LevelEnd;
                           }
                           Thread thread = new Thread(new ThreadStart(AudioManager.PlayHit));
                           thread.Start();
                        }
                     }
                  }
                  if (hit)
                     ball.HandleCollisions(GameObject.GameObjectType.BLOCK);
               }

               /*
               if (!muted && ballCollided)
               {
                  soundPlayer.Play();
                  ballCollided = false;
               }
               */
            }
            //----------------------------------------------------------------------------------
         }
         if (waitingForStart && Controls.Instance.ControlWasPressed(Controls.ControlType.Shoot) &&
            gameState == GameState.InProgress && !isPaused)
            GameStart();
         if (bar.Lives <= 0)
         {
            gameState = GameState.Lost;
            Thread thread = new Thread(new ThreadStart(AudioManager.PlayGameOver));
            thread.Start();
         }
      }

   }

   /// <summary>
   /// Lose a life and handle the repercussions of that
   /// </summary>
   private void LoseLife()
   {
      bar.Lives--;
      if (bar.Lives > 0)
      {
         waitingForStart = true;
         bar.Reset();
         ball.Reset();
      }
      foreach (Bomb b in bombs)
         objects.Remove(b);
      bombs = new List<Bomb>();

      Thread thread = new Thread(new ThreadStart(AudioManager.PlayLoseLife));
      thread.Start();
   }

   // Start Test Drivers
   //-----------------------------------------------------------

   /// <summary>
   /// Tests that the end of a level is detected
   /// </summary>
   /// <returns></returns>
   public bool TestEndLevel()
   {
      Block[,] blocksTest = new Block[2, 2]
      {
         {null, null },
         {null, null }
      };

      return CheckEndLevel(blocksTest);
   }

   /// <summary>
   /// Tests that the end of a level isn't falsly detected
   /// </summary>
   /// <returns></returns>
   public bool TestNotEndLevel()
   {
      Block[,] blocksTest = new Block[2, 2]
      {
         {null, null },
         {null, new Block(0, 0, 0, 0, Color.Yellow, 0) }
      };

      return CheckEndLevel(blocksTest);
   }

   /// <summary>
   /// Gets the number of enemies
   /// </summary>
   /// <returns></returns>
   public int GetEnemyCount()
   {
      return level.GetEnemies().Count;
   }

   /// <summary>
   /// Set up a collision between an enemy and the ball
   /// </summary>
   public void SetUpEnemyBallCollision()
   {
      Enemy e = new Enemy(50, 50, 50, 50, Color.Yellow, 1, null, null, null);
      ball = new Ball(50, 50, 50, 50, Color.Yellow, 0);
      level.AddEnemy(e, 1);
   }

   /// <summary>
   /// Set up a collision between a bomb and the ball
   /// </summary>
   public int SetUpBombBallCollision()
   {
      int ret = bombs.Count;
      Bomb b = new Bomb(50, 50, 50, 50, Color.Yellow, 0);
      ball = new Ball(50, 50, 50, 50, Color.Yellow, 0);
      bombs.Add(b);
      return ret;
   }

   /// <summary>
   /// Set up a collision between a bomb and the bar
   /// </summary>
   public void SetUpBombBarCollision()
   {
      Bomb b = new Bomb(50, 50, 50, 50, Color.Yellow, 0);
      bar = new Bar(50, 50, 50, 50, Color.Yellow, 1, 1);
      bombs.Add(b);
   }

   /// <summary>
   /// Set up an enemy to drop a bomb
   /// </summary>
   public int SetUpBombDrop()
   {
      int ret = bombs.Count;
      Enemy e = new Enemy(50, 50, 50, 50, Color.Yellow, 1, null, null, null);
      e.DroppedBomb = true;
      level.AddEnemy(e, 1);
      return ret;
   }

   /// <summary>
   /// Set up a collision between a bomb and the ball
   /// </summary>
   public float SetUpBarBallCollision()
   {
      float ret = ball.xDirection;
      bar = new Bar(50, 50, 50, 50, Color.Yellow, 1, 1);
      ball = new Ball(50, 50, 50, 50, Color.Yellow, 0);
      return ret;
   }

   /// <summary>
   /// Sets up a collision between a block and the ball
   /// </summary>
   public void SetUpBallBlockCollision()
   {
      foreach (Block b in level.GetBlocks())
      {
         if (b != null)
         {
            ball.X = b.X;
            ball.Y = b.Y;
            return;
         }
      }
   }

   /// <summary>
   /// Get the number of blocks that don't exsit
   /// </summary>
   /// <returns></returns>
   public int GetDeadBlocksCount()
   {
      int count = 0;
      foreach (Block b in level.GetBlocks())
      {
         if (b == null)
         {
            count += 1;
         }
      }
      return count;
   }

   /// <summary>
   /// Set up a collision with the ball on the bottom of the screen
   /// </summary>
   public void SetUpBallBottomCollision()
   {
      ball.Y = screenHeight;
      bar.Lives = 1;
   }
   //------------------------------------------------------------
   // End Test Drivers
}
