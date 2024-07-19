$(function () {
    //$('#Container').mixItUp();
});
var mydata = "";
var bookiespriority = [{ 0: "Williams" }, { 1: "Bet365" }, { 2: "Marathon" }, { 3: "Parimatch" }, { 4: "Fonbet" }, { 5: "Pinnacle" }, { 6: "Olimp" }];

function addeventlistener() {
    $("#droplist label").click(function () {
        //change index
        upprior($(this).text());
        drawPriorities();

        //swap priorities
    });
};
function upprior(value) {
    //find value index
    var index1 = findvalue(value);
    if (index1 != 0) {
        //swap value with better index
        var c = bookiespriority[index1 - 1][index1 - 1];
        bookiespriority[index1 - 1][index1 - 1] = bookiespriority[index1][index1];
        bookiespriority[index1][index1] = c;
    }

    
};
function findvalue(value)
{
    for (var i = 0; i < bookiespriority.length; i++) {
        if (bookiespriority[i][i] == value) {
            return parseInt(Object.keys(bookiespriority[i]));
        }
    }
};

function drawPriorities() {
    $(document).find('#droplist').empty();
    var toadd = $(document).find('#droplist');
    
        // Checkbox is checked.
    //add prioritybox
    for (var i = 0; i < bookiespriority.length; i++) {
        $('<li><p><label>').find("label").text(bookiespriority[i][i]).appendTo($(document).find('#droplist'));
    }
    addeventlistener();
    setCookie("bookiespriority", JSON.stringify(bookiespriority));
};

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}
function setCookie(name, value, options) {
    options = options || {};

    var expires = options.expires;

    if (typeof expires == "number" && expires) {
        var d = new Date();
        d.setTime(d.getTime() + expires * 1000);
        expires = options.expires = d;
    }
    if (expires && expires.toUTCString) {
        options.expires = expires.toUTCString();
    }

    value = encodeURIComponent(value);

    var updatedCookie = name + "=" + value;

    for (var propName in options) {
        updatedCookie += "; " + propName;
        var propValue = options[propName];
        if (propValue !== true) {
            updatedCookie += "=" + propValue;
        }
    }

    document.cookie = updatedCookie;
}
$(document).ready(function () {
    
    if (getCookie("bookiespriority") != undefined) {
        bookiespriority = JSON.parse(getCookie("bookiespriority"));
    }
    else {
        setCookie("bookiespriority", JSON.stringify(bookiespriority));
    }
    $('#prioron').change(function () {
        var toadd = $(document).find('#droplist');
        
        if ($(this).is(':checked')) {
            // Checkbox is checked.
            //add prioritybox
            setCookie("setprior", document.getElementById('prioron').checked);
            drawPriorities();
            addeventlistener()
            //s.appendTo($(this).next());
        }
        else {
            $(document).find('#droplist').empty();
            setCookie("setprior", document.getElementById('prioron').checked);
            // Checkbox is not checked.
        }
    });
    if (getCookie("setprior") != undefined) {
        if (getCookie("setprior")) {
            document.getElementById('prioron').click();
        }
    }
    else {
        setCookie("setprior", document.getElementById('prioron').checked);
    }
    // read the current/previous setting
    $(".check").each(function () {
        var name = $(this).prop('id');
        if ($.cookie(name) && $.cookie(name) == "true") {
            $(this).prop('checked', $.cookie(name));
        }
    });
    $("#minval").each(function () {
        var name = $(this).prop('id');
        if ($.cookie(name)) {
            $(this).prop('value', $.cookie(name));
        }
        else {
            $(this).prop('value', 0);
        }
    });
    $("#maxval").each(function () {
        var name = $(this).prop('id');
        if ($.cookie(name)) {
            $(this).prop('value', $.cookie(name));
        }
        else {
            $(this).prop('value', 20);
        }
    });
    // event management
    $(".check").change(function () {
        var name = $(this).prop("id");
        $.cookie(name, $(this).prop('checked'), {
            path: '/',
            expires: 365
        });
    });
    $("#minval").change(function () {
        var name = $(this).prop("id");
        $.cookie(name, $(this).prop('value'), {
            path: '/',
            expires: 365
        });
    });
    $("#maxval").change(function () {
        var name = $(this).prop("id");
        $.cookie(name, $(this).prop('value'), {
            path: '/',
            expires: 365
        });
    });
});
window.onload = function () {
    var publicip = "0.0.0.0:8080";
    var host = window.location.hostname;
    publicip = host + ':8080';
    var socket = io.connect(publicip);

    window.setInterval(function () {
        console.log('norm');
        socket.emit('restable');
    }, 5000);
    socket.on('gettable', function (data) {
        if (data) {
            mydata = data;
            draw(mydata.data);
            //console.log(mydata);
        }
        else {
            console.log('wrong');
        }
    });
};

