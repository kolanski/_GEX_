//Betcity.js
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
}