function OPenAllFrames()
{
    var len = document.getElementsByClassName('ipo-Fixture').length;
    var div = document.createElement('div');
    div.className = "gameData ";
    document.getElementsByClassName("ipo-FixtureList_SubHeading")[0].appendChild(div);
    var arr = [];
    for (var i = 0; i < len - 1; i++) {
        arr.push(document.getElementsByClassName('ipo-Fixture')[i].wrapper.stem.data.ID);
    }

    var html = [];
    for (var i = 0; i < arr.length; i++) {

        html.push('<li> ');
        html.push("<iframe width=\"600\" height=\"200\" src=\"" + "https://mobile.365sb.com/default.aspx?apptype=&ot=2&appversion=&cb=1429878108#type=Coupon;key=" + arr[i] + "\"></iframe>");

        html.push('</li>');
    }
    document.getElementsByClassName("gameData ")[0].innerHTML = html.join('');
}
structarr = [];
function Parse() {
    structarr = [];
    var listl;
    if (document.getElementsByClassName("gameData")[0] != undefined)
        listl = document.getElementsByClassName("gameData")[0];
    if (listl != undefined) {
        listl = listl.children;
    }
    if (listl != undefined)
        for (var i = 0; i < listl.length; i++)
        {
            var Score = '';
            var CurrentPoints = '';
            var Player1 = '';
            var Player2 = '';
            var ScoreBoard = listl[i].children[0].contentDocument.getElementsByClassName("IPTableRow");
            var data = [];
            var GamesArray = [];
            if (ScoreBoard != null && ScoreBoard.length>0) {
                //Count gamesscore
                var Score1row = ScoreBoard[0].getElementsByClassName("IPScoreCell");
                var Score2row = ScoreBoard[1].getElementsByClassName("IPScoreCell");
                for (var h = 0; h < Score1row.length; h++)
                {
                    if (h > 0)
                        Score += " ";
                    if (Score1row[h].className.indexOf("additional-points") == -1)
                    {
                        Score += Score1row[h].textContent + " " + Score2row[h].textContent;
                    }
                    else
                    {
                        CurrentPoints += Score1row[h].textContent + " " + Score2row[h].textContent;
                    }
                    
                }
                Player1 = listl[i].children[0].contentDocument.getElementsByClassName('Team')[0].textContent.replace("*","");
                Player2 = listl[i].children[0].contentDocument.getElementsByClassName('Team')[1].textContent.replace("*", "");
            }
            var event = listl[i].children[0].contentDocument.getElementsByClassName("IPTableCompetitionName").textContent;
            var list = listl[i].children[0].contentDocument.getElementsByClassName("ipe-Market_Button");
            for (var n = 0; n < list.length; n++) {
                var test = list[n].textContent;
                if (test.indexOf("Game Winner") != -1) {
                    if (list[n].className == "ipe-Market_Button")
                    { list[n].click() }
                }
            }
            structarr.push({
                'Event': event,
                'Player1': Player1,
                'Player2': Player2,
                'ScoreAll': Score,
                'GamePoints': CurrentPoints,
                'GamesArr': []
            });
            var MatchBets = listl[i].children[0].contentDocument.getElementsByClassName('ipe-Market');
            for (var h = 0; h < MatchBets.length; h++) {
                if (MatchBets[h].textContent.indexOf('Game Winner') != -1) {
                    var GameNumber = MatchBets[h].childNodes[0].childNodes[0].textContent.replace('Game Winner', '').replace('th', '').replace('nd', '').replace('st', '').replace('rd', '');
                    if (MatchBets[h].childNodes[1] != undefined) {
                        var PlayerEqual = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].textContent;
                        var Coefficent1 = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].childNodes[1].textContent;
                        var Coefficent2 = MatchBets[h].childNodes[1].childNodes[0].childNodes[1].childNodes[1].textContent;
                        PlayerEqual = PlayerEqual.replace('(Svr)', '');
                        if (PlayerEqual.indexOf(Player1) != -1) {
                            //{ SetNum: '0', GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
                            structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': '0', 'GameNumber': GameNumber, 'Coefficent1': Coefficent1, 'Coefficent2': Coefficent2 });;
                        }
                        else {
                            structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': '0', 'GameNumber': GameNumber, 'Coefficent1': Coefficent2, 'Coefficent2': Coefficent1 });
                        }
                    }
                }
            }
        }
}
function ManagerDelete()
{
    var listl;
    if (document.getElementsByClassName("gameData")[0] != undefined)
        listl = document.getElementsByClassName("gameData")[0];
    if (listl != undefined) {
        listl = listl.children;
    }
    for(var i=0;i<listl.length;i++)
    {
        var test = listl[i].children[0];
        if(test.contentWindow.location.hash=="#type=InPlay;")
        {
            console.log(i);
            document.getElementsByClassName("gameData")[0].removeChild(listl[i]);
        }
    }
}
function ManagerAdd()
{
    var len = document.getElementsByClassName('Fixture').length;
    var div = document.createElement('div');
    div.className = "gameData ";
    if (document.getElementsByClassName("gameData")[0]==undefined)
    document.getElementsByClassName("FixtureList PC_2 ")[0].appendChild(div);
    var arr = [];
    for (var i = 0; i < len - 1; i++) {
        found = false;
        var listl;
        if (document.getElementsByClassName("gameData")[0] != undefined)
            listl = document.getElementsByClassName("gameData")[0];
        if (listl != undefined) {
            listl = listl.children;
        }
        for (var k = 0; k < listl.length; k++)
        {
            if(listl[k].children[0].contentWindow.location.hash.indexOf(document.getElementsByClassName('Fixture')[i].wrapper.stem.data.ID)!=-1)
            {
                found = true;
            }
        }
        if(!found)
        arr.push(document.getElementsByClassName('Fixture')[i].wrapper.stem.data.ID);
    }

    var html = [];
    for (var i = 0; i < arr.length; i++) {

        html += ('<li> ');
        html += ("<iframe width=\"600\" height=\"200\" src=\"" + "https://mobile.365sb.com/default.aspx?apptype=&ot=2&appversion=&cb=1429878108#type=Coupon;key=" + arr[i] + "\"></iframe>");
        html += ('</li>');
    }

    document.getElementsByClassName("gameData ")[0].insertAdjacentHTML('beforeEnd', html);
}
function AdditiontoParse()
{
    var list = document.getElementsByClassName("MarketButton ");
    for (var n = 0; n < list.length; n++) {
        var test = list[n].textContent;
        if (test.indexOf("Game Winner") != -1) {
            if (list[n].className == "MarketButton ")
            { list[n].click() }
        }
    }
}