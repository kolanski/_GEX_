


function OpenAllFrames() {
    var inplaydata = document.getElementById("inplayForSport-tab-TENN");

    var len = inplaydata.getElementsByClassName("mkt").length;
    var div = document.createElement('div');
    div.className = "gameData ";
    document.getElementsByClassName("fragment inplay ")[0].appendChild(div);
    var arr = [];
    var list = inplaydata.getElementsByClassName("mkt")

    for (var i = 0; i < len; i++) {
        arr.push(list[i].getElementsByClassName("mkt-count")[0].children[0].href);
    }
    var html = [];
    for (var i = 0; i < arr.length; i++) {

        html.push('<li> ');
        html.push("<iframe width=\"600\" height=\"200\" src=\"" + arr[i] + "\"></iframe>");

        html.push('</li>');
    }
    document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');
}

function Parse() {
    structarr = [];
    var listl;
    if (document.getElementsByClassName("gameData")[0] != undefined)
        listl = document.getElementsByClassName("gameData")[0];
    if (listl != undefined) {
        listl = listl.children;
    }
    if (listl != undefined) {


        for (var i = 0; i < listl.length; i++) {
            var doc = listl[i].children[0].contentDocument;
            var Score = "";
            var Scoreboard = doc.getElementsByClassName("tn-board")[0];
            if (Scoreboard != undefined && Scoreboard.getElementsByClassName("team-players")[0]!=undefined) {
                var Players = Scoreboard.getElementsByClassName("team-players");
                var Player1 = Players[0].textContent;
                var Player2 = Players[1].textContent;
               // console.info(Player1);
                //Score section
                var Scoreval = "";
                var Currentpoints = "";
                var Score = Scoreboard.getElementsByTagName("tr");
                for(var g=0;g<Score[1].children.length;g++)
                {
                    if(Score[1].children[g]!=undefined&&Score[1].children[g].getAttribute("data-set_1_games")!=undefined)
                    {
                        if (Score[1].children[g].textContent.indexOf("-")==-1)
                        Scoreval += Score[1].children[g].textContent + " " + Score[2].children[g].textContent;
                        
                    }
                    if (Score[1].children[g] != undefined && Score[1].children[g].getAttribute("data-set_2_games") != undefined) {
                        if (Score[1].children[g].textContent.indexOf("-") == -1)
                            Scoreval += " "+Score[1].children[g].textContent + " " + Score[2].children[g].textContent;

                    }
                    if (Score[1].children[g] != undefined && Score[1].children[g].getAttribute("data-set_3_games") != undefined) {
                        if (Score[1].children[g].textContent.indexOf("-") == -1)
                            Scoreval += " " + Score[1].children[g].textContent + " " + Score[2].children[g].textContent;

                    }
                    if (Score[1].children[g] != undefined && Score[1].children[g].getAttribute("data-set_4_games") != undefined) {
                        if (Score[1].children[g].textContent.indexOf("-") == -1)
                            Scoreval += " " + Score[1].children[g].textContent + " " + Score[2].children[g].textContent;

                    }
                    if (Score[1].children[g] != undefined && Score[1].children[g].getAttribute("data-set_5_games") != undefined) {
                        if (Score[1].children[g].textContent.indexOf("-") == -1)
                            Scoreval += " " + Score[1].children[g].textContent + " " + Score[2].children[g].textContent;

                    }
                    if (Score[1].children[g] != undefined && Score[1].children[g].getAttribute("data-points_player1") != undefined) {
                        if (Score[1].children[g].textContent.indexOf("-") == -1)
                            Currentpoints += Score[1].children[g].textContent + " " + Score[2].children[g].textContent;

                    }
                    
                }
               // console.log(Scoreval);
               // console.log(Currentpoints);
                structarr.push(
    {
        'Event': "",
        'Player1': Player1,
        'Player2': Player2,
        'ScoreAll': Scoreval,
        'GamePoints': Currentpoints,
        'GamesArr': []
    });
                var games = listl[i].children[0].contentDocument.getElementsByClassName("expander-MSSGMW expander-button");
                for (var f = 0; f < games.length; f++) {
                    var wingamedata = games[f].textContent.match(/\d{2}|\d/ig);
                    var Set = wingamedata[0];
                    var Game = wingamedata[1];
                    if (games[f].nextSibling != undefined && games[f].nextSibling.nextSibling.getElementsByClassName("limited-row") != undefined) {
                        var coefs = games[f].nextSibling.nextSibling.getElementsByClassName("limited-row");
                        if (coefs != undefined) {
                            var coef1 = coefs[0].getElementsByClassName("price dec")[0].textContent;
                            var coef2 = coefs[1].getElementsByClassName("price dec")[0].textContent;
                            structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': Set, 'GameNumber': Game, 'Coefficent1': coef1, 'Coefficent2': coef2 });

                        }
                    }
                }
            }
        }
    }
    else
    {
        OpenAllFrames();
    }
}