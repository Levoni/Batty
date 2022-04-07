using System.Drawing;
using System;
using Batty_2._0;

public class Bar : GameObject
{
   private int defaultWidth; 
   public int DefaultX { get; private set; }
   private int DefaultY { get; set; }

   /// <summary>
   /// Constructor for bar
   /// </summary>
   /// <param name="xPos"></param>
   /// <param name="yPos"></param>
   /// <param name="oWidth"></param>
   /// <param name="oHeight"></param>
   /// <param name="oColor"></param>
   /// <param name="lives"></param>
   public Bar(int xPos, int yPos, int oWidth, int oHeight, Color oColor, int lives, int speed) : base(xPos, yPos, oWidth, oHeight, oColor)
   {
      type = GameObjectType.BAR;
      this.Lives = lives;
      this.Speed = speed;

      DefaultX = xPos;
      DefaultY = yPos;

      SizeModifier = 1;
      defaultWidth = oWidth;
      Width = defaultWidth;
   }

   /// <summary>
   /// Performs logic for collision with GameObject
   /// </summary>
   /// <param name="otherType"></param>
   public override void HandleCollisions(GameObjectType otherType)
   {
      if (otherType == GameObjectType.BOMB)
         Lives--;
      // Add cases for powerup if implemented
   }

   /// <summary>
   /// Moves the bar's position based on input
   /// </summary>
   public override void Update()
   {
      base.Update();
      if (Controls.Instance.ControlWasPressed(Controls.ControlType.Left))
         X -= Speed;
      if (Controls.Instance.ControlWasPressed(Controls.ControlType.Right))
         X += Speed;
      X = Math.Max(X, 0);
      X = Math.Min(X, GameManager.screenWidth - Width);
   }

   /// <summary>
   /// resets bar position 
   /// </summary>
   public void Reset()
   {
      Y = DefaultY;
      X = DefaultX;
   }

   // the players current lives
   public int Lives { get; set; }

   // speed of the bar
   public int Speed { get; set; }

   // how much the width is scaled
   public double SizeModifier { set { this.Width = (int)(value * defaultWidth); } }
}
