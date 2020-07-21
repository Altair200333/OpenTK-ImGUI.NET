using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace rescuePars
{
    //nice little wrap around default input system
    class Input
    {
        private static KeyboardState current;
        private static KeyboardState last;
        private static bool _firstMove = true;
        private static Vector2 mousePos;
        private static Vector2 deltaMouse;

        public static void onUpdateFrame()
        {
            last = current;
            current = Keyboard.GetState();

            var mouse = Mouse.GetState();
            if (_firstMove) // this bool variable is initially set to true
            {
                mousePos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - mousePos.X;
                var deltaY = mouse.Y - mousePos.Y;

                deltaMouse = new Vector2(deltaX, deltaY);
                mousePos = new Vector2(mouse.X, mouse.Y);
            }
        }

        public static Vector2 mouseDeltaPos()
        {
            return deltaMouse;
        }
        //user is holding key down
        public static bool keyDown(Key key)
        {
            return current.IsKeyDown(key);
        }

        //specified key is not pressed
        public static bool keyUp(Key key)
        {
            return current.IsKeyUp(key);
        }
        //user just smashed the key
        public static bool getKeyDown(Key key)
        {
            return current.IsKeyDown(key) && last.IsKeyUp(key);
        }
        //user just released the key 
        public static bool getKeyUp(Key key)
        {
            return current.IsKeyUp(key) && last.IsKeyDown(key);
        }
    }
}
