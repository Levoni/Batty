using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batty_2._0_Test
{
   [TestClass]
   public class CollisionManagerTest
   {
      [TestMethod]
      public void Constructor_NotNull_True()
      {
         CollisionManager CM = new CollisionManager();
         Assert.IsNotNull(CM);
      }

      [TestMethod]
      public void CheckCircleRectCollision_Colliding_False()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 5, 5, System.Drawing.Color.Black,1);
         Assert.IsFalse(colManager.CheckCircleRectCollision(ba, Bl));
      }

      [TestMethod]
      public void CheckCircleRectCollision_RectTopSideCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 60, 50, 10, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckCircleRectCollision(ba, Bl));
      }

      [TestMethod]
      public void CheckCircleRectCollision_RectRightSideCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 10, 50, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckCircleRectCollision(ba, Bl));
      }

      [TestMethod]
      public void CheckCircleRectCollision_RectBottomSideCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 50, 10, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckCircleRectCollision(ba, Bl));
      }

      [TestMethod]
      public void CheckCircleRectCollision_RectLeftSideCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(60, 0, 5, 50, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckCircleRectCollision(ba, Bl));
      }

      [TestMethod]
      public void RectInsideCircle_RectInsideCircleCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(0, 0, 25, 25, System.Drawing.Color.Black, 1);
         Block Bl = new Block(20, 20, 5, 5, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckCircleRectCollision(ba, Bl));
      }

      [TestMethod]
      public void RectInsideCircle_RectOutsideCircleCollide_False()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(0, 0, 25, 25, System.Drawing.Color.Black, 1);
         Block Bl = new Block(20, 0, 1, 1, System.Drawing.Color.Black, 1);
         Assert.IsFalse(colManager.CheckCircleRectCollision(ba, Bl));
      }

      [TestMethod]
      public void CircleInsideRect_CircleInsideRectCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 500, 500, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckCircleRectCollision(ba, Bl));
      }
      //start
      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnTopSide_Top()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 50, 10, System.Drawing.Color.Black, 1);
         List < Direction > dList = new List<Direction>();
         dList.Add(Direction.TOP);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }

      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnRightSide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(60, 0, 10, 50, System.Drawing.Color.Black, 1);
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.RIGHT);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }

      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnBottomSide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 60, 50, 10, System.Drawing.Color.Black, 1);
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.BOTTOM);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }

      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnLeftSide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 10, 50, System.Drawing.Color.Black, 1);
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.LEFT);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }

      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnTopRight_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(45, 0, 10, 30, System.Drawing.Color.Black, 1);
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP_RIGHT);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }

      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnBottomRight_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(45, 45, 30, 30, System.Drawing.Color.Black, 1);
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.BOTTOM_RIGHT);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }

      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnbottomLeft_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 45, 30, 30, System.Drawing.Color.Black, 1);
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.BOTTOM_LEFT);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }

      [TestMethod]
      public void FindCircleCollisionSide_BallCollidedOnTopLeft_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball ba = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 30, 30, System.Drawing.Color.Black, 1);
         List<Direction> dList = new List<Direction>();
         dList.Add(Direction.TOP_LEFT);
         Assert.IsTrue(colManager.FindCircleCollisionSide(ba, Bl)[0] == dList[0]);
      }
      [TestMethod]
      public void CheckRectRectCollision_RectRectCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Block ba = new Block(10, 10, 5, 5, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 30, 30, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckRectRectCollision(ba, Bl));
      }

      [TestMethod]
      public void CheckRectRectCollision_RectRectCollide_False()
      {
         CollisionManager colManager = new CollisionManager();
         Block ba = new Block(50, 50, 10, 10, System.Drawing.Color.Black, 1);
         Block Bl = new Block(0, 0, 10, 10, System.Drawing.Color.Black, 1);
         Assert.IsFalse(colManager.CheckRectRectCollision(ba, Bl));
      }

      [TestMethod]
      public void CheckCircleCircleCollision_CircleCircleEdgeCollide_True()
      {
         CollisionManager colManager = new CollisionManager();
         Ball bOne = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Ball BTwo = new Ball(10, 10, 50, 50, System.Drawing.Color.Black, 1);
         Assert.IsTrue(colManager.CheckCircleCircleCollision(bOne, BTwo));
      }

      [TestMethod]
      public void CheckCircleCircleCollision_CircleCircleCollide_False()
      {
         CollisionManager colManager = new CollisionManager();
         Ball bOne = new Ball(50, 50, 50, 50, System.Drawing.Color.Black, 1);
         Ball BTwo = new Ball(50, 0, 50, 40, System.Drawing.Color.Black, 1);
         Assert.IsFalse(colManager.CheckCircleCircleCollision(bOne, BTwo));
      }

   }
}
