using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGambAwesomium
{
    public class Scripts
    {
        public static string Williams
        {
            get
            {
                return
@"// JavaScript source code
var list = document.getElementsByClassName('leftPad title');
var scoreBoard = document.getElementById('scoreboard_frame');
var SetNumber = '';
var GameNumber ='';
var Player2 = '';
var Player1 = '';
var Coefficent2 = '';
var Coefficent1 = '';
var Score = '';
var CurrentGames = '';
var CurrentPoints ='';
var GamesArray = [];
var LenArr='';
for (var i = 0; i < list.length; i++)
{
    if(list[i].textContent.indexOf('Set - Game')!=-1&&list[i].textContent.indexOf('Point')==-1&&list[i].textContent.indexOf('Score')==-1&&list[i].textContent.indexOf('Total')==-1&&list[i].textContent.indexOf('Win ')==-1&&list[i].textContent.indexOf('Games')==-1)
    {
        SetNumber = list[i].childNodes[5].textContent;
        re=/^[0-9]/ig;
        GameNumber = SetNumber.slice(SetNumber.length-2 , SetNumber.length);
        SetNumber= SetNumber.match(re);
        Player1 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventselection')[0].textContent.trim();
        Player2 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventselection')[1].textContent.trim();
        Coefficent1 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventprice')[0].textContent.trim();
        Coefficent2 = list[i].parentNode.parentNode.parentNode.parentNode.childNodes[1].getElementsByClassName('eventprice')[1].textContent.trim();
        GamesArray.push({ SetNum: SetNumber, GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
        Score = '';
        for (var s = 1; s <= 4; s++)
        {
            if(scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent != '')
            {
                if (scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent.length > 1) {
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent[0] + ' ';
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_B').textContent[0] + ' ';
                }
                else
                {
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_A').textContent + ' ';
                    Score += scoreBoard.contentDocument.getElementById('set_' + s + '_B').textContent + ' ';
                }
            }
        }
        CurrentGames = scoreBoard.contentDocument.getElementById('games_A').textContent.toString() + ' ' + scoreBoard.contentDocument.getElementById('games_B').textContent.toString();
        CurrentPoints = scoreBoard.contentDocument.getElementById('points_A').textContent.toString() + ' ' + scoreBoard.contentDocument.getElementById('points_B').textContent.toString();
    }
}";
            }
        }
        public static string WilliamsTestBrowser = @"var testBrowser=document.getElementById('breadcrumb').childNodes[5].textContent;";
        public static string WilliamsGetLinksSet = @"var list = document.getElementsByClassName('rowLive');
var len = list.length;";
        public static string WilliamsGetLinksArr(int i)
        {
            return @"list["+i.ToString()+"].getElementsByTagName('td')[4].childNodes[1].getAttribute('Href')";
        }
        public static string Unibet
        {
            get
            {
                return @"// JavaScript source code
var Player1 = document.getElementsByClassName('event-name-home eventName')[0].textContent;
var Player2 = document.getElementsByClassName('event-name-away eventName')[0].textContent;
var EventName = document.getElementsByClassName('clearfix event-path-and-description')[0].textContent;
var Row1 = document.getElementsByClassName('set-row2')[0];
var Row2 = document.getElementsByClassName('set-row3')[0];
var Score = '';
var CurrentPoints = '';
var SetGame = '';
var Coefficent2 = '';
var Coefficent1 = '';
var GamesArray = [];
var SetNum = '';
var GameNum = '';
for (var i = 2; i < Row1.childNodes.length - 1; i++) {
    if (Row1.childNodes[i].textContent!='')
    Score += Row1.childNodes[i].textContent + ' ' + Row2.childNodes[i].textContent + ' ';
}
var list = document.getElementsByClassName('criteria');
CurrentPoints = Row1.childNodes[Row1.childNodes.length - 1].textContent + ' ' + Row2.childNodes[Row2.childNodes.length - 1].textContent;
for (i = 0; i < list.length; i++) {
    if (list[i].childNodes[0].textContent.length == 15 || list[i].childNodes[0].textContent.length == 14) {
        var SetGame = list[i].childNodes[0].textContent.replace('SET', '').replace('GAME', '').replace('-', '');
        var Coefficent2 = list[i].nextSibling.childNodes[2].childNodes[0].childNodes[1].textContent;
        var Coefficent1 = list[i].nextSibling.childNodes[1].childNodes[0].childNodes[1].textContent;
        SetNumber = SetGame[1].replace(' ','');
        GameNumber = (SetGame[SetGame.length - 2] + SetGame[SetGame.length - 1]).replace(' ', '');
        GamesArray.push({ SetNum: SetNumber, GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
    }
}";
            }
        }
        public static string UnibetGetLinksSet = @"var list = document.getElementsByClassName('clearfix event-path-and-description');
var arr=[];
for(var i=0;i<list.length;i++)
{
if(list[i].textContent.indexOf('Tennis')!=-1)
arr.push(document.getElementsByClassName('starred ir')[i].getAttribute('data-id'));
}";
        public static string Bet365GetLinksSet = @"var len=document.getElementsByClassName('Fixture').length;
var arr=[];
for(var i=0;i<len-1;i++)
{
arr.push(document.getElementsByClassName('Fixture')[i].wrapper.stem.data.ID);
}";
        public static string Bet365
        {
            get
            {
                return @"// JavaScript source code
var Score = '';
var CurrentPoints = '';
var Player1 = '';
var Player2 = '';
var ScoreBoard = document.getElementById('previousSets');
var GamesArray = [];
for (var h = 0; h < ScoreBoard.childNodes[0].childNodes.length; h++) {
    Score += ScoreBoard.childNodes[0].childNodes[h].textContent.replace('-', ' ') + ' ';
}
CurrentPoints = document.getElementById('team1ArenaPoint').textContent + ' ' + document.getElementById('team2ArenaPoint').textContent;

var tmpPl1 = document.getElementById('team1Name');
var tmpPl2 = document.getElementById('team2Name');
if (tmpPl1 != null) {
    Player1 = document.getElementById('team1Name').textContent;
    Player2 = document.getElementById('team2Name').textContent;
}

var MatchBets = document.getElementsByClassName('Market');

for (var h = 0; h < MatchBets.length; h++) {
    if (MatchBets[h].textContent.indexOf('Game Winner')!=-1) {
        var GameNumber = MatchBets[h].childNodes[0].childNodes[0].textContent.replace('Game Winner', '').replace('th', '').replace('nd', '').replace('st', '').replace('rd', '');
        var PlayerEqual = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].textContent;
        var Coefficent1 = MatchBets[h].childNodes[1].childNodes[0].childNodes[0].childNodes[1].textContent;
        var Coefficent2 = MatchBets[h].childNodes[1].childNodes[0].childNodes[1].childNodes[1].textContent;
        PlayerEqual = PlayerEqual.replace('(Svr)', '');
        if (PlayerEqual.indexOf(Player1) !=-1)
            GamesArray.push({ SetNum: '0', GameNum: GameNumber, Coef1: Coefficent1, Coef2: Coefficent2 });
        else
            GamesArray.push({ SetNum: '0', GameNum: GameNumber, Coef1: Coefficent2, Coef2: Coefficent1 });

    }

}";
            }
        }
        public static string Marathon
        {
            get
            {
                return @"// JavaScript source code


var Score = '';
var CurrentPoints = '';
var Player1 = '';
var Player2 = '';
var numset = '';
var numgame = '';
var coef1 = '';
var coef1 = '';
var GamesArray = [];
var splitted = [];

var PlayersData = document.getElementsByClassName('live-today-member-name nowrap');
var GamesData = document.getElementsByClassName('market-table-name');
if (PlayersData != null && PlayersData.length > 0) {
    Player1 = PlayersData[0].textContent.replace(',', ' ');
    Player2 = PlayersData[1].textContent.replace(',', ' ');
    Score = document.getElementsByClassName('cl-left red')[0].textContent;
    splitted = Score.split(' ');
    CurrentPoints = splitted[splitted.length - 1].replace(':', ' ').replace('(', '').replace(')', '');
    if (Player1.indexOf('.') != -1 || Player2.indexOf('.') != -1) {
        Player1 = (Player1);
        Player2 = (Player2);
    }
    if (splitted.length == 3) {
        Score = splitted[0].replace('(', '').replace(')', '');
    }
    else {
        Score = '';
        for (var s = 1; s < splitted.length - 1; s++) {
            Score += splitted[s].replace(':', ' ').replace(',', ' ');
        }
        Score = Score.replace('(', '').replace(')', '');
    }
    GamesData = document.getElementsByClassName('market-table-name');
    for (var k = 0; k < GamesData.length; k++) {
        if (GamesData[k].textContent.indexOf('To Win Game') != -1) {
            var TableWithGames = GamesData[k].nextSibling.nextSibling;
            var CountOfTr = 0;
            if (TableWithGames.getElementsByTagName('tr') != null)
                CountOfTr = TableWithGames.getElementsByTagName('tr').length;
            for (var h = 1; h <= CountOfTr; h++) {

                if (TableWithGames.getElementsByTagName('tr')[h] != null) {
                    //var index = GamesData[k].textContent
                    numset = GamesData[k].textContent.match(/\d/gi)[0];
                    numgame = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[0].getElementsByTagName('span')[0].textContent.replace('Game', '').replace(' ', '').replace(' ', '');
                    coef1 = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[1].getElementsByTagName('div')[0].textContent.replace('\n', '').replace(' ', '').replace(' ', '').trim();
                    coef2 = TableWithGames.getElementsByTagName('tr')[h].getElementsByTagName('td')[2].getElementsByTagName('div')[0].textContent.replace('\n', '').replace(' ', '').replace(' ', '').trim();
                    GamesArray.push({ SetNum: numset, GameNum: numgame, Coef1: coef1, Coef2: coef2 });
                }

            }

        }
    }
}";
            }
        }
        public static string MarathonGetLinksSet = @"var list = document.getElementsByClassName('available-event');
var arr=[];
for(var i=0;i<list.length;i++)
{
var toadd =list[i].parentNode;
if(toadd.textContent.indexOf('Tennis')!=-1&&toadd.textContent.indexOf('Table')==-1)
arr.push(list[i].getAttribute('data-event-treeid'));
}";
        public static string Fonbet
        {
            get
            {
                return @"var tables=document.getElementById('lineTable');
var live = tables;
var trs = live.getElementsByTagName('tr');
var istennis = false;

var structarr=[];

var structGame={'Event':''
                ,'Player1':''
                ,'Player2':''
                ,'ScoreAll':''
                ,'GamePoints':''
				,'GamesArr':[]
               };
for (var i = 0; i < trs.length; i++)
    if (trs[i].getAttribute('class') != null)
    {
        if (trs[i].getAttribute('class') == 'trSegment')
        {

            if (trs[i].textContent.indexOf('Table tennis')==-1&&trs[i].textContent.indexOf('Ten')!=-1)
            {
                var currentevent = trs[i].textContent;
                istennis = true;
            }
            else
                istennis = false;
        }
        if (istennis)
        {
            if (trs[i].getElementsByClassName('eventCellName') != null)
            {
                //console.info(i);
                var el=trs[i].getElementsByClassName('eventCellName')[0] ;

                if (el!=null)
                {
                    

                    if (el.getElementsByClassName('event')[0]!=null)
                        playerinn = el.getElementsByClassName('event')[0].childNodes[1];
                    else
                        if (el.getElementsByClassName('eventBlocked')[0]!=null)
                            playerinn = el.getElementsByClassName('eventBlocked')[0].childNodes[1];
                    //console.info(playerinn);

                    if (playerinn.length>8)

                    {
						var scoregame = el.getElementsByClassName('eventScore ')[0];
                    if (scoregame.textContent.indexOf('(') != -1)
                        scoregame = scoregame.textContent.slice(scoregame.textContent.indexOf('(')+1,scoregame.textContent.length-1);
                        var player1=playerinn.textContent.split('—')[0];
                        var player2=playerinn.textContent.split('—')[1];
                        console.info(currentevent);
                        console.info(player1);
                        if (player2[0]==' ')
                            player2=player2.slice(1,player2.length);
                        console.info(player2);
                        console.info(scoregame);
						structarr.push({'Event':currentevent
						,'Player1':player1
						,'Player2':player2
						,'ScoreAll':scoregame
						,'GamePoints':''
						,'GamesArr':[]
						});
                    }

                    else

                    {

					currentpoints = el.getElementsByClassName('eventScore ')[0];
                    if (currentpoints.textContent.indexOf('(') != -1)
                        currentpoints = currentpoints.textContent.slice(currentpoints.textContent.indexOf('(')+1,currentpoints.textContent.length-1);
                        var setnum=playerinn.textContent;
                        console.info(setnum);
                        console.info(currentpoints);
						structarr[structarr.length-1].GamePoints=currentpoints
                        var grids= trs[i].nextSibling.getElementsByClassName('grid');
                        for (var h=0;h<grids.length;h++)
                        {
                            if (grids[h].textContent.indexOf('Games')!=-1 && grids[h].textContent.indexOf('special')==-1 && grids[h].getElementsByTagName('thead')[0].getElementsByTagName('tr') != null)
                            {
                                for (var g=0;g<grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr').length;g++)
                                {
                                    var numgame = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[0].textContent.replace('Game ','');
                                    var coef1 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[1].textContent;
                                    var coef2 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[2].textContent;
                                    console.info(numgame.textContent);
                                    console.info(coef1.textContent);
                                    console.info(coef2.textContent);
									structarr[structarr.length-1].GamesArr.push({'SetNumber':setnum[0],'GameNumber':numgame,'Coefficent1':coef1,'Coefficent2':coef2});
                                }
                            }

                        }

                    }
                }
            }
        }

    }";
            }
        }
        public static string FonbetSettings = @"var lineContainerHeader='';
    lineContainerHeader = document.getElementById('lineContainerHeader');
    lineContainerHeader.textContent = '';
    lineTableHeaderDOM = document.createElement('table');
    lineTableHeaderDOM.className = 'lineTable';
    lineTableHeaderDOM.id = 'lineTableHeader';
    var a = document.createElement('thead'),
        b = document.createElement('tr');
    lineTableHeaderDOM.appendChild(a);
    a.appendChild(b);
    for (a = 0; a < client.locale.columnHeaders.length; a++) {
        var c = document.createElement('th');
        c.textContent = client.locale.columnHeaders[a];
        0 == a && (c.id = 'lineTableHeaderButton', c.onclick = function() {
            lineTableHeaderButtonClick(this)
        });
        b.appendChild(c)
    }
    tableBody = document.createElement('tbody');
    lineTableHeaderDOM.appendChild(tableBody);
    lineContainerHeader.appendChild(lineTableHeaderDOM);
    b = client.accountNumber;
    a = client.loggedIn;
    client.accountNumber = client.data.loginInfo.clientId;
    client.loggedIn = !0;
    var d;
    client.settings.checkWordWrapSegment.value || (d = 'rowNoWrap');
    client.accountNumber = b;
    client.loggedIn = a;
    lineContainer = document.getElementById('lineContainer');
    lineContainer.textContent = '';
    lineTableDOM = document.createElement('table');
    lineTableDOM.className = 'lineTable' + (d ? ' ' + d : '');
    lineTableDOM.id = 'lineTable';
    tableBody = document.createElement('tbody');
    lineTableDOM.appendChild(tableBody);
    lineContainer.appendChild(lineTableDOM);
    d = document.createElement('div');
    d.className = 'lineTableFooter';
    lineContainer.appendChild(d);
    lineContainer.onscroll = function(a) {
        lineContainerHeader.scrollLeft = a.target.scrollLeft
    }";
        public static string FonbetSet = @"var list = document.getElementsByClassName('event');
                var process = true;
                while (process)
                {
                    list = document.getElementsByClassName('event');
                    var test = list.length;
                    for (var x = 0; x < list.length; x++)
                    {
if(list[x].getElementsByClassName('detailArrowClose').length==1&&list[x].getElementsByClassName('detailArrowClose')[0].parentNode.textContent.indexOf('set')!=-1)
                        {list[x].onclick();}
                    }
                    if (test == list.length)
                        process = false;
                    
                }";
        public static string BetCity
        {
            get
            {
                return @"//Betcity.js
var Tables=document.getElementsByTagName('div')[3].getElementsByTagName('table');
var brs=$('.all').contents().filter(function() {    return this.nodeType === 3                         });

var structarr=[];
var brscnt=0;
for (var k=0;k<Tables.length;k++)
{
    if (Tables[k].id=='')
    {
        var player1=Tables[k].childNodes[0].childNodes[1].childNodes[1].textContent;
        var player2=Tables[k].childNodes[0].childNodes[1].childNodes[2].textContent;
		brscnt++;
		var Scorearr=Tables[k].getElementsByClassName('red')[0].textContent.replace('Score:','').split(/[()]/);
		if(Scorearr!=null&&Scorearr.length==3)
		{
        var Score=Scorearr[Scorearr.length-2].replace(/\:/g,' ').replace(/\,/g,' ');
		var Points=Scorearr[Scorearr.length-1];
		}
		structarr.push({'Event':brs[brscnt].textContent
						,'Player1':player1
						,'Player2':player2
						,'ScoreAll':Score
						,'GamePoints':Points
						,'GamesArr':[]
						});
        var list=Tables[k].getElementsByTagName('div');
        for (var i=0;i<list.length;i++)
        {
            if (list[i].textContent.indexOf('To win a game')!=-1)
            {                var numgame =list[i].childNodes[1].textContent.replace('W1','').replace(/[^0-9]/g, '');
                var coef1=list[i].childNodes[2].textContent;
                var coef2=list[i].childNodes[4].textContent;
				structarr[structarr.length-1].GamesArr.push({'SetNumber':'0','GameNumber':numgame,'Coefficent1':coef1,'Coefficent2':coef2});
            }
        }
        
    }
}";
            }
        }
        public static string BetInternetGet
        {
            get
            {
               return @"if(document.getElementById('InPlayHomepageWidgetPanel').className!='whiteDownArrow')$('#InPlayHomepageWidgetPanel').click();
var data= document.getElementById('HPIR_SportPanel_6');
var len=data.children[1].children[0].children;
var arr=[];
for(var i=0;i<len.length-1;i++)
{
	arr.push(data.children[1].children[0].children[i].children[0].href);
}";
            }
        }

        public static string BetInternet
        {
            get
            {
                return @"//betinternet.js
var Player1 = document.getElementsByClassName('OutcomeNameCell OutcomeNameCellPosition_1')[0].textContent;
var Player2 = document.getElementsByClassName('OutcomeNameCell OutcomeNameCellPosition_1')[1].textContent;
var Score = '';
var CurrentPoints = '';
var SetGame = '';
var coef1 = '';
var coef2 = '';
var GamesArray = [];
var SetNum = '';
var numset = '';
var numgame='';
var GameNum='';
for (var i=0;i<document.getElementsByClassName('panelHeader ContentCollapsed').length;i++)
{
    if (document.getElementsByClassName('panelHeader ContentCollapsed')[i].textContent.indexOf('Winner')!=-1)
        $($('.panelHeader.ContentCollapsed')[i]).trigger('click');

}
for (var i=0;i<document.getElementsByClassName('panelHeader ContentCollapsed').length;i++)
{
    if (document.getElementsByClassName('panelHeader ContentCollapsed')[i].textContent.indexOf('Winner')!=-1)
        $($('.panelHeader.ContentCollapsed')[i]).trigger('click');
}

for (var i=0;i<document.getElementsByClassName('panelHeader ContentExpanded').length;i++)
{
    if (document.getElementsByClassName('panelHeader ContentExpanded')[i].textContent.indexOf('Winner')!=-1&&document.getElementsByClassName('panelHeader ContentExpanded')[i].textContent.indexOf('Set')!=-1)
    {
        document.getElementsByClassName('panelHeader ContentExpanded')[i].textContent;
        numset = document.getElementsByClassName('panelHeader ContentExpanded')[i].textContent.match(/\d+/gi);
        console.info(numset[0].textContent);
        coef1 = document.getElementsByClassName('panelHeader ContentExpanded')[i].nextSibling.children[0].children[0].children[1].textContent;
        coef2 = document.getElementsByClassName('panelHeader ContentExpanded')[i].nextSibling.children[0].children[1].children[1].textContent;
        GamesArray.push( { SetNum: numset[0], GameNum: numset[1], Coef1: coef1, Coef2: coef2 });
    }
}";
            }
        }
    }
}
