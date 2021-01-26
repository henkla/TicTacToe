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
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TicTacToe
{
    public enum Player
    {
        X = 0,
        O = 1
    };

    public enum Outcome
    {
        XWins = 0,
        OWins = 1,
        Draws = 2
    }

    public enum RoundState
    {
        Unfinished = 0,
        Won = 1,
        Draw = 2,
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const Player _defaultStartingPlayer = Player.X;

        private RoundState _roundState;
        private Player _currentPlayer;
        private readonly List<Button> _buttons;
        private readonly IDictionary<Outcome, int> _gameState;
        private readonly IDictionary<Outcome, TextBlock> _textBlocks;

        public MainPage()
        {
            InitializeComponent();

            _buttons = new List<Button>()
            {
                A1Button,
                A2Button,
                A3Button,
                B1Button,
                B2Button,
                B3Button,
                C1Button,
                C2Button,
                C3Button
            };

            _textBlocks = new Dictionary<Outcome, TextBlock>()
            {
                { Outcome.OWins, OWinsTextBox },
                { Outcome.XWins, XWinsTextBox },
                { Outcome.Draws, DrawsTextBox }
            };

            _gameState = new Dictionary<Outcome, int>()
            {
                { Outcome.OWins, 0 },
                { Outcome.XWins, 0 },
                { Outcome.Draws, 0 }
            };

            _currentPlayer = _defaultStartingPlayer;
            _roundState = RoundState.Unfinished;

            ApplicationView.PreferredLaunchViewSize = new Size(500, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            xTurn.Background = new SolidColorBrush(Colors.Green);
            oTurn.Background = new SolidColorBrush(Colors.White);
        }

        private void GameFieldButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            button.Content = _currentPlayer.ToString();
            button.IsEnabled = false;

            CheckForWinnerAndHandleOutcome();

            // unless game is finished
            TogglePlayer();
            ActivePlayerChanged();
        }

        private async void CheckForWinnerAndHandleOutcome()
        {
            if (A1Button.Content == A2Button.Content && A2Button.Content == A3Button.Content && A1Button.Content != null)
            {
                _roundState = RoundState.Won;
            }
            else if (B1Button.Content == B2Button.Content && B2Button.Content == B3Button.Content && B1Button.Content != null)
            {
                _roundState = RoundState.Won;
            }
            else if (C1Button.Content == C2Button.Content && C2Button.Content == C3Button.Content && C1Button.Content != null)
            {
                _roundState = RoundState.Won;
            }// End of Horizontal check
            else if (A1Button.Content == B1Button.Content && B1Button.Content == C1Button.Content && A1Button.Content != null)
            {
                _roundState = RoundState.Won;
            }
            else if (A2Button.Content == B2Button.Content && B2Button.Content == C2Button.Content && A2Button.Content != null)
            {
                _roundState = RoundState.Won;
            }
            else if (A3Button.Content == B3Button.Content && B3Button.Content == C3Button.Content && A3Button.Content != null)
            {
                _roundState = RoundState.Won;
            }//End of vertical check
            else if (A1Button.Content == B2Button.Content && B2Button.Content == C3Button.Content && A1Button.Content != null)
            {
                _roundState = RoundState.Won;
            }
            else if (A3Button.Content == B2Button.Content && B2Button.Content == C1Button.Content && A3Button.Content != null)
            {
                _roundState = RoundState.Won;
            }//End of diagonal check
            else if (_buttons.All(b => b.Content != null))
            {
                _roundState = RoundState.Draw;
            }

            // do we have a finished round?
            if (_roundState != RoundState.Unfinished)
            {
                await HandleFinishedRound();
                PrepareNewRound();
            }
        }

        private async Task HandleFinishedRound()
        {
            var dialogText = "";

            if (_roundState == RoundState.Won)
            {
                dialogText = $"Congratulations {_currentPlayer}, you are the winner";

                switch (_currentPlayer)
                {
                    case Player.X:
                        _gameState[Outcome.XWins]++;
                        _textBlocks[Outcome.XWins].Text = _gameState[Outcome.XWins].ToString();
                        break;
                    case Player.O:
                        _gameState[Outcome.OWins]++;
                        _textBlocks[Outcome.OWins].Text = _gameState[Outcome.OWins].ToString();
                        break;
                }
            }
            else if (_roundState == RoundState.Draw)
            {
                dialogText = "The game is a draw!";
                _gameState[Outcome.Draws]++;
                _textBlocks[Outcome.Draws].Text = _gameState[Outcome.Draws].ToString();
            }

            var dialog = new MessageDialog(dialogText);
            await dialog.ShowAsync();
        }

        private void PrepareNewRound()
        {
            _buttons.ForEach(b =>
            {
                b.Content = null;
                b.IsEnabled = true;
            });

            _currentPlayer = _defaultStartingPlayer;
            _roundState = RoundState.Unfinished;
        }

        private void NewGameClicked(object sender, RoutedEventArgs e)
        {
            PrepareNewRound();

            // totally new game, so reset all previous game's outcome counters
            foreach (var key in _textBlocks.Keys)
            {
                _gameState[key] = 0;
                _textBlocks[key].Text = "0";
            }

            // every game must start with player X
            _currentPlayer = Player.X;
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
            switch (_currentPlayer)
            {
                case Player.X:
                    xTurn.Background = new SolidColorBrush(Colors.Green);
                    oTurn.Background = new SolidColorBrush(Colors.White);
                    break;
                case Player.O:
                    xTurn.Background = new SolidColorBrush(Colors.White);
                    oTurn.Background = new SolidColorBrush(Colors.Green);
                    break;
            }
        }

        private void TogglePlayer()
        {
            switch (_currentPlayer)
            {
                case Player.X:
                    _currentPlayer = Player.O;
                    break;
                case Player.O:
                    _currentPlayer = Player.X;
                    break;
            }
        }
    }
}
