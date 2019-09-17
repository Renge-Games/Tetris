using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public static class InputHandler
    {
        static bool downPressed = false;
        static bool leftPressed = false;
        static bool rightPressed = false;
        static bool shiftPressed = false;
        static bool upPressed = false;

        public static void HandleInput()
        {
            if (Keyboard.GetState().IsKeyUp(Keys.A) && leftPressed)
                leftPressed = false;
            if (Keyboard.GetState().IsKeyDown(Keys.A) && !leftPressed)
            {
                leftPressed = true;
                Level.MoveCurrentEntityX(-1);
            }
            if (Keyboard.GetState().IsKeyUp(Keys.D) && rightPressed)
                rightPressed = false;
            if (Keyboard.GetState().IsKeyDown(Keys.D) && !rightPressed)
            {
                rightPressed = true;
                Level.MoveCurrentEntityX(1);
            }
            if (Keyboard.GetState().IsKeyUp(Keys.S) && downPressed)
                downPressed = false;
            if (Keyboard.GetState().IsKeyDown(Keys.S) && !downPressed)
            {
                downPressed = true;
                Level.MoveCurrentEntityDown();
            }
            if (Keyboard.GetState().IsKeyUp(Keys.W) && upPressed)
                upPressed = false;
            if (Keyboard.GetState().IsKeyDown(Keys.W) && !upPressed)
            {
                upPressed = true;
                Level.RotateCurrentEntity();
            }
            if (Keyboard.GetState().IsKeyUp(Keys.LeftShift) && shiftPressed)
                shiftPressed = false;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !shiftPressed)
            {
                shiftPressed = true;
                Level.ToggleHold();
            }
        }
    }
}
