using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        public static class Miner
        {
            public const int mapSize = 10;
            public const int cellSize = 50;
            public static int[,] map = new int[mapSize + 2, mapSize + 2];
            public static Button[,] buttons = new Button[mapSize + 2, mapSize + 2];

            public static Form mainForm;

            public static void Init(Form tempForm)
            {
                mainForm = tempForm;
                ConfigureFromSize(mainForm);
                InitMap();
                InitButtons(mainForm);
            }
            public static void ConfigureFromSize(Form MainForm)
            {
                MainForm.Width = mapSize * cellSize + 40;
                MainForm.Height = (mapSize + 1) * cellSize + 50;
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

            public static void InitButtons(Form panelMain)
            {
                panelMain.Controls.Clear();
            }
        }
    }
}
