a = {
    'Ё': 'YO',
    'Й': 'I',
    'Ц': 'TS',
    'У': 'U',
    'К': 'K',
    'Е': 'E',
    'Н': 'N',
    'Г': 'G',
    'Ш': 'SH',
    'Щ': 'SCH',
    'З': 'Z',
    'Х': 'H',
    'Ъ': '',
    'ё': 'yo',
    'й': 'i',
    'ц': 'ts',
    'у': 'u',
    'к': 'k',
    'е': 'e',
    'н': 'n',
    'г': 'g',
    'ш': 'sh',
    'щ': 'sch',
    'з': 'z',
    'х': 'h',
    'ъ': '',
    'Ф': 'F',
    'Ы': 'I',
    'В': 'V',
    'А': 'a',
    'П': 'P',
    'Р': 'R',
    'О': 'O',
    'Л': 'L',
    'Д': 'D',
    'Ж': 'ZH',
    'Э': 'E',
    'ф': 'f',
    'ы': 'i',
    'в': 'v',
    'а': 'a',
    'п': 'p',
    'р': 'r',
    'о': 'o',
    'л': 'l',
    'д': 'd',
    'ж': 'zh',
    'э': 'e',
    'Я': 'Ya',
    'Ч': 'CH',
    'С': 'S',
    'М': 'M',
    'И': 'I',
    'Т': 'T',
    'Ь': '',
    'Б': 'B',
    'Ю': 'YU',
    'я': 'ya',
    'ч': 'ch',
    'с': 's',
    'м': 'm',
    'и': 'i',
    'т': 't',
    'ь': '',
    'б': 'b',
    'ю': 'yu'
};
transliterate = function (word) {
    return word.split('').map(function (char) {
        return typeof a[char] !== 'undefined' ? a[char] : char;
    }).join('');
};
var structarr = [];

function parse() {
    tmpeventsarr = [];
    var evs = apiWlb.getEvents();
    for (var i = 0; i < evs.dbCollection.data.length; i++) {
        if (evs.dbCollection.data[i].isLive == 1 && evs.dbCollection.data[i].idSport == 5) {
            var maindata = evs.dbCollection.data[i];
            var id = maindata.id;
            var tournament = maindata.countryName;
            var player1 = maindata.members[0];
            var player2 = maindata.members[1];
            var score = maindata.scores.join('');
            var currentsetnum = maindata.scores.length;
            //calculating base for fullgames
            var fullgamesnum = 0;
            if (maindata.scores.length > 1) {
                var tmpnum = 0;
                for (var s = 0; s < maindata.scores.length - 1; s++) {
                    var val1 = maindata.scores[s].trim()[0];
                    var val2 = maindata.scores[s].trim()[2];
                    tmpnum += parseInt(val1) + parseInt(val2);
                }
                fullgamesnum = tmpnum;
            }
            var c = apiWlb.getService("LineService");
            var lines = c.getLineByIdEvent(id);
            tmpeventsarr.push({
                'Event': tournament,
                'Player1': transliterate(player1).replace(/_/g, ' '),
                'Player2': transliterate(player2).replace(/_/g, ' '),
                'ScoreAll': score.replace(/  /g, ' '),
                'GamePoints': maindata.score,
                'GamesArr': searchlineid(id, currentsetnum, lines, fullgamesnum)
            });
        }
    }
    structarr = tmpeventsarr;
}

function searchlineid(id, setnum, lines, gamesnum) {
    var arr = [];
    var linesarr = Object.keys(lines).map(function (key) {
        return lines[key];
    });

    for (var i = 0; i < linesarr.length; i++) {
        if (linesarr[i].idEvent == id) {
            var textofmarket = linesarr[i].tipLine.freeTextR;
            if (textofmarket.indexOf('Кто выиграет @x гейм') != -1) {
                if (linesarr[i].koef !== undefined)
                    arr.push({
                        SetNumber: setnum,
                        GameNumber: linesarr[i].koef,
                        Coefficent1: linesarr[i].V[0],
                        Coefficent2: linesarr[i].V[1]
                    });
            } else if (textofmarket.indexOf('Кто выиграет следующий гейм') != -1 || textofmarket.indexOf('Победитель текущего гейма') != -1) {
                if (linesarr[i].koef !== undefined)
                    arr.push({
                        SetNumber: setnum,
                        GameNumber: parseInt(linesarr[i].koef) - gamesnum,
                        Coefficent1: linesarr[i].V[0],
                        Coefficent2: linesarr[i].V[1]
                    });
            }
        }
    }
    return arr;
}