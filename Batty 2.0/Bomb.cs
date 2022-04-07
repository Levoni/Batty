using System.Drawing;
public class Bomb : GameObject
{
   /// <summary>
   /// Constructor for bomb
   /// </summary>
   /// <param name="xPos"></param>
   /// <param name="yPos"></param>
   /// <param name="oWidth"></param>
   /// <param name="oHeight"></param>
   /// <param name="oColor"></param>
   /// <param name="speed"></param>
   public Bomb(int xPos, int yPos, int oWidth, int oHeight, Color oColor, int speed) : base(xPos, yPos, oWidth, oHeight, oColor)
   {
      type = GameObjectType.BOMB;
      this.speed = speed;
   }
    
   // how fast bombs travel down
    public int speed;

    /// <summary>
    /// changes the position of bomb each frame
    /// </summary>
    public override void Update()
   {
      Y += speed;
   }
}