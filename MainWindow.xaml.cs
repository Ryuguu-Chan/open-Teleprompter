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
using System.IO;

namespace open_Teleprompter
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool PlayMode = true;
        private double Speed = 1;

        private double ArrowCurrentYPos;

        private double CurrentXpos;
        private double CurrentYpos;

        // function that will load the text
        private void LoadText()
        {
            // if the file exists -> go read the file!
            if (File.Exists("text.txt"))
            {
                TelePromptText.Text = File.ReadAllText("text.txt");
                Canvas.SetTop(TelePromptText, this.Height);
            }
        }

        // The function that will scroll the text
        private async void ScrollText() 
        {

            CurrentXpos = Canvas.GetLeft(TelePromptText);
            CurrentYpos = Canvas.GetTop(TelePromptText);

            while (CurrentYpos > double.MinValue)
            {
                if (PlayMode == true)
                {
                    Canvas.SetTop(TelePromptText, CurrentYpos - Speed);

                    CurrentYpos -= Speed;

                    //debugText.Text = CurrentYpos.ToString();
                }
                await Task.Delay(1);
            }
        }

        // all the controls
        // as noted in the documentation
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add)
            {
                Speed += Speed < 10 ? 1 : 0;
            }
            else if (e.Key == Key.Subtract)
            {
                Speed -= Speed != 1 ? 1 : 0;
            }
            else if (e.Key == Key.Up)
            {
                Canvas.SetTop(TelepromptArrow00, (ArrowCurrentYPos - 10));
                Canvas.SetTop(TelepromptArrow01, (ArrowCurrentYPos - 10));

                ArrowCurrentYPos -= 10;
            }
            else if (e.Key == Key.Down)
            {
                Canvas.SetTop(TelepromptArrow00, (ArrowCurrentYPos + 10));
                Canvas.SetTop(TelepromptArrow01, (ArrowCurrentYPos + 10));

                ArrowCurrentYPos += 10;
            }
            else if (e.Key == Key.Space)
            {
                if (PlayMode == false)
                    PlayMode = true;
                else
                    PlayMode = false;
            }
            else if (e.Key == Key.F5)
            {
                LoadText();
            }
            else if (e.Key == Key.Enter)
            {
                PlayMode = false;
                Canvas.SetTop(TelePromptText, this.Height);
                CurrentXpos = Canvas.GetLeft(TelePromptText);
                CurrentYpos = Canvas.GetTop(TelePromptText);
                LoadText();
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TelePromptText.Width = this.Width;
            TelePromptText.Height = this.Height;

            Canvas.SetTop(TelePromptText, this.Height);

            Canvas.SetTop(TelepromptArrow00, (this.Height / 2) - (TelepromptArrow00.Height / 2));
            Canvas.SetTop(TelepromptArrow01, (this.Height / 2) - (TelepromptArrow01.Height / 2));
            Canvas.SetLeft(TelepromptArrow01, (this.Width - TelepromptArrow01.Width));

            ArrowCurrentYPos = Canvas.GetTop(TelepromptArrow00);

            LoadText();

            ScrollText();
        }

        // for making sure that the win+arrow keys won't work >:3
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
    }
}
