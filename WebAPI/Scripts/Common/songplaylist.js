jQuery(document).ready(function() {

    // inner variables
    var song = null;
    var tracker = $('.tracker');
    var volume = $('.volume');

    function initAudio(elem) {
        var url = elem.attr('audiourl');
        var title = elem.attr("title");
        var cover = elem.attr('cover');
        var artist = elem.attr('artist');

        $('.player .title').text(title);
        $('.player .title').attr("title", title);
        $('.player .artist').text(artist);
        $('.player .cover').css('background-image','url('+cover+')');

        song = new Audio(url);

        // timeupdate event listener
        $(song).unbind("timeupdate").bind("timeupdate", function () {
            var curtime = parseInt(song.currentTime, 10);
            $(".tracker").slider('value', curtime);
        })

        $('.playlist li').removeClass('active');
        elem.addClass('active');
        try {
            tracker.slider("value", 0)
        } catch (ex) {

        }
    }
    function playAudio() {
        song.play();

        tracker.slider("option", "max", song.duration);

        $('.play').addClass('hidden');
        $('.pause').addClass('visible');
    }
    function stopAudio() {
        song.pause();

        $('.play').removeClass('hidden');
        $('.pause').removeClass('visible');
    }

    // play click
    $('.play').click(function (e) {
        e.preventDefault();

        playAudio();
    });

    // pause click
    $('.pause').click(function (e) {
        e.preventDefault();

        stopAudio();
    });

    // forward click
    $('.fwd').click(function (e) {
        stopAudio();

        var next = $('.playlist li.active').next();
        if (next.length == 0) {
            next = $('.playlist li:first-child');
        }
        initAudio(next);
        setTimeout(function () {
            playAudio();
        }, 100)
    });

    // rewind click
    $('.rew').click(function (e) {
        stopAudio();

        var prev = $('.playlist li.active').prev();
        if (prev.length == 0) {
            prev = $('.playlist li:last-child');
        }
        initAudio(prev);
        stopAudio();
        setTimeout(function () {
            playAudio();
        }, 100)
    });

    // playlist elements - click
    $('.playlist li').click(function () {
        stopAudio();
        initAudio($(this));
        setTimeout(function () {
            playAudio();
        }, 100)
    });

    // initialization - first element in playlist
    initAudio($('.playlist li:first-child'));

    // set volume
    song.volume = 0.8;

    // initialize the volume slider
    volume.slider({
        range: 'min',
        min: 1,
        max: 100,
        value: 80,
        start: function(event,ui) {},
        slide: function(event, ui) {
            song.volume = ui.value / 100;
        },
        stop: function(event,ui) {},
    });

    // empty tracker slider
    tracker.slider({
        range: 'min',
        min: 0, max: 10,
        start: function(event,ui) {},
        slide: function(event, ui) {
            song.currentTime = ui.value;
        },
        stop: function(event,ui) {}
    });

    $("#mainwrap .playlist").niceScroll();
});
