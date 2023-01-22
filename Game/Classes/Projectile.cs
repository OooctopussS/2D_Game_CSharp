using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Game.Interfaces;
using System.Threading;
using System.Diagnostics;

namespace Game.Classes
{
    public class Projectile : GameObject
    {
        public bool Hide { get; set; } = false;
        public PointF velocity = new PointF(0, 0);
        public float MoveSpeed { get; set; } = 500f;
        public int Direction { get; private set; } = 0;
        public int forPlayer { get; private set; }
        public string playerTitle { get; private set; }
        public Projectile(string imgKey, RectangleF objectRect, Rectangle spriteRect) : base(imgKey, objectRect, spriteRect)
        {

        }
        public Projectile(string imgKey, RectangleF objectRect, Rectangle spriteRect, string gameObjName = "Label") : this(imgKey, objectRect, spriteRect)
        {
            TitleObject = gameObjName;
        }

        public Projectile(string imgKey, RectangleF objectRect, Rectangle spriteRect, int dir, int player) : this(imgKey, objectRect, spriteRect)
        {
            Direction = dir;
            forPlayer = player;
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
                            playerTitle = item.TitleObject;
                            return true;
                        }

                        if (Top - 1 <= item.Bottom && Bottom >= item.Bottom)
                        {
                            playerTitle = item.TitleObject;
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
                            playerTitle = item.TitleObject;
                            return true;
                        }
                        if (Left >= item.Left + item.GameObjectRect.Width / 2 && Left <= item.Right)
                        {
                            playerTitle = item.TitleObject;
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public override void Update()
        {
            if (!Hide)
            {
                playerTitle = "";
                velocity.X = MoveSpeed * Time.deltaTime * Direction;

                for (float i = 0; i <= Math.Abs(velocity.X); i += Constants.CollisionStep)
                {
                    GameObjectRect.X += Constants.CollisionStep * Math.Sign(velocity.X);
                    if (CollideX() || CollideY())
                    {
                        if (!GameController.isMainMenu)
                        {
                            Debug.WriteLine(GameController.Player1.heroState.ActiveInvulnerability);
                            if (forPlayer == 1 && playerTitle != "Player1")
                            {
                                if (!GameController.Player2.heroState.ActiveInvulnerability)
                                    GameController.Player2.heroState.Died = true;
                                else
                                {
                                    GameController.Player2.heroState.ActiveInvulnerability = false;
                                    GameController.Player1.heroState.FireBallActived = false;
                                    Hide = true;
                                    break;
                                }
                            }
                            if (forPlayer == 2 && playerTitle != "Player2")
                            {
                                if (!GameController.Player1.heroState.ActiveInvulnerability)
                                    GameController.Player1.heroState.Died = true;
                                else
                                {
                                    GameController.Player1.heroState.ActiveInvulnerability = false;
                                    GameController.Player2.heroState.FireBallActived = false;
                                    Hide = true;
                                    break;
                                }
                            }
                        }
                    }

                }
                GameController.Render(Constants.ImagesMap[ImagesMapKey], new RectangleF(GameObjectRect.X - Camera.x, GameObjectRect.Y - Camera.y, GameObjectRect.Width, GameObjectRect.Height), SpriteRect);
            }
        }
    }
}