Element.prototype.remove = function () {
    this.parentElement.removeChild(this);
};

$('input').on('click', function () {
    if ($(this).hasClass('grid')) {
        $('#Container ul').removeClass('small-block-grid-1').addClass('small-block-grid-3 medium-block-grid-4 large-block-grid-5');
    }
    else if ($(this).hasClass('list')) {
        $('#Container ul').removeClass('small-block-grid-3 medium-block-grid-4 large-block-grid-5').addClass('small-block-grid-1');
    }
});
$(".container").on("DOMNodeInserted", function () {
    //console.log("asd");
    //$('.container').mixItUp('filter',$(".active").attr("data-filter"));

});

$(document).foundation();
var obj;

function getBookAdd(book) {
    var a = book;
    var bk = "BookMaker ";
    switch (a) {
        case "Olimp":
            {
                return bk + "ol";
                //break;
            }
        case "Marathon":
            return bk + "mar";
        //break;
        case "BetCity":
            return bk + "bc";
        //break;
        case "PariMatch":
            return bk + "par";
        //break;
        case "Bet365":
            return bk + "bt365";
        //break;
        case "Williams":
            return bk + "wil";
        //break;
        case "Titanbet":
            return bk + "tb";
        //break;
        case "Pinnacle":
            return bk + "pin";
        //break;
        case "Fonbet":
            return bk + "fb";
        //break;
        case "Winline":
            return bk + "winline";
        //break;
    }

}
function test() {
    var obj = "";
    //Add main tag
    obj = $(obj).add("<div class='Bets'>");

    //Add precent
    //console.log(percent);
    $(obj).append("<div class='row percent'><div class='small-6 large-centered columns'>" + 'sad' + "</div></div>");
    //Add Player names
    $(obj).append("<div class='row players'>  <div class='large-12 large-5 columns Player1'>" + 'sad' + "</div>  <div class='large-12 large-6 columns'>" + "asdasd" + "</div></div>");
    //Add Score
    $(obj).append("<div class='row score'>  <div class='large-12 columns'>Score:" + "asddasasd" + "</div></div>");

    //Add Points
    $(obj).append("<div class='row points'>  <div class='large-12 columns'>Points:" + 'asd' + "</div></div>");
    //Add NumGame
    $(obj).append("<div class='row points'>  <div class='large-12 columns'>Numgame:" + 'asd' + "</div></div>");
    //Add Boookmakers
    $(obj).append("<div class='row books'>  <div class='small-6 small-5 columns' >" + 'asd' + "</div>  <div class='small-6 small-6 columns' >" + "asd" + "</div></div>");
    //Add Coefs
    $(obj).append("<div class='row coefs'>  <div class='small-6 small-5 columns'>" + 'asd' + "</div>  <div class='small-6 small-6 columns'>" + "asd" + "</div></div>");
    //SelectActive or not
    var newobj = "";
    $(obj).append("<li class='created'>");
    $(newobj).append(obj);
    $("#removeList").append(obj);
    //$('.container2').mixItUp();
};

