using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Game.Interfaces;

namespace Game.Classes
{

    public struct score
    {
        public int firstPlayerScore { get; set; }
        public int secondPlayerScore { get; set; }
    }
    public static class GameController
    {
        public static int FormWidth { get; set; }
        public static int FormHeight { get; set; }

        public static readonly Queue<Frame> renderQueue = new Queue<Frame>();
        public static readonly List<GameObject> gameObjects = new List<GameObject>();

        public static Player Player1 { get; private set; }
        public static Player Player2 { get; private set; }
        public static Projectile fireBall1 { get; private set; } = null;
        public static Projectile fireBall2 { get; private set; } = null;
        public static Bonus BonusShield1 { get; private set; } = null;
        public static Bonus BonusShield2 { get; private set; } = null;

        public static score mainScore = new score()
        {
            firstPlayerScore = 0,
            secondPlayerScore = 0
        };

        public static bool isMainMenu = true;
        public static bool Exit = false;
        public static bool Pause = false;
        public static bool ScoreActive = false;

        private static float currentFPSTimer = 0;
        private static float currentFPS = 0;

        public static void InitDictionary()
        {
            // mainMenu
            Constants.ImagesMap.Add("MainMenu_bg", new Bitmap("../../img/MainMenu/BgDark.png"));
            Constants.ImagesMap.Add("Player1", new Bitmap("../../img/Player/Player1.png"));
            Constants.ImagesMap.Add("Player2", new Bitmap("../../img/Player/Player2.png"));
            Constants.ImagesMap.Add("Input_e", new Bitmap("../../img/MainMenu/input_e.png"));
            Constants.ImagesMap.Add("Input_enter", new Bitmap("../../img/MainMenu/input_enter.png"));
            Constants.ImagesMap.Add("PlatformMenu", new Bitmap("../../img/MainMenu/platformMenu.png"));
            Constants.ImagesMap.Add("DisplayStart", new Bitmap("../../img/MainMenu/DisplayStart.png"));
            Constants.ImagesMap.Add("DisplayExit", new Bitmap("../../img/MainMenu/DisplayExit.png"));
            Constants.ImagesMap.Add("DisplayScore", new Bitmap("../../img/MainMenu/DisplayScore.png"));

            // 1
            Constants.ImagesMap.Add("Map1_bg", new Bitmap("../../img/Map1/bgMap1.png"));
            Constants.ImagesMap.Add("shroomBrownMidDouble", new Bitmap("../../img/Map1/shroomBrownMidDouble.png"));
            Constants.ImagesMap.Add("shroomBrownFull", new Bitmap("../../img/Map1/shroomBrownFull.png"));
            Constants.ImagesMap.Add("stemTopAlt", new Bitmap("../../img/Map1/stemTopAlt.png"));
            Constants.ImagesMap.Add("stemShroom", new Bitmap("../../img/Map1/stemShroom.png"));
            Constants.ImagesMap.Add("stem", new Bitmap("../../img/Map1/stem.png"));
            Constants.ImagesMap.Add("stemVine", new Bitmap("../../img/Map1/stemVine.png"));
            Constants.ImagesMap.Add("shroomTanMidDouble", new Bitmap("../../img/Map1/shroomTanMidDouble.png"));
            Constants.ImagesMap.Add("shroomTanFull", new Bitmap("../../img/Map1/shroomTanFull.png"));
            Constants.ImagesMap.Add("shroomRedMidDouble", new Bitmap("../../img/Map1/shroomRedMidDouble.png"));
            Constants.ImagesMap.Add("shroomRedFull", new Bitmap("../../img/Map1/shroomRedFull.png"));
            Constants.ImagesMap.Add("stemCrown", new Bitmap("../../img/Map1/stemCrown.png"));

            // 2 && 3
            Constants.ImagesMap.Add("Map2_bg", new Bitmap("../../img/Map2/bgMap2.png"));
            Constants.ImagesMap.Add("tundraLong", new Bitmap("../../img/Map2/tundraLong.png"));

            // projectile
            Constants.ImagesMap.Add("fireBall", new Bitmap("../../img/Player/fireBall.png"));

            // бонусы
            //  Constants.ImagesMap.Add("bonusSpeed", new Bitmap("../../img/Bonus/bonusSpeed.png"));
            //  Constants.ImagesMap.Add("bonusFly", new Bitmap("../../img/Bonus/bonusFly.png"));
            Constants.ImagesMap.Add("bonusShield", new Bitmap("../../img/Bonus/bonusShield.png"));
        }

