
//WilliamsMultigamesInone
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

//modded parser



























//Bet365multigamesinone
var len = document.getElementsByClassName('Fixture').length;
var div = document.createElement('div');
div.className = "gameData ";
document.getElementsByClassName("FixtureList PC_2 ")[0].appendChild(div);
var arr = [];
for (var i = 0; i < len - 1; i++) {
    arr.push(document.getElementsByClassName('Fixture')[i].wrapper.stem.data.ID);
}

var html = [];
for (var i = 0; i < arr.length; i++) {
    
    html.push('<li> ');
    html.push("<iframe width=\"600\" height=\"200\" src=\"" + "https://mobile.365sb.com/default.aspx?apptype=&ot=2&appversion=&cb=1429878108#type=Coupon;key=" + arr[i] + "\"></iframe>");
    
    html.push('</li>');
}
document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');










//modyfing bet365
if (document.getElementsByClassName("gameData")[0] != undefined || null) {
    var lis = document.getElementsByClassName("gameData")[0];
    for (var i = 0; i < lis.children.length; i++) {
        var frame1 = lis.children[i].children[0];
        // if (frame1.contentDocument.getElementsByClassName("MatchLive ")[0] != undefined)
        //document.getElementsByClassName("MatchLive ")[0].click();
        var Score = '';
        var CurrentPoints = '';
        var Player1 = '';
        var Player2 = '';
        var ScoreBoard = frame1.contentDocument.getElementById('previousSets');
        var data = [];
        var GamesArray = [];
        data.push({
            'Event': "",
            'Player1': Player1,
            'Player2': Player2,
            'ScoreAll': Score,
            'GamePoints': CurrentPoints,
            'GamesArr': []
        });
        if (ScoreBoard != null) {
            for (var h = 0; h < ScoreBoard.childNodes[0].childNodes.length; h++) {
                Score += ScoreBoard.childNodes[0].childNodes[h].textContent.replace('-', ' ') + ' ';
            }
            CurrentPoints = frame1.contentDocument.getElementById('team1ArenaPoint').textContent + ' ' + frame1.contentDocument.getElementById('team2ArenaPoint').textContent;
        }
        var tmpPl1 = frame1.contentDocument.getElementById('team1Name');
        var tmpPl2 = frame1.contentDocument.getElementById('team2Name');
        if (tmpPl1 != null) {
            Player1 = frame1.contentDocument.getElementById('team1Name').textContent;
            Player2 = frame1.contentDocument.getElementById('team2Name').textContent;
        }
        data[0].Player1 = Player1;
        data[0].Player2 = Player2;
        data[0].ScoreAll = Score;
        data[0].GamePoints = CurrentPoints;
        
        var MatchBets = frame1.contentDocument.getElementsByClassName('Market');
        
        for (var h = 0; h < MatchBets.length; h++) {
            if (MatchBets[h].textContent.indexOf('Game Winner') != -1) {
                var GameNumber = MatchBets[h].childNodes[0].childNodes[0].textContent.replace('Game Winner', '').replace('th', '').replace('nd', '').replace('st', '').replace('rd', '');
                var PlayerEqual = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].textContent;
                var Coefficent1 = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].childNodes[1].textContent;
                var Coefficent2 = MatchBets[h].childNodes[1].childNodes[0].childNodes[1].childNodes[1].textContent;
                PlayerEqual = PlayerEqual.replace('(Svr)', '');
                if (Player1.indexOf(PlayerEqual) != -1) {
                    //{ SetNum: '0', GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
                    data[0].GamesArr.push({ 'SetNumber': '0', 'GameNumber': GameNumber, 'Coefficent1': Coefficent1, 'Coefficent2': Coefficent2 });;
                }
                else {
                    data[0].GamesArr.push({ 'SetNumber': '0', 'GameNumber': GameNumber, 'Coefficent1': Coefficent2, 'Coefficent2': Coefficent1 });
                }

            }
        }
        console.log(data);
    }
}