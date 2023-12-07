using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UDP_Socket_stone_scissors_paper
{
    public class AppVM
    {
        MainWindow _view;
        Connections _connection;
        Commands _getSend;
        Commands _getAction;
        Commands _getGameFormat;

        string _action = "stone";

        public AppVM(MainWindow mainWindow) 
        {
            _view = mainWindow;

            _connection = new Connections();

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
            _view.Title = await _connection.Send(_action);
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
    }
}
