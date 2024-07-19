//Williams MOdded
//OpenOrls
var list = document.getElementsByClassName('rowLive');
var len = list.length;
var arr = [];
for (var i = 0; i < len; i++) {
    arr.push(list[i].getElementsByTagName('td')[4].childNodes[1].getAttribute('Href'));
}
myarr = [];
var html = [];
for (var i = 0; i < arr.length; i++) {

    html.push('<li> ');
    html.push("<iframe width=\"650\" height=\"200\" src=\"" + arr[i] + "\"></iframe>");

    html.push('</li>');
}
var div = document.createElement('div');
div.className = "gameData ";
document.getElementById("sports_holder").appendChild(div);
document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');
//Parse Modded
function Setup()
{
    var oddtype = document.getElementById("oddsSelect").value;
    if (oddtype != undefined && oddtype != 'DECIMAL') {
        document.getElementById("oddsSelect").children[1].selected = true;
        document.getElementById("oddsSelect").onchange();
    };
}

function OpenAllFrames()
{
    var list = document.getElementsByClassName('rowLive');
    var len = list.length;
    var arr = [];
    for (var i = 0; i < len; i++) {
        arr.push(list[i].getElementsByTagName('td')[4].childNodes[1].getAttribute('Href'));
    }
    myarr = [];
    var html = [];
    for (var i = 0; i < arr.length; i++) {

        html.push('<li> ');
        html.push("<iframe width=\"650\" height=\"200\" src=\"" + arr[i] + "\"></iframe>");
        html.push('</li>');
    }
    var div = document.createElement('div');
    div.className = "gameData ";
    document.getElementById("sports_holder").appendChild(div);
    document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');
}
structarr = [];

function Parse()
{
    structarr = [];
    var listl;
    if(document.getElementsByClassName("gameData")[0]!=undefined)
        listl = document.getElementsByClassName("gameData")[0];
    if(listl!=undefined)
    {
        listl = listl.children;
    }
    if(listl!=undefined)
    for(var i=0;i<listl.length;i++)
    {
        var list = listl[i].children[0].contentDocument.getElementsByClassName('leftPad title');
        var scoreBoard = listl[i].children[0].contentDocument.getElementById('scoreboard_frame');
        var SetNumber = '';
        var GameNumber = '';
        var Player2 = '';
        var Player1 = '';
        var Coefficent2 = '';
        var Coefficent1 = '';
        var Score = '';
        var CurrentGames = '';
        var CurrentPoints = '';
        var GamesArray = [];
        var LenArr = '';
        var data = [];
        structarr.push(
        {
            'Event': "",
            'Player1': Player1,
            'Player2': Player2,
            'ScoreAll': Score,
            'GamePoints': CurrentPoints,
            'GamesArr': []
        });
        if (list != undefined) {
            for (var l = 0; l < list.length; l++) {

                if (list[l].textContent.indexOf('Set - Game') != -1 && list[l].textContent.indexOf('Point') == -1 && list[l].textContent.indexOf('Score') == -1 && list[l].textContent.indexOf('Total') == -1 && list[l].textContent.indexOf('Win ') == -1 && list[l].textContent.indexOf('Games') == -1) {
                    SetNumber = list[l].childNodes[5].textContent;
                    re = /^[0-9]/ig;
                    GameNumber = SetNumber.slice(SetNumber.length - 2, SetNumber.length);
                    SetNumber = SetNumber.match(re);
                    Player1 = list[l].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventselection')[0].textContent.trim();
                    structarr[structarr.length - 1].Player1 = Player1;
                    Player2 = list[l].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventselection')[1].textContent.trim();
                    structarr[structarr.length - 1].Player2 = Player2;
                    Coefficent1 = list[l].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventprice')[0].textContent.trim();
                    Coefficent2 = list[l].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventprice')[1].textContent.trim();


                    structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': SetNumber, 'GameNumber': GameNumber, 'Coefficent1': Coefficent1, 'Coefficent2': Coefficent2 });
                    Score = '';
                    if (scoreBoard.contentDocument != undefined && scoreBoard.contentDocument.getElementById('set_' + 1 + '_A') != undefined) {
                        for (var s = 1; s <= 4; s++) {
                            if (scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent != '') {
                                if (scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent.length > 1) {
                                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent[0] + ' ';
                                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_B').textContent[0] + ' ';
                                }
                                else {
                                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent + ' ';
                                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_B').textContent + ' ';
                                }
                            }
                        }
                        CurrentGames = scoreBoard.contentDocument.getElementById('games_A').textContent.toString() + ' ' + scoreBoard.contentDocument.getElementById('games_B').textContent.toString();
                        CurrentPoints = scoreBoard.contentDocument.getElementById('points_A').textContent.toString() + ' ' + scoreBoard.contentDocument.getElementById('points_B').textContent.toString();
                    }
                    structarr[structarr.length - 1].ScoreAll = Score + " " + CurrentGames;
                    structarr[structarr.length - 1].GamePoints = CurrentPoints;
                }
            };
        }
    }
    
    if (document.getElementsByClassName("gameData")[0] == undefined)
        OpenAllFrames();
    Setup();
}