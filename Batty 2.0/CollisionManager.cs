using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CollisionManager
{ 

   /// <summary>
   /// checks if two rectangular game objects collide
   /// </summary>
   /// <param name="rectOne">rectangular gameobject</param>
   /// <param name="rectTwo">rectangular gameobject</param>
   /// <returns></returns>
   public bool CheckRectRectCollision(GameObject rectOne, GameObject rectTwo)
   {
      if (rectOne.X <= rectTwo.X + rectTwo.Width && rectOne.X + rectOne.Width >= rectTwo.X
         && rectOne.Y <= rectTwo.Y + rectTwo.Height && rectOne.Y + rectOne.Height >= rectTwo.Y)
         return true;
      return false;
   }

   /// <summary>
   /// checks if a circular game object collides with a rectangualr one
   /// </summary>
   /// <param name="Circle">circular gameobject</param>
   /// <param name="Rectangle">rectangular gameobject</param>
   /// <returns></returns>
   public bool CheckCircleRectCollision(GameObject Circle, GameObject Rectangle)
   {
      if (Circle.X <= Rectangle.X + Rectangle.Width && Circle.X + Circle.Width >= Rectangle.X
         && Circle.Y <= Rectangle.Y + Rectangle.Height && Circle.Y + Circle.Height >= Rectangle.Y)
      {
         bool collision = false;
         float cXMiddle = Circle.X + Circle.Width / 2;
         float cYMiddle = Circle.Y + Circle.Height / 2;
         float cRadius = Circle.Width / 2;
         float rRight = Rectangle.X + Rectangle.Width;
         float rBottom = Rectangle.Y + Rectangle.Height;

         if (CheckLineInsideCircle(cRadius, cXMiddle, cYMiddle, Rectangle.X, rRight, Rectangle.Y, Rectangle.Y))
         {
            collision = true;
         }
         else if (CheckLineInsideCircle(cRadius, cXMiddle, cYMiddle, Rectangle.X, rRight, rBottom, rBottom))
         {
            collision = true;
         }
         else if (CheckLineInsideCircle(cRadius, cXMiddle, cYMiddle, rRight, rRight, Rectangle.Y, rBottom))
         {
            collision = true;
         }
         else if (CheckLineInsideCircle(cRadius, cXMiddle, cYMiddle, Rectangle.X, Rectangle.X, Rectangle.Y, rBottom))
         {
            collision = true;
         }
         else if (CircleInsideRect(cRadius, cXMiddle, cYMiddle, Rectangle.X, rRight, Rectangle.Y, rBottom))
         {
            collision = true;
         }
         return collision;
      }
      return false;
   }

   /// <summary>
   /// Checks a circle circle Collision;
   /// </summary>
   /// <param name="circleOne">First circle gameobject</param>
   /// <param name="circleTwo">Second circle gameobject</param>
   /// <returns>True if collision false otherwise</returns>
   public bool CheckCircleCircleCollision(GameObject circleOne, GameObject circleTwo)
   {
      float radiusOne = circleOne.Width / 2;
      float radiusTwo = circleTwo.Width / 2;
      float cXOne = circleOne.X + circleOne.Width / 2;
      float cYOne = circleOne.Y + circleOne.Height / 2;
      float cXTwo = circleTwo.X + circleTwo.Width / 2;
      float cYTwo = circleTwo.Y + circleTwo.Height / 2;

      float xDif = cXTwo - cXOne;
      float yDif = cYTwo - cYOne;
      if (xDif * xDif + yDif * yDif <= (radiusOne + radiusTwo) * (radiusOne + radiusTwo))
         return true;
      return false;
   }


   /// <summary>
   /// Checks if a circle gameobject is enclosed by a rectagular gameobject
   /// </summary>
   /// <param name="circleRadius">radius of the circle</param>
   /// <param name="xCircle">center x pixel position of the circle</param>
   /// <param name="yCircle">center y pixel position of the circle</param>
   /// <param name="xMin">Minimum x pixel position of the rectangle</param>
   /// <param name="xMax">Maximum x pixel position of the rectangle</param>
   /// <param name="yMin">Minimum y pixel position of the rectangle</param>
   /// <param name="yMax">Maximum y pixel position of the rectangle</param>
   /// <returns>True if the circle is enveloped by the rectangle false otherwise</returns>
   private bool CircleInsideRect(float circleRadius, float xCircle, float yCircle, float xMin, float xMax, float yMin, float yMax)
   {
      if (xMax >= xCircle + circleRadius && xMin <= xCircle - circleRadius
         && yMax >= yCircle + circleRadius && yMin <= yCircle - circleRadius)
         return true;
      return false;
   }

   /// <summary>
   /// Checks if any point of a line is inside of the circle
   /// </summary>
   /// <param name="circleRadius">radius of the circle</param>
   /// <param name="xCircle">center x pixel position of the circle</param>
   /// <param name="yCircle">center y pixel position of the circle</param>
   /// <param name="xOne">x pixel position of the first endpoint on the line</param>
   /// <param name="xTwo">x pixel position of the second endpoint on the line</param>
   /// <param name="yOne">y pixel position of the first endpoint on the line</param>
   /// <param name="yTwo">y pixel position of the second endpoint on the line</param>
   /// <returns></returns>
   private bool CheckLineInsideCircle(float circleRadius, float xCircle, float yCircle, float xOne, float xTwo, float yOne, float yTwo)
   {
      float xDif = xTwo - xOne;
      float yDif = yTwo - yOne;

      float xChange = xDif / 250;
      float yChange = yDif / 250;

      float xPos = xOne;
      float yPos = yOne;

      for (int i = 0; i < 501; i++)
      {
         if (CheckPointInsideCircle((int)circleRadius, (int)xCircle, (int)yCircle, (int)xPos, (int)yPos))
         {
            return true;
         }
         xPos += xChange;
         yPos += yChange;
      }
      return false;
   }

   /// <summary>
   /// Checks if a point is inside of a circle
   /// </summary>
   /// <param name="radius">radius of the circle</param>
   /// <param name="xCircle">top left x pixel position of the circle</param>
   /// <param name="yCircle">top left y pixel position of the circle</param>
   /// <param name="xPoint">x pixel position of the point to check</param>
   /// <param name="yPoint">y pixel position of the point to check</param>
   /// <returns></returns>
   private bool CheckPointInsideCircle(int radius, int xCircle, int yCircle, int xPoint, int yPoint)
   {
      float xDif = xCircle - xPoint;
      float yDif = yCircle - yPoint;

      if ((xDif * xDif + yDif * yDif) <= radius * radius)
         return true;
      return false;
   }

   /// <summary>
   /// Finds the side the circle collided with the rctangle on
   /// </summary>
   /// <param name="circle">Circular gameobjcect</param>
   /// <param name="Rectangle">Rectangular gameobject</param>
   /// <returns>integer for side the circle was hit on, 1=top , = top-right,..., 7= top-left</returns>
   public List<Direction> FindCircleCollisionSide(GameObject circle, GameObject Rectangle)
   {
      List<Direction> dList = new List<Direction>();
      if (CheckPointInsideBox((int)Rectangle.X, (int)Rectangle.Y, (int)Rectangle.Width, (int)Rectangle.Height, (int)(circle.X + (circle.Width / 2)), (int)circle.Y))
         dList.Add(Direction.TOP);
      if (CheckPointInsideBox((int)Rectangle.X, (int)Rectangle.Y, (int)Rectangle.Width, (int)Rectangle.Height, (int)(circle.X + circle.Width), (int)(circle.Y + (circle.Height / 2))))
         dList.Add(Direction.RIGHT);
      if (CheckPointInsideBox((int)Rectangle.X, (int)Rectangle.Y, (int)Rectangle.Width, (int)Rectangle.Height, (int)(circle.X + (circle.Width / 2)), (int)(circle.Y + circle.Height)))
         dList.Add(Direction.BOTTOM);
      if (CheckPointInsideBox((int)Rectangle.X, (int)Rectangle.Y, (int)Rectangle.Width, (int)Rectangle.Height, (int)circle.X, (int)(circle.Y + (circle.Height / 2))))
         dList.Add(Direction.LEFT);
      if (dList.Count != 0)
         return dList;
      else if (circle.X + (circle.Width / 2) < Rectangle.X)
      {
         if (circle.Y + (circle.Height / 2) > Rectangle.Y)
            dList.Add(Direction.TOP_RIGHT);
         else
            dList.Add(Direction.BOTTOM_RIGHT);
      }
      else
      {
         if (circle.Y + (circle.Height / 2) < Rectangle.Y)
            dList.Add(Direction.BOTTOM_LEFT);
         else
            dList.Add(Direction.TOP_LEFT);
      }
      return dList;
   }

   /// <summary>
   /// checks if a point is inside of a rectagle
   /// </summary>
   /// <param name="xBox">top left x pixel position of the box</param>
   /// <param name="yBox">top left y pixel posiiton of the box</param>
   /// <param name="widthBox">pixel width of the box</param>
   /// <param name="heightBox">pixel height of the box</param>
   /// <param name="xPoint">the x pixel position of the point to check</param>
   /// <param name="yPoint">the y pixel position of the point to check</param>
   /// <returns></returns>
   private bool CheckPointInsideBox(int xBox, int yBox, int widthBox, int heightBox, int xPoint, int yPoint)
   {
      if (xPoint >= xBox && xPoint <= xBox + widthBox && yPoint >= yBox && yPoint <= yBox + heightBox)
         return true;
      return false;
   }
}