        public static void Init()
        {
            InitDictionary();
            Player1 = new Player("Player1", new RectangleF(FormWidth / 2 - FormWidth * 3.645f / 100 / 2, 0, FormWidth * 3.645f / 100, FormHeight * 6.48f / 100), new Rectangle(0, 0, 32, 32), "Player1");
            Player2 = new Player("Player2", new RectangleF(FormWidth / 2 - FormWidth * 3.645f / 100 / 2, 0, FormWidth * 3.645f / 100, FormHeight * 6.48f / 100), new Rectangle(0, 0, 32, 32), "Player2");


            Player1.label = new Label("Input_e", new RectangleF(Player1.Left + Player1.GameObjectRect.Width / 2 - 25 / 2, Player1.Top - 10 - 26, 25, 26), new Rectangle(0, 0, 13, 14), "label1Player");
            Player2.label = new Label("Input_enter", new RectangleF(Player2.Left + Player2.GameObjectRect.Width / 2 - 30 / 2, Player2.Top - 10 - 14, 30, 14), new Rectangle(0, 0, 30, 14), "label2Player");


            //SaveMap("../../Maps/MainMenu");
            LoadMap("../../Maps/MainMenu");

            gameObjects.Add(Player1.label);
            gameObjects.Add(Player2.label);

            gameObjects.Add(Player1);
            gameObjects.Add(Player2);

            //   gameObjects.Add(new Platform("bonusSpeed", new RectangleF(FormWidth * 5 / 10, FormHeight * 2 / 10, 38, 42), new Rectangle(0, 0, 16, 18)));
            //   gameObjects.Add(new Platform("bonusFly", new RectangleF(FormWidth * 6 / 10, FormHeight * 2 / 10, 38, 40), new Rectangle(0, 0, 38, 40)));

        }

        public static void SaveMap(string path)
        {
            StreamWriter sW = null;
            try
            {
                sW = new StreamWriter(path, false);
                foreach (var item in gameObjects)
                {

                        sW.WriteLine($"{item.GetType().Name};{item.ImagesMapKey};{item.GameObjectRect.X}/{item.GameObjectRect.Y}/{item.GameObjectRect.Width}/{item.GameObjectRect.Height};" +
                          $"{item.SpriteRect.X}/{item.SpriteRect.Y}/{item.SpriteRect.Width}/{item.SpriteRect.Height};{item.TitleObject}");
                }
            }
            catch
            {
                Debug.WriteLine("Ошибка. Не получилось создать или открыть файл.");
                Environment.Exit(0);
            }
            finally
            {
               if (sW != null)
                   sW.Close();
            }
        }

        public static void LoadMap(string path)
        {
            StreamReader sR = null;
            RectangleF tmpGameObjectRect;
            Rectangle tmpSpriteRect;
            try
            {
                sR = new StreamReader(path, false);
                string currentLine = sR.ReadLine();
                if (currentLine != null)
                {
                    do
                    {
                        string[] parts = currentLine.Split(';');
                        string[] partsObjectRect = parts[2].Split('/');
                        string[] partsSpriteRect = parts[3].Split('/');
                        tmpGameObjectRect = new RectangleF(float.Parse(partsObjectRect[0]), float.Parse(partsObjectRect[1]), float.Parse(partsObjectRect[2]), float.Parse(partsObjectRect[3]));
                        tmpSpriteRect = new Rectangle(Int32.Parse(partsSpriteRect[0]), Int32.Parse(partsSpriteRect[1]), Int32.Parse(partsSpriteRect[2]), Int32.Parse(partsSpriteRect[3]));
                        switch (parts[0])
                        {
                            case "Platform":
                                gameObjects.Add(new Platform(parts[1], tmpGameObjectRect, tmpSpriteRect, parts[4]));
                                break;
                            case "BackGround":
                                gameObjects.Add(new BackGround(parts[1], tmpGameObjectRect, tmpSpriteRect, parts[4]));
                                break;
                        }
                        currentLine = sR.ReadLine();
                    }
                    while (currentLine != null);
                }
            }
            catch
            {
                Debug.WriteLine("Ошибка. Не получилось создать или открыть файл.");
                Environment.Exit(0);
            }
            finally
            {
                if (sR != null)
                    sR.Close();
            }
        }

