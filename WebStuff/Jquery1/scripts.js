$(document).ready(function(){
    $("one").data("clicked", false );
    function bounceIn(){
    $('#uno').animate({height:'15%'}, 1000,'easeOutBounce');
    $('#dos').delay(250).animate({height:'15%'}, 1000, 'easeOutBounce');
    $('#tres').delay(500).animate({height:'15%'}, 1000, 'easeOutBounce');
    $('#quatro').delay(750).animate({height:'15%'}, 1000, 'easeOutBounce');
    $('#cinco').delay(1000).animate({height:'15%'}, 1000, 'easeOutBounce');
    $('#bday').delay(1500).animate({width: '65%'}, 2000, 'easeOutElastic');
    };
    $('.over').mouseenter(function(){
        $(this).filter(':not(:animated)').animate({height:'0'}, 700,'easeOutCirc');
    });
    $('.over').mouseleave(function(){
        $(this).animate({height:'15%'}, 1000,'easeOutBounce');
    });
    bounceIn();
     $('#bday').mouseenter(function(){
        $(this).filter(':not(:animated)').animate({height:'0'}, 1200,'easeOutCirc');
    });
    $('#bday').mouseleave(function(){
        $(this).animate({height:'15%'}, 800,'easeOutBounce');
    });
    $('.one').hover(function(){
        if(!$(this).data("clicked")){
            $(this).fadeTo('fast', .50);
        }
    }, function(){
        if(!$(this).data("clicked")){
            $(this).fadeTo('slow', 1);
        }
    });
    $('.one').click(function(){
        if(!$(this).data("clicked")){
            $(this).fadeTo('fast', 0);
        }
        else{
            $(this).fadeTo('slow', 1);
        }
        $(this).data("clicked", !$(this).data("clicked") ? true: false);
    });
});
