var BookMaker = require('./BookmakerPattern.js');

var Winline = new BookMaker("Winline");
function Setup() {
    Winline.ParentNavigate("http://winline22.com/stavki/sport/tennis/");
    setTimeout(function () {
        Winline.ParentBrowser[0].page.evaluate(
            function () {
                window.location.href = "http://winline22.com/stavki/sport/tennis/"
            }
        )
    }, 10000);

};
Setup();
function Reload() {
    Winline.ParentBrowser[0].page.open("http://winline22.com/stavki/sport/tennis/");
    setTimeout(function () {
        Winline.ParentBrowser[0].page.evaluate(
            function () {
                window.location.href = "http://winline22.com/stavki/sport/tennis/"
            }
        )
    }, 10000);
}
function Parse() {
    //Winline.ParentBrowser[0].page.render('screenshot.png');
    Winline.ParentBrowser[0].page.evaluate(function () {
        var test = document.getElementsByClassName("old__version ng-binding ng-scope")[0];
        if (test != undefined) {
            test.click();
            return;
        }
        if (window.location.href
            == "http://winline22.com/") {
            window.location.href =
                "http://winline22.com/stavki/sport/tennis/";
            return;
        }
        structarr = [];
        function Parse() {
            String.prototype.replaceAll = function (search, replacement) {
                var target = this;
                return target.split(search).join(replacement);
            };
            var arr = [];
            var len = document.getElementById("TABLEL").children.length;
            for (var z = 1; z < len; z++) {
                if (document.getElementById("TABLEL").children[z].className != "cham1")
                    arr.push(parseInt(document.getElementById("TABLEL").children[z].id.substring(6,

                        document.getElementById("TABLEL").children[z].id.length)));
            }

            console.log(arr.length);
            var tipsarray = [];
            for (var b = 0; b < tPlus.length; b++) {
                if (tPlus[b].ID_SPORT == 5 && tPlus[b].Live == 1) tipsarray.push

                    (tPlus[b])
            };

            for (var j = 0; j < mEvents.length; j++) {
                if (arr.indexOf(mEvents[j].ID_EVENT) != -1) {
                    var mEvent = mEvents[j];
                    Player1 = transliterate(mEvents[j].UCHASTNIK1);
                    Player2 = transliterate(mEvents[j].UCHASTNIK2);
                    Score = mEvents[j].SETSCORES.replaceAll(" - ", " ").replaceAll(":", " ");
                    Points = mEvents[j].SCORE;
                    // console.log("Player1: " + Player1);//mEvents[j].UCHASTNIK1);//
                    // console.log("Player2: " + Player2);//mEvents[j].UCHASTNIK2); //
                    /// console.log("Score: " + Score);//mEvents[j].SETSCORES);//
                    //// console.log("Points: " + Points);//mEvents[j].SCORE);//
                    // console.log("i:" + j);
                    structarr.push({
                        'Event': ''
                        , 'Player1': Player1.replace(/_/g, " ")
                        , 'Player2': Player2.replace(/_/g, " ")
                        , 'ScoreAll': Score
                        , 'GamePoints': Points
                        , 'GamesArr': []
                    });
                    var currentsetnum = (Score.split(" ").length) / 2;
                    //console.log("setnum: "+currentsetnum);
                    var mRospis = new Array();
                    for (i = 0; i < mEvent.mlines.length; i++) {
                        mRospis.push(mLines[LineId[mEvent.mlines[i]]]);
                    }

                    //console.log(mRospis.length);

                    for (var k = 0; k < mRospis.length; k++) {
                        //if ((mRospis[k].ID_TIP_RADAR < 94 && mRospis[k].ID_TIP_RADAR > 88) || mRospis[k].ID_TIP_RADAR == 

                        10979 || mRospis[k].ID_TIP_RADAR == 11047) {
        //    console.log(mRospis[k]);
        //}
        for (t = 0; t < tipsarray.length; t++) {
            for (h = 0; h < tipsarray[t].ID_TIP_RADAR.length; h++) {
                if (tipsarray[t].ID_TIP_RADAR[h] == mRospis[k].ID_TIP_RADAR) {
                    //console.log(tPlus[t].FREE);
                    //Победитель < br />следующего & nbsp; гейма
                    //Победитель < br />текущего & nbsp; гейма
                    //Победитель&nbsp;гейма
                    if (tipsarray[t].FREE == "Победитель<br />следующего&nbsp;гейма" || tipsarray[t].FREE

                        == "Победитель<br />текущего&nbsp;гейма" || tipsarray[t].FREE == "Победитель&nbsp;гейма") {
                        if (ParseInt(mRospis[k].KOEF) > 13)
                            currentsetnum = 0;
                        structarr[structarr.length - 1].GamesArr.push({
                            'SetNumber': currentsetnum,

                            'GameNumber': mRospis[k].KOEF, 'Coefficent1': mRospis[k].V[0] / 100, 'Coefficent2': mRospis[k].V[1] / 100
                        });
                        //  console.log(mRospis[k].ID_TIP_RADAR);
                        //  console.log(tipsarray[t].FREE);
                        // console.log(mRospis[k]);
                    }
                }
            }
        }
    }
}
            }
        }
Parse();
return structarr;
    }, function (result) { process.send({ winlinedata: result }); })
}
process.on('message', function (data) {
    console.log('Winline command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
    //  console.log(cnt);
});

function func() {
    Parse();
}

setTimeout(func, 30000);