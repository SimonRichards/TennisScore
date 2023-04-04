using System.Collections.Generic;
using System.Linq;

namespace TennisScore; 

public class Match {

    private readonly int[] _gamesWon = { 0, 0 };
    private readonly int[] _currentGameScore = { 0, 0 };
    private readonly Dictionary<string, int> _playerIndices = new();
    private const int TieBreakerGameCount = 6;
    private const int MinDeuceScore = 3;
    private const int MinLeadToWinGame = 2;
    private const int MinScoreToWinGame = 4;
    private const int MinScoreToWinTieBreakerGame = 7;
    private const int MaxScoreToWinGame = 7;

    public Match(string player1Name, string player2Name) {
        _playerIndices[player1Name] = 0;
        _playerIndices[player2Name] = 1;
    }
    
    public void PointWonBy(string player) {
        int playerIndex = _playerIndices[player];
        _currentGameScore[playerIndex]++;
        if (IsGameComplete()) {
            ResetGameScore();
            _gamesWon[playerIndex]++;
        }
    }

    public string Score() {
        var setScore = JoinScores(_gamesWon);
        return IsGameInProgress() ? $"{setScore}, {GetGameScore()}" : setScore;
    }

    private string GetGameScore() {
        if (IsTieBreaker()) {
            return JoinScores(_currentGameScore);
        }
        if (IsDeuce()) {
            return "Deuce";
        }
        foreach (var (name, index) in _playerIndices) {
            if (IsAdvantageToPlayer(index)) {
                return $"Advantage {name}";
            }
        }
        return JoinScores(_currentGameScore.Select(GetScoreInTennisNumbers));
    }

    private void ResetGameScore() {
        _currentGameScore[0] = _currentGameScore[1] = 0;
    }
    private bool IsGameComplete() {
        int highScore = _currentGameScore.Max();
        int lowScore = _currentGameScore.Min();
        if (IsTieBreaker()) {
            return highScore >= lowScore + MinLeadToWinGame && highScore >= MinScoreToWinTieBreakerGame;
        }
        return highScore == MaxScoreToWinGame || (highScore >= lowScore + MinLeadToWinGame && highScore >= MinScoreToWinGame);
    }

    private bool IsAdvantageToPlayer(int player) {
        int otherPlayer = 1 - player; 
        return _currentGameScore[player] > _currentGameScore[otherPlayer] && _currentGameScore[player] > MinDeuceScore;
    }

    private bool IsTieBreaker() => _gamesWon[0] == _gamesWon[1] && _gamesWon[0] == TieBreakerGameCount;

    private bool IsGameInProgress() => _currentGameScore.Sum() > 0;

    private bool IsDeuce() => _currentGameScore[0] == _currentGameScore[1] && _currentGameScore[0] >= MinDeuceScore;

    private static string JoinScores(IEnumerable<int> scores) => string.Join("-", scores);

    private static int GetScoreInTennisNumbers(int scoreInSensibleNumbers) =>
        new[] {
            0, 15, 30, 40
        }[scoreInSensibleNumbers];
}