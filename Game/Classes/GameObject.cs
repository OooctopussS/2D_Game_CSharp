using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.Classes
{
    public abstract class GameObject
    {

        public string TitleObject { get; set; } = "GameObject";

        public string ImagesMapKey;
        public Rectangle SpriteRect;
        public RectangleF GameObjectRect;
        public float Bottom
        {
            get
            {
                return GameObjectRect.Y + GameObjectRect.Height;
            }
        }
        public float Right
        {
            get
            {
                return GameObjectRect.X + GameObjectRect.Width;
            }
        }
        public float Top
        {
            get
            {
                return GameObjectRect.Y;
            }
        }
        public float Left
        {
            get
            {
                return GameObjectRect.X;
            }
        }


        public GameObject(string imgKey, RectangleF objectRect, Rectangle spriteRect)
        {
            ImagesMapKey = imgKey;
            SpriteRect = spriteRect;
            GameObjectRect = objectRect;
        }

        public GameObject(string imgKey, RectangleF objectRect, Rectangle spriteRect, string gameObjName): this(imgKey, objectRect, spriteRect)
        {
            TitleObject = gameObjName;
        }


        public abstract void Update();
    }
}
