using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
        }
        public Commands GetSend {  get { return _getSend; } }
        public Commands GetAction {  get { return _getAction; } }
        public Commands GetGameFormat {  get { return _getGameFormat; } }
        async void Send(object param)
        {
            string actionEnemy = await _connection.Send(_action);

            _stoneScissorsPaper.CheckRound(_action, actionEnemy);

            ShowRoundStat(_stoneScissorsPaper.GetResultRound);
        }
        void PlayerAction(object param)
        {
            _action = param as string;
        }
        void GameFormat(object param)
        {
            string? str = param as string;

            switch (str)
            {
                case "Human - Bot":
                case "Bot - Bot":
                    _connection.ConnectTo("127.0.0.1", 8080);
                    break;
                case "Human - Human":
                    _connection.ConnectTo("127.0.0.1", 8081);
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
        }
    }
}
