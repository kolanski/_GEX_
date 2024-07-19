// chat.js
var mydata;
window.onload = function () {
    var socket = io.connect('http://localhost:8080');
    var field = document.getElementById('field');
    var form = document.getElementById('form');
    var content = document.getElementById('content');

    var name = prompt('Как вас зовут?', 'Гость');
    form.onsubmit = function ()
    {
        var text = field.value;
        socket.emit('send', { message: name+":"+text });
        field.value = '';
        return false;
    };
	btn.onclick= function()
	{
		console.log('norm');
		socket.emit('restable');
	};

    var messages = [];
    
    socket.on('message', function (data) {
        if(data.message)
        {
            messages.push(data.message);
            var html = '';
            for(var i=0;i<messages.length;i++)
            {
                html += messages[i] + '<br/>';
            }
            content.innerHTML = html;
        }
        else
        {
            console.log('wrong');
        }
    });

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

