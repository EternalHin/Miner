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

            public static void Init(Form tempForm, Panel tempPanel)
            {
                mainForm = tempForm;
                panelMain = tempPanel;
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
                        panelMain.Controls.Add(button);
                        buttons[i + 1, j + 1] = button;
                    }
                }
            }

            public static void InitButtons(Form panelMain)
            {
                panelMain.Controls.Clear();
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        Button button = new Button();
                        button.Location = new Point(j * cellSize, i * cellSize);
                        button.Size = new Size(cellSize, cellSize);
                        panelMain.Controls.Add(button);
                        buttons[i + 1, j + 1] = button;
                    }
                }
            }
        }
    }
}
