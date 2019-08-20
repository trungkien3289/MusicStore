// html5media enables <video> and <audio> tags in all major browsers
// External File: http://api.html5media.info/1.1.8/html5media.min.js


// Add user agent as an attribute on the <html> tag...
// Inspiration: http://css-tricks.com/ie-10-specific-styles/
var b = document.documentElement;
b.setAttribute('data-useragent', navigator.userAgent);
b.setAttribute('data-platform', navigator.platform);

var MediaPlayList = {
    index : 0,
    playing : false,
    extension : '.mp3',
    tracks :[],
    init: function (songs) {
        var supportsAudio = !!document.createElement('audio').canPlayType;
        if (supportsAudio) {
            var that = this;
            that.tracks = that.convertSong(songs);
            that.trackCount = that.tracks.length;
            that.npAction = $('#npAction');
            that.npTitle = $('#npTitle');
            that.audio = $('#audio1').bind('play', function () {
                that.playing = true;
                //that.npTitle.text('Now Playing...');
            }).bind('pause', function () {
                that.playing = false;
                //that.npAction.text('Paused...');
            }).bind('ended', function () {
                that.npAction.text('Paused...');
                if ((that.index + 1) < trackCount) {
                    that.index++;
                    MediaPlayList.loadTrack(that.index);
                    that.audio.play();
                } else {
                    that.audio.pause();
                    that.index = 0;
                    MediaPlayList.loadTrack(that.index);
                }
            }).get(0);
            that.btnPrev = $('#btnPrev').click(function () {
                if ((that.index - 1) > -1) {
                    that.index--;
                    MediaPlayList.loadTrack(that.index);
                    if (that.playing) {
                        that.audio.play();
                    }
                } else {
                    that.audio.pause();
                    that.index = 0;
                    MediaPlayList.loadTrack(that.index);
                }
            });
            that.btnNext = $('#btnNext').click(function () {
                if ((that.index + 1) < that.trackCount) {
                    that.index++;
                    MediaPlayList.loadTrack(that.index);
                    if (that.playing) {
                        that.audio.play();
                    }
                } else {
                    that.audio.pause();
                    that.index = 0;
                    MediaPlayList.loadTrack(that.index);
                }
            });
            li = $('#plList li').click(function () {
                var id = parseInt($(this).index());
                if (id !== that.index) {
                    MediaPlayList.playTrack(id);
                }
            });

            that.loadTrack(this.index);
        }
    },
    loadTrack : function (id) {
        var that = this;
        $('.plSel').removeClass('plSel');
        $('#plList li:eq(' + id + ')').addClass('plSel');
        that.npTitle.text(that.tracks[id].name);
        that.index = id;
        that.audio.src = that.tracks[id].file;
    },
    playTrack : function (id) {
        this.loadTrack(id);
        this.audio.play();
    },
    convertSong : function (songs) {
        var result = [];
        if (songs) {
            for (var i = 0; i < songs.length; i++) {
                result.push({ track: i, name: songs[i].Title, file: songs[i].MediaUrl });
            }
        }

        return result;
    }
}