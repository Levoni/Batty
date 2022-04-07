//--------------------------------------------------------
// Authors: David, Matthew, Levon, Alex, and Derek
// Program: Batty
// Purpose: Handle a block game object
//--------------------------------------------------------
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class Block : GameObject
{
   public const int POINT_MULTIPLIER = 10;
   private const int DEFAULT_IMG_WIDTH = 320;
   private const int DEFAULT_IMG_HEIGHT = 160;

   /// <summary>
   /// Constructor.
   /// </summary>
   /// <param name="xPos">The x-coordinate of the block.</param>
   /// <param name="yPos">The y-coordinate of the block.</param>
   /// <param name="oWidth">The width of the block.</param>
   /// <param name="oHeight">The height of the block.</param>
   /// <param name="oColor">The color of the block.</param>
   /// <param name="health">The number of hits required to destroy the
   ///        block.</param>
   public Block(int xPos, int yPos, int oWidth, int oHeight, Color oColor, int health) :
      base(xPos, yPos, oWidth, oHeight, oColor)
   {
      type = GameObjectType.BLOCK;
      this.health = health;
      pointValue = POINT_MULTIPLIER * health;
      if (File.Exists("../../../Batty 2.0/Resources/Blocks/" + color.R + color.G + color.B + ".png"))
         image = Image.FromFile("../../../Batty 2.0/Resources/Blocks/" + color.R +
            color.G + color.B + ".png");
   }

   // the number of hits to destroy block
   public int health { get; set; }

   //the points awared after destroying block
   public int pointValue { get; }

   //protected Powerup itsPowerup;

      /// <summary>
      /// Compare current block with the given obj
      /// Returns true only if the given obj is a valid 
      /// with the same info as the current block
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
   public override bool Equals(object obj)
   {
      if (obj == null || (obj.GetType() != this.GetType()))
         return false;

      Block block = (Block)obj;

      if (X == block.X && Y == block.Y && Width == block.Width &&
         Height == block.Height && color.ToArgb() == block.color.ToArgb() &&
         health == block.health)
         return true;
      return false;
   }
}