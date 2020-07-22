using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace rescuePars
{
    ///<summary>nice little wrap around default input system</summary>
    class Input
    {
        /// <summary>current and previous keyboard input states. Used to find if user pressed of released the key</summary>
        private static KeyboardState current;

        private static KeyboardState last;
        private static MouseState mouseState;
        private static MouseState prevMouseState;

        private static bool _firstMove = true;
        private static Vector2 mousePos;
        private static Vector2 deltaMouse;

        public static void onUpdateFrame()
        {
            last = current;
            current = Keyboard.GetState();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (_firstMove)
            {
                mousePos = new Vector2(mouseState.X, mouseState.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouseState.X - mousePos.X;
                var deltaY = mouseState.Y - mousePos.Y;

                deltaMouse = new Vector2(deltaX, deltaY);
                mousePos = new Vector2(mouseState.X, mouseState.Y);
            }
        }

        ///<summary>return vector containing mouse shift on x and y
        public static Vector2 mouseDeltaPos()
        {
            return deltaMouse;
        }
        public static Vector2 mousePosition()
        {
            return mousePos;
        }
        ///<summary>user is holding key down</summary>
        public static bool keyDown(Key key)
        {
            return current.IsKeyDown(key);
        }

        ///<summary>specified key is not pressed</summary>
        public static bool keyUp(Key key)
        {
            return current.IsKeyUp(key);
        }

        ///<summary>user just smashed the key</summary>
        public static bool getKeyDown(Key key)
        {
            return current.IsKeyDown(key) && last.IsKeyUp(key);
        }

        ///<summary>user just released the key</summary>
        public static bool getKeyUp(Key key)
        {
            return current.IsKeyUp(key) && last.IsKeyDown(key);
        }

        public static bool keyDown(MouseButton key)
        {
            return mouseState[key];
        }

        public static bool keyUp(MouseButton key)
        {
            return !mouseState[key];
        }

        public static bool getKeyDown(MouseButton key)
        {
            return !prevMouseState[key] && mouseState[key];
        }

        ///<summary>user just released the key</summary>
        public static bool getKeyUp(MouseButton key)
        {
            return prevMouseState[key] && !mouseState[key];
        }

        ///<summary>how much user scrolled mouse wheel</summary>
        public static float mouseWheel()
        {
            return mouseState.WheelPrecise - prevMouseState.WheelPrecise;
        }
    }
}