$('.filter.tiny').click(function () {
    $('.active').attr('class', 'filter tiny round');
    $(this).attr('class', 'filter tiny round active');
    draw(mydata.data);
});
function draw(data) {
    //$('.container.fail').attr('class','container');
    $('#Container').children().children().remove();
    var tickets = [];
    for (var i in data) {
        var obj = "";
        var lastgame = false;
        var isbet365 = false;
        //Add main tag
        obj = $(obj).add("<div class='Bets'>");
        //Add precent
        var percent = ((data[i].Coefficent1 * data[i].Coefficent2) / (data[i].Coefficent1 + data[i].Coefficent2)) - 1;
        percent = percent * 100;
        percent = percent.toFixed(2);
        //console.log(percent);
        $(obj).append("<div class='row percent'><div class='small-6 large-centered columns'>" + percent.toString() + "</div></div>");
        //Add Player names
        $(obj).append("<div class='row players'>  <div class='large-12 large-5 columns Player1'>" + data[i].Player1 + "</div>  <div class='large-12 large-6 columns'>" + data[i].Player2 + "</div></div>");
        //Add Score
        $(obj).append("<div class='row score'>  <div class='large-12 columns'>Score:" + data[i].Score + "</div></div>");
        var mixScore = data[i].Score.replace(/\:|-/gi, ' ').split(' ');
        for (var k = mixScore.length - 1; k >= 0; k--) {
            if (mixScore[k] === '') {
                mixScore.splice(k, 1);
            }
        }
        var sum3 = 0;
        for (var k = mixScore.length - 1; k >= 0; k--) {
            if (mixScore[k] !== ' ') {
                sum3 += parseInt(mixScore[k], 10);
            }
        }
        //
        if (mixScore[0] == ' ')
            mixScore[0] = '';
        console.info(mixScore);
        console.info(sum3);
        if (!isNaN(parseInt(mixScore[mixScore.length - 1], 10)))
            var sum1 = parseInt(mixScore[mixScore.length - 1], 10);
        else
            var sum1 = parseInt(mixScore[mixScore.length - 3], 10);
        var sum2 = parseInt(mixScore[mixScore.length - 2], 10);

        //console.info(sum1);
        //console.info(sum2);



        if ((sum1 == 5 && (sum2 == 6 || sum2 == 4 || sum2 == 3 || sum2 == 2 || sum2 == 1 || sum2 == 0)) || (sum2 == 5 && (sum1 == 6 || sum1 == 4 || sum1 == 3 || sum1 == 2 || sum1 == 1 || sum1 == 0)))
            lastgame = true;
        if (data[i].Bookmaker1 == 'Bet365' || data[i].Bookmaker2 == 'Bet365')
            isbet365 = true;
        //else

        var testactive = false;
        if (data[i].ScoreGame != null)
            if (data[i].ScoreGame.indexOf('1') != -1 || data[i].ScoreGame.indexOf('3') != -1 || data[i].ScoreGame.indexOf('4') != -1)
                testactive = true;
        //Add Points
        $(obj).append("<div class='row points'>  <div class='large-12 columns'>Points:" + data[i].ScoreGame + "</div></div>");
        //Add NumGame
        $(obj).append("<div class='row points'>  <div class='large-12 columns'>Numgame:" + data[i].NumGame + "</div></div>");
        //Add Boookmakers
        $(obj).append("<div class='row books'>  <div class='small-6 small-5 columns " + getBookAdd(data[i].Bookmaker1) + "' >" + data[i].Bookmaker1 + "</div>  <div class='small-6 small-6 columns " + getBookAdd(data[i].Bookmaker2) + "' >" + data[i].Bookmaker2 + "</div></div>");
        //Add Coefs
        $(obj).append("<div class='row coefs'>  <div class='small-6 small-5 columns'>" + data[i].Coefficent1 + "</div>  <div class='small-6 small-6 columns'>" + data[i].Coefficent2 + "</div></div>");
        //SelectActive or not
        var newobj = "";

        //test for contain checked
        setme = function () {
            var arr = [];
            $.each($('.check'), function () {
                if ($(this).prop('checked') == false) arr.push($(this).next().text());
            })
            var psh = [];
            $(arr).each(function (index) {
                psh.push(newobj.text().indexOf(arr[index]))
            })
            var countofone = 0;
            for (var i = 0; i < psh.length; ++i) {
                if (psh[i] == -1)
                    countofone++;
            }
            if (countofone != psh.length)
                return false;
            else
                return true;
        }
        //**********
        var imin = 0;
        var imax = 1000;
        if ($("#minval").val() != "")
            imin = $("#minval").val();
        if ($("#maxval").val() != "")
            imax = $("#maxval").val();
        if (percent < 0) percent = percent * (-1);
        if (testactive && parseInt(data[i].NumGame.split()) == (sum1 + sum2 + 1) || parseInt(data[i].NumGame.split()) == (sum3 + 1))
            newobj = $(newobj).add("<li class='current' style='display:none;'>");
        else
            newobj = $(newobj).add("<li class='noncurrent' style='display:none;'>");
        $(newobj).append(obj);
        if ($(newobj).attr('class') == $('.active').attr('data-filter') && (!(lastgame && isbet365)) && setme() && parseFloat(percent) > parseFloat(imin) && parseFloat(percent) < parseFloat(imax)) {
            //$("#Container>ul").append(newobj);
            //$(newobj).fadeIn();
            tickets.push(newobj);
        }
        if ($('.active').attr('data-filter') == 'all' && (!(lastgame && isbet365)) && setme() && parseFloat(percent) > parseFloat(imin) && parseFloat(percent) < parseFloat(imax)) {
            //$("#Container>ul").append(newobj);
            //$(newobj).fadeIn();
            tickets.push(newobj);
        }

        //window.setTimeout(50);
    }
    //SortContainers
    function compare(a, b) {
        var aperc = parseFloat(a.find(".row")[0].textContent);
        var bperc = parseFloat(b.find(".row")[0].textContent);
        if (aperc < bperc) {
            return -1;
        }
        if (aperc > bperc) {
            return 1;
        }
        // a ?????? ???? ?????? b
        return 0;
    }
    function compareprior(a, b) {
        var aperc1 = a.find(".row.books")[0].children[0].textContent;
        var aperc2 = a.find(".row.books")[0].children[1].textContent;
        var bperc1 = b.find(".row.books")[0].children[0].textContent;
        var bperc2 = b.find(".row.books")[0].children[1].textContent;
        var sum1 = findvalue(aperc1) + findvalue(aperc2);
        var sum2 = findvalue(bperc1) + findvalue(bperc2);
        if (sum1 > sum2) {
            return -1;
        }
        if (sum1 < sum2) {
            return 1;
        }
        // a ?????? ???? ?????? b
        return 0;
    }
    var sorted = tickets.sort(compare).reverse();
    //sorting for priorities
    if (document.getElementById('prioron').checked)
    {
        sorted = sorted.sort(compareprior);
    }
    for (var p = 0; p < sorted.length; p++) {
        $("#Container>ul").append(sorted[p]);
        $(sorted[p]).fadeIn();
    }
    if ($('.players').length > 0) {
        if (soundon.checked) {
            var audioElement = document.createElement('audio');
            audioElement.setAttribute('src', 'https://www.freesound.org//data/previews/104/104946_161750-lq.ogg');
            audioElement.setAttribute('autoplay', 'autoplay');
            //audioElement.load()
            //$.get();
            audioElement.addEventListener("load", function () {

                audioElement.play();
            })
        };
    }
    // $('#Container').mixItUp('filter', $(".active").attr("data-filter"));
    $('.players').click(function (e) {
        var newsobj = '';
        newsobj = $(newsobj).add(this.parentNode);
        $(this).attr('class', 'delplayers');
        $(newsobj).attr('class', 'Bets remove');
        newsobj = $(newsobj).append("<button class='deleting'>X</button>");
        var jop = '';

        jop = $(jop).add("<li>");
        jop = $(jop).append(newsobj);
        $("#removeList").append(jop);
    });
    $('.deleting').click(function (e) {
        this.parentNode.parentNode.remove();
    })
    $('#removeList>li').click();
    //window.setTimeout(draw, 0);
};


//var bookiespriority={"Williams":1,"Bet365":2,"Marathon":3,"Parimatch":4,"Fonbet":5,"Pinnacle":6,"Olimp":7}