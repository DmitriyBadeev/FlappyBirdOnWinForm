using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBirdGame
{
    public class Setting
    {
        public static readonly string NameOfGame = "Flappy Bird";

        public static readonly int HeightWindowOfWall = 110;

        public static readonly int WidthWall = 52;

        public static readonly int HeightWall = 320;

        public static readonly int CountWallsInStage = 6;

        public static readonly int StartPosition = 200;

        public static readonly int OffsetFromStartPosition = 300;

        public static readonly int HeightWindow = 400;

        public static readonly int WidthWindow = 1000;

        public static readonly int VelocityGame = 5;

        public static readonly double Gravity = 9.8 / 8;

        public static readonly int FlappyUp = 20;

        public static readonly int StartVelocity = -8;

        public static readonly int WidthBird = 34;
        
        public static readonly int HeightBird = 24;
    }
}
