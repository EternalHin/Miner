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

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Miner.Init(this, panelMain);
        }

    }
        public static class Miner
        {
            public const int mapSize = 12;
            public const int cellSize = 50;
            public static int[,] map = new int[mapSize + 2, mapSize + 2];
            public static Button[,] buttons = new Button[mapSize + 2, mapSize + 2];
            public static int[,] currentPictureToSet = new int[mapSize + 2, mapSize + 2];
            public static int number;

            public static Image sprite;
            public static bool firstStep = true;

            public static Form mainForm;
            public static Panel panelMain;

            public static void Init(Form tempForm, Panel tempPanel)
            {
                sprite = new Bitmap(@"Sprite\Sprite.png");
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
                        map[i, j] = 10;
                        currentPictureToSet[i, j] = 0;
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
                        button.BackgroundImage = FindNeedImage(1, 4);
                        button.MouseUp += new MouseEventHandler(OnButtonPressMouse);
                        panelMain.Controls.Add(button);
                        buttons[i + 1, j + 1] = button;
                    }
                }
            }

            public static void OnButtonPressMouse(object sender, MouseEventArgs e)
            {
                Button pressedButton = sender as Button;

                switch (e.Button.ToString())
                {
                    case "Right":
                        OnRightButtonPressed(pressedButton);
                        break;
                    case "Left":
                        OnLeftButtonPress(pressedButton);
                        break;
                }
                if (CheckWin())
                {
                    MessageBox.Show("Победа!");
                    Init(mainForm, panelMain);
                }

            }

            public static bool CheckWin()
            {
                int countMin = 0;
                int countFlag = 0;
                for (int i = 1; i <= mapSize; i++)
                {
                    for (int j = 1; j <= mapSize; j++)
                    {
                        if (map[i, j] == -1 && currentPictureToSet[i, j] == 1)
                            countMin++;
                        if (currentPictureToSet[i, j] != 0)
                            countFlag++;
                    }
                }

                return countMin == number && countFlag == countMin;
            }

            public static void OnRightButtonPressed(Button pressedButton)
            {
                int x = pressedButton.Location.Y / cellSize + 1;
                int y = pressedButton.Location.X / cellSize + 1;

                currentPictureToSet[x, y]++;
                currentPictureToSet[x, y] %= 3;
                switch (currentPictureToSet[x, y])
                {
                    case 0:
                        pressedButton.BackgroundImage = FindNeedImage(1, 4);
                        break;
                    case 1:
                        pressedButton.BackgroundImage = FindNeedImage(2, 0);
                        break;
                    case 2:
                        pressedButton.BackgroundImage = FindNeedImage(2, 2);
                        break;
                }
            }

            public static Image FindNeedImage(int xPos, int yPos)
            {
                Image temp = new Bitmap(cellSize, cellSize);
                Graphics graph = Graphics.FromImage(temp);
                graph.DrawImage(sprite,
                    new Rectangle(new Point(0, 0), new Size(cellSize, cellSize)),
                        0 + 32 * yPos, 0 + 32 * xPos, 33, 33, GraphicsUnit.Pixel);

                return temp;
            }


            public static void OnLeftButtonPress(Button pressedButton)
            {
                int x = pressedButton.Location.Y / cellSize + 1;
                int y = pressedButton.Location.X / cellSize + 1;
                if (firstStep)
                {
                    pressedButton.Enabled = false;
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
                                buttons[i, j].BackgroundImage = FindNeedImage(2, 3);
                        }
                    }
                    MessageBox.Show("Игра окончена");
                    Init(mainForm, panelMain);
                }

                OpenCells(x, y);
            }



            public static void OpenCells(int x, int y)
            {
                OpenCell(x, y);

                if (map[x, y] > 0)
                    return;


                for (int tX = x - 1; tX <= x + 1; tX++)
                {
                    for (int tY = y - 1; tY <= y + 1; tY++)
                    {
                        if (!IsInBorder(tX, tY))
                            continue;
                        if (!buttons[tX, tY].Enabled)
                            continue;
                        if (map[tX, tY] == 0)
                            OpenCells(tX, tY);
                        else if (map[tX, tY] > 0)
                            OpenCell(tX, tY);
                    }
                }
            }

            private static void OpenCell(int i, int j)
            {
                buttons[i, j].Enabled = false;

                switch (map[i, j])
                {
                    case 1:
                        buttons[i, j].BackgroundImage = FindNeedImage(0, 1);
                        break;
                    case 2:
                        buttons[i, j].BackgroundImage = FindNeedImage(0, 2);
                        break;
                    case 3:
                        buttons[i, j].BackgroundImage = FindNeedImage(0, 3);
                        break;
                    case 4:
                        buttons[i, j].BackgroundImage = FindNeedImage(0, 4);
                        break;
                    case 5:
                        buttons[i, j].BackgroundImage = FindNeedImage(1, 0);
                        break;
                    case 6:
                        buttons[i, j].BackgroundImage = FindNeedImage(1, 1);
                        break;
                    case 7:
                        buttons[i, j].BackgroundImage = FindNeedImage(1, 2);
                        break;
                    case 8:
                        buttons[i, j].BackgroundImage = FindNeedImage(1, 3);
                        break;
                    case -1:
                        buttons[i, j].BackgroundImage = FindNeedImage(2, 1);
                        break;
                    case 0:
                        buttons[i, j].BackgroundImage = FindNeedImage(0, 0);
                        break;
                }
            }


            public static bool IsInBorder(int i, int j)
            {
                if (i < 1 || j < 1 || j > mapSize || i > mapSize)
                    return false;

                return true;
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
