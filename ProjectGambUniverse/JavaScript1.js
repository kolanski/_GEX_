var BookMaker = require('./BookmakerPattern.js');
var Bet365 = new BookMaker('Bet365');

function Setup()
{
    Bet365.ParentNavigate("https://mobile.365sb.com/default.aspx?apptype=&appversion=&cb=1430338157#type=InPlay;key=13;ip=1;lng=1");
};
Setup();
function Reload()
{
    Bet365.ParentBrowser[0].page.evaluate(function () {
        Element.prototype.remove = function () {
            this.parentElement.removeChild(this);
        }
        NodeList.prototype.remove = HTMLCollection.prototype.remove = function () {
            for (var i = 0, len = this.length; i < len; i++) {
                if (this[i] && this[i].parentElement) {
                    this[i].parentElement.removeChild(this[i]);
                }
            }
        }
        document.getElementsByClassName("gameData").remove();
    });
}
oldresult = [];
function Parse()
{

    Bet365.ParentBrowser[0].page.evaluate(function () {
        function OPenAllFrames()
        {
            var len = document.getElementsByClassName('ipo-Fixture').length;
            var div = document.createElement('div');
            div.className = "gameData ";
            document.getElementsByClassName("ipo-FixtureList_SubHeading")[0].appendChild(div);
            var arr = [];
            for (var i = 0; i < len - 1; i++) {
                arr.push(document.getElementsByClassName('ipo-Fixture')[i].wrapper.stem.data.ID);
            }

            var html = [];
            for (var i = 0; i < arr.length; i++) {

                html.push('<li> ');
                html.push("<iframe width=\"600\" height=\"200\" src=\"" + "https://mobile.365sb.com/default.aspx?apptype=&ot=2&appversion=&cb=1429878108#type=Coupon;key=" + arr[i] + "\"></iframe>");

                html.push('</li>');
            }
            document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');
        }
        structarr = [];
        function Parse() {
            structarr = [];
            var listl;
            if (document.getElementsByClassName("gameData")[0] != undefined)
                listl = document.getElementsByClassName("gameData")[0];
            if (listl != undefined) {
                listl = listl.children;
            }
            if (listl != undefined)
                for (var i = 0; i < listl.length; i++)
                {
                    var Score = '';
                    var CurrentPoints = '';
                    var Player1 = '';
                    var Player2 = '';
                    var ScoreBoard = listl[i].children[0].contentDocument.getElementsByClassName("IPTableRow");
                    var data = [];
                    var GamesArray = [];
                    if (ScoreBoard != null && ScoreBoard.length>0) {
                        //Count gamesscore
                        var Score1row = ScoreBoard[0].getElementsByClassName("IPScoreCell");
                        var Score2row = ScoreBoard[1].getElementsByClassName("IPScoreCell");
                        for (var h = 0; h < Score1row.length; h++)
                        {
                            if (h > 0)
                                Score += " ";
                            if (Score1row[h].className.indexOf("additional-points") == -1)
                            {
                                Score += Score1row[h].textContent + " " + Score2row[h].textContent;
                            }
                            else
                            {
                                CurrentPoints += Score1row[h].textContent + " " + Score2row[h].textContent;
                            }
                    
                        }
                        Player1 = listl[i].children[0].contentDocument.getElementsByClassName('Team')[0].textContent.replace("*","");
                        Player2 = listl[i].children[0].contentDocument.getElementsByClassName('Team')[1].textContent.replace("*", "");
                    }
                    var event = listl[i].children[0].contentDocument.getElementsByClassName("IPTableCompetitionName").textContent;
                    var list = listl[i].children[0].contentDocument.getElementsByClassName("ipe-Market_Button");
                    for (var n = 0; n < list.length; n++) {
                        var test = list[n].textContent;
                        if (test.indexOf("Game Winner") != -1) {
                            if (list[n].className == "ipe-Market_Button")
                            { list[n].click() }
                        }
                    }
                    structarr.push({
                        'Event': event,
                        'Player1': Player1,
                        'Player2': Player2,
                        'ScoreAll': Score,
                        'GamePoints': CurrentPoints,
                        'GamesArr': []
                    });
                    var MatchBets = listl[i].children[0].contentDocument.getElementsByClassName('ipe-Market ');
                    for (var h = 0; h < MatchBets.length; h++) {
                        if (MatchBets[h].textContent.indexOf('Game Winner') != -1) {
                            var GameNumber = MatchBets[h].childNodes[0].childNodes[0].textContent.replace('Game Winner', '').replace('th', '').replace('nd','').replace('st', '').replace('rd', '');
                            if (MatchBets[h].childNodes[1] != undefined) {
                                var PlayerEqual = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].textContent;
                                var Coefficent1 = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].childNodes[1].textContent;
                                var Coefficent2 = MatchBets[h].childNodes[1].childNodes[0].childNodes[1].childNodes[1].textContent;
                                PlayerEqual = PlayerEqual.replace('(Svr)', '');
                                if (PlayerEqual.indexOf(Player1) != -1) {
                                    //{ SetNum: '0', GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
                                    structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': '0', 'GameNumber': GameNumber, 'Coefficent1': Coefficent1, 
                                        'Coefficent2': Coefficent2 });;
                                }
                                else {
                                    structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': '0', 'GameNumber': GameNumber, 'Coefficent1': Coefficent2, 
                                        'Coefficent2': Coefficent1 });
                                }
                            }
                        }
                    }
                }
            if (document.getElementsByClassName("gameData")[0] == undefined)
                OPenAllFrames();
        }
        Parse();
        return structarr;

    }, function (result) {
        if (result != null&&result.length>2 ) {
            oldresult = result;
        }
        else {
            if (oldresult != null && oldresult.length > 2) {
                result = oldresult;
            }
        }
        process.send({ Bet365data: result });
    });
}
process.on('message', function (data) {
    console.log('Bet365 command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
    //  console.log(cnt);
});