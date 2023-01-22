using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Game.Classes
{
    public class BackGround : GameObject
    {
        public BackGround(string imgKey, RectangleF objectRect, Rectangle spriteRect) : base(imgKey, objectRect, spriteRect)
        {

        }
        public BackGround(string imgKey, RectangleF objectRect, Rectangle spriteRect, string gameObjName = "BackGround") : this(imgKey, objectRect, spriteRect)
        {
            TitleObject = gameObjName;
        }

        public override void Update()
        {
            GameController.Render(Constants.ImagesMap[ImagesMapKey], new RectangleF(GameObjectRect.X, GameObjectRect.Y, GameObjectRect.Width, GameObjectRect.Height), SpriteRect);
        }
    }
}