        public static void Render(Image img, RectangleF dest, Rectangle src)
        {
            renderQueue.Enqueue(new Frame(img, dest, src));
        }
        static float GetRandom(Random rnd, int leftBorder, int rightBorder)
        {

            float value = (float)(rnd.Next(leftBorder, rightBorder) + rnd.NextDouble());

            Debug.WriteLine(value);

            return value;
        }
        public static void SpawnBonuses()
        {
            Random rnd = new Random();
            BonusShield1 = null;
            BonusShield2 = null;

            BonusShield1 = new Bonus("bonusShield", new RectangleF(GetRandom(rnd, 100, FormWidth - 100), GetRandom(rnd, 400, FormHeight - 400), 38, 41), new Rectangle(0, 0, 18, 21), "Shield");
            BonusShield2 = new Bonus("bonusShield", new RectangleF(GetRandom(rnd, 100, FormWidth - 100), GetRandom(rnd, 400, FormHeight - 400), 38, 41), new Rectangle(0, 0, 18, 21), "Shield");
            gameObjects.Add(BonusShield1);
            gameObjects.Add(BonusShield2);
        }

        private static void GameSession()
        {
            if (Player1.heroState.Died)
            {
                GameProcess.sessionScore.secondPlayerScore++;
                Player1.heroState.Died = false;
                GameProcess.MakeMap();
                GameProcess.StartRound = true;
                GameProcess.RoundNumber++;
                Player1.heroState.FireBallActived = false;
                Player1.heroState.ActiveInvulnerability = false;
                Player2.heroState.FireBallActived = false;
                Player2.heroState.ActiveInvulnerability = false;
                SpawnBonuses();
            }

            if (Player2.heroState.Died)
            {
                GameProcess.sessionScore.firstPlayerScore++;
                Player2.heroState.Died = false;
                GameProcess.MakeMap();
                GameProcess.StartRound = true;
                GameProcess.RoundNumber++;
                Player1.heroState.FireBallActived = false;
                Player1.heroState.ActiveInvulnerability = false;
                Player2.heroState.FireBallActived = false;
                Player2.heroState.ActiveInvulnerability = false;
                SpawnBonuses();
            }

            if (GameProcess.sessionScore.firstPlayerScore == 5)
            {
                mainScore.firstPlayerScore++;
                isMainMenu = true;
            }

            if (GameProcess.sessionScore.secondPlayerScore == 5)
            {
                mainScore.secondPlayerScore++;
                isMainMenu = true;
            }
            if (isMainMenu)
            {
                GameProcess.RoundNumber = 1;
                gameObjects.Clear();
                LoadMap("../../Maps/MainMenu");
                Player1.GameObjectRect.X = FormWidth * 5 / 10;
                Player1.GameObjectRect.Y = 0;
                gameObjects.Add(Player1);
                Player2.GameObjectRect.X = FormWidth * 5 / 10;
                Player2.GameObjectRect.Y = 0;
                gameObjects.Add(Player2);
                gameObjects.Add(Player1.label);
                gameObjects.Add(Player2.label);
            }
        }

        private static void CheckDisplayStart(Player tmpHero)
        {
            GameObject tmpPlatform = null;
            foreach (var item in gameObjects)
            {
                if (item.TitleObject == "DisplayStart")
                {
                    tmpPlatform = item;
                }
            }

            if (!Player1.heroState.Ready && !Player2.heroState.Ready)
            {
                tmpPlatform.SpriteRect.Y = tmpHero.TitleObject == "Player1" ? 64 : 128;
                tmpHero.heroState.Ready = true;
                return;
            }

            if (Player1.heroState.Ready && !Player2.heroState.Ready)
            {
                tmpPlatform.SpriteRect.Y = tmpHero.TitleObject == "Player1" ? 0 : 192;
                tmpHero.heroState.Ready = tmpHero.TitleObject == "Player1" ? false : true;
                return;
            }

            if (!Player1.heroState.Ready && Player2.heroState.Ready)
            {
                tmpPlatform.SpriteRect.Y = tmpHero.TitleObject == "Player1" ? 192 : 0;
                tmpHero.heroState.Ready = tmpHero.TitleObject == "Player1" ? true : false;
                return;
            }
        }

