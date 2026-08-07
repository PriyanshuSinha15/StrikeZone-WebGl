mergeInto(LibraryManager.library, {

    PV_GetUnity: function ()
    {
        return window.unityInstance || null;
    },

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
            console.log("PlayVlay -> Start");

            if(window.unityInstance)
            {

            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnStart",
                ""
            );
            }
        });

        window.PlayVlay.onPause(function ()
        {
            console.log("PlayVlay Pause");

            if(window.unityInstance){
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnPause",
                ""
            );
            }
        });

        window.PlayVlay.onResume(function ()
        {
            console.log("PlayVlay Resume");

            if(winow.unityInstance){
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnResume",
                ""
            );
            }
        });

        window.PlayVlay.onRestart(function ()
        {
            console.log("PlayVlay Restart");

            if(window.unityInstance){
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnRestart",
                ""
            );
            }
        });

        window.PlayVlay.onSetMuted(function (muted)
        {
            console.log("PlayVlay onSetMuted");

            if(window.unityInstance){
            window.unityInstance.SendMessage(
                "PlayVlayReceiver",
                "OnSetMuted",
                muted ? "1" : "0"
            );
            }
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