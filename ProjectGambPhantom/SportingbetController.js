var BookMaker = require('./BookmakerPattern.js');

var SportingBet = new BookMaker("SportingBet");
function Setup() {
    SportingBet.ParentNavigate("http://fr.sportingbet.com/sports-bet-in-play/5.html");

};
Setup();
function Reload() {
    SportingBet.ParentBrowser[0].page.open("http://fr.sportingbet.com/sports-bet-in-play/5.html");
}
function Parse() {
    SportingBet.ParentBrowser[0].page.evaluate(function () {
        function OpenAllFrames() {
            var list = document.getElementsByClassName("headerSub-ul-item ev-parent tennis");
            
            var len = list.length;
            var arr = [];
            for (var i = 0; i < len; i++) {
                arr.push("http://fr.sportingbet.com/sports-bet-in-play/5-" + list[i].id.slice(2) + ".html");
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
            document.getElementById("inplayApp").appendChild(div);
            document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');
        }
        structarr = [];
        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }        ;
        function Parse() {

            structarr = [];
            var listl;
            if (document.getElementsByClassName("gameData")[0] != undefined)
                listl = document.getElementsByClassName("gameData")[0];
            if (listl != undefined) {
                listl = listl.children;
            }
            if (listl != undefined)
                for (var i = 0; i < listl.length; i++) {
                    var document1 = listl[i].children[0].contentDocument;
                    if (document1.getElementById("event-header-details") != undefined && document1.getElementsByClassName("teams")[0] != undefined&& document1.getElementById("games1")!=undefined) {
                        var event = document1.getElementById("event-header-details").textContent;
                        var Player1 = document1.getElementsByClassName("teams")[0].children[0].children[0].children[1].textContent.trim();
                        var Player2 = document1.getElementsByClassName("teams")[0].children[0].children[0].children[2].textContent.trim();
                        var Set1 = document1.getElementById("sets-player1");
                        var Set2 = document1.getElementById("sets-player2")
                        var Score = "";
                        var ScoreSet = document1.getElementById("games1").textContent + " " + document1.getElementById("games2").textContent;;
                        var ScorePoints = document1.getElementById("points1").textContent + " " + document1.getElementById("points2").textContent;;
                        for (var o = 0; o < Set1.children.length; o++) {
                            if (isNumber(Set1.children[o].textContent)) {
                                Score = Score + Set1.children[o].textContent + " " + Set2.children[o].textContent + " ";
                            }
                        }
                        if (Score.length > 2) {
                            Score = Score.trim() + " " + ScoreSet;
                        }
                        else {
                            Score = ScoreSet;
                        }
                        structarr.push({
                            'Event': event,
                            'Player1': Player1,
                            'Player2': Player2,
                            'ScoreAll': Score,
                            'GamePoints': ScorePoints,
                            'GamesArr': []
                        });
                        var list = document1.getElementsByClassName("headerSub groupHeader");
                        for (var f = 0; f < list.length; f++) {
                            if (list[f].children[1].textContent.indexOf("Jeu") != -1 && list[f].children[1].textContent.indexOf("Score") == -1 && (list[f].children[1].textContent.length == 12 || list[f].children[1].textContent.length == 13)) {
                                var SetGameText = list[f].children[1].textContent.match(/\d{2}|\d/ig);
                                var Set = SetGameText[1];
                                var Game = SetGameText[0];
                                Set;
                                Game;
                                var Coefficent1 = list[f].nextSibling.nextSibling.getElementsByClassName("results")[0].getElementsByClassName("priceText wide  EU")[0].textContent;
                                var Coefficent2 = list[f].nextSibling.nextSibling.getElementsByClassName("results")[1].getElementsByClassName("priceText wide  EU")[0].textContent;
                                structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': Set, 'GameNumber': Game, 'Coefficent1': Coefficent1, 'Coefficent2': Coefficent2 });
                            }
                        }
                    }
                }
            else
                OpenAllFrames();
           
        }
        Parse();
        return structarr;
    }, function (result) { process.send({ sportingbetdata: result }); })
}
process.on('message', function (data) {
    console.log('Sportingbet command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
  //  console.log(cnt);
});