using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class BallTest
   {
      private Ball b;
      private Ball left;
      private Ball right;
      private Ball top;
      private Ball bottom;
      
      [TestInitialize]
      public void MyTestInitialize()
      {
         b = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 2);
         left = new Ball(0, 50, 50, 50, System.Drawing.Color.Black, 2);
         right = new Ball(490, 50, 50, 50, System.Drawing.Color.Black, 2);
         top = new Ball(50, 0, 50, 50, System.Drawing.Color.Black, 2);
         bottom = new Ball(50, 490, 50, 50, System.Drawing.Color.Black, 2);
      }
      //start
      [TestMethod]
      public void Constructor_NotNull_True()
      {
         Assert.IsNotNull(b);
      }

      [TestMethod]
      public void SetCollisionDirection_ValidNumber_True()
      {
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP);
         b.AddCollisionDirection(dList);
         Assert.IsTrue(b.collisiondirections[0] == dList[0]);
      }

      [TestMethod]
      public void SetCollisionDirection_DuplicateDirections_1Direcctions()
      {
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP);
         b.AddCollisionDirection(dList);
         b.AddCollisionDirection(dList);
         Assert.AreEqual(b.collisiondirections.Count,1);
      }

      [TestMethod]
      public void Update_NoCollisionReleased_ContinueInSameDirection()
      {
         GameManager.screenHeight = 500;
         GameManager.screenWidth = 500;
         float initialX = b.X;
         float initialY = b.Y;
         b.xDirection = 1;
         b.yDirection = 1;
         b.Released = true;
         b.Update();
         float tempX = (float)(initialX + b.Speed * (b.xDirection / (Math.Sqrt((b.xDirection * b.xDirection) + (b.yDirection * b.yDirection)))));
         float tempY = (float)(initialY + b.Speed * (b.yDirection / (Math.Sqrt((b.xDirection * b.xDirection) + (b.yDirection * b.yDirection)))));
         Assert.IsTrue(b.X == tempX && b.Y == tempY);
      }

      [TestMethod]
      public void Update_BallHitLeftWall_XDirectionChanged()
      {
         GameManager.screenHeight = 500;
         GameManager.screenWidth = 500;
         left.xDirection = -1;
         left.yDirection = 1;
         float initialX = left.xDirection;
         left.Released = true;
         left.Update();
         Assert.AreNotEqual(left.xDirection, initialX);
      }

      [TestMethod]
      public void Update_BallHitRightWall_XDirectionChanged()
      {
         GameManager.screenHeight = 500;
         GameManager.screenWidth = 500;
         right.xDirection = 1;
         right.yDirection = 1;
         float initialX = right.xDirection;
         right.Released = true;
         right.Update();
         Assert.AreNotEqual(right.xDirection, initialX);
      }

      [TestMethod]
      public void Update_BallHitTopWall_YDirectionChanged()
      {
         GameManager.screenHeight = 500;
         GameManager.screenWidth = 500;
         top.xDirection = 1;
         top.yDirection = 1;
         float initialY = top.yDirection;
         top.Released = true;
         top.Update();
         Assert.AreNotEqual(b.yDirection, initialY);
      }

      [TestMethod]
      public void Update_SetBottomReachedFlag_FlagSet()
      {
         GameManager.screenHeight = 500;
         GameManager.screenWidth = 500;
         bottom.xDirection = 1;
         bottom.yDirection = -1;
         bottom.Released = true;
         bottom.Update();
         Assert.IsTrue(bottom.atBottom);
      }


      
      [TestMethod]
      public void HandleCollisions_TopCOllision_True()
      {
         b.xDirection = 1;
         b.yDirection = -1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == 1 && b.yDirection == 1);
      }
      
      [TestMethod]
      public void HandleCollisions_RightCOllision_True()
      {
         right.xDirection = 1;
         right.yDirection = 1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.RIGHT);
         right.AddCollisionDirection(dList);
         right.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(right.xDirection == -1 && right.yDirection == 1);
      }
      
      [TestMethod]
      public void HandleCollisions_BottomCollision_True()
      {
         b.xDirection = 1;
         b.yDirection = 1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.BOTTOM);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == 1 && b.yDirection == -1);
      }
      
      [TestMethod]
      public void HandleCollisions_LeftCOllision_True()
      {
         b.xDirection = -1;
         b.yDirection = 1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.LEFT);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == 1 && b.yDirection == 1);
      }
      
      [TestMethod]
      public void HandleCollisions_TopRightCOllision_True()
      {
         b.xDirection = 1;
         b.yDirection = 1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP_RIGHT);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == -1 && b.yDirection == 1);
      }
      
      [TestMethod]
      public void HandleCollisions_BottomRightCOllision_True()
      {
         b.xDirection = 1;
         b.yDirection = 1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.BOTTOM_RIGHT);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == -1 && b.yDirection == -1);
      }
      
      [TestMethod]
      public void HandleCollisions_BottomLeftCOllision_True()
      {
         b.xDirection = 1;
         b.yDirection = 1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.BOTTOM_LEFT);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == 1 && b.yDirection == -1);
      }
      
      [TestMethod]
      public void HandleCollisions_TopLeftCOllision_True()
      {
         b.xDirection = -1;
         b.yDirection = -1;
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP_LEFT);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == 1 && b.yDirection == 1);
      }
      
      [TestMethod]
      public void HandleCollisions_NoCollision_True()
      {
         b.xDirection = 1;
         b.yDirection = 1;
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == 1 && b.yDirection == 1);
      }

      [TestMethod]
      public void HandleCollisions_TopCollisionAndTopRight_DownDirection()
      {
         Ball tempB = new Ball(10,10,50,50,System.Drawing.Color.Black,0);
         Block tempbl = new Block(9, 10, 50, 30, System.Drawing.Color.Black, 3);
         b.xDirection = 1;
         b.yDirection = -1;
         
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP);
         dList.Add(Direction.TOP_RIGHT);
         b.AddCollisionDirection(dList);
         b.HandleCollisions(GameObject.GameObjectType.BLOCK);
         Assert.IsTrue(b.xDirection == 1 && b.yDirection == 1);
      }

      [TestMethod]
      public void Ball_Reset_DefaultPosition()
      {
         int initBallX = 10;
         int initBallY = 10;

         b.Released = true;
         b.X += b.Speed;
         b.Y += b.Speed;

         b.Reset();

         Assert.AreEqual(b.X, initBallX);
         Assert.AreEqual(b.Y, initBallY);
      }

      [TestMethod]
      public void Ball_Bounds_InBounds()
      {
         int minBound = 0;
         int maxBound = 20;

         b.MinInitPos = minBound;
         b.MaxInitPos = maxBound;
         b.X = minBound - b.Width;
         b.Update();
         Assert.AreEqual(minBound, b.X);

         b.X = maxBound + b.Width;
         b.Update();
         Assert.AreEqual(maxBound, b.X);
      }

      [TestMethod]
      public void Ball_ModifySpeed_Increased()
      {
         int speedMofider = 50;
         int startSpeed = b.Speed;
         b.SpeedModifier = speedMofider;

         Assert.IsTrue(startSpeed < b.Speed);
      }

      [TestMethod]
      public void Ball_ModifySpeed_Decreased()
      {
         double speedMofider = .5;
         int startSpeed = b.Speed;
         b.SpeedModifier = speedMofider;

         Assert.IsTrue(startSpeed > b.Speed);
      }

      [TestMethod]
      public void Ball_SetBallAngle_90Degrees()
      {
         b.SetBallAngle(90);
         Assert.IsTrue(b.xDirection > -0.01 && b.yDirection == -1);
      }
   }
}

