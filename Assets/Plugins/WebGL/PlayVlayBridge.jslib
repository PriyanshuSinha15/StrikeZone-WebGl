mergeInto(LibraryManager.library, {

    PV_RegisterCallbacks: function ()
    {
        if (!window.PlayVlay)
            return;

        window.PlayVlay.onInit(function (ctx)
        {
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnInit",
                JSON.stringify(ctx)
            );
        });

        window.PlayVlay.onStart(function ()
        {
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnStart",
                ""
            );
        });

        window.PlayVlay.onPause(function ()
        {
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnPause",
                ""
            );
        });

        window.PlayVlay.onResume(function ()
        {
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnResume",
                ""
            );
        });

        window.PlayVlay.onRestart(function ()
        {
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnRestart",
                ""
            );
        });

        window.PlayVlay.onSetMuted(function (muted)
        {
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnSetMuted",
                muted ? "1" : "0"
            );
        });
    },

    PV_Ready: function ()
    {
        if(window.PlayVlay)
            window.PlayVlay.ready();
    },

    PV_ReportScore: function(score)
    {
        if(window.PlayVlay)
            window.PlayVlay.reportScore(score);
    },

    PV_GameOver: function(score)
    {
        if(window.PlayVlay)
            window.PlayVlay.gameOver(score);
    },

    PV_ReportHighScore: function(score)
    {
        if(window.PlayVlay)
            window.PlayVlay.reportHighScore(score);
    },

    PV_ReachedLevel: function(level)
    {
        if(window.PlayVlay)
            window.PlayVlay.reachedLevel(level);
    },

    PV_HapticSuccess: function()
    {
        if(window.PlayVlay)
            window.PlayVlay.haptic("success");
    }
});