let startpage = "https://www.bet365.com/";
let tennisbuttonclass = "ipc-InPlayClassificationIcon ipc-InPlayClassificationIcon-13";
let overviewbuttonclass = "ip-ControlBar_BBarItem";
let eventplayersclass = "ipo-Fixture_ScoreDisplay ipo-ScoreDisplayPoints ";

//checking current setview correct
function settennis() {
    var ten = document.getElementsByClassName(tennisbuttonclass);
    if (ten[0].attributes[0].value.indexOf("Selected") == -1) {
        ten[0].click();
    }
}
setTimeout(function () { settennis() }, 3000);
//selecting players and score
var structarr = [];
function parse() {
    tmpeventsarr = [];
    var events = document.getElementsByClassName(eventplayersclass);
    for (var i = 0; i < events.length; i++) {
        //exctracting data
        var plrsrc = events[i].wrapper;
        var tournament = plrsrc.stem.data.CT;
        var player1 = plrsrc.stem.data.NA.split(" v ")[0];
        var player2 = plrsrc.stem.data.NA.split(" v ")[1];
        var score = plrsrc.stem.data.SS;
        var scorepoints = plrsrc.stem.data.XP;
        var gamedata = plrsrc.teamStack.drawText;
        tmpeventsarr.push({
            'Event': tournament,
            'Player1': player1,
            'Player2': player2,
            'ScoreAll': score.replace(/,/g, " "),
            'GamePoints': scorepoints.replace("-", ":"),
            'GamesArr': []
        });
        //searching game
        var marketsarr = plrsrc.stem._delegateList;
        var arr = [];
        for (var k = 0; k < marketsarr.length; k++) {
            if (marketsarr[k] !== undefined && marketsarr[k].mainMarkets != undefined) {
                if (marketsarr[k].mainMarkets.marketRenderers[2].stem != undefined) {
                    var markets = marketsarr[k].mainMarkets.marketRenderers[2].stem._actualChildren;
                    if (scorepoints == "0-0") {
                        tmpeventsarr[tmpeventsarr.length - 1].GamesArr.push({
                            SetNumber: "0",
                            GameNumber: gamedata.substr(gamedata.indexOf("Game") + 4).trim(),
                            Coefficent1: (eval(markets[0].data.OD) + 1).toFixed(2),
                            Coefficent2: (eval(markets[1].data.OD) + 1).toFixed(2)
                        });
                    }
                    else {
                        tmpeventsarr[tmpeventsarr.length - 1].GamesArr.push({
                            SetNumber: "0",
                            GameNumber: parseInt(gamedata.substr(gamedata.indexOf("Game") + 4).trim()) + 1,
                            Coefficent1: (eval(markets[0].data.OD) + 1).toFixed(2),
                            Coefficent2: (eval(markets[1].data.OD) + 1).toFixed(2)
                        });
                    }

                }
            }
        }
    }
    structarr = tmpeventsarr;
}