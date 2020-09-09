using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GomokuGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int mapSize = 15;
        int cellSize = 30;
        int[,] map;
        Button[,] buttons;
        bool isMoved;
        Random random;
        public MainWindow()
        {
            InitializeComponent();
            CreateMap();
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            //(main.Children[new Random().Next(0, 15 * 15 - 1)] as Button).Content = "wadawd";
            int rand = random.Next(0, 15 * 15 + 1);
            (main.Children[random.Next(0, rand)] as Button).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void CreateMap()
        {
            map = new int[mapSize, mapSize];
            buttons = new Button[mapSize, mapSize];
            isMoved = true;
            main.Width = mapSize * cellSize;
            main.Height = mapSize * cellSize;
            this.Width = main.Width + 100;
            this.Height = main.Height + 100;
            random = new Random();
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = 0;
                    Button button = new Button();
                    button.Width = cellSize; button.Height = cellSize;
                    button.Click += Button_Click;
                    buttons[i, j] = button;
                    main.Children.Add(button);

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button pressedButton = sender as Button;
            if (pressedButton.IsEnabled)
            {
                bool isCircle = false;
                pressedButton.FontSize = 20;
                int x = Convert.ToInt32(pressedButton.TranslatePoint(new Point(0, 0), main).X) / cellSize;
                int y = Convert.ToInt32(pressedButton.TranslatePoint(new Point(0, 0), main).Y) / cellSize;
                //MessageBox.Show($"{y}:{x}");
                switch (isMoved)
                {
                    case true:
                        pressedButton.Foreground = Brushes.Red;
                        pressedButton.Content = "X";
                        isMoved = false;
                        map[y, x] = 1;
                        break;
                    case false:
                        pressedButton.Foreground = Brushes.Yellow;
                        pressedButton.Content = "O";
                        isMoved = true;
                        map[y, x] = 2;
                        isCircle = true;
                        break;
                }
                //string array = "";
                //for(int i = 0; i< mapSize; i++)
                //{
                //    for(int j = 0; j < mapSize; j++)
                //    {
                //        array += map[i, j] + "   ";
                //    }
                //    array += "\n";
                //}
                //MessageBox.Show(array);
                pressedButton.IsEnabled = false;
                if (CheckGame(isCircle))
                    ResetGame_Click(null, null);
            }
        }

        private void ResetGame_Click(object sender, RoutedEventArgs e)
        {
            main.Children.Clear();
            CreateMap();
        }

        public bool CheckGame(bool isCircle)
        {
            int move = isCircle ? 2 : 1;
            string win = move == 2 ? "Победили нолики!" : "Победили крестики!";
            for (int i = 0; i < mapSize - 4; i++)
                for (int j = 0; j < mapSize - 4; j++)
                    if (map[i, j] == move && map[i + 1, j + 1] == move && map[i + 2, j + 2] == move && map[i + 3, j + 3] == move && map[i + 4, j + 4] == move)
                    {
                        MessageBox.Show(win); 
                        return true;
                    }
            for (int i = 4; i < mapSize; i++)
                for (int j = 0; j < mapSize - 4; j++)
                    if (map[i, j] == move && map[i - 1, j + 1] == move && map[i - 2, j + 2] == move && map[i - 3, j + 3] == move && map[i - 4, j + 4] == move)
                    {
                        MessageBox.Show(win); 
                        return true;
                    }
            for (int i = 0; i < mapSize; i++)
                for (int j = 4; j < mapSize; j++)
                    if (map[i, j] == move && map[i, j - 1] == move && map[i, j - 2] == move && map[i, j - 3] == move && map[i, j - 4] == move)
                    {
                        MessageBox.Show(win); 
                        return true;
                    }
            for (int i = 0; i < mapSize - 4; i++)
                for (int j = 0; j < mapSize; j++)
                    if (map[i, j] == move && map[i + 1, j] == move && map[i + 2, j] == move && map[i + 3, j] == move && map[i + 4, j] == move)
                    {
                        MessageBox.Show(win); 
                        return true;
                    }
            return false;
        }
    }
}
