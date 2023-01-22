using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Classes
{
    public static class Camera
    {
        public static float x;
        public static float y;
        public static float targetX;
        public static float targetY;

        public static void CalculateTarget()
        {
            targetX = Clamp((GameController.Player1.GameObjectRect.X + GameController.Player2.GameObjectRect.X) * 0.5f - GameController.FormWidth * 0.5f, -GameController.FormWidth * 0.125f, GameController.FormWidth * 0.125f);
            targetY = Clamp((GameController.Player1.GameObjectRect.Y + GameController.Player2.GameObjectRect.Y) * 0.5f - GameController.FormHeight * 0.5f, -GameController.FormHeight * 0.5f, GameController.FormHeight * 0.5f);

        }
        public static void Move()
        {
            x = targetX;
            y = targetY;
        }

        public static float Clamp(float current, float min, float max)
        {
            if (current < min) return min;
            else if (current > max) return max;
            else return current;
        }
    }
}
