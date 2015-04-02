using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Player;

namespace CSGSSTemplate
{
    /// <summary>
    /// A sample system class that run that game's logic.
    /// </summary>
    public static class System
    {
        //A ball to be displayed.
        //Note: the sprite uses the default viewport in this case.
        private static Sprite ball = new Sprite(); 
        //The ball's velocity, X and Y.
        private static int ballVelX = 3;
        private static int ballVelY = -2;

        /// <summary>
        /// Initializes the system.
        /// </summary>
        static System()
        {
            //Create a 64x64 bitmap.
            Bitmap ballBmp = new Bitmap(64, 64);
            //Create a circle in blue on the Bitmap.
            ballBmp.FillEllipse(new Rect(64, 64), Colors.Blue);
            //Assign the bitmap to the sprite.
            ball.Bitmap = ballBmp;
        }

        /// <summary>
        /// Updates the game every frame.
        /// </summary>
        public static void Update()
        {
            //Increase the ball's position by its velocity.
            ball.X += ballVelX;
            ball.Y += ballVelY;

            //Bounce the ball off the walls if needed.
            if (ball.X < 0 || ball.X > 640 - ball.Width)
            {
                ball.X = ball.X.MinMax(0, 640 - ball.Width);
                ballVelX *= -1;
            }
            if (ball.Y < 0 || ball.Y > 480 - ball.Height)
            {
                ball.Y = ball.Y.MinMax(0, 480 - ball.Height);
                ballVelY *= -1;
            }
        }
    }
}