        public static void Update()
        {
            CheckFireBallAndBonus();

            if (Player1.heroState.Ready && Player2.heroState.Ready)
            {
                GameProcess.Start();
            }
            foreach (var item in gameObjects)
            {
                item.Update();
            }
            if (!isMainMenu)
            {
                GameSession();
            }
            Camera.CalculateTarget();
            Camera.Move();
        }

        private static void CheckFireBallAndBonus()
        {
            if (fireBall1 != null && fireBall1.Hide)
                fireBall1 = null;

            if (fireBall2 != null && fireBall2.Hide)
                fireBall2 = null;

            if (BonusShield1 != null && BonusShield1.Hide)
                BonusShield1 = null;

            if (BonusShield2 != null && BonusShield2.Hide)
                BonusShield2 = null;

            if (fireBall1 != null && (fireBall1.Left >= GameController.FormWidth || fireBall1.GameObjectRect.Right <= 0))
            {
                GameController.Player1.heroState.FireBallActived = false;
                fireBall1 = null;
            }

            if (fireBall2 != null && (fireBall2.Left >= GameController.FormWidth || fireBall2.GameObjectRect.Right <= 0))
            {
                GameController.Player2.heroState.FireBallActived = false;
                fireBall2 = null;
            }
        }

        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (GameProcess.StartRound)
            {
                return;
            }

            if (Pause)
            {
                Pause = false;
                return;
            }

            if (ScoreActive)
            {
                ScoreActive = false;
                return;
            }

