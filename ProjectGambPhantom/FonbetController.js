//Fonbet main module test
//Setup imports
var phantom = require('phantom');
//var wait = require('wait.for');
//Global variables to setup
var mainpage;
var mainphantom;

//Setup variables
console.log('Fonbet Started');

//process.stdout.write("fonbet comm test");
phantom.create(function (ph) {
    ph.createPage(function (page) {
        mainpage = page;
        mainphantom = ph;
        console.log('Fonbet Created');
        Navigate();
    });
});

function Navigate() {
    mainpage.open('https://live.bkfonbet.com/?locale=en#4',
      function (status) {
        console.log('Opened site status: %s', status);
        if (status == 'success')
            console.log('Fonbet Navigated');
        //Exit();
    });
};
var ret = [];
var cnt = 0;
oldresult = [];
function Parse() {
    var page = mainpage;
    cnt = 0;
    mainpage.evaluate(function () {
        //setup fonbet
        function setup() {
            var list = document.getElementsByClassName('event');
            var process = true;
            while (process) {
                list = document.getElementsByClassName('event');
                var test = list.length;
                for (var x = 0; x < list.length; x++) {
                    if (list[x].getElementsByClassName('detailArrowClose').length == 1 && list[x].getElementsByClassName('detailArrowClose')[0].parentNode.textContent.indexOf('set') != -1) { list[x].onclick(); }
                }
                if (test == list.length || cnt > 30)
                    process = false;
                else
                    cnt++;
            }
        }
        
        var structarr = [];
        setup();
        var structGame = {
            'Event': ''
            ,'Player1': ''
            ,'Player2': ''
            ,'ScoreAll': ''
            ,'GamePoints': ''
            ,'GamesArr': []
        };
        structarr = [];
        Event = '';
        
        var line = document.getElementById("lineTable");
        node = line.getElementsByClassName("tdSegmentColor tdSegmentColorSport4")[0].parentNode;
        while (node != null) {
            var Score = '';
            var CurrentPoints = '';
            var Player1 = '';
            var Player2 = '';
            if (node.className == "trSegment") {
                Event = node.textContent;
            }
            if (node.className == "trEvent sportColor4 level1") {
                if (node.getElementsByClassName('event')[0] != undefined) {
                    var playerinn = node.getElementsByClassName('event')[0].childNodes[1];
                    if (playerinn.length > 8 && node.getElementsByClassName('eventScore ')[0].textContent != "") {
                        var scoregame = node.getElementsByClassName('eventScore ')[0];
                        if (scoregame.textContent.indexOf('(') != -1)
                            scoregame = scoregame.textContent.slice(scoregame.textContent.indexOf('(') + 1, scoregame.textContent.length - 1);
                        scoregame = scoregame.replace(/\-/gi, " ");
                        if (scoregame.indexOf("(") != -1) {
                            scoregame = scoregame.slice(0, scoregame.indexOf("("));
                        }
                        var player1 = playerinn.textContent.split('—')[0];
                        var player2 = playerinn.textContent.split('—')[1];
                        if (player1.indexOf("(") != -1) player1 = player1.slice(0, player1.indexOf('('));
                        if (player2.indexOf("(") != -1) player2 = player2.slice(0, player2.indexOf('('));
                        console.info(Event);
                        
                        if (player2[0] == ' ')
                            player2 = player2.slice(1, player2.length);
                        console.info(player1);
                        structarr.push({
                            'Event': Event
                            , 'Player1': player1
                            , 'Player2': player2
                            , 'ScoreAll': scoregame
                            , 'GamePoints': ''
                            , 'GamesArr': []
                        });
                    }
                }
            }
            if (node.className == "trEventChild sportColor4 level2") {
                console.info("level 2");
                var currentpoints = node.getElementsByClassName('eventScore ')[0];
                if (currentpoints.textContent.indexOf('(') != -1)
                    currentpoints = currentpoints.textContent.slice(currentpoints.textContent.indexOf('(') + 1, currentpoints.textContent.length - 1);
                if (node.getElementsByClassName("event ")[0] != undefined) {
                    var setnum = node.getElementsByClassName("event ")[0].childNodes[1].textContent[0];
                    if (currentpoints.length == undefined)
                        currentpoints = currentpoints.textContent;
                    console.info(currentpoints);
                    console.info(setnum);
                    if (structarr.length > 0) {
                        if (currentpoints.indexOf('points') != -1) {
                            currentpoints= currentpoints.slice(0, str.indexOf("points") - 1)
                        }
                        structarr[structarr.length - 1].GamePoints = currentpoints.replace(":", " ").replace(/-/g, " ").replace('*','');
                    }
                }
            }
            
            if (node.className == "trEventDetails sportColor4 level2") {
                var grids = [];
                grids = node.getElementsByClassName('grid');
                console.info(grids.length);
                for (var h = 0; h < grids.length; h++) {
                    if (grids[h].textContent.indexOf('Games') != -1 && grids[h].textContent.indexOf('special') == -1 && grids[h].textContent.indexOf('score') == -1 && grids[h].getElementsByTagName('thead')[0].getElementsByTagName('tr') != null) {
                        for (var g = 0; g < grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr').length; g++) {
                            var numgame = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[0].textContent.replace('Game ', '');
                            var coef1 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[1].textContent;
                            var coef2 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[2].textContent;
                            console.info(numgame);
                            console.info(coef1);
                            console.info(coef2);
                            if (structarr.length > 0) {
                                if (grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[1].attributes["class"].value != "eventCellBlock")
                                structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': setnum[0], 'GameNumber': numgame, 'Coefficent1': coef1, 'Coefficent2': coef2 });
                            }
                        }
                    }
                }
            }
            node = node.nextSibling;
        }
        
        return structarr;
    }, function (result) {
        //console.log(result);
        ret = result;
        if (result != null && result.length > 2) {
            oldresult = result;
        }
        else {
            if (oldresult != null && oldresult.length > 2) {
                result = oldresult;
            }
        }
        process.send({ fonbetdata: result });
       // ph.exit();
       // console.log(cnt);
    });
   // console.log(cnt);
   // setTimeout(function () {  }, 1000) ;
};
function Exit() {
    mainphantom.exit();
};
//Messages Handlers
process.on('message', function (data) {
    console.log('Fonbet command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
  //  console.log(cnt);
});