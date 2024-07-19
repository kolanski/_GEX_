//Basic pattern for all bookmakers
//extension methods
var Events = require('events');

Array.prototype.remove = function (from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

//testsequence



//Constructor
function BookMaker(bookname) {
    this.Phantom = require('node-slimerjs');
    this.name = bookname;
    this.BookmakerTabs = [];
    this.BookmakerTennisGames = [];
    this.BookmakerWebBrowsers = [];
    this.GamesLinks = [];
    this.ParentBrowser = [];
    this.DefaultParentUrl = "";
    this.Channel = new Events.EventEmitter();
    
};

BookMaker.prototype.SetUpBrowser = function (DefaultUrl) {
    this.DefaultParentUrl = DefaultUrl;
};
options = {
    "diskCache": true,
    "localStoragePath": "c:/bin/storage"
};
BookMaker.prototype.OpenPage = function (Url) {
    
    mypage = new Object;
    myph = new Object;
    mybook = this.BookmakerTabs;
    myname = this.name;
    this.Phantom.create(function (ph) {
        ph.createPage(function (page) {
            console.log(myname + ' Created');
            //console.log(page);
            mypage = page;
            myph = ph;
            //console.log(mypage);
            mybook.push({ "page": mypage, "phant": myph });
            page.set('viewportSize', { width: 1280, height: 960 });
            page.open(Url,
      function (status) {
                console.log('Opened site status: %s', status);
                if (status == 'success')
                    console.log(myname + ' Navigated');
            });
            
        });
    }, options);
    
};
BookMaker.prototype.getTitle = function (ind) {
    st = "";
    if (this.BookmakerTabs[ind] != null)
        title = this.BookmakerTabs[ind].page.evaluate(function () {
            return document.getElementById('header_container').textContent;
        });
    console.log(title);
    return title;

};
BookMaker.prototype.ParentNavigate = function (Url) {
    mypage1 = new Object;
    myph1 = new Object;
    mybook1 = this.ParentBrowser;
    myname1 = this.name;
    this.Phantom.create( function (ph) {
        ph.createPage(function (page) {
            console.log(myname1 + ' Created');
            //console.log(page);
            //console.log(mypage);
            mybook1.push({ "page": page, "phant": ph });
            page.set('viewportSize', { width: 1280, height: 960 });
            page.open(Url, function (status) {
                console.log('Opened site status: %s', status);
                if (status == 'success') {
                    console.log(myname1 + ' Parent Navigated');
                    page.evaluate(function () {
                        console.log(document.length);
                        return document.title;
                    }, function (result) {
                        console.log(result);
                    });
                    //nnn.log(ret);
                }
            });
        });
    });
};
BookMaker.prototype.CreateTab = function (Url) {
    this.OpenPage(Url);
};

BookMaker.prototype.DeleteTab = function (Index) {
    // console.log(this.BookmakerTabs[Index]);
    //this.BookmakerTabs[Index].close();
    if (this.BookmakerTabs[Index] != null) {
        this.BookmakerTabs[Index].phant.exit();
        this.BookmakerTabs.remove(Index);
    }
};

BookMaker.prototype.Navigate = function (SiteUrl) {
    
    // this.Phantom.create
    console.log(this.name + " trying navigate to " + SiteUrl);
    var mypage = this.OpenPage(SiteUrl);
};

module.exports = BookMaker;
