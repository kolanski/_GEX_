// JavaScript source code
var list = document.getElementsByClassName('leftPad title');
var scoreBoard = document.getElementById('scoreboard_frame');
var SetNumber = '';
var GameNumber ='';
var Player2 = '';
var Player1 = '';
var Coefficent2 = '';
var Coefficent1 = '';
var Score = '';
var CurrentGames = '';
var CurrentPoints ='';
var GamesArray = [];
var LenArr='';
for (var i = 0; i < list.length; i++)
{
    if(list[i].textContent.indexOf('Set - Game')!=-1&&list[i].textContent.indexOf('Point')==-1&&list[i].textContent.indexOf('Score')==-1&&list[i].textContent.indexOf('Total')==-1&&list[i].textContent.indexOf('Win ')==-1&&list[i].textContent.indexOf('Games')==-1)
    {
        SetNumber = list[i].childNodes[5].textContent;
        re = /^[0-9]/ig;
        GameNumber = SetNumber.slice(SetNumber.length - 2, SetNumber.length);
        SetNumber= SetNumber.match(re);
        Player1 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventselection')[0].textContent.trim();
        Player2 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventselection')[1].textContent.trim();
        Coefficent1 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventprice')[0].textContent.trim();
        Coefficent2 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventprice')[1].textContent.trim();
        GamesArray.push({ SetNum: SetNumber, GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
        Score = '';
        for (var s = 1; s <= 4; s++)
        {
            if(scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent != '')
            {
                if (scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent.length > 1) {
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent[0] + ' ';
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_B').textContent[0] + ' ';
                }
                else
                {
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent + ' ';
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_B').textContent + ' ';
                }
            }
        }
        CurrentGames = scoreBoard.contentDocument.getElementById('games_A').textContent.toString() + ' ' + scoreBoard.contentDocument.getElementById('games_B').textContent.toString();
        CurrentPoints = scoreBoard.contentDocument.getElementById('points_A').textContent.toString() + ' ' + scoreBoard.contentDocument.getElementById('points_B').textContent.toString();
        LenArr = GamesArray.length;
    }
}