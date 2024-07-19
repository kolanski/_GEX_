//Gambling.js
// chat.js
var mydata;
window.onload = function () {
    var socket = io.connect('http://localhost:8080');
	btn.onclick= function()
	{
		console.log('norm');
		socket.emit('restable');
	};

    socket.on('gettable', function (data) {
        if (data) {
			mydata=data;
            console.log(mydata);
        }
        else {
            console.log('wrong');
        }
    });
};

