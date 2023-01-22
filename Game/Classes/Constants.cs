using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.Classes
{
    public static class Constants
    {
        public const float CollisionStep = 1f;
        public const float Gravity = 900f;
        public static Dictionary<string, Image> ImagesMap = new Dictionary<string, Image>();
    }
}
