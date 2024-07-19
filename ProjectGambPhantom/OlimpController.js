var BookMaker = require('./BookmakerPattern.js');
var Olimp = new BookMaker("Olimp");
function Setup() {
    Olimp.ParentNavigate("http://www.olimpru.com/index.php?action=setlang&id=2");
    setTimeout(function () {
        Olimp.ParentBrowser[0].page.set('settings.userAgent', {
            userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2409.0 Safari/537.36"
        });
        Olimp.ParentBrowser[0].page.open("http://www.olimpru.com/index.php?page=line&action=1&live=1", function (status) {
            console.log('Opened site status: %s', status);
            if (status == 'success') {
                console.log(myname1 + ' Parent Navigated');
                Olimp.ParentBrowser[0].page.evaluate(function () {
                    console.log(document.length);
                    return document.title;
                }, function (result) {
                    console.log(result);
                });
                Olimp.ParentBrowser[0].page.evaluate(function () {
                    var cnt = 0;
                    var alldata = document.getElementsByClassName("smallwnd3")[0].getElementsByTagName("td");
                    for (var i = 0; i < alldata.length; i++) {
                        if (alldata[i].textContent.indexOf("Tennis") != -1) {
                            alldata[i].children[1].click();
                            cnt++;
                        }
                    }                    ;
                    setTimeout(function () { document.getElementsByClassName("msbtn1")[2].click() },4000);
                   // setTimeout(function () { document.getElementById("refrate").options[0].selected = true; change_rr(); }, 2000);
                    return cnt;
                }, function (result) {
                    console.log(result);
                });
            }
            else {
                Olimp.ParentBrowser[0].page.evaluate(function () {
                    if (window.navigator.userAgent != undefined)
                        return window.navigator.userAgent;
                    var cnt = 0;
                    var alldata = document.getElementsByClassName("smallwnd3")[0].getElementsByTagName("td");
                    for (var i = 0; i < alldata.length; i++) {
                        if (alldata[i].textContent.indexOf("Tennis") != -1) {
                            alldata[i].children[1].click();
                            cnt++;
                        }
                    }                    ;
                    setTimeout(function () { document.getElementsByClassName("msbtn1")[2].click() }, 4000);
                    // setTimeout(function () { document.getElementById("refrate").options[0].selected = true; change_rr(); }, 2000);
                    return cnt;
                }, function (result) {
                    console.log(result);
                });
            }
        });
    /*Olimp.ParentBrowser[0].page.evaluate(
        function () 
        {
        console.log(document.length);
        return document.title;
        },
        function (result) 
        {
            console.log(result);
        }
    )*/
    },
     10000);
};
function Reload() {
    Olimp.ParentBrowser[0].page.open("http://www.olimpru.com/index.php?page=line&action=1&live=1", 
        function (status) {
        console.log('Opened site status: %s', status);
        if (status == 'success') {
            console.log(myname1 + ' Parent Navigated');
            Olimp.ParentBrowser[0].page.evaluate(function () {
                console.log(document.length);
                return document.title;
            }, function (result) {
                console.log(result);
            });
            Olimp.ParentBrowser[0].page.evaluate(function () {
                var cnt = 0;
                var alldata = document.getElementsByClassName("smallwnd3")[0].getElementsByTagName("td");
                for (var i = 0; i < alldata.length; i++) {
                    if (alldata[i].textContent.indexOf("Tennis") != -1) {
                        alldata[i].children[1].click();
                        cnt++;
                    }
                }                ;
                setTimeout(function () { document.getElementsByClassName("msbtn1")[2].click() }, 100);
                return cnt;
            }, function (result) {
                if (result == 1)
                    console.log("Olimp" + " Succes Reloaded");
                else
                    console.log("Olimp fail");
            });
        }
        else {
            setTimeout(function () { Reload(); },5000);
        }
    });
}
function Parse() {
    //Olimp.ParentBrowser[0].page.render('screenshot.png');
    var kk;
    Olimp.ParentBrowser[0].page.evaluate(function () {
        if (document.getElementsByClassName('msbtn1')[2] != undefined)
            document.getElementsByClassName('msbtn1')[2].click();
        //Olimpbet js
        structarr = [];
        if (document.getElementById('betline') != undefined && document.getElementById('betline').getElementsByClassName('smallwnd2')[0] != undefined) {
            
            var table = document.getElementById('betline').getElementsByClassName('smallwnd2')[0];
            var games = table.getElementsByClassName('hi');
            //cycle
            for (var s = 0; s < games.length; s++) {
                var bets = games[s].nextSibling.nextSibling;
                var players = '';
                var len = games[s].children[1].children[0].children[0].childNodes.length;
                for (var g = 0; g < len; g++) {
                    if (games[s].children[1].children[0].children[0].childNodes[g].nodeName == '#text') {
                        
                        players = games[s].children[1].children[0].children[0].childNodes[g];
                    }
                }
                players = players.textContent.replace(/\d/g, '').slice(1).split(' - ');
                var score = ['', '', ''];
                if (games[s].getElementsByClassName('txtmed')[0] != undefined)
                    score = games[s].getElementsByClassName('txtmed')[0].textContent.split(/[()]/);
                //console.log(score);
                structarr.push(
                    {
                        'Event': '',
                        'Player1': players[0].trim(),
                        'Player2': players[1].trim(),
                        'ScoreAll': score.slice(1, 2).toString().replace(/,/g,"").replace(/:/g," "),
                        'GamePoints': score[2].replace(/:/g, " "),
                        'GamesArr': []
                    });
                var betgame = 0;
                if (bets.getElementsByTagName('div')[1] != undefined)
                    betgame = bets.getElementsByTagName('div')[1].children;
                if (betgame != 0)
                    for (var i = 0; i < betgame.length; i++) {
                        if ((betgame[i].textContent.indexOf('set') != -1 && betgame[i].textContent.indexOf('game') != -1)) {
                            //console.log(betgame[i].textContent + ' ' + betgame[i].textContent.length);
                            if (betgame[i].textContent.length == 17 || betgame[i].textContent.length == 18) {
                                //console.log(betgame[i].textContent);
                                if (betgame[i + 3].textContent!=undefined&& betgame[i + 3].textContent.match(/\d+.\d+/g)!=null) {
                                    var coef1 = betgame[i + 2].textContent.match(/\d+.\d+/g)[0];
                                    var coef2 = betgame[i + 3].textContent.match(/\d+.\d+/g)[0];
                                    var setnum = betgame[i].textContent.match(/\d{2}|\d/ig);
                                    //console.log(coef1);
                                    //console.log(coef2);
                                    
                                    structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': setnum[0], 'GameNumber': setnum[1], 'Coefficent1': coef1, 'Coefficent2': coef2 });
                                }
                            }
                        }
                    };
            };
        }
        else {

        }
        ;
        if(document.getElementById("refrate")!=undefined&& document.getElementById("refrate").previousSibling.previousSibling!=undefined)
        document.getElementById("refrate").previousSibling.previousSibling.click();
        return structarr;
    }, function (result) {
        //console.log(result.length);
        process.send({ olimpdata: result });
        kk = result;
    });
    console.log(kk);
};
//Main
Setup();
//Messages Handlers
process.on('message', function (data) {
    console.log('Olimp command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
  //  console.log(cnt);
});