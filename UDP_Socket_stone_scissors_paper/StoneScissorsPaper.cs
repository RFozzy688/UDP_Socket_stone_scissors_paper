using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Socket_stone_scissors_paper
{
    public struct ResultRound
    {
        public string _figures;
        public string _roundsCount;
        public string _resultCurrentRound;
        public string _resultRounds;
        public string _resultGame;
    }
    public struct ResultGame
    {
        public string _resultMatch;
        public string _gamesCount;
        public string _resultGames;
        public string _mostPopularfigure;
        public string _leastPopularfigure;
    }
    public class StoneScissorsPaper
    {
        string? _figuresInRound;
        int _roundsCount;
        const int _maxRoundsCount = 5;
        string? _resultGame;
        int _playerRoundWin;
        int _playerRoundLose;
        string? _resultCurrentRound;
        ResultRound _resultRound = new ResultRound();
        int _playerGameWin;
        int _playerGameLose;
        int _gamesCount;
        string _resultMatch;
        const int _maxGamesCount = 3;
        Dictionary<string, int> _statFigures;
        ResultGame _stResultGame = new ResultGame();

        public StoneScissorsPaper()
        {
            ResetFieldsRound();
            RoundStat();

            _statFigures = new Dictionary<string, int>();
            
            ResetFieldsGame();
            GameStat();
        }

        public ResultRound GetResultRound {  get { return _resultRound; } }
        public ResultGame GetResultGame {  get { return _stResultGame; } }
        public void CheckRound(string actionPlayer, string actionEnemy)
        {
            if (_gamesCount == _maxGamesCount)
            {
                ResetFieldsGame();
            }

            if (_roundsCount == _maxRoundsCount)
            {
                ResetFieldsRound();
            }

            _statFigures[actionPlayer]++;
            _roundsCount++;
            _figuresInRound = actionPlayer + " - " + actionEnemy;

            switch (_figuresInRound)
            {
                case "paper - stone":
                case "scissors - paper":
                case "stone - scissors":
                    _playerRoundWin++;
                    _resultCurrentRound = "win";
                    break;
                case "paper - scissors":
                case "scissors - stone":
                case "stone - paper":
                    _playerRoundLose++;
                    _resultCurrentRound = "lose";
                    break;
                default:
                    _resultCurrentRound = "draw";
                    break;
            }

            if (_roundsCount == _maxRoundsCount)
            {
                if (_playerRoundWin > _playerRoundLose)
                {
                    _resultGame = "win";
                    _playerGameWin++;
                }
                else if (_playerRoundWin < _playerRoundLose)
                {
                    _resultGame = "lose";
                    _playerGameLose++;
                }
                else
                {
                    _resultGame = "draw";
                }

                _gamesCount++;

                if (_gamesCount == _maxGamesCount)
                {
                    if (_playerGameWin > _playerGameLose)
                    {
                        _resultMatch = "win";
                    }
                    else if (_playerRoundWin < _playerRoundLose)
                    {
                        _resultMatch = "lose";
                    }
                    else
                    {
                        _resultMatch = "draw";
                    }
                }

                GameStat();
            }

            RoundStat();
        }
        public void ResetFieldsRound()
        {
            _figuresInRound = "N/A";
            _roundsCount = 0;
            _resultGame = "N/A";
            _playerRoundWin = 0;
            _playerRoundLose = 0;
            _resultCurrentRound = "N/A";
        }
        public void ResetFieldsGame()
        {
            _playerGameWin = 0;
            _playerGameLose = 0;
            _gamesCount = 0;
            _resultMatch = "N/A";

            _statFigures["stone"] = 0;
            _statFigures["scissors"] = 0;
            _statFigures["paper"] = 0;
        }
        public void RoundStat()
        {
            _resultRound._figures = _figuresInRound;
            _resultRound._roundsCount = $"{_roundsCount} / 5";
            _resultRound._resultCurrentRound = _resultCurrentRound;
            _resultRound._resultRounds = $"{_playerRoundWin} : {_playerRoundLose}";
            _resultRound._resultGame = _resultGame;
        }
        public void GameStat()
        {
            _stResultGame._resultMatch = _resultMatch;
            _stResultGame._gamesCount = $"{_gamesCount} / 3";
            _stResultGame._resultGames = $"{_playerGameWin} : {_playerGameLose}";
            _stResultGame._mostPopularfigure = _statFigures.MaxBy(x => x.Value).Value != 0 ? _statFigures.MaxBy(x => x.Value).Key : "N/A";
            _stResultGame._leastPopularfigure = _statFigures.MinBy(x => x.Value).Value != 0 ? _statFigures.MinBy(x => x.Value).Key : "N/A";
        }
        public void AdmitDraw()
        {
            _roundsCount = 5;
            _resultCurrentRound = "draw";
            _resultGame = "draw";
            _playerRoundWin = 0;
            _playerRoundLose = 0;
            _figuresInRound = "stone - stone";
            _gamesCount++;

            RoundStat();
            GameStat();
        }
        public void AdmitYourselfDefeat()
        {
            _roundsCount = 5;
            _resultCurrentRound = "lose";
            _resultGame = "lose";
            _playerRoundWin = 0;
            _playerRoundLose = 5;
            _figuresInRound = "stone - paper";
            _gamesCount++;
            _playerGameLose++;

            RoundStat();
            GameStat();
        }
        public void AdmitEnemylfDefeat()
        {
            _roundsCount = 5;
            _resultCurrentRound = "win";
            _resultGame = "win";
            _playerRoundWin = 5;
            _playerRoundLose = 0;
            _figuresInRound = "paper - stone";
            _gamesCount++;
            _playerGameWin++;

            RoundStat();
            GameStat();
        }
    }
}
