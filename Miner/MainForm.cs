using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace miner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Miner.Init(this, panelMain);
        }

        public static class Miner
        {
            private const int mapSize = 12;
            public const int cellSize = 50;
            public static int[,] map = new int[mapSize + 2, mapSize + 2];
            public static Button[,] buttons = new Button[mapSize + 2, mapSize + 2];

            public static Form mainForm;
            public static Panel panelMain;

            public static int number;
            public static bool firstStep = true;


            public static void Init(Form tempForm, Panel tempPanel)
            {
                mainForm = tempForm;
                panelMain = tempPanel;
                firstStep = true;
                ConfigureFromSize(mainForm, panelMain);
                InitMap();
                InitButtons(panelMain);
            }

            public static void ConfigureFromSize(Form MainPanel, Panel panelMain)
            {
                MainPanel.Width = mapSize * cellSize + 40;
                MainPanel.Height = (mapSize + 1) * cellSize + 50;
                panelMain.Width = mapSize * cellSize;
                panelMain.Height = (mapSize + 1) * cellSize;
            }

            public static void InitMap()
            {
                for (int i = 0; i <= mapSize; i++)
                {
                    for (int j = 0; j <= mapSize; j++)
                    {
                        map[i, j] = 0;
                    }
                }
            }

            public static void InitButtons(Panel panelMain)
            {
                panelMain.Controls.Clear();
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        Button button = new Button();
                        button.Location = new Point(j * cellSize, i * cellSize);
                        button.Size = new Size(cellSize, cellSize);
                        button.MouseUp += new MouseEventHandler(OnButtonPressMouse);
                        panelMain.Controls.Add(button);
                        buttons[i + 1, j + 1] = button;
                    }
                }
            }

            public static void OnButtonPressMouse(object sender, MouseEventArgs e)
            {
                Button pressButton = sender as Button;
                switch (e.Button.ToString())
                {
                    case "Right":

                        break;
                    case "Left":
                        OnLeftButtonPress(pressButton);
                        break;
                }
            }
            public static void OnLeftButtonPress(Button pressedButton)
            {
                int x = pressedButton.Location.Y / cellSize + 1;
                int y = pressedButton.Location.X / cellSize + 1;
                pressedButton.Enabled = false;
                if (firstStep)
                {
                    SeedMap();
                    firstStep = false;
                }

                if (map[x, y] == -1)
                {
                    for (int i = 1; i <= mapSize; i++)
                    {
                        for (int j = 1; j <= mapSize; j++)
                        {
                            if (map[i, j] == -1)
                                buttons[i, j].BackColor = Color.Red;
                        }
                    }
                    MessageBox.Show("GG");
                    Init(mainForm, panelMain);
                    return;
                }

                buttons[x, y].Text = CheckMin(x, y).ToString();
            }
            public static void SeedMap()
            {
                Random rnd = new Random();
                number = rnd.Next(mapSize, 4 * mapSize);
                int x = rnd.Next(0, mapSize) + 1;
                int y = rnd.Next(0, mapSize) + 1;

                for (int n = 0; n < number; n++)
                {
                    while (map[x, y] == -1 || !buttons[x, y].Enabled)
                    {
                        x = rnd.Next(0, mapSize) + 1;
                        y = rnd.Next(0, mapSize) + 1;
                    }

                    map[x, y] = -1;
                }

                for (int i = 1; i <= mapSize; i++)
                {
                    for (int j = 1; j <= mapSize; j++)
                    {
                        if (map[i, j] == -1)
                            continue;
                        map[i, j] = CheckMin(i, j);

                    }
                }
            }
            public static int CheckMin(int xPos, int yPos)
            {
                int count = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (map[xPos + i, yPos + j] == -1)
                            count++;
                    }
                }
                return count;
            }
        }
    }
}
