String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};





var arr = [];
var structarr = [];
var len = document.getElementById("TABLEL").children.length;
for (var z = 1; z< len; z++) {
    if (document.getElementById("TABLEL").children[z].className != "cham1")
        arr.push(parseInt(document.getElementById("TABLEL").children[z].id.substring(6, document.getElementById("TABLEL").children[z].id.length)));
}

console.log(arr.length);
var tipsarray = [];
for (var b = 0; b < tPlus.length; b++) { if (tPlus[b].ID_SPORT == 5 && tPlus[b].Live==1) tipsarray.push(tPlus[b]) };

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
            , 'Player1': Player1
            , 'Player2': Player2
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
            //if ((mRospis[k].ID_TIP_RADAR < 94 && mRospis[k].ID_TIP_RADAR > 88) || mRospis[k].ID_TIP_RADAR == 10979 || mRospis[k].ID_TIP_RADAR == 11047) {
            //    console.log(mRospis[k]);
            //}
            for (t = 0; t < tipsarray.length; t++) {
                for (h = 0; h < tipsarray[t].ID_TIP_RADAR.length; h++)
                {
                    if (tipsarray[t].ID_TIP_RADAR[h] == mRospis[k].ID_TIP_RADAR)
                    {
                        //console.log(tPlus[t].FREE);
                        //Победитель < br />следующего & nbsp; гейма
                        //Победитель < br />текущего & nbsp; гейма
                        //Победитель&nbsp;гейма
                        if (tipsarray[t].FREE == "Победитель<br />следующего&nbsp;гейма" || tipsarray[t].FREE == "Победитель<br />текущего&nbsp;гейма" || tipsarray[t].FREE == "Победитель&nbsp;гейма") {
                            structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': currentsetnum, 'GameNumber': mRospis[k].KOEF, 'Coefficent1': mRospis[k].V[0]/100, 'Coefficent2': mRospis[k].V[1]/100 });
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


















/*

for (var i = 0; i < mLines.length; i++) {
    if (arr.indexOf(mLines[i].ID_EVENT) != -1 && mLines[i].ID_TIP_RADAR == 91) {
        console.log("Player1: " + transliterate(mEvents[EventId[mLines[i].ID_EVENT]].UCHASTNIK1));
        console.log("Player2: " + transliterate(mEvents[EventId[mLines[i].ID_EVENT]].UCHASTNIK2)); 
        console.log("Score: " + transliterate(mEvents[EventId[mLines[i].ID_EVENT]].SETSCORES)); 
        console.log("Points: " + transliterate(mEvents[EventId[mLines[i].ID_EVENT]].SCORE)); 
        
        structarr.push({
            'Event': ''
            ,'Player1': player1
            ,'Player2': player2
            ,'ScoreAll': Score
            ,'GamePoints': Points
            ,'GamesArr': []
        });
        console.log(mLines[i].KOEF);
        console.log(parseFloat(mLines[i].V[0]/100));
        console.log(parseFloat(mLines[i].V[1] / 100));
        var _mEvent = mEvents[EventId[mLines[i].ID_EVENT]];
        
        

        var mRospis = new Array();

        for (i = 0; i < _mEvent.mlines.length; i++)
            mRospis.push(mLines[LineId[_mEvent.mlines[i]]]);

        for (i = 0; i < mRos.length; i++)
            mRospis.push(mRos[i]);

        console.info(mRospis.length);
        if (((mRospis[i].ID_TIP_RADAR < 94 && mRospis[i].ID_TIP_RADAR > 88) || mRospis[i].ID_TIP_RADAR == 10979 || mRospis[i].ID_TIP_RADAR == 11047) && rapidinner[2].length == 0)
            rapidinner[2] = radar12(mRospis[i], mEvent, t, 'ВЫИГРАЕТ ГЕЙМ', mRospis[i].KOEF + ' гейм' + genius(mRospis[i].ID_TIP_RADAR - 88));
        /*var mRospis=new Array();for(i=0;i<mEvent.mlines.length;i++) mRospis.push(mLines[LineId[mEvent.mlines[i]]]);for(i=0;i<mRos.length;i++) mRospis.push(mRos[i]);
        
if(((mRospis[i].ID_TIP_RADAR<94&&mRospis[i].ID_TIP_RADAR>88)||mRospis[i].ID_TIP_RADAR==10979||mRospis[i].ID_TIP_RADAR==11047)&&rapidinner[2].length==0)
rapidinner[2]=radar12(mRospis[i],mEvent,t,'ВЫИГРАЕТ ГЕЙМ',mRospis[i].KOEF+' гейм'+genius(mRospis[i].ID_TIP_RADAR-88));
         * */
/*if (sumscore > numgame)
    pushset
else
    notpuushset
structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': '0', 'GameNumber': numgame, 'Coefficent1': coef1, 'Coefficent2': coef2 });
}
}
*/

function radarkoeflarg(mRos, mEvent, t) {
    if (!mEventradar) return;
    var rapidinner = new Array();
    rapidinner[0] = rapidinner[1] = rapidinner[2] = rapidinner[3] = rapidinner[4] = "";
    var i = 0;
    var koef = 0;
    function genius(n) {
        if (n < 10) return ' (' + n + 'сет)';
        {
            return "";
        }
        else if (mEvent.Time_Event.indexOf("сет") > -1)
            return ' (' + mEvent.Time_Event + ')';
    }
    var mRospis = new Array(); for (i = 0; i < mEvent.mlines.length; i++) mRospis.push(mLines[LineId[mEvent.mlines[i]]]);
    for (i = 0; i < mRos.length; i++) mRospis.push(mRos[i]);
    for (i = 0; i < mRospis.length; i++) {
        if ((mRospis[i].ID_TIP_RADAR == 10 || mRospis[i].ID_TIP_RADAR == 6322) && rapidinner[0].length == 0) rapidinner[0] = radar12(mRospis[i], mEvent, t, 'ИСХОД (ПОБЕДИТЕЛЬ)', 'матч');
        if ((mRospis[i].ID_TIP_RADAR == 11 || mRospis[i].ID_TIP_RADAR == 10997) && (rapidinner[1].length == 0 || parseInt(mRospis[i].KOEF) < koef)) { rapidinner[1] = radar12short(mRospis[i], mEvent, t, mRospis[i].KOEF + " сет"); koef = parseInt(mRospis[i].KOEF); }
        if (((mRospis[i].ID_TIP_RADAR < 94 && mRospis[i].ID_TIP_RADAR > 88) || mRospis[i].ID_TIP_RADAR == 10979 || mRospis[i].ID_TIP_RADAR == 11047) && rapidinner[2].length == 0)
            rapidinner[2] = radar12(mRospis[i], mEvent, t, 'ВЫИГРАЕТ ГЕЙМ', mRospis[i].KOEF + ' гейм' + genius(mRospis[i].ID_TIP_RADAR - 88));
        if ((mRospis[i].ID_TIP_RADAR == 83 || mRospis[i].ID_TIP_RADAR == 6481) && rapidinner[3].length == 0) rapidinner[3] = radartotal(mRospis[i], mEvent, t, "", "геймов<br/>в матче");
    }
    if (rapidinner[0].length == 0 && rapidinner[1].length > 0)
        rapidinner[1] = '<div style="float:left;margin-top:17px;color:#fff;font-size:14px;width:229px;text-align:center;margin-left:42px;">ИСХОД (ПОБЕДИТЕЛЬ)</div>' + rapidinner[1];
    if (rapidinner[3].length == 0 && rapidinner[4].length > 0)
        rapidinner[4] = '<div style="float:left;margin-top:17px;color:#fff;font-size:14px;width:229px;text-align:center;margin-left:46px;"><div style="float:left;margin-left:8px;width:63px;">м</div><div style="float:left;width:79px;">ТОТАЛ</div><div style="float:left;width:63px;">б</div></div>' + rapidinner[4];
    rapidinner[5] = ""
    for (i = 0; i < 5; i++) if (rapidinner[i].length > 0) rapidinner[5] = rapidinner[5] + rapidinner[i];
    if (mEvent.State == 2) rapidinner[5] = rapidinner[5].split("lineclick").join("nolineclick");
    id_("radarframeklarg").innerHTML = rapidinner[5];
}

