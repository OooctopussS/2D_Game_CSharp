using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.Classes
{
    public class Label : GameObject
    {
        public bool Hide { get; set; } = true;
        public Label(string imgKey, RectangleF objectRect, Rectangle spriteRect) : base(imgKey, objectRect, spriteRect)
        {

        }
        public Label(string imgKey, RectangleF objectRect, Rectangle spriteRect, string gameObjName = "Label") : this(imgKey, objectRect, spriteRect)
        {
            TitleObject = gameObjName;
        }
       
        public override void Update()
        {
            if (!Hide)
                GameController.Render(Constants.ImagesMap[ImagesMapKey], new RectangleF(GameObjectRect.X - Camera.x, GameObjectRect.Y - Camera.y, GameObjectRect.Width, GameObjectRect.Height), SpriteRect);
        }
    }
}
