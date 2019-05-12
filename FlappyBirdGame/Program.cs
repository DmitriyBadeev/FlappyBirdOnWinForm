using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlappyBirdGame.Models;

namespace FlappyBirdGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var game = new GameModel(
                Setting.CountWallsInStage, 
                Setting.StartPosition, 
                Setting.OffsetFromStartPosition, 
                Setting.HeightWindow, 
                Setting.WidthWindow, 
                Setting.VelocityGame
            );

            var form = new ViewForm(game);
            Application.Run(form);
        }
    }
}
