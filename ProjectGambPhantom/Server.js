// script.js
console.log("Server Started");
var Handler = require('./ParseHandler.js');
//var io = require('socket.io-client');
var io = require('socket.io').listen(3000);
var totalcountofrequest = 0;
var interval1Id;
var processing = false;

function clearScreen()
{
    process.stdout.write('\033c');
       totalcountofrequest = 0;
       interval1Id = setInterval(function () {
           console.log("logging count:" + totalcountofrequest);
           io.sockets.emit("processing", {});
           processing = true;
           clearInterval(interval1Id);
       }, 4000);
};
var fonbetstart = new Date();
var olimpstart = new Date();
var marathonstart = new Date();
var betcitystart = new Date();
var parimatchstart = new Date();
var williamsstart = new Date();
var bet365start = new Date();
var pinnaclestart = new Date();
var titanbetstart = new Date();
var sportingbetstart = new Date();
var winlinestart = new Date();

io.sockets.on('connection', function (socket) {
    //io.sockets.emit('this', { will: 'be received by everyone' });
    
    //Handler.ParseHandler(socket, "parse", 'reload', "fonbet", fonbetstart, fonbet, clearScreen);
    //Handler.ParseHandler(socket, "parse1", 'reload1', "olimp", olimpstart, olimp);
    //Handler.ParseHandler(socket, "parse2", 'reload2', "marathon", marathonstart, marathon);
    //Handler.ParseHandler(socket, "parse3", 'reload3', "betcity", betcitystart, betcity);
    //Handler.ParseHandler(socket, "parse4", 'reload4', "parimatch", parimatchstart, parimatch);
    //Handler.ParseHandler(socket, "parse5", 'reload5', "williams", williamsstart, williams);
    //Handler.ParseHandler(socket, "parse6", 'reload6', "bet365", bet365start, bet365);
    //Handler.ParseHandler(socket, "parse7", 'reload7', "pinnacle", pinnaclestart, pinnacle);
    //Handler.ParseHandler(socket, "parse8", 'reload8', "titanbet", titanbetstart, titanbet);
    //Handler.ParseHandler(socket, "parse9", 'reload9', "sportingbet", sportingbetstart, sportingbet1);
    //Handler.ParseHandler(socket, "parse10", 'reload10', "winline", winlinestart, winline);

   socket.on('reload', function (msg) {
       console.log('clientwant to reload fonbet');
       fonbet.send({ command: 'Reload' });
      
   
   });
   socket.on('reload1', function (msg) {
       console.log('clientwant to reload olimp');
       olimp.send({ command: 'Reload' });
   });
   socket.on('reload2', function (msg) {
       console.log('clientwant to reload marathon');
       marathon.send({ command: 'Reload' });
   });
   socket.on('reload3', function (msg) {
       console.log('clientwant to reload betcity');
       betcity.send({ command: 'Reload' });
   });
   socket.on('reload4', function (msg) {
       console.log('clientwant to reload parimatch');
       parimatch.send({ command: 'Reload' });
   });
   socket.on('reload5', function (msg) {
       console.log('clientwant to reload williams');
       williams.send({ command: 'Reload' });
   });
   socket.on('reload6', function (msg) {
       console.log('clientwant to reload bet365');
       bet365.send({ command: 'Reload' });
   });
   socket.on('reload8', function (msg) {
       console.log('clientwant to reload titanbet');
       titanbet.send({ command: 'Reload' });
   });
   socket.on('reload9', function (msg) {
       console.log('clientwant to reload sportingbet');
       sportingbet1.send({ command: 'Reload' });
   });
    socket.on('restartall', function () {
        console.log('RESTART WORKING!!!!!');
        olimp.send({ command: 'Reload' });
        fonbet.send({ command: 'Reload' });
        sportingbet1.send({ command: 'Reload' });
        titanbet.send({ command: 'Reload' });
        bet365.send({ command: 'Reload' });
        williams.send({ command: 'Reload' });
        parimatch.send({ command: 'Reload' });
        betcity.send({ command: 'Reload' });
        marathon.send({ command: 'Reload' });
    });
  
});
//socket.emit('private message', { user: 'me', msg: 'whazzzup?' });

var exec = require('child_process');
var foncount, olcount, marcount, betcount, parcount, wilcount, bet365count, pincount, titcount;
var fonbet;
var olimp;
var marathon;
var betcity;
var parimatch;
var williams;
var bet365;
var pinnacle;
var titanbet;
var sportingbet1;
var winline;



function SetupFonbet()
{
    fonbet = exec.fork(__dirname + '/FonbetController.js');
    //Handler.Init(fonbet,"fonbet", fonbetstart,)
    fonbet.on('message', function (data) {
        var end = new Date() - fonbetstart;
        totalcountofrequest++;
        console.info("Execution time: %dms", end);
        if (data.fonbetdata != null) {
            console.log('fonbetMessage:', data.fonbetdata.length);
            foncount = data.fonbetdata.length;
        }
        io.sockets.emit("fonbetMessage", { data: data.fonbetdata,time:data.timestamp });

    });
    fonbet.on('exit', function (code) {
        console.log('exit with code' + code); SetupFonbet();
    });
};

function SetupWinline() {
    winline = exec.fork(__dirname + '/WinlineController.js');
    //Handler.Init(fonbet,"fonbet", fonbetstart,)
    winline.on('message', function (data) {
        var end = new Date() - winlinestart;
        totalcountofrequest++;
        console.info("Execution time: %dms", end);
        if (data.winlinedata != null) {
            console.log('winlineMessage:', data.winlinedata.length);
            wincount = data.winlinedata.length;
        }
        io.sockets.emit("winlineMessage", { data: data.winlinedata });

    });
    winline.on('exit', function (code) {
        console.log('exit with code' + code); SetupWinline();
    });
};


