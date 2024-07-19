function ParseHandler(socket, message,messageReload, nameBook, dateStart, bookToEmit, callback,callback1) {
    socket.on(message, function (msg) {
        if (callback != null && callback != undefined)
            callback();
        console.log('clientwant to parse ' + nameBook);
        bookToEmit.send({ command: 'Parse' });
        dateStart = new Date();

    });
    if (messageReload != null && messageReload != undefined)
        socket.on(messageReload, function (msg) {
            if (callback1 != null && callback1 != undefined)
                callback1();
            console.log('clientwant to reload ' + nameBook);
            bookToEmit.send({ command: 'Reload' });
        });
};
function Init(bookToInit,bookName,dateStart,dataName,message,io,callback)
{
    bookToInit.on('message', function (data) {
        var end = new Date() - dateStart;
        //totalcountofrequest++;
        console.info("Execution time: %dms", end);
        if (data[dataName] != null) {
            console.log(bookName+'Message:', data[dataName].length);
            //foncount = data[dataName].length;
        }
        io.sockets.emit(message, { data: data[dataName] });

    });
    bookToInit.on('exit', function (code) {
        console.log('exit with code' + code); callback();
    });
};
module.exports.ParseHandler = ParseHandler;
module.exports.Init = Init;