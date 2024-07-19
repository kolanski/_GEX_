var BookMaker = require('./BookmakerPattern.js');       

var Pinnacle = new BookMaker("Pinnacle");
function Setup() {
    Pinnacle.ParentNavigate("http://www.pin1111.com/m/Mobile/Login");
    setTimeout(function () {
        Pinnacle.ParentBrowser[0].page.evaluate(function () {
            document.getElementsByName("CustomerId")[0].value = "MP100000";
            document.getElementsByName("Password")[0].value = "jdjsladklla!!";

            document.getElementsByClassName('header-link ui-link')[0].click();
        });
    }, 6000);
    setTimeout(function () {
        Pinnacle.ParentBrowser[0].page.evaluate(
            function () {
                var list = document.getElementsByClassName("ui-link");
                for (var i = 0; i < list.length; i++) {
                    if (list[i].href.indexOf("Tennis") != -1) { if (list[i].textContent.indexOf("Live") != -1) list[i].click() }
                }
            }
        )
    }, 10000);
    setTimeout(function () {
        Pinnacle.ParentBrowser[0].page.evaluate(
            function () {
                var list = document.getElementsByClassName("content-collapsible ui-collapsible-set ui-group-theme-inherit ui-corner-all")[0].children;
                for (var i = 0; i < list.length; i++) {
                    list[i].children[0].click();
                }
            }
        )
    }, 14000);
}
Setup();
function Reload() {
    Pinnacle.ParentBrowser[0].page.open("http://www.pin1111.com/m/Mobile");
    setTimeout(function () { Pinnacle.ParentBrowser[0].page.open("document.getElementsByClassName('header-link ui-link')[0].click();"); }, 3000);
    setTimeout(function () {
        Pinnacle.ParentBrowser[0].page.evaluate(
            function () {
                var list = document.getElementsByClassName("ui-link");
                for (var i = 0; i < list.length; i++) {
                    if (list[i].href.indexOf("Tennis") != -1) { if (list[i].textContent.indexOf("Live") != -1) list[i].click() }
                }
            }
        )
    }, 6000);
    setTimeout(function () {
        Pinnacle.ParentBrowser[0].page.evaluate(
            function () {
                var list = document.getElementsByClassName("content-collapsible ui-collapsible-set ui-group-theme-inherit ui-corner-all")[0].children;
                for (var i = 0; i < list.length; i++) {
                    list[i].children[0].click();
                }
            }
        )
    }, 9000);
};
var refcount = 0;
function Parse() {
    
    if (refcount > 5) {
        Pinnacle.ParentBrowser[0].page.evaluate(function () { window.location.reload(); });
        refcount = 0;
    }
    refcount++;
    Pinnacle.ParentBrowser[0].page.evaluate(
        function () {
            //Open Games in iframe
            
            structarr = [];
            function Setup() {
                var list = document.getElementsByClassName("content-collapsible ui-collapsible-set ui-group-theme-inherit ui-corner-all")[0].children;
                for (var i = 0; i < list.length; i++) {
                    list[i].children[0].click();
                }
            }
            
            function OpenAllFrames() {
                var list = document.getElementsByClassName("ui-block-b game-teams one-half");
                if (list != undefined) {
                    var len = list.length;
                    var arr = [];
                    for (var i = 0; i < list.length; i++) {
                        if (list[i].textContent.indexOf("Game") != -1)
                            arr.push(list[i].parentNode.href)
                    }
                    myarr = [];
                    var html = [];
                    if (arr.length > 0) {
                        for (var i = 0; i < arr.length; i++) {
                            
                            html.push('<li> ');
                            html.push("<iframe width=\"650\" height=\"200\" src=\"" + arr[i] + "\"></iframe>");
                            html.push('</li>');
                        }
                        var div = document.createElement('div');
                        div.className = "gameData ";
                        document.getElementById("featuredSports").appendChild(div);
                        document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');
                    }
                }
                else {
                    Setup();
                }
            }
            function Parse() {
                if (document.getElementsByClassName("gameData")[0] != undefined) {
                    
                    
                    structarr = [];
                    var listl;
                    if (document.getElementsByClassName("gameData")[0] != undefined)
                        listl = document.getElementsByClassName("gameData")[0];
                    if (listl != undefined) {
                        listl = listl.children;
                    }
                    for (var i = 0; i < listl.length; i++) {
                        var Table = listl[i].children[0].contentDocument.getElementsByClassName("ui-listview")[0];
                        if (Table != undefined) {
                            var data1 = Table.children[0].children[0].textContent;
                            var data2 = Table.children[1].children[0].textContent;
                            
                            var Player1 = data1.slice(0, data1.indexOf("Game"));
                            var Player2 = data2.slice(0, data2.indexOf("Game"));
                            var gamedata = data1.match(/\d{2}|\d/ig);
                            if (structarr[structarr.length - 1] == undefined)
                                structarr.push(
                                    {
                                        'Event': "",
                                        'Player1': Player1,
                                        'Player2': Player2,
                                        'ScoreAll': '',
                                        'GamePoints': '',
                                        'GamesArr': []
                                    });
                            else
                                if (structarr[structarr.length - 1].Player1 != Player1)
                                    structarr.push(
                                        {
                                            'Event': "",
                                            'Player1': Player1,
                                            'Player2': Player2,
                                            'ScoreAll': '',
                                            'GamePoints': '',
                                            'GamesArr': []
                                        });
                            var coef2 = Table.children[1].children[1].textContent;
                            var coef1 = Table.children[0].children[1].textContent;
                            
                            if (gamedata != null)
                                structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': gamedata[1], 'GameNumber': gamedata[0], 'Coefficent1': coef1, 'Coefficent2': coef2 });
                        }

                    }
                    
                }
                else {
                    Setup();
                    OpenAllFrames();
                }
            }
            Parse();
            //refresh after 4 parse
            return structarr;
        }, 
            function (result) {
            //console.log(result);
            process.send({ pinnacledata: result });
        });
}

process.on('message', function (data) {
    console.log('Pinnacle command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
    //  console.log(cnt);
});