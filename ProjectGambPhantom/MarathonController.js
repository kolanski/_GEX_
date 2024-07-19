var phantom = require('phantom');
var BookMaker = require('./BookmakerPattern.js');

var Marathon = new BookMaker("Marathon");
function Setup() {
    Marathon.ParentNavigate("https://www.betmarathon.com/en/live/22723");
    setTimeout(function () {
    Marathon.ParentBrowser[0].page.evaluate(function () {
        var all = document.getElementsByClassName("first member-area  ");
        var stringtext = "";
        for (var i = 0; i < all.length; i++) {
            var stringtext = stringtext + all[i].parentNode.parentNode.id.replace("event_", "") + ",";
        }
        document.location.href = "https://www.betmarathon.com/en/live/22723?openedMarkets=" + stringtext.slice(0, stringtext.length - 1) + "&oddsType=Decimal"; return "Decimal";
    }, function (result) { console.log(result); })
    }, 5000);
};
Setup();
function Parse() {

    Marathon.ParentBrowser[0].page.evaluate(function () {
        Markets.ONLY_ONE_EVENT_OPEN_AM = false;
        structarr = [];
        function loadallgames() {
            var all = document.getElementsByClassName("first member-area  ");
            var stringtext = "";
            for (var i = 0; i < all.length; i++) {
                var stringtext = stringtext + all[i].parentNode.parentNode.id.replace("event_", "") + ",";
            }
            document.location.href = "https://www.betmarathon.com/en/live/22723?openedMarkets=" + stringtext.slice(0, stringtext.length - 1) + "&oddsType=Decimal";
        }
        function checkupdate() {
            var test = document.location.href.slice(56, document.location.href.length - 17)
            var all = document.getElementsByClassName("first member-area  ");
            var stringtext = "";
            for (var i = 0; i < all.length; i++) {
                var stringtext = stringtext + all[i].parentNode.parentNode.id.replace("event_", "") + ",";
            }
            //console.log(test);
            //console.log(stringtext.slice(0, stringtext.length - 1));
            if (test != stringtext.slice(0, stringtext.length - 1)) {
                console.log("reloading");
                loadallgames();
            }
        }
        var events = document.getElementsByClassName("foot-market");
        for (var i = 0; i < events.length; i++) {
            var eventname = events[i].parentNode.parentNode.parentNode.children[1].textContent.replace(/(?:\r\n|\r|\n)/g, "");
            var tables = events[i].getElementsByTagName("tbody");
            var games = [];
            for (var f = 0; f < tables.length; f++) { if (tables[f].id != "") games.push(tables[f]) };
            for (var j = 0; j < games.length; j++) {
                var PlayersData = games[j].getElementsByClassName('live-today-member-name nowrap');
                if (PlayersData[0] == undefined) PlayersData = games[j].getElementsByClassName('live-member-name nowrap');
                var GamesData = games[j].getElementsByClassName('market-table-name');
                var Score = games[j].getElementsByClassName('cl-left red')[0];
                if (Score == undefined)
                    Score = games[j].getElementsByClassName('cl-left grey')[0];
                Score = Score.textContent;
                var splitted = Score.split(' ');
                var CurrentPoints = splitted[splitted.length - 1].replace(':', ' ').replace('(', '').replace(')', '');
                var Player1 = PlayersData[0].textContent;
                var Player2 = PlayersData[1].textContent;

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

                structarr.push(
                    {
                        'Event': "",
                        'Player1': PlayersData[0].textContent,
                        'Player2': PlayersData[1].textContent,
                        'ScoreAll': Score,
                        'GamePoints': CurrentPoints,
                        'GamesArr': []
                    });
                for (var k = 0; k < GamesData.length; k++) {
                    if (GamesData[k].textContent.indexOf('To Win Game') != -1) {
                        var TableWithGames = GamesData[k].nextSibling.nextSibling;
                        var CountOfTr = 0;
                        if (TableWithGames.getElementsByTagName('tr') != null)
                            CountOfTr = TableWithGames.getElementsByTagName('tr').length;
                        for (var h = 1; h <= CountOfTr; h++) {

                            if (TableWithGames.getElementsByTagName('tr')[h] != null) {
                                //var index = GamesData[k].textContent
                                numset = GamesData[k].textContent.match(/\d/gi)[0];
                                numgame = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[0].getElementsByTagName('span')[0].textContent.replace

                                    ('Game', '').replace(' ', '').replace(' ', '');
                                coef1 = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[1].getElementsByTagName('div')[0].textContent.replace('\n',

                                    '').replace(' ', '').replace(' ', '').trim();
                                coef2 = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[2].getElementsByTagName('div')[0].textContent.replace('\n',

                                    '').replace(' ', '').replace(' ', '').trim();
                                structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': numset, 'GameNumber': numgame, 'Coefficent1': coef1, 'Coefficent2': coef2 });

                            }
                        }
                    }
                }
            };
        }
        checkupdate();
        return structarr;
    }, function (result) { process.send({ marathondata: result }) });
};
function Reload() {

};
process.on('message', function (data) {
    console.log('Marathon command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
    //  console.log(cnt);
});
setTimeout(function () { },10000)