using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FlappyBirdGame;
using FlappyBirdGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlappyBirdGameTests
{
    [TestClass]
    public class GameModelTest
    {
        [TestMethod]
        public void RightFallBird()
        {
            var game = new GameModel(
                Setting.CountWallsInStage,
                Setting.StartPosition,
                Setting.OffsetFromStartPosition,
                Setting.HeightWindow,
                Setting.WidthWindow,
                Setting.VelocityGame
            );

            Assert.AreEqual(game.FlappyBird.Velocity, 0);

            game.FlappyBird.UpdatePosition();

            Assert.AreEqual(Setting.Gravity, game.FlappyBird.Velocity);
            Assert.AreEqual(Setting.VelocityGame, game.FlappyBird.TraveledDistance);

            game.FlappyBird.UpdatePosition();

            Assert.AreEqual(2 * Setting.Gravity, game.FlappyBird.Velocity);
            Assert.AreEqual(2 * Setting.VelocityGame, game.FlappyBird.TraveledDistance);
        }

        [TestMethod]
        public void RightUpBird()
        {
            var game = new GameModel(
                Setting.CountWallsInStage,
                Setting.StartPosition,
                Setting.OffsetFromStartPosition,
                Setting.HeightWindow,
                Setting.WidthWindow,
                Setting.VelocityGame
            );

            Assert.AreEqual(game.FlappyBird.Velocity, 0);

            game.FlappyUp();

            Assert.AreEqual(Setting.StartVelocity, game.FlappyBird.Velocity);
            Assert.AreEqual(Setting.HeightWindow / 2 - Setting.FlappyUp, game.FlappyBird.LocationY);
        }

        [TestMethod]
        public void RightWallMotion()
        {
            var game = new GameModel(
                Setting.CountWallsInStage,
                Setting.StartPosition,
                Setting.OffsetFromStartPosition,
                Setting.HeightWindow,
                Setting.WidthWindow,
                Setting.VelocityGame
            );

            for (var i = 0; i < 100; i++)
                game.UpdateState();

            Assert.AreEqual(Setting.CountWallsInStage, game.Walls.Count);
            var listWalls = game.Walls.OrderBy(w => w.Location).ToList();
            
            for (var i = 0; i < game.CountWallsInStage - 1; i++)
            {
                Assert.AreEqual(game.IntervalBetweenWalls, listWalls[i+1].Location - listWalls[i].Location);
            }
        }

        [TestMethod]
        public void RightScore()
        {
            var game = new GameModel(
                Setting.CountWallsInStage,
                Setting.StartPosition,
                Setting.OffsetFromStartPosition,
                Setting.HeightWindow,
                Setting.WidthWindow,
                Setting.VelocityGame
            );

            
            game.FlappyBird.TraveledDistance = Setting.OffsetFromStartPosition + 30;
            game.UpdateState();
            Assert.AreEqual(1, game.Score);

            for (var i = 2; i < 100; i++)
            {
                game.FlappyBird.TraveledDistance += game.IntervalBetweenWalls;
                game.UpdateState();
                Assert.AreEqual(i, game.Score);
            }
        }
    }
}
