using BepInEx;
using BepInEx.Logging;
using CommonAPI;
using CommonAPI.Systems;
using DspILSAnalyzer.UI;
using HarmonyLib;
using UnityEngine;

namespace DspILSAnalyzer
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(CommonAPIPlugin.GUID)]
    [CommonAPISubmoduleDependency(nameof(ProtoRegistry), nameof(CustomKeyBindSystem))]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "de.sveri.dspilsanalyzer";
        public const string NAME = "DspILSAnalyzer";
        public const string VERSION = "0.2.0";

        public static ILSAnalyzerWindow mainWindow = null;
        // public static MainLogic mainLogic = null;
        public static Harmony harmony;

        public void Awake()
        {
            Logger.LogInfo($"Plugin {GUID} is loading...!");
            Log.LogSource = Logger;
            harmony = new(GUID);
            // harmony.PatchAll(typeof(WarningSystemPatch));
            // harmony.PatchAll(typeof(UIentryCount));

#if DEBUG
            Init();
#else
            harmony.PatchAll(typeof(Plugin));
#endif

            Logger.LogInfo($"Plugin {GUID} is loaded!");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIGame), nameof(UIGame._OnCreate))]
        internal static void Init()
        {
            Log.Info("Initializing Plugin ...");

            mainWindow = ILSAnalyzerWindow.CreateWindow();

#if !DEBUG
            CustomKeyBindSystem.RegisterKeyBind<PressKeyBind>(new BuiltinKey
            {
                key = new CombineKey((int)KeyCode.I, CombineKey.CTRL_COMB, ECombineKeyAction.OnceClick, false),
                conflictGroup = 2052,
                name = "ShowFactoryLocator",
                canOverride = true
            });
            ProtoRegistry.RegisterString("KEYShowILSAnalyzer", "Show ILS Analyzer Window", "");
#endif
            Log.Info("Plugin initialized");
        }

        public void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.F4))
            {
                if (!mainWindow.active)
                    mainWindow.OpenWindow();
                else
                    mainWindow._Close();
                return;
            }
#else
            if (CustomKeyBindSystem.GetKeyBind("ShowFactoryLocator").keyValue) 
            {
                if (!mainWindow.active)
                    mainWindow.OpenWindow();
                else
                    mainWindow._Close();
                return;
            }
#endif

            if (mainWindow != null && mainWindow.active)
            {
                mainWindow._OnUpdate();
                if (VFInput.escape)
                {
                    VFInput.UseEscape();
                    mainWindow._Close();
                }
            }
        }

        public void OnDestroy()
        {
            harmony.UnpatchSelf();
            // if (mainWindow != null)
            // {
            //     Destroy(mainWindow.gameObject);
            //     mainWindow = null;
            // }
            // UIentryCount.OnDestory();
        }
    }

    public static class Log
    {
        public static ManualLogSource LogSource;
        public static void Error(object obj) => LogSource.LogError(obj);
        public static void Warn(object obj) => LogSource.LogWarning(obj);
        public static void Info(object obj) => LogSource.LogInfo(obj);
        public static void Debug(object obj) => LogSource.LogDebug(obj);
    }
}