            switch (e.KeyCode.ToString())
            {
                case "W":
                    if (Player1.heroState.OnGround)
                        Player1.Jump();
                    break;
                case "A":
                    Player1.Direction = -1;
                    break;
                case "S":
                    if (Player1.velocity.Y == 0) Player1.heroState.GoDown = true;
                    break;
                case "D":
                    Player1.Direction = 1;
                    break;
                case "E":
                    if (GameController.isMainMenu)
                    {
                        if (Player1.heroState.OnDisplayExit)
                            Exit = true;

                        if (Player1.heroState.OnDisplayStart)
                            CheckDisplayStart(Player1);

                        if (Player1.heroState.OnDisplayScore)
                            ScoreActive = true;
                    }

                    if (!Player1.heroState.FireBallActived)
                    {
                        Player1.heroState.FireBallActived = true;
                        int dir;
                        if (Player1.SpriteRect.Y == 0) dir = -1;
                            else dir = 1;
                        fireBall1 = new Projectile("fireBall", new RectangleF((dir == 1) ? Player1.GameObjectRect.Right + 1 : Player1.GameObjectRect.Left - 20,
                            Player1.GameObjectRect.Top + Player1.GameObjectRect.Height / 2, 20, 20), new Rectangle(0, 0, 444, 444), dir, 1);
                        gameObjects.Add(fireBall1);
                    }
                    break;
                case "Left":
                    Player2.Direction = -1;
                    break;
                case "Right":
                    Player2.Direction = 1;
                    break;
                case "Up":
                    if (Player2.heroState.OnGround)
                        Player2.Jump();
                    break;
                case "Down":
                    if (Player2.velocity.Y == 0) Player2.heroState.GoDown = true;
                    break;
                case "Return":
                    if (GameController.isMainMenu)
                    {
                        if (Player2.heroState.OnDisplayExit) Exit = true;

                        if (Player2.heroState.OnDisplayStart)
                            CheckDisplayStart(Player2);

                        if (Player2.heroState.OnDisplayScore)
                            ScoreActive = true;
                    }

                    if (!Player2.heroState.FireBallActived)
                    {
                        int dir;
                        if (Player2.SpriteRect.Y == 0) dir = -1;
                            else dir = 1;

                        fireBall2 = new Projectile("fireBall", new RectangleF((dir == 1) ? Player2.GameObjectRect.Right + 1 : Player2.GameObjectRect.Left - 20,
                            Player2.GameObjectRect.Top + Player2.GameObjectRect.Height / 2, 20, 20), new Rectangle(0, 0, 444, 444), dir, 2);
                        gameObjects.Add(fireBall2);
                        Player2.heroState.FireBallActived = true;
                    }
                    break;
                case "Escape":
                    Pause = true;
                    break;
            }
        }

        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "A":
                    Player1.heroState.GoLeft = false;
                    if (Player1.heroState.GoRight) Player1.Direction = 1;
                    break;
                case "D":
                    Player1.heroState.GoRight = false;
                    if (Player1.heroState.GoLeft) Player1.Direction = -1;
                    break;
                case "Left":
                    Player2.heroState.GoLeft = false;
                    if (Player2.heroState.GoRight) Player2.Direction = 1;
                    break;
                case "Right":
                    Player2.heroState.GoRight = false;
                    if (Player2.heroState.GoLeft) Player2.Direction = -1;
                    break;
            }
            if (!Player1.heroState.GoLeft && !Player1.heroState.GoRight) Player1.Direction = 0;
            if (!Player2.heroState.GoLeft && !Player2.heroState.GoRight) Player2.Direction = 0;
        }

        private static Font fontPause = new Font("Courier New", 54);
        private static Brush brushDark = new SolidBrush(Color.FromArgb(220, Color.Black));
        private static StringFormat formatAllCenter = new StringFormat
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        private static Font fontPauseText = new Font("Courier New", 32);
        private static StringFormat formatXCenter = new StringFormat
        {
            Alignment = StringAlignment.Center
        };

        private static void CalculateFPS()
        {
            if ((currentFPSTimer += Time.deltaTime) > 0.5f)
            {
                currentFPSTimer = 0;
                currentFPS = (float)Math.Truncate(1 / Time.deltaTime);
            }
        }

        private static void DrawBlackTab(Graphics g, string firstLine, string secondLine)
        {
            g.FillRectangle(brushDark, 0, 0, FormWidth, FormHeight);
            g.DrawString(firstLine, fontPause, Brushes.White, FormWidth / 2, FormHeight / 2, formatAllCenter);
            g.DrawString(secondLine, fontPauseText, Brushes.White, FormWidth / 2, FormHeight / 2 + 54, formatXCenter);
        }
        public static void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            foreach (var item in renderQueue)
            {
                g.DrawImage(item.SpriteList, item.dest, item.src, GraphicsUnit.Pixel);
            }

            if (GameProcess.StartRound && !(GameProcess.sessionScore.firstPlayerScore == 5 || GameProcess.sessionScore.secondPlayerScore == 5))
            {
                DrawBlackTab(g, $"Round {GameProcess.RoundNumber}", $"{GameProcess.sessionScore.firstPlayerScore} - {GameProcess.sessionScore.secondPlayerScore}");
                if ((currentFPSTimer += Time.deltaTime) > 1.5f)
                {
                    GameProcess.StartRound = false;
                }
            }
            if (GameProcess.StartRound && (GameProcess.sessionScore.firstPlayerScore == 5 || GameProcess.sessionScore.secondPlayerScore == 5))
            {
                DrawBlackTab(g, $"Player {(GameProcess.sessionScore.firstPlayerScore == 5 ? 1 : 2)} WIN", $"{GameProcess.sessionScore.firstPlayerScore} - {GameProcess.sessionScore.secondPlayerScore}");
                if ((currentFPSTimer += Time.deltaTime) > 1.5f)
                {
                    GameProcess.StartRound = false;
                }
            }

            if (Pause)
            {
                DrawBlackTab(g, "Pause", "press any key to resume");
            }

            if (ScoreActive)
            {
                DrawBlackTab(g, "Score", $"{mainScore.firstPlayerScore} - {mainScore.secondPlayerScore}");
            }

            if (!Pause && !ScoreActive && !GameProcess.StartRound)
            {
                CalculateFPS();
                g.DrawString(currentFPS.ToString(), fontPause, Brushes.Green, new PointF(0, 0));
            }

            if (!Pause) renderQueue.Clear();
        }
    }
}
