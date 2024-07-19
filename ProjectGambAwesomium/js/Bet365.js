// JavaScript source code
var Score = '';
var CurrentPoints = '';
var Player1 = '';
var Player2 = '';
var ScoreBoard = document.getElementById('previousSets');
var GamesArray = [];
for (var h = 0; h < ScoreBoard.childNodes[0].childNodes.length; h++) {
    Score += ScoreBoard.childNodes[0].childNodes[h].textContent.replace('-', ' ') + ' ';
}
CurrentPoints = document.getElementById('team1ArenaPoint').textContent + ' ' + document.getElementById('team2ArenaPoint').textContent;

var tmpPl1 = document.getElementById('team1Name');
var tmpPl2 = document.getElementById('team2Name');
if (tmpPl1 != null) {
    Player1 = document.getElementById('team1Name').textContent;
    Player2 = document.getElementById('team2Name').textContent;
}

var MatchBets = document.getElementsByClassName('Market');

for (var h = 0; h < MatchBets.length; h++) {
    if (MatchBets[h].textContent.indexOf('Game Winner')!=-1) {
        var GameNumber = MatchBets[h].childNodes[0].childNodes[0].textContent.replace('Game Winner', '').replace('th', '').replace('nd', '').replace('st', '').replace('rd', '');
        var PlayerEqual = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].textContent;
        var Coefficent1 = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].childNodes[1].textContent;
        var Coefficent2 = MatchBets[h].childNodes[1].childNodes[0].childNodes[1].childNodes[1].textContent;
        PlayerEqual = PlayerEqual.replace('(Svr)', '');
        if (Player1.indexOf(PlayerEqual) !=-1)
            GamesArray.push({ SetNum: '0', GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
        else
            GamesArray.push({ SetNum: '0', GameNum: GameNumber, Coef1: Coefficent2, Coef2: Coefficent1 });

    }

}