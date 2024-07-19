var BookMaker = require('./BookmakerPattern.js');

var Betcity = new BookMaker("Betcity");
function Setup() {
    Betcity.ParentNavigate("http://www2.betsbc.com/en/");
    setTimeout(function () {
        Betcity.ParentBrowser[0].page.open("https://www2.betsbc.com/live_n/#/live/bets/", function (status) {
            console.log('Opened site status: %s', status);
            if (status == 'success') {
                console.log(myname1 + ' Parent Navigated');
                Betcity.ParentBrowser[0].page.evaluate(function () {
                    console.log(document.length);
                    return document.title;
                }, function (result) {
                    console.log(result);
                });
                var timer = setInterval(function () {
                    /// call your function here
                    Betcity.ParentBrowser[0].page.evaluate(function () {
                        
                        $('a.uncheck_all').click();
                        //$(".item-pointer:contains('Tennis')").click(); 
                        var list = document.getElementsByClassName("item ng-scope");
                        
                        for (var i = 0; i < list.length; i++) {
                            if (list[i].textContent.indexOf("Tennis") != -1 && list[i].textContent.indexOf("Table") == -1) {
                                console.log("Tennis");
                                list[i].children[0].click();
                            }
                        };
                        setTimeout($('a.btn.f2').click(), 5000);
                        return list.length;
                    }, function (result) {
                        console.log("bcresult:" + result);
                        if (result != "0") {
                            clearInterval(timer); setTimeout(function () {
//Betcity.ParentBrowser[0].page.render('github.png');
                            }, 2000);
                        }
                    });
                }, 10000);
            }
        });
    }, 10000);

};

function Reload()
{
    Betcity.ParentBrowser[0].page.open("https://www2.betsbc.com/live_n/#/live/bets/", function (status) {
        console.log('Opened site status: %s', status);
        if (status == 'success') {
            console.log(myname1 + ' Parent Navigated');
            Betcity.ParentBrowser[0].page.evaluate(function () {
                console.log(document.length);
                return document.title;
            }, function (result) {
                console.log(result);
            });

            var timer = setInterval(function () {
                /// call your function here
                Betcity.ParentBrowser[0].page.evaluate(function () {
                    
                    $('a.uncheck_all').click();
                    //$(".item-pointer:contains('Tennis')").click(); 
                    var list = document.getElementsByClassName("item ng-scope");
                    
                    for (var i = 0; i < list.length; i++) {
                        if (list[i].textContent.indexOf("Tennis") != -1 && list[i].textContent.indexOf("Table") == -1) {
                            console.log("Tennis");
                            list[i].children[0].click();
                        }
                    };
                    setTimeout($('a.btn.f2').click(), 5000);
                    return list.length;
                }, function (result) {
                    console.log("bcresult:" + result);
                    if (result != "0") {
                        clearInterval(timer); setTimeout(function () {
//Betcity.ParentBrowser[0].page.render('github.png');
                        }, 2000);
                    }
                });
            }, 10000);
        }
    });
};

function Parse()
{
    Betcity.ParentBrowser[0].page.evaluate(function () {
        var Players = document.getElementsByClassName("lbk");
        var structarr = [];
        for (var i = 0; i < Players.length; i++) {
            var player1 = Players[i].children[1].textContent;
            var player2 = Players[i].children[2].textContent;
            var Scorearr = Players[i].nextSibling.nextSibling.getElementsByClassName('red')[0].textContent.replace('Score:', '').split(/[()]/);
            if (Scorearr != null && Scorearr.length == 3) {
                var Score = Scorearr[Scorearr.length - 2].replace(/\:/g, ' ').replace(/\,/g, ' ');
                var Points = Scorearr[Scorearr.length - 1];
            }
            structarr.push({
                'Event': ''
                ,'Player1': player1
                ,'Player2': player2
                ,'ScoreAll': Score
                ,'GamePoints': Points
                ,'GamesArr': []
            });
            var Tables = Players[i].nextSibling.children[0].children;
            if (Tables != undefined) {
                for (var j = 0; j < Tables.length; j++) {
                    if (Tables[j].textContent.indexOf('To win a game') != -1) {
                        var numgame = Tables[j].childNodes[1].textContent.replace('W1', '').replace(/[^0-9]/g, '');
                        var coef1 = Tables[j].childNodes[2].textContent;
                        var coef2 = Tables[j].childNodes[4].textContent;
                        structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': '0', 'GameNumber': numgame, 'Coefficent1': coef1, 'Coefficent2': coef2 });
                    }
                }
            }
        }
        
        return structarr;
    }, 
    function (result) { process.send({ betcitydata: result }); });
};
//Main
Setup();
//
process.on('message', function (data) {
    console.log('Betcity command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
  //  console.log(cnt);
});