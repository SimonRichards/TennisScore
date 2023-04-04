using NUnit.Framework;

namespace TennisScore;

public class MatchTest {
    [SetUp]
    public void Setup() {
    }

    [Test]
    public void Test1() { 
        Match match = new Match("player 1", "player 2");
        match.PointWonBy("player 1");
        match.PointWonBy("player 2");
        Assert.That(match.Score(), Is.EqualTo("0-0, 15-15"));

        match.PointWonBy("player 1");
        match.PointWonBy("player 1");
        Assert.That(match.Score(), Is.EqualTo("0-0, 40-15"));

        match.PointWonBy("player 2");
        match.PointWonBy("player 2");
        Assert.That(match.Score(), Is.EqualTo("0-0, Deuce"));

        match.PointWonBy("player 1");
        Assert.That(match.Score(), Is.EqualTo("0-0, Advantage player 1"));

        match.PointWonBy("player 1");
        Assert.That(match.Score(), Is.EqualTo("1-0"));

        for (int i = 0; i < 4; ++i) {
            WinGame(match, "player 1");
            Assert.That(match.Score(), Is.EqualTo($"{2 + i}-0"));
        }
        for (int i = 0; i < 5; ++i) {
            WinGame(match, "player 2");
            Assert.That(match.Score(), Is.EqualTo($"5-{1 + i}"));
        }
        WinGame(match, "player 1");
        Assert.That(match.Score(), Is.EqualTo("6-5"));
        WinGame(match, "player 2");
        Assert.That(match.Score(), Is.EqualTo("6-6"));
        
        for (int i = 0; i < 5; ++i) {
            match.PointWonBy("player 1");
            Assert.That(match.Score(), Is.EqualTo($"6-6, {1 + i}-0"));
        }
        for (int i = 0; i < 5; ++i) {
            match.PointWonBy("player 2");
            Assert.That(match.Score(), Is.EqualTo($"6-6, 5-{1 + i}"));
        }
        
        match.PointWonBy("player 1");
        Assert.That(match.Score(), Is.EqualTo($"6-6, 6-5"));
        match.PointWonBy("player 1");
        Assert.That(match.Score(), Is.EqualTo($"7-6"));
        // game, set, match
    }
    private void WinGame(Match match, string player) {
        for (int i = 0; i < 4; ++i) {
            match.PointWonBy(player);
        }
    }

}