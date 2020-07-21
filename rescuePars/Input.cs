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

        public static void onUpdateFrame()
        {
            last = current;
            current = Keyboard.GetState();

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
