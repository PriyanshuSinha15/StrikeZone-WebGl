using System.Runtime.InteropServices;
using UnityEngine;

public static class PlayVlayBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")] private static extern void PV_RegisterCallbacks();
    [DllImport("__Internal")] private static extern void PV_Ready();
    [DllImport("__Internal")] private static extern void PV_ReportScore(int score);
    [DllImport("__Internal")] private static extern void PV_GameOver(int score);
    [DllImport("__Internal")] private static extern void PV_ReportHighScore(int score);
    [DllImport("__Internal")] private static extern void PV_ReachedLevel(int level);
    [DllImport("__Internal")] private static extern void PV_HapticSuccess();

#else

    private static void PV_RegisterCallbacks() { }
    private static void PV_Ready() { }
    private static void PV_ReportScore(int score) { }
    private static void PV_GameOver(int score) { }
    private static void PV_ReportHighScore(int score) { }
    private static void PV_ReachedLevel(int level) { }
    private static void PV_HapticSuccess() { }

#endif

    public static void RegisterCallbacks() => PV_RegisterCallbacks();
    public static void Ready() => PV_Ready();
    public static void ReportScore(int score) => PV_ReportScore(score);
    public static void GameOver(int score) => PV_GameOver(score);
    public static void ReportHighScore(int score) => PV_ReportHighScore(score);
    public static void ReachedLevel(int level) => PV_ReachedLevel(level);
    public static void HapticSuccess() => PV_HapticSuccess();
}