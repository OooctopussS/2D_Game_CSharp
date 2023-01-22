using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Game.Interfaces;

namespace Game.Classes
{
    public class Platform : GameObject, IPhysicalPassive
    {
        public Platform(string imgKey, RectangleF objectRect, Rectangle spriteRect) : base(imgKey, objectRect, spriteRect)
        {

        }
        public Platform(string imgKey, RectangleF objectRect, Rectangle spriteRect, string gameObjName = "Platform") : this(imgKey, objectRect, spriteRect)
        {
            TitleObject = gameObjName;
        }

        public override void Update()
        {
            GameController.Render(Constants.ImagesMap[ImagesMapKey], new RectangleF(GameObjectRect.X - Camera.x, GameObjectRect.Y - Camera.y, GameObjectRect.Width, GameObjectRect.Height), SpriteRect);
        }
    }

}
