// JavaScript source code
function CutPlayer(tocut) {
    var ret = tocut;
    var i = 0;
    while (i < ret.length) {
        if (ret[i] == '.') {
            ret = ret.remove(i - 1, 2);
            i = i - 1;
        }
        i++;
    }
    return ret;
}

var Score = '';
var CurrentPoints = '';
var Player1 = '';
var Player2 = '';
var numset = '';
var numgame = '';
var coef1 = '';
var coef1 = '';
var GamesArray = [];
var splitted = [];

var PlayersData = document.getElementsByClassName('live-member-name nowrap');
var GamesData = document.getElementsByClassName('market-table-name');
if (PlayersData != null && PlayersData.length > 0) {
    Player1 = PlayersData[0].textContent.replace(',', ' ');
    Player2 = PlayersData[1].textContent.replace(',', ' ');
    Score = document.getElementsByClassName('cl-left red')[0].textContent;
    splitted = Score.split(' ');
    CurrentPoints = splitted[splitted.length - 1].replace(':', ' ').replace('(', '').replace(')', '');
    if (Player1.indexOf('.') != -1 || Player2.indexOf('.') != -1) {
        Player1 = CutPlayer(Player1);
        Player2 = CutPlayer(Player2);
    }
    if (splitted.length == 3) {
        Score = splitted[0].replace('(', '').replace(')', '');
    }
    else {
        Score = '';
        for (var s = 1; s < splitted.length - 1; s++) {
            Score += splitted[s].replace(':', ' ').replace(',', ' ');
        }
        Score = Score.replace('(', '').replace(')', '');
    }
    GamesData = document.getElementsByClassName('market-table-name');
    for (var k = 0; k < GamesData.length; k++) {
        if (GamesData[k].textContent.indexOf('To Win Game') != -1) {
            var TableWithGames = GamesData[k].nextSibling.nextSibling;
            var CountOfTr = 0;
            if (TableWithGames.getElementsByTagName('tr') != null)
                CountOfTr = TableWithGames.getElementsByTagName('tr').length;
            for (var h = 2; h <= CountOfTr; h++) {

                if (TableWithGames.getElementsByTagName('tr')[h] != null) {
                    //var index = GamesData[k].textContent
                    numset = GamesData[k].textContent.match(/\d/gi)[0];
                    numgame = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[0].getElementsByTagName('span')[0].textContent.replace('Game', '').replace(' ', '').replace(' ', '');
                    coef1 = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[1].getElementsByTagName('div')[0].textContent.replace('\n', '').replace(' ', '').replace(' ', '').trim();
                    coef2 = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[2].getElementsByTagName('div')[0].textContent.replace('\n', '').replace(' ', '').replace(' ', '').trim();
                    GamesArray.push({ SetNum: numset, GameNum: numgame, Coef1: coef1, Coef2: coef2 });
                }

            }

        }
    }
}