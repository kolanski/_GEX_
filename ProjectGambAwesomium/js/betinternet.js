//betinternet.js
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
for (var i=0;i<document.getElementsByClassName("panelHeader ContentCollapsed").length;i++)
{
    if (document.getElementsByClassName("panelHeader ContentCollapsed")[i].textContent.indexOf("Winner")!=-1)
        $($(".panelHeader.ContentCollapsed")[i]).trigger("click");

}
for (var i=0;i<document.getElementsByClassName("panelHeader ContentCollapsed").length;i++)
{
    if (document.getElementsByClassName("panelHeader ContentCollapsed")[i].textContent.indexOf("Winner")!=-1)
        $($(".panelHeader.ContentCollapsed")[i]).trigger("click");
}

for (var i=0;i<document.getElementsByClassName("panelHeader ContentExpanded").length;i++)
{
    if (document.getElementsByClassName("panelHeader ContentExpanded")[i].textContent.indexOf("Winner")!=-1&&document.getElementsByClassName("panelHeader ContentExpanded")[i].textContent.indexOf("Set")!=-1)
    {
        document.getElementsByClassName("panelHeader ContentExpanded")[i].textContent;
        numset = document.getElementsByClassName("panelHeader ContentExpanded")[i].textContent.match(/\d+/gi);
        console.info(numset[0].textContent);
        coef1 = document.getElementsByClassName("panelHeader ContentExpanded")[i].nextSibling.children[0].children[0].children[1].textContent;
        coef2 = document.getElementsByClassName("panelHeader ContentExpanded")[i].nextSibling.children[0].children[1].children[1].textContent;
        GamesArray.push( { SetNum: numset[0], GameNum: numset[1], Coef1: coef1, Coef2: coef2 });
    }
}