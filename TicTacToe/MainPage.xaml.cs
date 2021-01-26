using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TicTacToe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        enum Player { X, O};
        Player currentPlayer = Player.X;
        int xWins = 0;
        int oWins = 0;
        int draws = 0;
        bool winnerIsDeclared = false;
        bool isDraw = false;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(500, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            xTurn.Background = new SolidColorBrush(Colors.Green);
            oTurn.Background = new SolidColorBrush(Colors.White);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (currentPlayer == Player.X)
            {
                button.Content = "X";
            }
            else if(currentPlayer == Player.O)
            {
                button.Content = "O";
            }

            button.IsEnabled = false;

            CheckForWinner();

            if (currentPlayer == Player.X)
            {
                currentPlayer = Player.O;
            }
            else if (currentPlayer == Player.O)
            {
                currentPlayer = Player.X;
            }

            ActivePlayerChanged();
        }

        private async void CheckForWinner()
        {
            if (A1Button.Content == A2Button.Content && A2Button.Content == A3Button.Content && A1Button.Content != null)
            {
                winnerIsDeclared = true;
            }
            else if (B1Button.Content == B2Button.Content && B2Button.Content == B3Button.Content && B1Button.Content != null)
            {
                winnerIsDeclared = true;
            }
            else if (C1Button.Content == C2Button.Content && C2Button.Content == C3Button.Content && C1Button.Content != null)
            {
                winnerIsDeclared = true;
            }// End of Horizontal check
            else if (A1Button.Content == B1Button.Content && B1Button.Content == C1Button.Content && A1Button.Content != null)
            {
                winnerIsDeclared = true;
            }
            else if (A2Button.Content == B2Button.Content && B2Button.Content == C2Button.Content && A2Button.Content != null)
            {
                winnerIsDeclared = true;
            }
            else if (A3Button.Content == B3Button.Content && B3Button.Content == C3Button.Content && A3Button.Content != null)
            {
                winnerIsDeclared = true;
            }//End of vertical check
            else if (A1Button.Content == B2Button.Content && B2Button.Content == C3Button.Content && A1Button.Content != null)
            {
                winnerIsDeclared = true;
            }
            else if (A3Button.Content == B2Button.Content && B2Button.Content == C1Button.Content && A3Button.Content != null)
            {
                winnerIsDeclared = true;
            }//End of diagonal check
            else if
                (
                A1Button.Content != null &&
                A2Button.Content != null &&
                A3Button.Content != null &&
                B1Button.Content != null &&
                B2Button.Content != null &&
                B3Button.Content != null &&
                C1Button.Content != null &&
                C2Button.Content != null &&
                C3Button.Content != null &&
                winnerIsDeclared == false
                ) { winnerIsDeclared = false; isDraw = true; } // End of draw check

            if (winnerIsDeclared)
            {
                A1Button.IsEnabled = false;
                A2Button.IsEnabled = false;
                A3Button.IsEnabled = false;
                B1Button.IsEnabled = false;
                B2Button.IsEnabled = false;
                B3Button.IsEnabled = false;
                C1Button.IsEnabled = false;
                C2Button.IsEnabled = false;
                C3Button.IsEnabled = false;

                if (currentPlayer == Player.X)
                {
                    var xIsWinner = new MessageDialog("Congratulations X, you are the winner");
                    await xIsWinner.ShowAsync();
                    xWins++;
                    XWinsTextBox.Text = xWins.ToString();
                }
                else if (currentPlayer == Player.O)
                {
                    var oIsWinner = new MessageDialog("Congratulations O, you are the winner");
                    await oIsWinner.ShowAsync();
                    oWins++;
                    OWinsTextBox.Text = oWins.ToString();
                }
                NewRoundPreparations();
            }
            else if (winnerIsDeclared == false && isDraw == true)
            {
                var gameIsDraw = new MessageDialog("The game is a draw!");
                await gameIsDraw.ShowAsync();
                draws++;
                DrawsTextBox.Text = draws.ToString();
                NewRoundPreparations();
            }
        }

        private void NewRoundPreparations()
        {
            A1Button.Content = null;
            A2Button.Content = null;
            A3Button.Content = null;
            B1Button.Content = null;
            B2Button.Content = null;
            B3Button.Content = null;
            C1Button.Content = null;
            C2Button.Content = null;
            C3Button.Content = null;

            A1Button.IsEnabled = true;
            A2Button.IsEnabled = true;
            A3Button.IsEnabled = true;
            B1Button.IsEnabled = true;
            B2Button.IsEnabled = true;
            B3Button.IsEnabled = true;
            C1Button.IsEnabled = true;
            C2Button.IsEnabled = true;
            C3Button.IsEnabled = true;

            winnerIsDeclared = false;
            isDraw = false;
        }


        private void NewGameClicked(object sender, RoutedEventArgs e)
        {
            NewRoundPreparations();

            xWins = 0;
            oWins = 0;
            draws = 0;
            XWinsTextBox.Text = xWins.ToString();
            OWinsTextBox.Text = oWins.ToString();
            DrawsTextBox.Text = draws.ToString();
            currentPlayer = Player.X;
            ActivePlayerChanged();
        }

        private void ExitGameClicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private async void AboutClicked(object sender, RoutedEventArgs e)
        {
            var message = new MessageDialog("Game created by Daniel Riddersporre as a training project!\n\nVersion 1.0");
            message.Title = "About";
            await message.ShowAsync();
        }

        private void ActivePlayerChanged()
        {
            if (currentPlayer == Player.X)
            {
                xTurn.Background = new SolidColorBrush(Colors.Green);
                oTurn.Background = new SolidColorBrush(Colors.White);
            }
            else if (currentPlayer == Player.O)
            {
                xTurn.Background = new SolidColorBrush(Colors.White);
                oTurn.Background = new SolidColorBrush(Colors.Green);
            }
        }
    }
}
