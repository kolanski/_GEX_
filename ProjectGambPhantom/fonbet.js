//fonbet.js
//fonbet parser module
var phantom = require('phantom');
//navigating to fonbet
var mainpage;
phantom.create(function (ph) {
    ph.createPage(function (page) {
        page.open('https://live.bkfonbet.com/?locale=en#4',
      function (status) {
            console.log('Opened site? %s', status);
            another_funny(page, ph);
            console.log(mainpage);
        });
    });
});

//main parsing function
function oldparse()
{
    var table = document.getElementsByTagName("Table")[2];
    var rows = table.getElementsByTagName("tr");
    for (var i = 0; i < rows.length; i++) {
        var node = rows[i];
        var atr = node.attributes["class"];
        if (atr != null && atr.value.tostring() == "sportHead") {
            var cols = rows[i].getElementsByTagName("td");

        }
    }
}
/*function another_funny(page, ph) {
    mainpage = page;
    page.evaluate(functWion () {
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
                if (test == list.length)
                    process = false;
                console.log("test:" + test + " " + list.length);
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
        
        function funny() {
            structarr = [];
            var tables = document.getElementById('lineTable');
            var live = tables;
            var trs = live.getElementsByTagName('tr');
            var istennis = false;
            var playerinn = "";
            for (var i = 0; i < trs.length; i++)
                if (trs[i].getAttribute('class') != null) {
                    if (trs[i].getAttribute('class') == 'trSegment') {
                        
                        if (trs[i].textContent.indexOf('Table tennis') == -1 && trs[i].textContent.indexOf('Ten') != -1) {
                            var currentevent = trs[i].textContent;
                            istennis = true;
                        }
                        else
                            istennis = false;
                    }
                    if (istennis) {
                        if (trs[i].getElementsByClassName('eventCellName') != null) {
                            //console.info(i);
                            var el = trs[i].getElementsByClassName('eventCellName')[0];
                            
                            if (el != null) {
                                
                                if (el.getElementsByClassName('event')[0] != null)
                                    playerinn = el.getElementsByClassName('event')[0].childNodes[1];
                                else
                                    if (el.getElementsByClassName('eventBlocked')[0] != null)
                                        playerinn = el.getElementsByClassName('eventBlocked')[0].childNodes[1];
                                //console.info(playerinn);
                                
                                if (playerinn.length > 8) {
                                    var scoregame = el.getElementsByClassName('eventScore ')[0];
                                    if (scoregame.textContent.indexOf('(') != -1)
                                        scoregame = scoregame.textContent.slice(scoregame.textContent.indexOf('(') + 1, scoregame.textContent.length - 1);
                                    scoregame = scoregame.replace(/\-/gi," ");
                                    var player1 = playerinn.textContent.split('—')[0];
                                    var player2 = playerinn.textContent.split('—')[1];
                                    console.info(currentevent);
                                    console.info(player1);
                                    if (player2[0] == ' ')
                                        player2 = player2.slice(1, player2.length);
                                    
                                    structarr.push({
                                        'Event': currentevent
                                        ,'Player1': player1
                                        ,'Player2': player2
                                        ,'ScoreAll': scoregame
                                        ,'GamePoints': ''
                                        ,'GamesArr': []
                                    });
                                }

                                else {
                                    
                                    currentpoints = el.getElementsByClassName('eventScore ')[0];
                                    if (currentpoints.textContent.indexOf('(') != -1)
                                        currentpoints = currentpoints.textContent.slice(currentpoints.textContent.indexOf('(') + 1, currentpoints.textContent.length - 1);
                                    var setnum = playerinn.textContent;
                                    console.info(setnum);
                                    console.log(currentpoints);
                                    if (structarr[structarr.length - 1].GamePoints != "undefined") {
                                        structarr[structarr.length - 1].GamePoints = currentpoints
                                        var grids = trs[i].nextSibling.getElementsByClassName('grid');
                                        for (var h = 0; h < grids.length; h++) {
                                            if (grids[h].textContent.indexOf('Games') != -1 && grids[h].textContent.indexOf('special') == -1 && grids[h].getElementsByTagName('thead')[0].getElementsByTagName('tr') != null) {
                                                for (var g = 0; g < grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr').length; g++) {
                                                    var numgame = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[0].textContent.replace('Game ', '');
                                                    var coef1 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[1].textContent;
                                                    var coef2 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[2].textContent;
                                                    console.info(numgame.textContent);
                                                    console.info(coef1.textContent);
                                                    console.info(coef2.textContent);
                                                    structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': setnum[0], 'GameNumber': numgame, 'Coefficent1': coef1, 'Coefficent2': coef2 });
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }

                }
            return structarr;
        }
        
        h2Arr = funny();
        return h2Arr;
    },
  function (result) {
        console.log(result);        
       // ph.exit();
    });
};

*/