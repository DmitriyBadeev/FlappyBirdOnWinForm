using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBirdGame.Models
{
    public class Bird
    {
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        
        public double Velocity { get; set; }

        public double TraveledDistance { get; set; } 

        public double LastWall { get; set; }

        public Bird(Point startLocation)
        {
            LocationX = startLocation.X;
            LocationY = startLocation.Y;
            Velocity = 0;
        }

        public void UpdatePosition()
        {
            Velocity += Setting.Gravity;
            LocationY += Velocity;

            TraveledDistance += Setting.VelocityGame;
        }

        public void UpdatePosition(double x, double y)
        {
            LocationY = y;
            LocationX = x;
        }
    }
}
