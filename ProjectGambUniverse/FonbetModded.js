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
            if (playerinn.length > 8) {
                var scoregame = node.getElementsByClassName('eventScore ')[0];
                if (scoregame.textContent.indexOf('(') != -1)
                    scoregame = scoregame.textContent.slice(scoregame.textContent.indexOf('(') + 1, scoregame.textContent.length - 1);
                scoregame = scoregame.replace(/\-/gi, " ");
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
            if(structarr.length>0)
            {
                structarr[structarr.length - 1].GamePoints = currentpoints;
            }
        }
    }

    if (node.className == "trEventDetails sportColor4 level2") {
        var grids = [];
        grids = node.getElementsByClassName('grid');
        console.info(grids.length);
        for (var h = 0; h < grids.length; h++) {
            if (grids[h].textContent.indexOf('Games') != -1 && grids[h].textContent.indexOf('special') == -1 && grids[h].getElementsByTagName('thead')[0].getElementsByTagName('tr') != null) {
                for (var g = 0; g < grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr').length; g++) {
                    var numgame = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[0].textContent.replace('Game ', '');
                    var coef1 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[1].textContent;
                    var coef2 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[2].textContent;
                    console.info(numgame);
                    console.info(coef1);
                    console.info(coef2);
                    structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': setnum[0], 'GameNumber': numgame, 'Coefficent1': coef1, 'Coefficent2': coef2 });

                }
            }
        }
    }
    node = node.nextSibling;
}


/*
 if(node.className == "trEventChild sportColor4 level2")
        {
            console.info("level 2");
            var currentpoints = node.getElementsByClassName('eventScore ')[0];
            if (currentpoints.textContent.indexOf('(') != -1)
                currentpoints = currentpoints.textContent.slice(currentpoints.textContent.indexOf('(') + 1, currentpoints.textContent.length - 1);
            if (node.getElementsByClassName("event ")[0] != undefined) {
                var setnum = node.getElementsByClassName("event ")[0].childNodes[1].textContent;
                console.info(currentpoints);
                console.info(setnum);
            }
            node = node.nextSibling
        }
        if(node.className == "trEventDetails sportColor4 level2")
        {
            var grids = [];
            grids = node.getElementsByClassName('grid');
            console.info(grids.length);
            node = node.nextSibling;
        }*/