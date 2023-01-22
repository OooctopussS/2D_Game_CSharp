using System;
using System.Drawing;
using Game.Interfaces;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Game.Classes
{
    public struct CharacterState
    {
        public bool GoLeft { get; set; }
        public bool GoRight { get; set; }
        public bool GoDown { get; set; }
        public bool Jump { get; set; }
        public bool OnGround { get; set; }
        public bool OnDisplayStart { get; set; }
        public bool OnDisplayExit { get; set; }
        public bool OnDisplayScore { get; set; }
        public bool Ready { get; set; }
        public bool UnderPlatformJump { get; set; }
        public bool FireBallActived { get; set; }
        public bool ActiveInvulnerability { get; set; }
        public bool Died { get; set; }
    }

    public class Player : GameObject, IPhysicalActive
    {
        public Label label = null;

        public Projectile fireBall = null;
        public float JumpForce { get; private set; } = 600f;
        public float MoveSpeed { get; set; } = 500f;
        public int Direction { get; set; } = 0;

        public PointF velocity = new PointF(0, 0);

        public CharacterState heroState = new CharacterState() { GoLeft = false, GoRight = false, GoDown = false, Jump = false,
            OnGround = false, OnDisplayStart = false, OnDisplayExit = false, Ready = false, UnderPlatformJump = false, OnDisplayScore = false, FireBallActived = false, ActiveInvulnerability = false};

        public Player(string imgKey, RectangleF objectRect, Rectangle spriteRect) : base(imgKey, objectRect, spriteRect)
        {

        }
        public Player(string imgKey, RectangleF objectRect, Rectangle spriteRect, string gameObjName = "Player") : this(imgKey, objectRect, spriteRect)
        {
            TitleObject = gameObjName;
        }

        public bool CollideY()
        {
            foreach (var item in GameController.gameObjects)
            {
                if (item is IPhysicalPassive)
                {

                    if (Left + GameObjectRect.Width / 1.5f >= item.Left && Left + GameObjectRect.Width / 2.5f <= item.Right)
                    {
                        // Спуск по маленькой платформе
                        if (heroState.GoDown && !heroState.Jump && item.TitleObject == "SPlatform")
                        {
                            if (Bottom >= item.Top && Bottom <= item.Bottom) return false;
                        }

                        // Приземление на платформу
                        if (Bottom >= item.Top && Bottom <= item.Top + (item.TitleObject != "SPlatform" ? item.GameObjectRect.Height : item.GameObjectRect.Height / 2.5))
                        {
                            if (heroState.Jump && item.TitleObject == "SPlatform") heroState.UnderPlatformJump = true;
                            GameObjectRect.Y = item.Top - GameObjectRect.Height;

                            if (item.TitleObject == "DisplayStart")
                            {
                                label.Hide = false;
                                label.GameObjectRect.X = Left + GameObjectRect.Width / 2 - label.GameObjectRect.Width / 2;
                                label.GameObjectRect.Y = Top - 10 - label.GameObjectRect.Height;
                                heroState.OnDisplayStart = true;
                            }

                            if (item.TitleObject == "DisplayExit")
                            {
                                label.Hide = false;
                                label.GameObjectRect.X = Left + GameObjectRect.Width / 2 - label.GameObjectRect.Width / 2;
                                label.GameObjectRect.Y = Top - 10 - label.GameObjectRect.Height;
                                heroState.OnDisplayExit = true;
                            }

                            if (item.TitleObject == "DisplayScore")
                            {
                                label.Hide = false;
                                label.GameObjectRect.X = Left + GameObjectRect.Width / 2 - label.GameObjectRect.Width / 2;
                                label.GameObjectRect.Y = Top - 10 - label.GameObjectRect.Height;
                                heroState.OnDisplayScore = true;
                            }

                            if (heroState.GoDown) heroState.GoDown = false;
                            return true;
                        }

                        // Столкновение снизу с платформой
                        if (heroState.Jump && item.TitleObject != "SPlatform")
                        {
                            if (Top - 1 <= item.Bottom && Bottom >= item.Bottom)
                            {
                                GameObjectRect.Y = item.Bottom + 2;
                                return true;
                            }
                        }

                    }
                    if (Left >= item.Left - item.GameObjectRect.Width && Right <= item.Right + item.GameObjectRect.Width)
                    {
                        if (item.TitleObject == "SPlatform" && Top <= item.Top && Bottom >= item.Bottom) heroState.GoDown = false;
                    }
                }
            }
            return false;
        }

        public bool CollideX()
        {
            foreach (var item in GameController.gameObjects)
            {
                if (item is IPhysicalPassive)
                {
                    if (item.TitleObject != "SPlatform")
                    {
                        if ((Top >= item.Top && Top <= item.Bottom) ||
                            (item.Bottom >= Top && item.Bottom <= Bottom))
                        {
                            if (Right >= item.Left && Right <= item.Left + item.GameObjectRect.Width / 2)
                            {
                                GameObjectRect.X = item.Left - GameObjectRect.Width;
                                return true;
                            }
                            if (Left >= item.Left + item.GameObjectRect.Width / 2 && Left <= item.Right)
                            {
                                GameObjectRect.X = item.Right;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void Jump()
        {
            heroState.Jump = true;
            velocity.Y = -JumpForce;
        }

        private void ResetState()
        {
            if (GameController.isMainMenu)
            {
                heroState.OnDisplayStart = false;
                heroState.OnDisplayExit = false;
                heroState.OnDisplayScore = false;
            }
            heroState.UnderPlatformJump = false;
            heroState.OnGround = false;
            label.Hide = true;
        }

        private static float currentFPSTimer = 0;
        private void DrawSprite()
        {
            if ((currentFPSTimer += Time.deltaTime) > 0.2f)
            {
                currentFPSTimer = 0;
                if (velocity.X > 0) SpriteRect.Y = 32;
                else if (velocity.X < 0) SpriteRect.Y = 0;
                if (velocity.X != 0 && !heroState.Jump)
                {
                    SpriteRect.X += 32;
                    if (SpriteRect.X >= 92) SpriteRect.X = 0;
                }
            }
        }

        public override void Update()
        {

            if (GameController.isMainMenu)
            {
                if (Top >= GameController.FormHeight)
                {
                    GameObjectRect.Y = 0;
                }
            }

            if (Left >= GameController.FormWidth)
            {
                 GameObjectRect.X = 0;
            }
            if (Right <= 0)
            {
                 GameObjectRect.X = GameController.FormWidth;
            }

            if (!GameController.isMainMenu)
            {
                if (Top >= GameController.FormHeight)
                {
                    heroState.Died = true;
                }
            }

            ResetState();

            velocity.Y += Constants.Gravity * Time.deltaTime;

            for (float i = 0; i <= Math.Abs(velocity.Y); i += Constants.CollisionStep)
            {
                GameObjectRect.Y += Constants.CollisionStep * Time.deltaTime * Math.Sign(velocity.Y);
                if (CollideY())
                {
                    if (velocity.Y > 0)
                    {
                        heroState.OnGround = true;
                        heroState.Jump = false;
                    }
                    if (!heroState.UnderPlatformJump) velocity.Y = 0;
                    break;
                }
            }
            
            if (Direction == 1)
                heroState.GoRight = true;

            if (Direction == -1)
                heroState.GoLeft = true;

            velocity.X = MoveSpeed * Time.deltaTime * Direction; //Time.fixedDeltaTime

            for (float i = 0; i <= Math.Abs(velocity.X); i += Constants.CollisionStep)
            {
                GameObjectRect.X += Constants.CollisionStep * Math.Sign(velocity.X);

                DrawSprite();

                if (CollideX())
                {
                    break;
                }
            }

            GameController.Render(Constants.ImagesMap[ImagesMapKey], new RectangleF(GameObjectRect.X - Camera.x, GameObjectRect.Y - Camera.y, GameObjectRect.Width, GameObjectRect.Height) , SpriteRect);
        }
    }
}
