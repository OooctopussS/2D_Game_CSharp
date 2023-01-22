using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace Game.Classes
{
    public class Bonus : GameObject
    {
        public bool Hide { get; set; } = false;
        public string playerWhoTake { get; private set; }
        public Bonus(string imgKey, RectangleF objectRect, Rectangle spriteRect) : base(imgKey, objectRect, spriteRect)
        {

        }
        public Bonus(string imgKey, RectangleF objectRect, Rectangle spriteRect, string gameObjName = "Label") : this(imgKey, objectRect, spriteRect)
        {
            TitleObject = gameObjName;
        }

        private bool CollideY()
        {
            foreach (var item in GameController.gameObjects)
            {
                if (item is Player)
                {
                    if (Left + GameObjectRect.Width / 1.5f >= item.Left && Left + GameObjectRect.Width / 2.5f <= item.Right)
                    {
                        if (Bottom >= item.Top && Bottom <= item.Top + item.GameObjectRect.Height / 2.5)
                        {
                            playerWhoTake = item.TitleObject;
                            return true;
                        }

                        if (Top - 1 <= item.Bottom && Bottom >= item.Bottom)
                        {
                            playerWhoTake = item.TitleObject;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool CollideX()
        {
            foreach (var item in GameController.gameObjects)
            {
                if (item is Player)
                {
                    if ((Top >= item.Top && Top <= item.Bottom) ||
                        (item.Bottom >= Top && item.Bottom <= Bottom))
                    {

                        if (Right >= item.Left && Right <= item.Left + item.GameObjectRect.Width / 2)
                        {
                            playerWhoTake = item.TitleObject;
                            return true;
                        }

                        if (Left >= item.Left + item.GameObjectRect.Width / 2 && Left <= item.Right)
                        {
                            playerWhoTake = item.TitleObject;
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public override void Update()
        {
            playerWhoTake = "";

            if ((CollideX() || CollideY()) && Hide == false)
            {
                if (playerWhoTake == "Player1") GameController.Player1.heroState.ActiveInvulnerability = true;
                if (playerWhoTake == "Player2") GameController.Player2.heroState.ActiveInvulnerability = true;
                Hide = true;
            }

            if (!Hide)
                GameController.Render(Constants.ImagesMap[ImagesMapKey], new RectangleF(GameObjectRect.X - Camera.x, GameObjectRect.Y - Camera.y, GameObjectRect.Width, GameObjectRect.Height), SpriteRect);
        }
    }
}
