//--------------------------------------------------------
// Authors: David, Matthew, Levon, Alex, and Derek
// Program: Batty
// Purpose: Handle a ball game object
//--------------------------------------------------------
using System.Drawing;
using System.Collections.Generic;
using System;
using Batty_2._0;
using System.Threading;

public enum Direction { TOP = 90, TOP_RIGHT = 45, RIGHT = 0, BOTTOM_RIGHT = 315, BOTTOM = 270, BOTTOM_LEFT = 225, LEFT = 180, TOP_LEFT = 135, NONE = -1 };

public class Ball : GameObject
{
   private int defaultSpeed;

   /// <summary>
   /// Public set only property to let change the default X position of ball
   /// Used injunction with bar resizing
   /// </summary>
   public int DefaultX { private get; set; }
   private int DefaultY { get; set; }

   public int Speed { get; set; }

   public float xDirection { get; set; }
   public float yDirection { get; set; }
   public bool atBottom = false;

   /// <summary>
   /// Property that is true when ball is moving
   /// False when ball is not moving on top of bar
   /// </summary>
   public bool Released { get; set; }

   /// <summary>
   /// Initial ball speed used to calculate new speeds
   /// Used to implement ball speed option
   /// </summary>
   public int InitSpeed { get; set; }

   public int MaxInitPos { private get; set; }
   public int MinInitPos { private get; set; }

   /// <summary>
   /// Property that sets the ball speed to the default speed times the given value
   /// Set only
   /// </summary>
   public double SpeedModifier { set { Speed = (int)(value * defaultSpeed); } }
   public Direction collisionDirection { get; private set; }

   public List<Direction> collisiondirections { get; set; }

   /// <summary>
   /// Constructor
   /// </summary>
   /// <param name="xPos">top left X pixel position</param>
   /// <param name="yPos">top left Y pixel position</param>
   /// <param name="oWidth">Width of object in pixels</param>
   /// <param name="oHeight">Height of object in pixels</param>
   /// <param name="oColor">Color of object</param>
   public Ball(int xPos, int yPos, int oWidth, int oHeight, Color oColor, int inSpeed) : base(xPos, yPos, oWidth, oHeight, oColor)
   {
      type = GameObjectType.BALL;
      collisionDirection = Direction.NONE;
      Speed = inSpeed;
      defaultSpeed = inSpeed;
      Released = false;
      DefaultX = xPos;
      DefaultY = yPos;
      SpeedModifier = 1;
      collisiondirections = new List<Direction>();
   }

   /// <summary>
   /// Sets the collision direction enum to the correct direction.
   /// </summary>
   /// <param name="direction">Direction ball was hit from: 0 = top, 1 = Top-right, ..., 7 = top-left.</param>
   public void AddCollisionDirection(List<Direction> directions)
   {
      foreach (Direction d in directions)
      {
         bool contains = false;
         foreach(Direction cd in collisiondirections)
         {
            if(d == cd)
            {
               contains = true;
            }
         }
         if(!contains)
            collisiondirections.Add(d);
      }
   }

   /// <summary>
   /// Flips the X and/or Y direction based off of last direction hit from.
   /// </summary>
   /// <param name="otherType">Type of gameobject the ball collide</param>
   public override void HandleCollisions(GameObjectType otherType)
   {
      if (otherType != GameObjectType.BOMB && collisiondirections.Count != 0)
      {
         double xTemp = 0;
         double yTemp = 0;
         if (collisiondirections.Count % 2 == 1)
         {
            foreach(Direction d in collisiondirections)
            {
               xTemp += -1 * Math.Cos(DegreeToRadian((int)d));
               yTemp += -1 * Math.Sin(DegreeToRadian((int)d));
            }
         }
         else if(collisiondirections.Count % 2 == 0)
         {
            foreach(Direction d in collisiondirections)
            {
               if(d == Direction.TOP || d == Direction.RIGHT|| d == Direction.BOTTOM|| d == Direction.LEFT)
               {
                  xTemp += -1 * Math.Cos(DegreeToRadian((int)d));
                  yTemp += -1 * Math.Sin(DegreeToRadian((int)d));
               }
            }
         }
         if(xTemp < -.01)
         {
            xDirection = -1 * Math.Abs(xDirection); 
         }
         else if(xTemp > .01)
         {
            xDirection = Math.Abs(xDirection);
         }
         if (yTemp < -.01)
         {
            yDirection = Math.Abs(yDirection);
         }
         else if (yTemp > .01)
         {
            yDirection = Math.Abs(yDirection) * -1;
         }
         collisiondirections.Clear();
      }
      collisionDirection = Direction.NONE;
   }

   /// <summary>
   /// Change ball angle by the opposite of the given angle
   /// </summary>
   /// <param name="angle"></param>
   public void SetBallAngle(float angle)
   {
      xDirection = (float)(-1 * Math.Cos(DegreeToRadian((int)angle)));
      yDirection = (float)(-1 * Math.Sin(DegreeToRadian((int)angle)));
   }

   /// <summary>
   /// Moves the ball in the appropriate direction at the correct speed while 
   /// checking for the walls of the level.
   /// </summary>
   public override void Update()
   {
      if(Released)
      {
         if (X + Width >= GameManager.screenWidth)
         {
            xDirection *= -1;
            Thread thread = new Thread(new ThreadStart(AudioManager.PlayHit));
            thread.Start();
         }
         if (X <= 0)
         {
            xDirection *= -1;
            Thread thread = new Thread(new ThreadStart(AudioManager.PlayHit));
            thread.Start();
         }
         if (Y <= 0)
         {
            yDirection *= -1;
            Thread thread = new Thread(new ThreadStart(AudioManager.PlayHit));
            thread.Start();
         }
         if (Y + Height >= GameManager.screenHeight)
            atBottom = true;
         float vectorMagnatude = FindVectorMagnatude(xDirection, yDirection);

         X = X + (Speed * (xDirection / vectorMagnatude));
         Y = Y + (Speed * (yDirection / vectorMagnatude));
      }
      else
      {
         if (Controls.Instance.ControlWasPressed(Controls.ControlType.Left))
            X -= InitSpeed;
         if (Controls.Instance.ControlWasPressed(Controls.ControlType.Right))
            X += InitSpeed;

         X = Math.Max(X, MinInitPos);
         X = Math.Min(X, MaxInitPos);
      }
 
   }

   /// <summary>
   /// Sets the ball to its state at the start of the game
   /// </summary>
   public void Reset()
   {
      Y = DefaultY;
      X = DefaultX;
      atBottom = false;
      Released = false;
   }

   /// <summary>
   /// returns the curent vector magnitude
   /// </summary>
   /// <param name="x"></param>
   /// <param name="y"></param>
   /// <returns></returns>
   private float FindVectorMagnatude(float x, float y)
   {
      return (float)Math.Sqrt(x * x + y * y);
   }

   /// <summary>
   /// returns the angle of the balls vector in degrees
   /// </summary>
   /// <param name="degree"></param>
   /// <returns></returns>
   private double DegreeToRadian(int degree)
   {
      return Math.PI * degree / 180;
   }

}
