//Olimpbet js
structarr = [];
var table = document.getElementById("betline").getElementsByClassName("smallwnd2")[0];
var games = table.getElementsByClassName("hi");
//cycle
for (var s = 0; s < games.length; s++) {
    var bets = games[s].nextSibling.nextSibling;
    var players = "";
    var len = games[s].children[1].children[0].children[0].childNodes.length;
    for (var g = 0; g < len; g++) {
        if (games[s].children[1].children[0].children[0].childNodes[g].nodeName == "#text") {

            players = games[s].children[1].children[0].children[0].childNodes[g];
            //console.log(players.textContent.replace(/\d/g, '').slice(1));
        }
    };
    players = players.textContent.replace(/\d/g, '').slice(1).split(" - ");
    var score = games[s].getElementsByClassName("txtmed")[0].textContent.split(/[()]/);
    //console.log(score);
    structarr.push(
        {
            'Event': "",
            'Player1': players[0],
            'Player2': players[1],
            'ScoreAll': score.slice(0, 2).toString(),
            'GamePoints': score[2],
            'GamesArr': []
        });
    if (bets.getElementsByTagName("div")[1]!=undefined)
    var betgame = bets.getElementsByTagName("div")[1].children;
    for (var i = 0; i < betgame.length; i++) {
        if ((betgame[i].textContent.indexOf("set") != -1 && betgame[i].textContent.indexOf("game") != -1))
        {
            //console.log(betgame[i].textContent + " " + betgame[i].textContent.length);
            if (betgame[i].textContent.length == 17 || betgame[i].textContent.length == 18)
            {
                //console.log(betgame[i].textContent);
                var coef1 = betgame[i + 2].textContent.match(/\d+.\d+/g)[0];
                var coef2 = betgame[i + 3].textContent.match(/\d+.\d+/g)[0];
                var setnum = betgame[i].textContent.match(/\d{2}|\d/ig);
                //console.log(coef1);
                //console.log(coef2);
                
                structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': setnum[0], 'GameNumber': setnum[1], 'Coefficent1': coef1, 'Coefficent2': coef2 });
            }
        }
    };
};