function SetupWilliams() {
    williams = exec.fork(__dirname + '/WilliamsController.js');
    williams.on('message', function (data) {
        var end = new Date() - williamsstart;
        totalcountofrequest++;
        console.info("Execution time: %dms", end);
        if (data.williamsdata != null) {
            console.log('williamsMessage:', data.williamsdata.length);
            wilcount = data.williamsdata.length;
        }
        io.sockets.emit("williamsMessage", { data: data.williamsdata });

    });
    williams.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupWilliams();
    });
};
function SetupOlimp()
{
    olimp = exec.fork(__dirname + '/OlimpController.js');
    olimp.on('message', function (data) {
        var end = new Date() - olimpstart;
        totalcountofrequest++;
        console.info("Execution time: %dms", end);
        if (data.olimpdata != null)
            console.log('olimpMessage:', data.olimpdata.length);
        io.sockets.emit("olimpMessage", { data: data.olimpdata });
    });
    olimp.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupOlimp();
    });
};
function SetupMarathon() {
    marathon = exec.fork(__dirname + '/MarathonController.js');
    marathon.on('message', function (data) {
        var end = new Date() - marathonstart;
        console.info("Execution time: %dms", end);
        totalcountofrequest++;
        if (data.marathondata != null)
            console.log('marathonMessage:', data.marathondata.length);
        io.sockets.emit("marathonMessage", { data: data.marathondata });
    });
    marathon.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupMarathon();
    });
};
function SetupBetcity() {
    betcity = exec.fork(__dirname + '/BetcityController.js');
    betcity.on('message', function (data) {
        var end = new Date() - betcitystart;
        console.info("Execution time: %dms", end);
        totalcountofrequest++;
        if (data.betcitydata != null)
            console.log('betcityMessage:', data.betcitydata.length);
        io.sockets.emit("betcityMessage", { data: data.betcitydata });
    });
    betcity.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupBetcity();
    });
};
function SetupParimatch()
{
    parimatch = exec.fork(__dirname + '/PariMatchController.js');
    parimatch.on('message', function (data) {
        var end = new Date() - parimatchstart;
        console.info("Execution time: %dms", end);
        totalcountofrequest++;
        if (data.parimatchdata != null)
            console.log('parimatchMessage:', data.parimatchdata.length);
        io.sockets.emit("parimatchMessage", { data: data.parimatchdata });
    });
    parimatch.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupParimatch();
    });
};
function SetupBet365() {
    bet365 = exec.fork(__dirname + '/Bet365Controller.js');
    bet365.on('message', function (data) {
        var end = new Date() - bet365start;
        console.info("Execution time: %dms", end);
        totalcountofrequest++;
        if (data.Bet365data != null)
            console.log('bet365Message:', data.Bet365data.length);
        io.sockets.emit("bet365Message", { data: data.Bet365data });
    });
    bet365.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupBet365();
    });
};
function SetupTitanbet() {
    titanbet = exec.fork(__dirname + '/TitanbetController.js');
    titanbet.on('message', function (data) {
        var end = new Date() - titanbetstart;
        console.info("Execution time: %dms", end);
        totalcountofrequest++;
        if (data.titanbetdata != null)
            console.log('titanbetMessage:', data.titanbetdata.length);
        io.sockets.emit("titanbetMessage", { data: data.titanbetdata });
    });
    titanbet.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupTitanbet();
    });
};
function SetupPinnacle() {
    pinnacle = exec.fork(__dirname + '/PinnacleController.js');
    pinnacle.on('message', function (data) {
        var end = new Date() - pinnaclestart;
        console.info("Execution time: %dms", end);
        totalcountofrequest++;
        if (data.pinnacledata != null)
            console.log('pinnacleMessage:', data.pinnacledata.length);
        io.sockets.emit("pinnacleMessage", { data: data.pinnacledata });
    });
    pinnacle.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupPinnacle();
    });
};
function SetupSportingbet() {
    sportingbet1 = exec.fork(__dirname + '/SportingbetController.js');
    sportingbet1.on('message', function (data) {
        var end = new Date() - sportingbetstart;
        console.info("Execution time: %dms", end);
        totalcountofrequest++;
        if (data.sportingbetdata != null)
            console.log('sportingbetMessage:', data.sportingbetdata.length);
        io.sockets.emit("sportingbetMessage", { data: data.sportingbetdata });
    });
    sportingbet1.on('exit', function (code) {
        console.log('exit with code' + code);
        SetupSportingbet();
    });
};


SetupFonbet();
SetupOlimp();
SetupMarathon();
SetupBetcity();
SetupParimatch();
SetupWilliams();
SetupBet365();
SetupPinnacle();
SetupTitanbet();
SetupSportingbet();
SetupWinline();
//exec wrapper

setTimeout(function () {
    fonbetstart = new Date();    fonbet.send(  { command: 'Parse' });
    olimpstart = new Date();     olimp.send(   { command: 'Parse' });
    marathonstart = new Date();  marathon.send({ command: 'Parse' });
    betcitystart = new Date();   betcity.send( { command: 'Parse' });
    parimatchstart = new Date(); parimatch.send({ command: 'Parse' });
    williamsstart = new Date();  williams.send({ command: 'Parse' });
    bet365start = new Date();    bet365.send({ command: 'Parse' });
    pinnaclestart = new Date();  pinnacle.send({ command: 'Parse' });
    titanbetstart = new Date();  titanbet.send({ command: 'Parse' });
    sportingbetstart = new Date(); sportingbet1.send({ command: 'Parse' });
    winlinestart = new Date(); winline.send({ command: 'Parse' });
}, 30000);
