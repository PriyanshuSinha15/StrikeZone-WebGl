mergeInto(LibraryManager.library, {

    PV_RegisterCallbacks: function ()
    {
        console.log("[PlayVlay] RegisterCallbacks");

        if (window.__PVCallbacksRegistered)
        {
            console.log("[PlayVlay] Callbacks already registered.");
            return;
        }

        if (!window.PlayVlay)
        {
            console.warn("[PlayVlay] SDK not found.");
            return;
        }

        if (!window.unityInstance)
        {
            console.warn("[PlayVlay] Unity instance not found.");
            return;
        }

        window.__PVCallbacksRegistered = true;

        // ------------------ Init ------------------

        if (window.PlayVlay.onInit)
        {
            window.PlayVlay.onInit(function (ctx)
            {
                console.log("[PlayVlay] onInit");

                window.unityInstance.SendMessage(
                    "PlayVlayReceiver",
                    "OnInit",
                    JSON.stringify(ctx)
                );
            });
        }

        // ------------------ Start ------------------

        if (window.PlayVlay.onStart)
        {
            window.PlayVlay.onStart(function ()
            {
                console.log("[PlayVlay] onStart");

                window.unityInstance.SendMessage(
                    "PlayVlayReceiver",
                    "OnStart",
                    ""
                );
            });
        }

        // ------------------ Pause ------------------

        if (window.PlayVlay.onPause)
        {
            window.PlayVlay.onPause(function ()
            {
                console.log("[PlayVlay] onPause");

                window.unityInstance.SendMessage(
                    "PlayVlayReceiver",
                    "OnPause",
                    ""
                );
            });
        }

        // ------------------ Resume ------------------

        if (window.PlayVlay.onResume)
        {
            window.PlayVlay.onResume(function ()
            {
                console.log("[PlayVlay] onResume");

                window.unityInstance.SendMessage(
                    "PlayVlayReceiver",
                    "OnResume",
                    ""
                );
            });
        }

        // ------------------ Restart ------------------

        if (window.PlayVlay.onRestart)
        {
            window.PlayVlay.onRestart(function ()
            {
                console.log("[PlayVlay] onRestart");

                window.unityInstance.SendMessage(
                    "PlayVlayReceiver",
                    "OnRestart",
                    ""
                );
            });
        }

        // ------------------ Mute ------------------

        if (window.PlayVlay.onSetMuted)
        {
            window.PlayVlay.onSetMuted(function (muted)
            {
                console.log("[PlayVlay] onSetMuted : " + muted);

                window.unityInstance.SendMessage(
                    "PlayVlayReceiver",
                    "OnSetMuted",
                    muted ? "1" : "0"
                );
            });
        }

        console.log("[PlayVlay] All callbacks registered.");
    },

    //===================================================

    PV_Ready: function ()
    {
        console.log("[PlayVlay] Ready()");

        if (window.PlayVlay && window.PlayVlay.ready)
        {
            window.PlayVlay.ready();
        }
    },

    //===================================================

    PV_ReportScore: function (score)
    {
        console.log("[PlayVlay] ReportScore : " + score);

        if (window.PlayVlay && window.PlayVlay.reportScore)
        {
            window.PlayVlay.reportScore(score);
        }
    },

    //===================================================

    PV_GameOver: function (score)
    {
        console.log("[PlayVlay] GameOver : " + score);

        if (window.PlayVlay && window.PlayVlay.gameOver)
        {
            window.PlayVlay.gameOver(score);
        }
    },

    //===================================================

    PV_ReportHighScore: function (score)
    {
        console.log("[PlayVlay] ReportHighScore : " + score);

        if (window.PlayVlay && window.PlayVlay.reportHighScore)
        {
            window.PlayVlay.reportHighScore(score);
        }
    },

    //===================================================

    PV_ReachedLevel: function (level)
    {
        console.log("[PlayVlay] ReachedLevel : " + level);

        if (window.PlayVlay && window.PlayVlay.reachedLevel)
        {
            window.PlayVlay.reachedLevel(level);
        }
    },

    //===================================================

    PV_HapticSuccess: function ()
    {
        console.log("[PlayVlay] Haptic Success");

        if (window.PlayVlay && window.PlayVlay.haptic)
        {
            window.PlayVlay.haptic("success");
        }
    }

});