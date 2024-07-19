var BookMaker = require('./BookmakerPattern.js');
var Xbet = new BookMaker('1xbet');

function Setup()
{
    Xbet.ParentNavigate("https://1-x-bet.com/en/multi/");
};

Setup();
function Reload() {
    Xbet.ParentBrowser[0].page.open("https://1-x-bet.com/en/multi/");
};

function Parse()
{

}






































//Rewrited function foradd any count of games
function addEventFun(el_ev, speed_move) {
    // Проверяем на максимальное количество и не было ли ранее выбранно событие
    if ( selectedGames.indexOf(el_ev.data('id')) == -1) {
        // Анимирует, как именно - я не вникал
        if (el_ev.is('.active')) {
            el_ev.removeClass('active').find('.squ_check').prop('checked', false);
            el_ev.animate({ opacity: 0, transform: 'scale(1.3)' }, 300, function () {
                el_ev.parent().remove()
            });
            $('[data-id=' + el_ev.data('event') + ']').removeClass('active').show();
        } else {
            var html = document.documentElement;
            el_ev.addClass('active').find('.squ_check').prop('checked', true);
            el_ev.css({
                position: 'fixed',
                top: el_ev.offset().top - html.scrollTop,
                left: el_ev.offset().left
            }).animate({
                top: $('.multi_sports_select li.selected_events').offset().top - html.scrollTop - 5,
                left: $('.multi_sports_select li.selected_events').offset().left - 80,
                transform: 'scale(0.3)'
            }, speed_move).animate({
                opacity: 0,
                transform: 'scale(1)'
            }, 200, function () {
                el_ev.css({
                    opacity: 1,
                    display: 'none',
                    top: 'auto',
                    left: 'auto',
                    position: 'relative'
                })
            });
            // Выбираем событие
            addEventsCol(el_ev.data('id'), el_ev.find('.name').html());
            
            if ($('#add_con_selected [data-id=' + el_ev.data('id') + ']').length > 0) {

            } else {
                var nthNum = 1,
                    nth1 = $('#add_con_selected .add_con_col:nth-child(1) .block, #add_con_selected .add_con_col:nth-child(1) .liga').length,
                    nth2 = $('#add_con_selected .add_con_col:nth-child(2) .block, #add_con_selected .add_con_col:nth-child(2) .liga').length,
                    nth3 = $('#add_con_selected .add_con_col:nth-child(3) .block, #add_con_selected .add_con_col:nth-child(3) .liga').length;
                
                if (nth1 < nth2 || nth1 < nth3) {
                    nthNum = 1
                }
                if (nth2 < nth1 || nth2 < nth3) {
                    nthNum = 2
                }
                if (nth3 < nth1 || nth3 < nth2) {
                    nthNum = 3
                }
                
                $('#add_con_selected .add_con_col:nth-child(' + nthNum + ') .events_ul').append(
                    '<li class="del_ico">' +
                        el_ev.parent().html() +
                    '</li>'
                );
            }
        }
        
        // Cчётчик (нахрен не нужно, проверить и удалить)
        $('.multi_sports_select li.selected_events a .count').html($(".add_con .cont:not(#add_con_selected) .events_ul li .block.active").length);
        
        // Cкрытие/показ лиги если в неё есть/нету событий
        if (el_ev.parent().parent().find('li .block:not(.active)').length == 0) {
            el_ev.parent().parent().prev().fadeOut(300);
        } else {
            el_ev.parent().parent().prev().fadeIn(300);
        }
    }
}
