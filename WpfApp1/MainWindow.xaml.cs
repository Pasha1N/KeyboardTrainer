using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace KeyboardTrainer
{
    public partial class MainWindow : Window
    {
        private ButtonIsPressed buttonIsPressed = new ButtonIsPressed();
        private Border border;
        private bool capsLockIspressed;
        private ICollection<Grid> grids = new List<Grid>();
        private ICollection<Key> limitedKeys = new List<Key>();
        private int lengthSampleString;
        private int numberOfCorrectClicks;
        private int numberOfClickInSecond;
        private Brush previousColor;
        private ICollection<int> unnecessaryCharacterCodes = new List<int>();
        private DispatcherTimer update;


        public MainWindow()
        {
            InitializeComponent();
            Initializing();
            update = new DispatcherTimer();
            update.Interval = TimeSpan.FromSeconds(1);
            update.Tick += new EventHandler(Update_Timer);
        }

        private void Difficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            difficultyText.Text = ((int)difficulty.Value).ToString();
        }

        private void RegisterOfSelfGeneratedString_Checked(object sender, RoutedEventArgs e)
        {
            //92 это максимальное число символов в само генерируемой строке
            difficulty.Maximum = 92;
        }

        private void RegisterOfSelfGeneratedString_Unchecked(object sender, RoutedEventArgs e)
        {
            //115 это максимальное число символов в само генерируемой строке
            difficulty.Maximum = 115;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //from и to это диапазон кодов в таблице ASCII
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
                to = 122;
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
                // [1,7] этот диапазон говорит что в одном случае из семи будет генерироваться пробел в само генерируемой строке
                int number = random.Next(1, 7);

                if (previousNumber != 1 && number == 1)
                {
                    sampleString2.Text = string.Concat(sampleString2.Text, " ");
                }
                else
                {
                    bool work = true;
                    int randomNumber = random.Next(from, to);

                    if (registerOfSelfGeneratedString.IsChecked == false)
                    {
                        char symbol = (char)randomNumber;
                        sampleString2.Text = string.Concat(sampleString2.Text, symbol.ToString());
                    }
                    else if (registerOfSelfGeneratedString.IsChecked == true)
                    {
                        foreach (int item in unnecessaryCharacterCodes)
                        {
                            if (randomNumber == item)
                            {
                                work = false;
                                break;
                            }
                        }

                        if (work)
                        {
                            char symbol = (char)randomNumber;
                            sampleString2.Text = string.Concat(sampleString2.Text, symbol.ToString());
                        }
                    }
                }
                previousNumber = number;
            }

            lengthSampleString = sampleString2.Text.Length;
            start.IsEnabled = false;
            stop.IsEnabled = true;
            update.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            PressedStop();
        }

        public void Update_Timer(object sender, EventArgs e)
        {
            int seconds = 60;
            int speed = numberOfClickInSecond * seconds;
            characters.Text = speed.ToString();
            numberOfClickInSecond = 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
            {
                capsLockIspressed = true;
            }

            if (capsLockIspressed && e.Key == Key.LeftShift || capsLockIspressed && e.Key == Key.RightShift)
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

                LowerCase.Register = LowerCase.Register == false;
            }
            else
            {
                bool work = true;
                work = AreRestrictionsFound(e.Key);
                if (work)
                {
                    foreach (Grid grid in grids)
                    {
                        foreach (object item in grid.Children)
                        {
                            border = item as Border;

                            if (border.Tag != null && border.Tag.ToString() == e.Key.ToString())
                            {
                                if (!buttonIsPressed.IsPressed)
                                {
                                    buttonIsPressed.IsPressed = true;
                                    previousColor = border.Background;
                                }

                                border.Background = Brushes.LawnGreen;
                            }
                        }
                    }
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            buttonIsPressed.IsPressed = false;
            bool work = true;

            if (e.Key == Key.CapsLock)
            {
                capsLockIspressed = false;
            }

            work = AreRestrictionsFound(e.Key);

            if (work)
            {
                foreach (Grid grid in grids)
                {
                    foreach (object item in grid.Children)
                    {
                        border = item as Border;

                        if (border.Tag != null && border.Tag.ToString() == e.Key.ToString())
                        {
                            border.Background = previousColor;

                            if (sampleString2.Text.Length > 0)
                            {
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
                                        ++numberOfCorrectClicks;
                                        ++numberOfClickInSecond;

                                        if (resultString1.Text.Length == lengthSampleString)
                                        {
                                            PressedStop();
                                        }
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
            }
        }

        public bool AreRestrictionsFound(Key erequiredKey)
        {
            bool work = true;

            foreach (Key key in limitedKeys)
            {
                if (erequiredKey == key)
                {
                    work = false;
                    break;
                }
            }
            return work;
        }

        public void Initializing()
        {
            grids.Add(grid3);
            grids.Add(grid4);
            grids.Add(grid5);
            grids.Add(grid6);
            grids.Add(grid7);

            limitedKeys.Add(Key.Capital);
            limitedKeys.Add(Key.LeftShift);
            limitedKeys.Add(Key.RightShift);

            unnecessaryCharacterCodes.Add(91);
            unnecessaryCharacterCodes.Add(92);
            unnecessaryCharacterCodes.Add(93);
            unnecessaryCharacterCodes.Add(94);
            unnecessaryCharacterCodes.Add(95);
            unnecessaryCharacterCodes.Add(96);
        }

        public void PressedStop()
        {
            sampleString1.Text = string.Empty;
            sampleString2.Text = string.Empty;
            resultString1.Text = string.Empty;
            fails.Text = string.Empty;
            characters.Text = string.Empty;
            difficultyText.Text = string.Empty;
            difficulty.Value = 1;

            stop.IsEnabled = false;
            start.IsEnabled = true;
        }
    }
}