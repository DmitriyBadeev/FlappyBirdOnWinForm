using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlappyBirdGame.Models;

namespace FlappyBirdGame
{
    class ViewForm : Form
    {
        private GameModel currentGame;

        private Timer timer;

        private int maxScore;

        private bool FlySwitch;

        public ViewForm(GameModel game)
        {
            DoubleBuffered = true;
            Text = Setting.NameOfGame;

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            currentGame = game;

            ClientSize = new Size
            {
                Width = game.WidthWindow,
                Height = game.HeightWindow + 80
            };

            timer = new Timer { Interval = 20 };
            timer.Tick += (sender, args) => currentGame.UpdateState();
           
            currentGame.StateChanged += isGameOver =>
            {
                Invalidate();

                if (isGameOver)
                    timer.Stop();
            };

            Paint += (sender, args) =>
            {
                var bgImage = new Bitmap(Config.BackgroundImagePath);
                DrawOnFullWidth(bgImage, args, Setting.HeightWindow - bgImage.Height);

                DrawBird(args);
                DrawWalls(args);

                var baseImage = new Bitmap(Config.BaseImagePath);
                DrawOnFullWidth(baseImage, args, Setting.HeightWindow);

                if (currentGame.Score > maxScore)
                    maxScore = currentGame.Score;
                    
                DrawScore(args);

                if (currentGame.IsGameOver)
                    DrawGameOver(args);

                else if (!timer.Enabled)
                    DrawText(args, "Press any key to start"); 
            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (currentGame.IsGameOver)
            {
                currentGame = new GameModel(
                    Setting.CountWallsInStage,
                    Setting.StartPosition,
                    Setting.OffsetFromStartPosition,
                    Setting.HeightWindow,
                    Setting.WidthWindow,
                    Setting.VelocityGame);

                currentGame.StateChanged += isGameOver =>
                {
                    Invalidate();

                    if (isGameOver)
                        timer.Stop();
                };

                timer.Start();
            }
            else
            {
                if (!timer.Enabled)
                    timer.Start();

                if (e.KeyCode == Keys.Space)
                    currentGame.FlappyBird.FlappyUp();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (!currentGame.IsGameOver)
            {
                if (!timer.Enabled)
                    timer.Start();

                currentGame.FlappyBird.FlappyUp();
            }
        }

        private void DrawOnFullWidth(Bitmap img, PaintEventArgs args, int y)
        {
            int countImg = currentGame.WidthWindow / img.Width + 1;
            var prevPosition = 0;
            for (var i = 0; i < countImg; i++)
            {
                args.Graphics.DrawImage(img, prevPosition, y);
                prevPosition += img.Width;
            }
        }

        private void DrawBird(PaintEventArgs args)
        {
            if (!FlySwitch)
            {
                var birdImg = new Bitmap(Config.FlappyImagePath);
                args.Graphics.DrawImage(birdImg, (float)currentGame.FlappyBird.LocationX, (float)currentGame.FlappyBird.LocationY);
                FlySwitch = true;
            }
            else
            {
                var birdImg = new Bitmap(Config.FlappyImagePathUp);
                args.Graphics.DrawImage(birdImg, (float)currentGame.FlappyBird.LocationX, (float)currentGame.FlappyBird.LocationY);
                FlySwitch = false;
            }
        }

        private void DrawWalls(PaintEventArgs args)
        {
            var wallImg = new Bitmap(Config.WallImagePath);
            var rotateWallImg = new Bitmap(Config.WallImagePath);
            rotateWallImg.RotateFlip(RotateFlipType.Rotate180FlipX);

            for (var i = 0; i < currentGame.CountWallsInStage; i++)
            {
                var currentWall = currentGame.Walls.Dequeue();

                args.Graphics.DrawImage(wallImg, (float)currentWall.Location, (float)currentWall.WindowStart);
                args.Graphics.DrawImage(rotateWallImg, (float)currentWall.Location, (float)currentWall.WindowEnd - Setting.HeightWall);

                currentGame.Walls.Enqueue(currentWall);
            }
        }

        private void DrawScore(PaintEventArgs args)
        {
            args.Graphics.DrawString($"Score: {currentGame.Score.ToString()} Record: {maxScore}",
                new Font(FontFamily.GenericMonospace, 14), new SolidBrush(Color.Black), 0, 0);
        }

        private void DrawGameOver(PaintEventArgs args)
        {
            args.Graphics.DrawString("Game Over",
                new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold), new SolidBrush(Color.Black),
                Setting.WidthWindow / 2 - 150, Setting.HeightWindow / 2 - 50);

            args.Graphics.DrawString($"Score: {currentGame.Score.ToString()} Record: {maxScore}",
                new Font(FontFamily.GenericMonospace, 17), new SolidBrush(Color.Black),
                Setting.WidthWindow / 2 - 150, Setting.HeightWindow / 2);

            args.Graphics.DrawString("Press any key to restart",
                new Font(FontFamily.GenericMonospace, 15), new SolidBrush(Color.Black),
                Setting.WidthWindow / 2 - 150, Setting.HeightWindow / 2 + 50);
        }

        private void DrawText(PaintEventArgs args, string text)
        {
            args.Graphics.DrawString(text,
                new Font(FontFamily.GenericMonospace, 15), new SolidBrush(Color.Black),
                Setting.WidthWindow / 2 - 150, Setting.HeightWindow / 2);
        }
    }
}
