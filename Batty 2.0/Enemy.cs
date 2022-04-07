using System;
using System.Drawing;
using System.Collections.Generic;
public class Enemy : GameObject
{
   public const int SPEED = 5;

   private int xDirection;
   
   public bool DroppedBomb { get; set; }

   private Random random;
   static int randomSeedValue = 0;

   private List<Image> animationCycle;
   private int animationCounter;

   private const int FRAME_SPEED = 4;
   private int frameCounter;

   /// <summary>
   /// Constructor.
   /// </summary>
   /// <param name="xPos">The x-coordinate of the enemy in pixels.</param>
   /// <param name="yPos">The y-coordinate of the enemy in pixels.</param>
   /// <param name="oWidth">The width of the enemy in pixels.</param>
   /// <param name="oHeight">The height of the enemy in pixels.</param>
   /// <param name="oColor">The color of the enemy.</param>
   /// <param name="health">The health of the enemy.</param>
   public Enemy(int xPos, int yPos, int oWidth, int oHeight, Color oColor, int health, Image frame1, Image frame2, Image frame3) : base(xPos, yPos, oWidth, oHeight, oColor)
   {
      type = GameObjectType.ENEMY;
      this.Health = health;
      DroppedBomb = false;
      xDirection = 1;
      random = new Random(randomSeedValue);
      randomSeedValue += 100;
      animationCycle = new List<Image>(new[] { frame1, frame2, frame3, frame2 });
      image = animationCycle[0];
      animationCounter = 0;
      frameCounter = 0;
   }

   public override void Update()
   {
      int randomDirectionChange = random.Next(100);
      int randomBombDrop = random.Next(1000);

      if (randomDirectionChange == 1)
         xDirection = -xDirection;

      if (randomBombDrop == 1 && DroppedBomb == false)
         DroppedBomb = true;

      if (frameCounter == FRAME_SPEED)
      {
         frameCounter = 0;
         animationCounter = (animationCounter + 1) % animationCycle.Count;
         image = animationCycle[animationCounter];

         if (xDirection == 1)
         {
            if (X + SPEED + Width >= GameManager.screenWidth)
            {
               X = GameManager.screenWidth - Width;
               xDirection = -1;
            }
            else
               X += SPEED;
         }
         else if (xDirection == -1)
         {
            if (X - SPEED <= 0)
            {
               X = 0;
               xDirection = 1;
            }
            else
               X -= SPEED;
         }
      }
      else
         ++frameCounter;
      
      


      //Bitmap map = new Bitmap("");
      //map.
   }
   //Bitmap b = new Bitmap()
   public int Health { get; set; }

   /// <summary>
   /// Compares the current Enemy object to a given object and returns true
   /// if the other object is an instance of the Enemy class and if all
   /// class variables are the same between the two.
   /// </summary>
   /// <param name="obj">The object to compare to.</param>
   /// <returns></returns>
   public override bool Equals(object obj)
   {
      if (obj == null || (obj.GetType() != this.GetType()))
         return false;

      Enemy enemy = (Enemy)obj;

      if (X == enemy.X && Y == enemy.Y && Width == enemy.Width &&
         Height == enemy.Height && color == enemy.color &&
         Health == enemy.Health)
         return true;
      return false;
   }
}
