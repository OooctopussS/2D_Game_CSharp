using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Classes
{
    public static class Time
    {
        public static float deltaTime { get; private set; }
        public static float fixedDeltaTime
        { 
            get 
            {
                return 0.015f;
            }
        }
        public static float timeSinceStart { get; private set; }
        private static long frameBeginning;
        public static void SetFrameBeginning(long time) => frameBeginning = time;
        public static void CalculateDeltaTime(long time) => deltaTime = (time - frameBeginning) / 10000000f;
        public static void CalculateTimeSinceStart() => timeSinceStart += deltaTime;
    }
}
