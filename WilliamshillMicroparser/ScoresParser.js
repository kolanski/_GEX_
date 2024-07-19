//init page addr http://scoreboards.williamhill.com/tennis/?ver=1/en-gb/
var isinit = true;
var page = require('webpage').create();
var system = require('system');
var initpageurl = 'http://scoreboards.williamhill.com/tennis/?ver=1/en-gb/';
function init() {
    page.open(initpageurl, function (status) {
        var divCount = page.evaluate(function () {
            return document.getElementsByTagName('DIV').length;
        });
        console.log(divCount);
    });
    isinit = true;
    console.log("Init is called!");
}
function updateScoreboards()
{

}
console.log("Functions init is OK!");