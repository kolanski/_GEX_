var BookMaker = require('./BookmakerPattern.js');
var Bet365 = new BookMaker('Bet365');
var socket = require('socket.io');
io = socket.listen(30365);

io.sockets.on('connection', function (socket) {
    socket.on('parseddata', function (msg) {
        console.log('clientwant to reload marathon');
        if (msg != "" && msg != null && msg != undefined) {
            var data = JSON.parse(msg);
            process.send({ Bet365data: data });
        }
    });
}
);

function Parse() {
    io.sockets.emit("parse", {});
}
function Reload() {
    io.sockets.emit("reload", {});
}
process.on('message', function (data) {
    console.log('Bet365 command: ' + data.command);
    if (data.command == 'Parse') {
        Parse();
    }
    if (data.command == 'Reload') {
        Reload();
    }
    //  console.log(cnt);
});
//setInterval(Parse,5000);