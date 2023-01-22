using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Game.Classes
{
    public static class GameProcess
    {
        private static float currentFPSTimer = 0;
        public static bool StartRound { get; set; } = false;
        public static int RoundNumber { get; set; } = 0;
        public static score sessionScore = new score()
        {
            firstPlayerScore = 0,
            secondPlayerScore = 0
        };
        private static void DisplayStartCounting()
        {
            if ((currentFPSTimer += Time.deltaTime) > 1f)
            {
                currentFPSTimer = 0;
                foreach (var item in GameController.gameObjects)
                {
                    if (item.TitleObject == "DisplayStart")
                    {
                        if (item.SpriteRect.Y < 512)
                        {
                            item.SpriteRect.Y += 64;
                        }
                        else
                        {
                            GameController.isMainMenu = false;
                            GameController.Player1.heroState.Ready = false;
                            GameController.Player2.heroState.Ready = false;
                        }

                        break;
                    }
                }
            }
        }

        public static void MakeMap()
        {
            GameController.gameObjects.Clear();
            var random = new Random(DateTime.Now.Millisecond);
            switch (random.Next(1, 4))
            {
                case 1:
                    GameController.LoadMap("../../Maps/Map1");
                    GameController.Player1.GameObjectRect.X = GameController.FormWidth * 2 / 10;
                    GameController.Player1.GameObjectRect.Y = 0;
                    GameController.gameObjects.Add(GameController.Player1);
                    GameController.Player2.GameObjectRect.X = GameController.FormWidth * 8 / 10;
                    GameController.Player2.GameObjectRect.Y = 0;
                    GameController.gameObjects.Add(GameController.Player2);
                    break;
                case 2:
                    GameController.LoadMap("../../Maps/Map2");
                    GameController.Player1.GameObjectRect.X = GameController.FormWidth * 5 / 10;
                    GameController.Player1.GameObjectRect.Y = GameController.FormHeight * 2 / 10 - GameController.Player1.GameObjectRect.Height;
                    GameController.gameObjects.Add(GameController.Player1);
                    GameController.Player2.GameObjectRect.X = GameController.FormWidth * 5 / 10;
                    GameController.Player2.GameObjectRect.Y = GameController.FormHeight - GameController.Player2.GameObjectRect.Height;
                    GameController.gameObjects.Add(GameController.Player2);
                    break;
                case 3:
                    GameController.LoadMap("../../Maps/Map3");
                    GameController.Player1.GameObjectRect.X = GameController.FormWidth * 5 / 10;
                    GameController.Player1.GameObjectRect.Y = 0;
                    GameController.gameObjects.Add(GameController.Player1);
                    GameController.Player2.GameObjectRect.X = GameController.FormWidth * 5 / 10;
                    GameController.Player2.GameObjectRect.Y = 0;
                    GameController.gameObjects.Add(GameController.Player2);
                    break;
            }
        }

        // SpawnWeapon
        public static void Start()
        {
            DisplayStartCounting();
            if (!GameController.isMainMenu)
            {
                sessionScore.firstPlayerScore = 0;
                sessionScore.secondPlayerScore = 0;
                MakeMap();
                GameController.SpawnBonuses();
                StartRound = true;
            }
        }
    }
}
