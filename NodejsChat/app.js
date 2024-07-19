// _index.js
var express = require('express')
  , routes = require('./routes')
  , user = require('./routes/user')
  , http = require('http')
  , path = require('path');
var socket = require('socket.io');
var app = express();
var io = socket.listen(app.listen(8080));
var gamb = socket.listen(45000);

//var redis = require('socket.io-redis');
//io.adapter(redis({ host: 'localhost', port: 3000 }));
var mysql = require('mysql');
var connection = mysql.createConnection({
    host     : 'localhost',
    user     : 'root',
    password : 'uasdoasjdj',
    "database": "Bets"
});
connection.connect();
var mongoose = require('mongoose');
var passport = require('passport');
var LocalStrategy = require('passport-local').Strategy;

app.configure(function () {
    app.set('port', process.env.PORT || 3000);
    app.set('views', __dirname + '/views');
    app.set('view engine', 'jade');
    app.use(express.favicon());
    app.use(express.logger('dev'));
    app.use(express.bodyParser());
    app.use(express.methodOverride());
    app.use(express.cookieParser('-n`.?&h+f:Jfg@#@;mq1t{=b{Ko<:T;uU0qq4F'));
    app.use(express.session());
    app.use(passport.initialize());
    app.use(passport.session());
    app.use(function (req, res, next) {
        app.locals.pretty = true
        next()
    });
    app.use(app.router);
    app.use(express.static(path.join(__dirname, 'public')));
});
// passport config
var Account = require('./models/account');
passport.use(new LocalStrategy(Account.authenticate()));
passport.serializeUser(Account.serializeUser());
passport.deserializeUser(Account.deserializeUser());
// mongoose
mongoose.connect('mongodb://localhost/passport_local_mongoose');

// routes
require('./routes')(app);

app.configure('development', function () {
    app.use(express.errorHandler());
});


io.sockets.on('connection', function (client) {
    client.emit('message', { message: 'Добро пожаловать!' });
	 var socketId = client.id;
    var clientIp = client.request.connection.remoteAddress;
	var currentdate = new Date(); 
var datetime = "Last Sync: " + currentdate.getDate() + "/"
                + (currentdate.getMonth()+1)  + "/" 
                + currentdate.getFullYear() + " @ "  
                + currentdate.getHours() + ":"  
                + currentdate.getMinutes() + ":" 
                + currentdate.getSeconds();
	console.log(datetime+" "+clientIp);

    client.on('send', function (data) {
        io.sockets.emit('message', { message: data.message });
    });
    client.on('restable', function () {
        var myrows = [];
        var arr;
        
        connection.query('Select distinct Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2 from Arbitrage', function (err, rows, fields) {
            
            
            for (var i in rows) {
                var pl1 = rows[i].Player1;
                var pl2 = rows[i].Player2;
                var Bookmaker1 = rows[i].Bookmaker1;
                var Bookmaker2 = rows[i].Bookmaker2;
                var NumGame = rows[i].NumGame;
                var Score = rows[i].Score.replace("-"," ");
                var ScoreGame = rows[i].ScoreGame;
                var Coefficent1 = rows[i].Coefficent1;
                var Coefficent2 = rows[i].Coefficent2;
                myrows.push({ "Player1": pl1, "Player2": pl2, "Bookmaker1": Bookmaker1, "Bookmaker2": Bookmaker2, "NumGame": NumGame, "Score": Score, "ScoreGame": ScoreGame, "Coefficent1": Coefficent1, "Coefficent2": Coefficent2 });	
            }
            client.emit('gettable', { data: myrows });
        });
    });
	    client.on('restableTable', function () {
        var myrows = [];
        var arr;
        //console.log('wow');
        connection.query('Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from betcity UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from bet365 UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from fonbet UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from internetbet UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from marathon UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from parimatch UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from williams UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from olimp UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from pinnacle UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from titanbet UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from sportingbet UNION Select Player1,Player2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,Coefficent1,Coefficent2,NumSet from winline', function (err, rows, fields) {
            for (var i in rows) {
                var pl1 = rows[i].Player1;
                var pl2 = rows[i].Player2;
                var Bookmaker1 = rows[i].Bookmaker1;
                var Bookmaker2 = rows[i].Bookmaker2;
                var NumGame = rows[i].NumGame;
				var NumSet=rows[i].NumSet;
                var Score = rows[i].Score.replace("-"," ");
                var ScoreGame = rows[i].ScoreGame;
                var Coefficent1 = rows[i].Coefficent1;
                var Coefficent2 = rows[i].Coefficent2;
                myrows.push({ "Player1": pl1, "Player2": pl2, "Bookmaker1": Bookmaker1, "Bookmaker2": Bookmaker2, "NumGame": NumGame, "Score": Score, "ScoreGame": ScoreGame, "Coefficent1": Coefficent1, "Coefficent2": Coefficent2,"NumSet":NumSet });
            }
			//console.log(myrows);
            client.emit('gettableTable', { data: myrows });
        });
    });
    client.on('restartme', function () {
        console.log("client want to restart");
        io.sockets.emit("restartall", { });
        });
    client.on('wantdumpme', function (data) {
        console.log("client want to dump");
        gamb.sockets.emit("dumpdata", { data.data });
    });
    client.on('changebookbase', function (data) {
        console.log("client want to changebookbase");
        console.log(data);
    });
});