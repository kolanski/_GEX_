function Person(Player1, Player2, Bookmaker1, Bookmaker2, NumGame, NumSet, Score, ScoreGame, Coefficent1, Coefficent2) {
    this._Player1 = Player1;
    this._Player2 = Player2;
    this._Bookmaker1 = Bookmaker1;
    this._Bookmaker2 = Bookmaker2;
    this._NumGame = NumGame;
    this._NumSet = NumSet;
    this._Score = Score;
    this._ScoreGame = ScoreGame;
    this._Coefficent1 = Coefficent1;
    this._Coefficent2 = Coefficent2;

    this.Player1 = function() {
        return this._Player1;
    };

    this.Player2 = function() {
        return this._Player2;
    };
    this.Bookmaker1 = function() {
        return this._Bookmaker1;
    };
    this.Bookmaker2 = function() {
        return this._Bookmaker2;
    };
    this.NumGame = function() {
        return this._NumGame;
    };
    this.NumSet = function() {
        return this._NumSet;
    };

    this.Score = function() {
        return this._Score;
    };
    this.ScoreGame = function() {
        return this._ScoreGame;
    };
    this.Coefficent1 = function() {
        return this._Coefficent1;
    };
    this.Coefficent2 = function() {
        return this._Coefficent2;
    };
};
var socket;
window.onload = function () {
    var publicip = "0.0.0.0:8080";
    var host = window.location.hostname;
    publicip = host + ':8080';
    socket = io.connect(publicip);

    window.setInterval(function() {
        console.log('norm');

    }, 5000);
    socket.on('gettableTable', function(data) {
        if (data) {
            mydata = data;
            drawme(mydata.data);
            console.log(mydata);
        } else {
            console.log('wrong');
        }
    });
};
$(document).ready(function() {
    var table = $('#table_id').DataTable({
		paging: false,
        columns: [{
            data: null,
            render: 'Player1',
			name:'Player1'
        }, {
            data: null,
            render: 'Player2'
        }, {
            data: null,
            render: 'Bookmaker1'
        }, {
            data: null,
            render: 'Bookmaker2'
        }, {
            data: null,
            render: 'NumGame'
        }, {
            data: null,
            render: 'NumSet'
        }, {
            data: null,
            render: 'Score'
        }, {
            data: null,
            render: 'ScoreGame'
        }, {
            data: null,
            render: 'Coefficent1'
        }, {
            data: null,
            render: 'Coefficent2'
        }]
    });


    // table.row.add( new Person( 'Airi Satou',     33, 'Accountant' ) );
    // table.row.add( new Person( 'Angelica Ramos', 47, 'Chief Executive Officer (CEO)' ) );
    // table.row.add( new Person( 'Ashton Cox',     66, 'Junior Technical Author' ) );
    table.draw();
})
buttnclick = function() {
    var table = $('#table_id').DataTable();
    table
        .clear(); 
    socket.emit('restableTable');

};
buttnclick2 = function () {
    socket.emit('restartme');
};
buttnclick3 = function () {
    socket.emit('wantdumpme', {"data": username.textContent});
};
function drawme(data) {
        //$('.container.fail').attr('class','container');
		var table = $('#table_id').DataTable();
        for (var i in data) {
            table.row.add(new Person(
                data[i].Player1,
                data[i].Player2,
                data[i].Bookmaker1,
                data[i].Bookmaker2,
                data[i].NumGame,
                data[i].NumSet,
                data[i].Score,
                data[i].ScoreGame,
                data[i].Coefficent1,
                data[i].Coefficent2
		))};table.draw();
        };