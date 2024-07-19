var BookMaker = require('./BookmakerPattern.js');

var parimatch = new BookMaker("PariMatch");
function Setup()
{
    parimatch.ParentNavigate("http://www.parimatch.by/en/live.html");
    setTimeout(function () {
        parimatch.ParentBrowser[0].page.evaluate(function ()
            {
        var ten = document.getElementById("tennis");
            if (ten != undefined) {
                ten.previousSibling.click();
                showLives();
            }
            }
    ); },13000);
};

function Reload()
{
    parimatch.ParentBrowser[0].page.open("http://www.parimatch.by/en/live.html");
    setTimeout(function () {
        parimatch.ParentBrowser[0].page.evaluate(function () {
            var ten = document.getElementById("tennis");
            if (ten != undefined) {
                ten.previousSibling.click();
                showLives();
            };
        }
        );
    }, 13000);
};
function Parse()
{
    
    parimatch.ParentBrowser[0].page.evaluate(function () {
        structarr = [];
        var tables = document.getElementsByClassName("props");
        for (var i = 0; i < tables.length; i++) {
            if (tables[i].previousSibling.getElementsByClassName('l')[0] != undefined) {
                var Player1 = tables[i].previousSibling.getElementsByClassName('l')[0].childNodes[0].textContent;
                if (Player1 != undefined) {
                    var Player2 = "";
                    var players = tables[i].previousSibling.getElementsByClassName('l')[0].childNodes;
                    if (players[1].textContent.length > 3) Player2 = players[1].textContent;
                    if (players[2].textContent.length > 3) Player2 = players[2].textContent;
                    if (players[3].textContent.length > 3) Player2 = players[3].textContent;
                }
                var ScoreAll = "";
                var ScoreGame = "0 0";
                var playersdata = tables[i].previousSibling.getElementsByClassName('l')[1].textContent;
                if (playersdata.indexOf(':') != -1) {
                    ScoreAll = playersdata.slice(playersdata.indexOf('(') + 1).replace("-", " ").replace(/,/g, " ").replace(")", "");
                    var toremove1 = playersdata.slice(playersdata.indexOf("(") + 1).replace("-", " ").replace(/,/g, " ");
                    var toremove2 = toremove1.slice(toremove1.indexOf(")"), toremove1.indexOf(")"));
                    ScoreGame = toremove1.slice(toremove1.indexOf(")") + 2).replace(/:/g, " ");
                    ScoreAll = toremove1.slice(0, toremove1.indexOf(")"));
                    if (ScoreAll.indexOf(")") != -1) ScoreAll = ScoreAll.slice(0, ScoreAll.indexOf(")"));
                }
                structarr.push(
                    {
                        'Event': "",
                        'Player1': Player1.trim(),
                        'Player2': Player2.trim(),
                        'ScoreAll': ScoreAll,
                        'GamePoints': ScoreGame,
                        'GamesArr': []
                    });
                var GamesList = tables[i].getElementsByClassName('dyn').length;
                for (var j = 0; j < GamesList; j++) {
                    var markets = tables[i].getElementsByClassName('dyn')[j].textContent;
                    if (markets.indexOf("Set ") != -1 && markets.indexOf("game ") != -1 && markets.indexOf("point") == -1 && markets.indexOf("score") == -1 && markets.indexOf("Who will ") == -1) {
                        var SetGameText = tables[i].getElementsByClassName('dyn')[j].childNodes[1].textContent.match(/\d{2}|\d/ig);
                        var Set = SetGameText[0];
                        var Game = SetGameText[1];
                        if (tables[i].getElementsByClassName('dyn')[j].childNodes[3].childNodes[1] != undefined) {
                            var coef1 = tables[i].getElementsByClassName('dyn')[j].childNodes[3].childNodes[1].textContent;
                            var coef2 = tables[i].getElementsByClassName('dyn')[j].childNodes[5].childNodes[1].textContent;
                            if (coef2 == Player2 || coef2.length > 5) coef2 = tables[i].getElementsByClassName('dyn')[j].childNodes[5].childNodes[3].textContent;
                            if (coef1 == Player1) {
                                coef1 = tables[i].getElementsByClassName('dyn')[j].childNodes[3].childNodes[3].textContent;
                            }
                            if (coef1 != "undefined" && coef2 != "undefined") {
                                structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': Set, 'GameNumber': Game, 'Coefficent1': coef1, 'Coefficent2': coef2 });

                            }
                        }
                    }
                }
            }
        }
        if (refreshOdds != null && refreshOdds != undefined)
            refreshOdds(this);
        else
            location.reload();

        return structarr;
    }, function (result) { process.send({ parimatchdata: result });});
}
Setup();
process.on('message', function (data) {
    console.log('PariMatch command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
  //  console.log(cnt);
});