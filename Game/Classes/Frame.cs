using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.Classes
{
    public class Frame
    {
        public Image SpriteList { get; private set; }
        public RectangleF dest { get; private set; }
        public RectangleF src { get; private set; }
        public Frame(Image img, RectangleF dest, RectangleF src)
        {
            this.dest = dest;
            this.src = src;
            SpriteList = img;
        }
    }
}
