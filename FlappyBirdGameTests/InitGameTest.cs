using System;
using FlappyBirdGame;
using FlappyBirdGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlappyBirdGameTests
{
    [TestClass]
    public class InitGameTest
    {
        [TestMethod]
        public void CountWalls_WhenGameInit()
        {
            var game = new GameModel(
                Setting.CountWallsInStage,
                Setting.StartPosition,
                Setting.OffsetFromStartPosition,
                Setting.HeightWindow,
                Setting.WidthWindow,
                Setting.VelocityGame
            );

            Assert.AreEqual(game.CountWallsInStage, game.Walls.Count);
            Assert.AreEqual(6, game.Walls.Count);
        }

        [TestMethod]
        public void RightIntervals_WhenGameInit()
        {
            var game = new GameModel(
                Setting.CountWallsInStage,
                Setting.StartPosition,
                Setting.OffsetFromStartPosition,
                Setting.HeightWindow,
                Setting.WidthWindow,
                Setting.VelocityGame
            );

            Assert.AreEqual(game.CountWallsInStage, game.Walls.Count);
            Assert.AreEqual(6, game.Walls.Count);

            var currentWall = game.Walls.Dequeue();
            for (var i = 0; i < game.CountWallsInStage - 1; i++)
            {
                var nextWall = game.Walls.Dequeue();
                Assert.AreEqual(nextWall.Location - currentWall.Location, game.IntervalBetweenWalls);
                currentWall = nextWall;
            }
        }

        [TestMethod]
        public void BirdRightCreate_WhenGameInit()
        {
            var game = new GameModel(
                Setting.CountWallsInStage,
                Setting.StartPosition,
                Setting.OffsetFromStartPosition,
                Setting.HeightWindow,
                Setting.WidthWindow,
                Setting.VelocityGame
            );

            Assert.AreEqual(Setting.HeightWindow / 2, game.FlappyBird.LocationY);
            Assert.AreEqual(Setting.StartPosition, game.FlappyBird.LocationX);
            Assert.AreEqual(0, game.FlappyBird.Velocity);
            Assert.AreEqual(0, game.FlappyBird.TraveledDistance);
        }
    }
}
