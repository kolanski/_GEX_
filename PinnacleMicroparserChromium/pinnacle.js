var tennispage = "https://beta.pinnacle.com/ru/Sports/33/Live";

function insertdata(inlogin, inpassword) {
    var login = document.getElementsByName("CustomerId");
    var password = document.getElementsByName("Password");
    login[0].value = inlogin;
    password[0].value = inpassword;
}

function submitform() {
    var button=document.getElementsByName("loginSubmit");
    button[0].click();
}

//login to page
testpage();
var structarr = [];
var init = false;
function parse() {
    tmpeventsarr = [];
    var events = document.querySelectorAll("div.live-event");
    var tournamenttest = "";
    var homeplayertst = "";
    var awayplayertest = "";
    for (var i = 0; i < events.length; i++) {
        var current_event = events[i];
        var tournament = current_event.getElementsByClassName("league-name")[0].textContent.trim();
        var home = current_event.getElementsByClassName("home")[0];
        var away = current_event.getElementsByClassName("away")[0];
        var player1 = home.getElementsByClassName("match")[0].textContent.trim();
        var player2 = away.getElementsByClassName("match")[0].textContent.trim();
        var moneyline1 = home.getElementsByClassName("moneyline")[0].textContent.trim();
        var moneyline2 = away.getElementsByClassName("moneyline")[0].textContent.trim();
        //test is game
        if (player1.indexOf("Game") != -1 && !isNaN(parseFloat(moneyline1))) {
            //is game working on it
            if (tmpeventsarr.length == 0) {
                var tmparr = [];
                tmparr.push(getgame(player1, player2, moneyline1, moneyline2));
                tmpeventsarr.push({
                    'Event': tournament,
                    'Player1': player1.substr(0, player1.indexOf("Game")),
                    'Player2': player2.substr(0, player2.indexOf("Game")),
                    'ScoreAll': "",
                    'GamePoints': "0:0",
                    'GamesArr': tmparr
                });
            }
            else {
                if (tmpeventsarr[tmpeventsarr.length-1].Player1 == player1.substr(0, player1.indexOf("Game"))) {
                    tmpeventsarr[tmpeventsarr.length - 1].GamesArr.push(getgame(player1, player2, moneyline1, moneyline2));
                }
                else {
                    var tmparr = [];
                    tmparr.push(getgame(player1, player2, moneyline1, moneyline2));
                    tmpeventsarr.push({
                        'Event': tournament,
                        'Player1': player1.substr(0, player1.indexOf("Game")),
                        'Player2': player2.substr(0, player2.indexOf("Game")),
                        'ScoreAll': "",
                        'GamePoints': "0:0",
                        'GamesArr': tmparr
                    });
                }
            }
        }
    }
    structarr = tmpeventsarr;
}
function testpage() {
    var currentwindowlocation = window.location.href;
    if (currentwindowlocation != tennispage) {
        switch (currentwindowlocation) {
            case "https://www.pinnacle.com/ru/login":
                console.log("good");
                insertdata("DG9000000", "!heavyloadedpass");
                submitform();
                break;
            default:
                console.log("bad");
                window.location.href = tennispage;
                break;
        }
    }
    else {
        init = true;
    }
}
function getgame(player1, player2, moneyline1, moneyline2)
{
    var setnum = player1.substr(player1.indexOf("Game") + 5, 2);
    var gamenum = player1.substr(player1.indexOf("Set") + 4, 2)
    return {
        SetNumber: setnum,
        GameNumber: gamenum,
        Coefficent1: moneyline1,
        Coefficent2: moneyline2
    }
}