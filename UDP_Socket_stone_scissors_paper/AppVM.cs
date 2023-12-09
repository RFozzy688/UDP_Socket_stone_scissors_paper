using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace UDP_Socket_stone_scissors_paper
{
    public class AppVM
    {
        MainWindow _view;
        Connections _connection;
        StoneScissorsPaper _stoneScissorsPaper;
        Commands _getSend;
        Commands _getAction;
        Commands _getGameFormat;
        Commands _getOfferDraw;
        Commands _getAdmitDefeat;
        DispatcherTimer _timer;
        Random _random = new Random();
        int _counter = 0;

        string _action = "stone";

        public AppVM(MainWindow mainWindow) 
        {
            _view = mainWindow;

            _connection = new Connections();
            _stoneScissorsPaper = new StoneScissorsPaper();

            ShowRoundStat(_stoneScissorsPaper.GetResultRound);
            ShowGameStat(_stoneScissorsPaper.GetResultGame);

            _getSend = new Commands(Send);
            _getAction = new Commands(PlayerAction);
            _getGameFormat = new Commands(GameFormat);

            GameFormat((object)"Human - Bot");

            CheckButtons();

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(StepBot_Timer);
            _timer.Interval = new TimeSpan(0, 0, 1);
        }
        public int GetLocalPort {  get; set; }
        public int GetRemotePort {  get; set; }
        public Commands GetSend {  get { return _getSend; } }
        public Commands GetAction {  get { return _getAction; } }
        public Commands GetGameFormat {  get { return _getGameFormat; } }
        async void Send(object param)
        {
            string actionEnemy = string.Empty;
            
            if (_view.Human_Human.IsChecked == true)
            {
                _view.Send.IsEnabled = false;

                await _connection.SendMessageAsync(_action);

                actionEnemy = await _connection.ReceiveMessageAsync();

                _view.Send.IsEnabled = true;
            }
            else
            {
                actionEnemy = await _connection.Send(_action);
            }

            _stoneScissorsPaper.CheckRound(_action, actionEnemy);

            ShowRoundStat(_stoneScissorsPaper.GetResultRound);
        }
        void PlayerAction(object param)
        {
            _action = param as string;
        }
        async void GameFormat(object param)
        {
            CheckButtons();
            _stoneScissorsPaper.ResetFieldsRound();
            _stoneScissorsPaper.ResetFieldsGame();
            _stoneScissorsPaper.RoundStat();
            _stoneScissorsPaper.GameStat();
            ShowRoundStat(_stoneScissorsPaper.GetResultRound);
            ShowGameStat(_stoneScissorsPaper.GetResultGame);

            string? str = param as string;

            switch (str)
            {
                case "Human - Bot":
                    _connection.ConnectTo("127.0.0.1", 8080);
                    break;
                case "Bot - Bot":
                    _connection.ConnectTo("127.0.0.1", 8080);
                    GameFormatBotBot();
                    break;
                case "Human - Human":
                    CreateWndConnectToPlayer();
                    _connection.ConnectTo("127.0.0.1", GetLocalPort, GetRemotePort);
                    PlayerAction((object)"stone");
                    break;
            }
        }
        void ShowRoundStat(ResultRound stat)
        {
            _view.Figures.Text = stat._figures;
            _view.RoundsCount.Text = stat._roundsCount;

            if (stat._roundsCount.CompareTo("5 / 5") == 0)
            {
                ShowGameStat(_stoneScissorsPaper.GetResultGame);
            }

            _view.ResultCurrentRound.Text = stat._resultCurrentRound;
            _view.ResultRounds.Text = stat._resultRounds;
            _view.ResultGame.Text = stat._resultGame;
        }
        void ShowGameStat(ResultGame resultGame)
        {
            _view.ResultMatch.Text = resultGame._resultMatch;
            _view.GamesCount.Text = resultGame._gamesCount;
            _view.ResultGames.Text = resultGame._resultGames;
            _view.MostPopularFigure.Text = resultGame._mostPopularfigure;
            _view.LeastPopularFigure.Text = resultGame._leastPopularfigure;

            if (resultGame._resultMatch.CompareTo("N/A") != 0 && _view.Bot_Bot.IsChecked == false)
            {
                MessageBox.Show("Game over");
            }
        }
        void CheckButtons()
        {
            if (_view.Human_Bot.IsChecked == true)
            {
                _view.MakeStep.IsChecked = true;
                _view.MakeStep.IsEnabled = true;
                _view.OfferDraw.IsEnabled = false;
                _view.AdmitDefeat.IsEnabled = false;

                _view.Stone.IsChecked = true;
                _view.Scissors.IsChecked = false;
                _view.Paper.IsChecked = false;

                _view.Stone.IsEnabled = true;
                _view.Scissors.IsEnabled = true;
                _view.Paper.IsEnabled = true;

                _view.Send.IsEnabled = true;
            }
            else if (_view.Bot_Bot.IsChecked == true)
            {
                _view.MakeStep.IsChecked = true;
                _view.MakeStep.IsEnabled = false;
                _view.OfferDraw.IsEnabled = false;
                _view.AdmitDefeat.IsEnabled = false;

                _view.Stone.IsChecked = false;
                _view.Scissors.IsChecked = false;
                _view.Paper.IsChecked = false;

                _view.Stone.IsEnabled = false;
                _view.Scissors.IsEnabled = false;
                _view.Paper.IsEnabled = false;

                _view.Send.IsEnabled = false;
            }
            else if (_view.Human_Human.IsChecked == true)
            {
                _view.MakeStep.IsChecked = true;
                _view.MakeStep.IsEnabled = true;
                _view.OfferDraw.IsEnabled = true;
                _view.AdmitDefeat.IsEnabled = true;

                _view.Stone.IsChecked = true;
                _view.Scissors.IsChecked = false;
                _view.Paper.IsChecked = false;

                _view.Stone.IsEnabled = true;
                _view.Scissors.IsEnabled = true;
                _view.Paper.IsEnabled = true;

                _view.Send.IsEnabled = true;
            }
        }
        void GameFormatBotBot()
        {
            _timer.Start();
        }
        void StepBot_Timer(object sender, EventArgs e)
        {
            string[] action = { "stone", "scissors", "paper" };

            _action = action[_random.Next(0, 3)];
            Send(null);

            _counter++;

            if (_counter == 15) 
            {
                _counter = 0;
                _timer.Stop();
                MessageBox.Show("Game over");
            }
        }
        void CreateWndConnectToPlayer()
        {
            ConnectToPlayer connectToPlayer = new ConnectToPlayer(this);
            connectToPlayer.ShowDialog();
        }
    }
}
