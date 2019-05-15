using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBirdGame.Models
{
    public class GameModel
    {
        public int HeightWindow { get; }

        public int WidthWindow { get; }

        public int CountWallsInStage { get; }

        public double IntervalBetweenWalls => (WidthWindow / CountWallsInStage) + 40;

        public Queue<Wall> Walls { get; private set; }

        public Bird FlappyBird { get; }

        public double Velocity { get; }

        public event Action<bool> StateChanged;

        public bool IsGameOver { get; private set; }

        public int Score { get; private set; }

        private Random random = new Random();

        public GameModel(int countWalls, int startPosition, int offsetFromStartPosition, 
                         int heightWindow, int widthWindow, double velocity)
        {
            CountWallsInStage = countWalls;
            HeightWindow = heightWindow;
            WidthWindow = widthWindow;
            Velocity = velocity;
            IsGameOver = false;
            Score = 0;

            InitWalls(startPosition, offsetFromStartPosition);

            var startLocation = new Point(startPosition, heightWindow / 2);
            FlappyBird = new Bird(startLocation);
        }

        private void InitWalls(double startPosition, double offsetFromStartPosition)
        {
            Walls = new Queue<Wall>();
            var startWindow = random.Next(Setting.HeightWindowOfWall + 10, HeightWindow - 10);

            Walls.Enqueue(new Wall(startPosition + offsetFromStartPosition, startWindow));

            var prevWallPosition = startPosition + offsetFromStartPosition;
            for (var i = 1; i < CountWallsInStage; i++)
            { 
                startWindow = random.Next(Setting.HeightWindowOfWall + 10, HeightWindow - 10);
                Walls.Enqueue(new Wall(prevWallPosition + IntervalBetweenWalls, startWindow));
                prevWallPosition += IntervalBetweenWalls;
            }
        }

        private bool IsСollisionBirdInWall(Wall wall) =>
            (FlappyBird.LocationY < wall.WindowEnd ||
             FlappyBird.LocationY + Setting.HeightBird > wall.WindowStart) &&
            (FlappyBird.LocationX + Setting.WidthBird >= wall.Location &&
             FlappyBird.LocationX <= wall.Location + Setting.WidthWall);

        private PointF GetPointCollision(Wall wall)
        {
            var collisionPoint = new PointF();

            if (FlappyBird.LocationY < wall.WindowEnd)
            {
                if (FlappyBird.LocationY < wall.WindowStart &&
                    FlappyBird.LocationY + Setting.HeightBird > wall.WindowEnd)
                {
                    collisionPoint.X = (float)FlappyBird.LocationX;
                    collisionPoint.Y = (float)wall.WindowEnd;
                }
                else
                {
                    collisionPoint.X = (float)wall.Location - Setting.WidthBird - 3;
                    collisionPoint.Y = (float)FlappyBird.LocationY;
                }
            }
            else
            {
                if (FlappyBird.LocationY < wall.WindowStart &&
                    FlappyBird.LocationY + Setting.HeightBird > wall.WindowEnd)
                {
                    collisionPoint.X = (float)FlappyBird.LocationX;
                    collisionPoint.Y = (float)wall.WindowStart - Setting.HeightBird - 5;
                }
                else
                {
                    collisionPoint.X = (float)wall.Location - Setting.WidthBird - 3;
                    collisionPoint.Y = (float)FlappyBird.LocationY;
                }
            }

            return collisionPoint;
        }

        private void UpdateScore()
        {
            if (FlappyBird.TraveledDistance >= Setting.OffsetFromStartPosition
                && FlappyBird.TraveledDistance <= Setting.OffsetFromStartPosition + IntervalBetweenWalls)
            {
                Score = 1;
                FlappyBird.LastWall = Setting.OffsetFromStartPosition;
            }
            else if (FlappyBird.LastWall + IntervalBetweenWalls <= FlappyBird.TraveledDistance && Score >= 1)
            {
                Score++;
                FlappyBird.LastWall = FlappyBird.LastWall + IntervalBetweenWalls;
            }
        }

        public void UpdateState()
        {
            for (var i = 0; i < CountWallsInStage; i++)
            {
                var currentWall = Walls.Dequeue();
                currentWall.Location -= Velocity;

                if (currentWall.Location < -Setting.WidthWall)
                {
                    var lastWall = Walls.Last();
                    var startWindow = random.Next(Setting.HeightWindowOfWall + 10, HeightWindow - 10);
                    var newWall = new Wall(lastWall.Location + IntervalBetweenWalls, startWindow);
                    Walls.Enqueue(newWall);
                }
                else
                    Walls.Enqueue(currentWall);
            }

            FlappyBird.UpdatePosition();
            UpdateScore();

            if (FlappyBird.LocationY >= HeightWindow - Setting.HeightBird)
            {
                FlappyBird.UpdatePosition(Setting.StartPosition, HeightWindow - Setting.HeightBird - 5);
                StateChanged?.Invoke(true);
                IsGameOver = true;
                return;
            }

            foreach (var wall in Walls)
            {
                if (IsСollisionBirdInWall(wall))
                {
                    var pointCollision = GetPointCollision(wall);
                    FlappyBird.UpdatePosition(pointCollision.X, pointCollision.Y);
                    StateChanged?.Invoke(true);
                    IsGameOver = true;
                    return;
                }
            }

            StateChanged?.Invoke(false);
        }
    }
}