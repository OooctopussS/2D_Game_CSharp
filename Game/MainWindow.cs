using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Classes;
using System.Diagnostics;

namespace Game
{
    public partial class MainWindow : Form
    {
        private bool needUpdate = false;
        public MainWindow()
        {
            InitializeComponent();
            Cursor.Hide();
            DisplayMode(FullScreen: true, Border: false);
            GameController.FormWidth = Screen.PrimaryScreen.Bounds.Width;
            GameController.FormHeight = Screen.PrimaryScreen.Bounds.Height;

            GameController.Init();
            KeyDown += new KeyEventHandler(GameController.OnKeyDown);
            KeyUp += new KeyEventHandler(GameController.OnKeyUp);

            while (!GameController.Exit)
            {
                if (!GameController.Pause)
                {
                    Time.SetFrameBeginning(DateTime.Now.Ticks);
                    needUpdate = false;
                    Refresh();
                    Application.DoEvents();
                    if (needUpdate)
                        GameController.Update();
                    Show();
                    Time.CalculateDeltaTime(DateTime.Now.Ticks);
                    Time.CalculateTimeSinceStart();
                }
                else
                {
                    Refresh();
                    Application.DoEvents();
                }
            }
            Application.Exit();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void DisplayMode(bool FullScreen, bool Border)
        {
            if (FullScreen)
            {
                WindowState = FormWindowState.Maximized;
                TopMost = false;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                TopMost = false;
            }

            if (Border)
            {
                FormBorderStyle = FormBorderStyle.FixedDialog;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            GameController.OnPaint(sender, e);
            needUpdate = true;
        }
    }
}
