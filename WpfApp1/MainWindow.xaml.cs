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

namespace KeyboardSimulator
{
    public partial class MainWindow : Window
    {
        private Border border = null;
        private bool capsLockIspressed = false;
        private Brush previousColor = null;
        private ICollection<Grid> grids = new List<Grid>();

        public MainWindow()
        {
            InitializeComponent();
            InitializingTheGrids();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
            {
                capsLockIspressed = true;
            }

            if (capsLockIspressed && e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                foreach (Grid grid in grids)
                {
                    foreach (object item in grid.Children)
                    {
                        border = item as Border;
                        TextBlock textBlock = border.Child as TextBlock;

                        if (textBlock != null)
                        {
                            if (LowerCase.Register)
                            {
                                textBlock.Text = textBlock.Text.ToUpper();
                            }
                            else
                            {
                                textBlock.Text = textBlock.Text.ToLower();
                            }
                        }
                    }
                }
                LowerCase.Register = LowerCase.Register == true ? false : true;
            }
            else
            {
                foreach (Grid grid in grids)
                {
                    foreach (object item in grid.Children)
                    {
                        border = item as Border;

                        if (border.Tag != null && border.Tag.ToString() == e.Key.ToString())
                        {
                            previousColor = border.Background;
                            border.Background = Brushes.Coral;
                        }
                    }
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
            {
                capsLockIspressed = false;
            }

            foreach (Grid grid in grids)
            {
                foreach (object item in grid.Children)
                {
                    border = item as Border;

                    if (border.Tag != null && border.Tag.ToString() == e.Key.ToString())
                    {
                        border.Background = previousColor;
                        TextBlock textBlock = border.Child as TextBlock;
                        string text = string.Empty;

                        if (textBlock != null)
                        {
                            text = textBlock.Text;

                            if (textBlock.Text == "Space" || textBlock.Text == "SPACE")
                            {
                                text = " ";
                            }

                            if (text == sampleString2.Text[0].ToString())
                            {
                                resultString1.Text = string.Concat(resultString1.Text, text);
                                sampleString1.Text = string.Concat(sampleString1.Text, sampleString2.Text[0]);
                                sampleString2.Text = sampleString2.Text.Remove(0, 1);
                            }
                            else
                            {
                                fails.Text = (int.Parse(fails.Text) + 1).ToString();
                            }
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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int from = 97;
            int to = 122;
            int countSymbols = 0;

            if (sampleString2.Text.Length > 0)
            {
                sampleString2.Text = null;
            }

            if (registerOfSelfGeneratedString.IsChecked == true)
            {
                from = 65;
                to = 90;
            }
            else if (registerOfSelfGeneratedString.IsChecked == false)
            {
                from = 97;
                to = 122;
            }

            countSymbols = (int)difficulty.Value;
            Random random = new Random();
            int previousNumber = 0;

            for (int i = 0; i < countSymbols; i++)
            {
                int number = random.Next(1, 7);

                if (previousNumber != 1 && number == 1)
                {
                    sampleString2.Text = string.Concat(sampleString2.Text, " ");
                }
                else
                {
                    int randomNumber = random.Next(from, to);
                    char symbol = (char)randomNumber;
                    sampleString2.Text = string.Concat(sampleString2.Text, symbol.ToString());
                }
                previousNumber = number;
            }
            start.IsEnabled = false;
        }

        private void difficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            difficultyText.Text = ((int)difficulty.Value).ToString();
        }

        private void registerOfSelfGeneratedString_Checked(object sender, RoutedEventArgs e)
        {
            difficulty.Maximum = 92;
        }

        private void registerOfSelfGeneratedString_Unchecked(object sender, RoutedEventArgs e)
        {
            difficulty.Maximum = 115;
        }
    }
}