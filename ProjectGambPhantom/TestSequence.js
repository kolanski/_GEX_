var phantom = require('phantom');

mainpage = "";
phantom.create(function (ph) {
    ph.createPage(function (page) {
        mainpage = page;
        mainphantom = ph;
        console.log('Test Created');
        Navigate();
    });
});
function Navigate() {
    mainpage.open('https://www.betmarathon.com/en/live.htm',
      function (status) {
        console.log('Opened site status: %s', status);
        if (status == 'success')
            console.log('Fonbet Navigated');
        //Exit();
    });
};

function Parse()
{
    prot = console;
    
    function some (callback) {
      mainpage.evaluate(function () {
            //setup fonbet
            return document.title;
        }, function (result) {  
            return result;
        });
        return callback;
    }    ;

    
    //return callback;
    
};
setTimeout(function () {  console.log("Parse result"+Parse());}, 5000);