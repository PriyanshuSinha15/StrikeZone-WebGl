mergeInto(LibraryManager.library, {

    PV_Ready: function ()
    {
        if (window.PlayVlay)
            window.PlayVlay.ready();
    },

    PV_ReportScore: function(score)
    {
        if (window.PlayVlay)
            window.PlayVlay.reportScore(score);
    },

    PV_GameOver: function(score)
    {
        if (window.PlayVlay)
            window.PlayVlay.gameOver(score);
    },

    PV_ReportHighScore: function(score)
    {
        if(window.PlayVlay)
            window.PlayVlay.reportHighScore(score);
    },

    PV_HapticSuccess: function()
    {
        if(window.PlayVlay)
            window.PlayVlay.haptic("success");
    }
});