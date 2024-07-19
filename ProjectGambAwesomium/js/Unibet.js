// JavaScript source code
var Player1 = document.getElementsByClassName('event-name-home eventName')[0].textContent;
var Player2 = document.getElementsByClassName('event-name-away eventName')[0].textContent;
var EventName = document.getElementsByClassName('clearfix event-path-and-description')[0].textContent;
var Row1 = document.getElementsByClassName('set-row2')[0];
var Row2 = document.getElementsByClassName('set-row3')[0];
var Score = '';
var ScoreSet = '';
var SetGame = '';
var Coefficent2 = '';
var Coefficent1 = '';
var GamesArray = [];
var SetNum = '';
var GameNum = '';
for (var i = 2; i < Row1.childNodes.length - 1; i++) {
    if (Row1.childNodes[i].textContent!='')
    Score += Row1.childNodes[i].textContent + ' ' + Row2.childNodes[i].textContent + ' ';
}
var list = document.getElementsByClassName('criteria');
ScoreSet = Row1.childNodes[Row1.childNodes.length - 1].textContent + ' ' + Row2.childNodes[Row2.childNodes.length - 1].textContent;
for (i = 0; i < list.length; i++) {
    if (list[i].childNodes[0].textContent.length == 15 || list[i].childNodes[0].textContent.length == 14) {
        var SetGame = list[i].childNodes[0].textContent.replace('SET', '').replace('GAME', '').replace('-', '');
        var Coefficent2 = list[i].nextSibling.childNodes[2].childNodes[0].childNodes[1].textContent;
        var Coefficent1 = list[i].nextSibling.childNodes[1].childNodes[0].childNodes[1].textContent;
        SetNumber = SetGame[1].replace(' ','');
        GameNumber = (SetGame[SetGame.length - 2] + SetGame[SetGame.length - 1]).replace(' ', '');
        GamesArray.push({ SetNum: SetNumber, GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
    }
}