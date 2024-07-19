function loadallgames() {
    var all = document.getElementsByClassName("first member-area  ");
    var stringtext = "";
    for (var i = 0; i < all.length; i++) {
        var stringtext = stringtext + all[i].parentNode.parentNode.id.replace("event_", "") + ",";
    }
    document.location.href = "https://www.betmarathon.com/en/live/22723?openedMarkets=" + stringtext.slice(0, stringtext.length - 1) + "&oddsType=Decimal";
}
function checkupdate() {
    var test = document.location.href.slice(56, document.location.href.length - 17)
    var all = document.getElementsByClassName("first member-area  ");
    var stringtext = "";
    for (var i = 0; i < all.length; i++) {
        var stringtext = stringtext + all[i].parentNode.parentNode.id.replace("event_", "") + ",";
    }
    //console.log(test);
    //console.log(stringtext.slice(0, stringtext.length - 1));
    if (test != stringtext.slice(0, stringtext.length - 1)) {
        console.log("reloading");
        loadallgames();
    }
}
//usused &pageAction=default
//
//Request URL: https://liveupdate.marathonbookmakers.com/en/liveupdate/22723?openedMarkets=4552771&callback=jQuery110208736931775711298_1472158221203&markets=4552771&available=-1049479683&updated=1472158218221&oddsType=Decimal&siteStyle=SIMPLE&_=1472158221204
//https://www.betmarathon.com/en/live/22723?openedMarkets=4507938,4509269,4508643,4508556,4509251,4509256,4508452,4509265&oddsType=Decimal