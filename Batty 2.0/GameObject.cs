using System.Drawing; 

public class GameObject
{
   /// <summary>
   /// Constructor for game object
   /// </summary>
   /// <param name="xPos"></param>
   /// <param name="yPos"></param>
   /// <param name="oWidth"></param>
   /// <param name="oHeight"></param>
   /// <param name="oColor"></param>
   public GameObject(int xPos, int yPos, int oWidth, int oHeight, Color oColor)
   {
      X = xPos;
      Y = yPos;
      Width = oWidth;
      Height = oHeight;
      color = oColor;
      image = null;
   }

   public enum GameObjectType 
    {
        BAR,
        BALL,
        BLOCK,
        BOMB,
        ENEMY
    }

   public GameObjectType type { get; set; }

   public float Height { get; set; }

   public float Width {get; set;}

   public float X { get; set; }

   public float Y { get; set; }

   public Color color { get; set; }

   public Image image { get; set; }

   /// <summary>
   /// Default collision (unused) 
   /// </summary>
   /// <param name="otherType"></param>
   public virtual void HandleCollisions(GameObjectType otherType)
   {
      ;
   }
     
   /// <summary>
   /// Default Update (Unused)
   /// </summary>
   public virtual void Update()
   {
      ;
   }

}
