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
using WpfApp1;

namespace KeyboardSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Border border = null;
        private ICollection<Grid> grids = new List<Grid>();

        public MainWindow()
        {
            InitializeComponent();
            InitializingTheGrids();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString());

            foreach (Grid grid in grids)
            {
                foreach (object item in grid.Children)
                {
                    border = item as Border;

                    if (border.Tag != null && border.Tag.ToString() == e.Key.ToString())
                    {
                       PreviousColor.Color = border.Background;

                        border.Background = Brushes.Coral;
                    }
                }      
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Grid grid in grids)
            {
                foreach (object item in grid.Children)
                {
                    border = item as Border;

                    if (border.Tag != null && border.Tag.ToString() == e.Key.ToString())
                    {
                        border.Background = PreviousColor.Color;
                       // border.Background = Brushes.Blue;

                        TextBlock textBlock = border.Child as TextBlock;

                        if (textBlock != null)
                        {
                            resultString.Text = string.Concat(resultString.Text, textBlock.Text);
                        }
                    }
                }
            }
        }

        public void InitializingTheGrids()
        {
            grids.Add(grid3);
            grids.Add(grid4);
            grids.Add(grid5);
            grids.Add(grid6);
            grids.Add(grid7);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int previousNumber = 0;

            for (int i = 0; i < 115; i++)
            {
                int number = random.Next(1,7);

                if (previousNumber != 1&&number == 1)
                {
                        sampleString.Text = string.Concat(sampleString.Text, " ");
                }
                else
                {
                    int randomNumber = random.Next(97, 122);
                    char symbol = (char)randomNumber;
                    sampleString.Text = string.Concat(sampleString.Text, symbol.ToString());
                }
                previousNumber = number;
            }
        }
    }
}